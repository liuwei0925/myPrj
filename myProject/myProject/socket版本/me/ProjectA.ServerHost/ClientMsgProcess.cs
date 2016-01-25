using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Communication.Sockets.Core;
using Communication.Sockets.Core.Server;
using Communication.Sockets.Core.Client;
using System.Windows.Forms;
using MysqlBasicClass;
using System.Linq;

namespace Aviad.ProjectA.ServerHost
{
    public class MsgPair
    {
        public byte[] msg;
        public MsgPair(byte[] bytes, GlobalMessage msgType)
        {
            msg = bytes;
            type = msgType;
        }
        public GlobalMessage type;
    }

    public partial class ClientMsgProcess
    {
        public delegate void onClientPublishMessage(string msg,Socket sock, GlobalMessage type);
        public event onClientPublishMessage clientPublishMessage;

        public delegate void onSendMsgToServer(byte[] buffer, Socket sock, GlobalMessage type);
        public event onSendMsgToServer sendMsgToServer;

        public delegate void onSendMsgToClient(byte[] buffer, Socket sock, GlobalMessage type);
        public event onSendMsgToClient sendMsgToClient;

        public void analyse(byte[] buffer, Socket clientSock)
        {
            int totalLen = buffer.Length;
            int currentIndex = 0;
            while (currentIndex < totalLen)
            {
                if (verify(buffer) == false)
                {
                    sendMsgToServer(null, clientSock, GlobalMessage.PACKETERROR);
                    return;
                }
                int DataLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buffer, 8 + currentIndex));
                byte[] dataBuffer = new byte[DataLength];
                Array.Copy(buffer, 12 + currentIndex, dataBuffer, 0, DataLength);
                
                string msg = System.Text.Encoding.UTF8.GetString(dataBuffer);
                MsgPair msgPair = dispatch(msg, clientSock);
                currentIndex += DataLength + 12;
               // ClientUpdateMessage(byteMessage, clientSock, 2);
                if (msgPair != null)
                sendMsgToServer(msgPair.msg, clientSock, msgPair.type);
            }
        }

        public bool verify(byte[] buffer)
        {
            byte[] PsxMsgFlag = new byte[] { 0xff, 0xfe, 0xff, 0xfe, 0xff, 0xfe, 0xff, 0xfe };
            byte[] bytes = new byte[PsxMsgFlag.Length];
            Array.Copy(buffer, bytes, PsxMsgFlag.Length);

            if (PsxMsgFlag.SequenceEqual(bytes))
                return true;
            else 
                return false;
        }

        MsgPair dispatch(string msg, Socket clientSock)
        {
            int separateIndex = msg.IndexOf("\r\n");
            if (separateIndex != -1)
            {
                string msgHead = msg.Substring(0, separateIndex);
                int index = msgIndex[msgHead];

                ClientMsgType msgType = (ClientMsgType)index;

                GlobalMessage globalMsg;
               
                if (msgType.Equals(ClientMsgType.LOGIN))
                    globalMsg = GlobalMessage.LOGIN;
                else if (msgType.Equals(ClientMsgType.LOGOUT))
                    globalMsg = GlobalMessage.LOGOUT;
                else if (msgType.Equals(ClientMsgType.KEEPALIVEACK))
                    globalMsg = GlobalMessage.KEEPALIVEACK;
                else
                    globalMsg = GlobalMessage.DATA;
                clientPublishMessage(msg, clientSock, globalMsg);

                string newMsg = ClientMsgDelegate[index](msg, clientSock);
                if (newMsg == null)
                    return null;
                byte[]buffer = addHead(newMsg);

                MsgPair msgPair = new MsgPair(buffer, globalMsg);
                
                return msgPair;
            }
            else
                return null;
        
        }
        public byte[] addHead(string msg)
        {
            byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(msg);
            byte[] PsxMsgFlag = new byte[] { 0xff, 0xfe, 0xff, 0xfe, 0xff, 0xfe, 0xff, 0xfe };

            byte[] bytesInt = BitConverter.GetBytes((Int32)IPAddress.HostToNetworkOrder((Int32)dataBytes.Length));

            byte[] fullBytes = new byte[PsxMsgFlag.Length + bytesInt.Length + dataBytes.Length];
            Array.Copy(PsxMsgFlag, 0, fullBytes, 0, PsxMsgFlag.Length);
            Array.Copy(bytesInt, 0, fullBytes, PsxMsgFlag.Length, bytesInt.Length);
            Array.Copy(dataBytes, 0, fullBytes, PsxMsgFlag.Length + bytesInt.Length, dataBytes.Length);
            return fullBytes;
        }

        string login(string msg, Socket clientSock)
        {
            string operatorName = TextFollowing(msg, "OPERATORNAME:");

            //myGlobals.clientSock_operators.Add(clientSock, operatorName);
            //myGlobals.operators_clientSock.Add(operatorName, clientSock);
            if (!myGlobals.operatorData.operators.ContainsKey(operatorName))
                myGlobals.operatorData.addOpeartor(operatorName, clientSock);
            else
            {
                string msg2Client = "LOGINACK\r\n"
                                   + "STATUS:FAILED\r\n"
                                   + "REASIONCODE:4\r\n";

                byte[] bytes = addHead(msg2Client);
                sendMsgToClient(bytes, clientSock, GlobalMessage.DATA);
            }
            return msg;

        }
        string createConfRoom(string msg, Socket clientSock)
        {
            string others = TextFollowing(msg, "OTHERS:");
            string operatorName = TextFollowing(msg, "OPERATORNAME:");
            string localRoomId = TextFollowing(msg, "LOCALCONFROOMID:");
            string operatorNum = TextFollowing(msg, "OPERATORNUMBER:");
            string serverRoomId = "";


            string type = "SIP";

            Numbers tmpNumbers = processNumbers(others, type);

            RoomRecord room = new RoomRecord();
            room.operatorNum = operatorNum;

            if (tmpNumbers == null)
            {
                room.sipNumbers.Add(operatorNum);
                myGlobals.myRooms.rooms.Add(operatorNum, room);
                return msg;
            }


            PlanRecord plan = new PlanRecord();
            plan.pocNumbers = tmpNumbers.pocNumbers;
            plan.operatorNum = operatorNum;

            int flag = 0;
         
            foreach (var kv in tmpNumbers.pocNumbers)
            {
               if (!myGlobals.gatewayData.gateway[kv.Key].isOccupied)
                {
                    myGlobals.gatewayData.gateway[kv.Key].isOccupied = true;
                    tmpNumbers.sipNumbers.Add(myGlobals.gatewayData.gateway[kv.Key].sipNo);
                   flag = 1;
                }
            }

            if (flag == 1)
                myGlobals.myPlans.plans.Add(operatorNum, plan);

           
            string sipNumbers = string.Join(";", tmpNumbers.sipNumbers.ToArray());
            msg = msg.Replace(others, sipNumbers);

            room.sipNumbers = tmpNumbers.sipNumbers;
            room.sipNumbers.Add(operatorNum);
            myGlobals.pocSendDeal.addMember(operatorName, localRoomId, serverRoomId, tmpNumbers.pocNumbers);

            myGlobals.myRooms.rooms.Add(operatorNum, room);

            return msg;

        }
        string stopConfRoom(string msg, Socket clientSock)
        {

            string others = TextFollowing(msg, "OTHERS:");
            string operatorName = TextFollowing(msg, "OPERATORNAME:");
            string serverRoomId = TextFollowing(msg, "SERVERROOMID:");
            string localRoomId = TextFollowing(msg, "LOCALCONFROOMID:");
            string type = "SIP";


            Numbers tmpNumbers = processNumbers(others, type);

           // string sipNumbers = string.Join(";", tmpNumbers.sipNumbers.ToArray());

            //foreach (var kv in tmpNumbers.pocNumbers)
            //{
            //    sipNumbers += ";" + myGlobals.gatewayData.gateway[kv.Key].sipNo;
            //}
            //foreach (var kv in myGlobals.gatewayData.gateway)
            //{
            //    if (kv.Value.serverRoomId == serverRoomId)
            //        sipNumbers += ";" + kv.Value.sipNo;
            //}
            //msg = msg.Replace(others, sipNumbers);

            myGlobals.pocSendDeal.deleteMember(operatorName, localRoomId, serverRoomId, tmpNumbers.pocNumbers);

            string operatorNum = myGlobals.myRooms.getOperatorNum(serverRoomId);

            List<string> sipNumbers = myGlobals.myRooms.rooms[operatorNum].sipNumbers;


            if (sipNumbers.Count == 0)
                return null;
            string sipNumbersStr = string.Join(";", sipNumbers.ToArray());

            msg = msg.Replace(others, sipNumbersStr);
            

            return msg;

        }

        string addConfMember(string msg, Socket clientSock)
        {
            string others = TextFollowing(msg, "OTHERS:");
            string operatorName = TextFollowing(msg, "OPERATORNAME:");
            string serverRoomId = TextFollowing(msg, "SERVERROOMID:");
            string localRoomId = TextFollowing(msg, "LOCALCONFROOMID:");

            string type = TextFollowing(msg, "SPTYPE:");

            Numbers tmpNumbers = processNumbers(others, type);

            if (tmpNumbers == null)
                return null;
            foreach (var kv in tmpNumbers.pocNumbers)
            {
                if (myGlobals.gatewayData.gateway[kv.Key].serverRoomId == serverRoomId)
                {
                }
                else if(!myGlobals.gatewayData.gateway[kv.Key].isOccupied)
                {
                    myGlobals.gatewayData.gateway[kv.Key].isOccupied = true;
                    tmpNumbers.sipNumbers.Add(myGlobals.gatewayData.gateway[kv.Key].sipNo);
                }
            }

            string sipNumbers = string.Join(";", tmpNumbers.sipNumbers.ToArray());
            msg = msg.Replace(others, sipNumbers);
            myGlobals.pocSendDeal.addMember(operatorName, localRoomId, serverRoomId, tmpNumbers.pocNumbers);

            if (tmpNumbers.sipNumbers.Count == 0)
                return null;

            string operatorNum = myGlobals.myRooms.getOperatorNum(serverRoomId);

            foreach (var value in tmpNumbers.sipNumbers)
            {
                myGlobals.myRooms.rooms[operatorNum].sipNumbers.Add(value);
            }

			return msg;

        }
        string kickConfMember(string msg, Socket clientSock)
        {

            string others = TextFollowing(msg, "NUMBER:");
            string operatorName = TextFollowing(msg, "OPERATORNAME:");
            string serverRoomId = TextFollowing(msg, "SERVERROOMID:");
            string localRoomId = TextFollowing(msg, "LOCALCONFROOMID:");

            //string type = TextFollowing(msg, "TYPE:");

            string type = "SIP";
            Numbers tmpNumbers = processNumbers(others, type);

            string sipNumbers = string.Join(";", tmpNumbers.sipNumbers.ToArray());
            msg = msg.Replace(others, sipNumbers);


            myGlobals.pocSendDeal.deleteMember(operatorName, localRoomId, serverRoomId, tmpNumbers.pocNumbers);

            if (tmpNumbers.sipNumbers.Count == 0)
                return null;

            string operatorNum = myGlobals.myRooms.getOperatorNum(serverRoomId);

            foreach (var value in tmpNumbers.sipNumbers)
            {
                myGlobals.myRooms.rooms[operatorNum].sipNumbers.Remove(value);
            }

			return msg;

        }
        string changePassword(string msg, Socket clientSock)
        {
			return msg;

        }
        string serverCreateConfRoomAck(string msg, Socket clientSock)
        {
			return msg;
            
        }
        string setOperatorState(string msg, Socket clientSock)
        {

           
			return msg;
        }
        string setOperatorNumber(string msg, Socket clientSock)
        {
           
			return msg;

        }
        string changeOperatorNumber(string msg, Socket clientSock)
        {
           
			return msg;

        }
        string serverCreateIncomingConfRoomAck(string msg, Socket clientSock)
        {
           
			return msg;

        }
        string logout(string msg, Socket clientSock)
        {
           
			return msg;

        }
        string getConfRoomInfo(string msg, Socket clientSock)
        {
           
			return msg;

        }
        string getOperatorInfo(string msg, Socket clientSock)
        {
           
			return msg;

        }
        string keepAliveAck(string msg, Socket clientSock)
        {
           
			return msg;

        }
        string muteConfMember(string msg, Socket clientSock)
        {
           
			return msg;

        }
        string unMuteConfMember(string msg, Socket clientSock)
        {
           
			return msg;

        }
        string setConfBroadcast(string msg, Socket clientSock)
        {
           
			return msg;

        }
        string unSetConfBroadcast(string msg, Socket clientSock)
        {
           
			return msg;

        }
        string switchConfRoom(string msg, Socket clientSock)
        {
           
			return msg;

        }
        string setConfHostVideo(string msg, Socket clientSock)
        {

           
			return msg;
        }
        string restartConfModel(string msg, Socket clientSock)

        {
           
			return msg;

        }
        string addMemberNotify(string msg, Socket clientSock)
        {
           

            string number = TextFollowing(msg, "NUMBER:");
            string type = TextFollowing(msg, "TYPE:");
            
            USER_INFO userInfo = new USER_INFO();
            userInfo.name = number;
            userInfo.phone = number;
            userInfo.type = type;
            myGlobals.dbManager.AddUser(userInfo);
			return msg;
        }
        string deleteMemberNotify(string msg, Socket clientSock)
        {
           
            string number = TextFollowing(msg, "NUMBER:");
            //string type = TextFollowing(msg, "TYPE:");

            USER_INFO userInfo = new USER_INFO();
            //userInfo.name = number;
            userInfo.phone = number;
           // userInfo.type = type;
            myGlobals.dbManager.DeleteUser(userInfo);

			return msg;

        }


        public Numbers processNumbers(string message, string type)
        {

            if (string.IsNullOrEmpty(message))
            {
                return null;
            }
            Numbers tmpPhoneNumbers = new Numbers();

            char splitChar = ';';
            string[] tempArr = message.Split(splitChar);
            for (int i = 0; i < tempArr.Length; i++)
            {
                //userInfo = dbManager.GetUser("", tempArr[i], "", "", "", "");
                USER_INFO userInfo = new USER_INFO();
                userInfo.phone = tempArr[i];
                USER_INFO[] userInfoArr = myGlobals.dbManager.GetUser(userInfo);
                if (userInfoArr.Length > 0)
                {
                    tmpPhoneNumbers.addNumber(userInfoArr[0].type, tempArr[i]);
                }
                else
                {
                    //dbManager.AddUser("", tempArr[i], "", "", "", type);
                    userInfo.type = type;
                    if (myGlobals.pocTypes.Contains(type))
                        myGlobals.dbManager.AddUser(userInfo);
                    tmpPhoneNumbers.addNumber(type, tempArr[i]);
                }
            }
            return tmpPhoneNumbers;
        }

        public string TextFollowing(string searchTxt, string value)
        {
            string separateString = "\r\n";
            if (!String.IsNullOrEmpty(searchTxt) && !String.IsNullOrEmpty(value))
            {
                int index = searchTxt.IndexOf(value);
                if (-1 < index)
                {
                    int separateIndex = searchTxt.IndexOf(separateString, index);
                    if (-1 < separateIndex)
                    {
                        int start = index + value.Length;
                        return searchTxt.Substring(start, separateIndex - start);
                    }
                }
            }
            return null;
        }
    }
}
