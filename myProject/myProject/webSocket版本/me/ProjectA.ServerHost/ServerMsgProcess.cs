using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using MysqlBasicClass;
using System.Net.Sockets;
using System.Linq;

namespace Aviad.ProjectA.ServerHost
{
    public partial class ServerMsgProcess
    {
        public class MsgPair
        {
            public string msg;
            public MsgPair(string message, GlobalMessage msgType)
            {
                msg = message;
                type = msgType;
            }
            public GlobalMessage type;
        }

        public delegate void onServerPublishMessage(string msg,Socket sock, GlobalMessage type);
        public event onServerPublishMessage serverPublishMessage;

        public delegate void onSendMsgToClient(string msg, Socket sock, GlobalMessage type);
        public event onSendMsgToClient sendMsgToClient;

        public delegate void onSendMsgToServer(byte[] buffer, Fleck.IWebSocketConnection sock, GlobalMessage type);
        public event onSendMsgToServer sendMsgToServer;

        public delegate void onReleaseConnection(Socket sock);
        public event onReleaseConnection releaseConnection;

        public delegate void onMsgBusMsg(string msg, int param);
        //  private long sock;


        public void analyse(byte[] buffer, Socket serverSock)
        {
            int totalLen = buffer.Length;
            int currentIndex = 0;
            // sock = keyOrValue;
            while (currentIndex < totalLen)
            {
                if (verify(buffer) == false)
                    break;
                int DataLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buffer, 8 + currentIndex));
                byte[] dataBuffer = new byte[DataLength];
                Array.Copy(buffer, 12 + currentIndex, dataBuffer, 0, DataLength);
                //  string msg = ConvertBytesToString(dataBuffer, dataBuffer.Length);

                string msg = System.Text.Encoding.UTF8.GetString(dataBuffer);
                MsgPair msgPair = dispatch(msg, serverSock);

                if (msgPair != null)
                    sendMsgToClient(msgPair.msg, serverSock, msgPair.type);
                currentIndex += DataLength + 12;
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

        MsgPair dispatch(string msg, Socket serverSock)
        {
            int separateIndex = msg.IndexOf("\r\n");
            if (separateIndex != -1)
            {
                string msgHead = msg.Substring(0, separateIndex);
                int index = msgIndex[msgHead];
                ServerMsgType msgType = (ServerMsgType)index;

                GlobalMessage globalMsg;
                //if (msgType.Equals(ServerMsgType.LOGINACK))
                //    globalMsg = GlobalMessage.LOGINACK;
                //else if (msgType.Equals(ServerMsgType.LOGOUTACK))
                //    globalMsg = GlobalMessage.LOGOUTACK;
                if (msgType.Equals(ServerMsgType.LOGINACK))
                {
                    string status = TextFollowing(msg, "STATUS:");
                    if (status.Equals("OK"))
                        globalMsg = GlobalMessage.LOGINACKOK;
                    else
                        globalMsg = GlobalMessage.LOGINACKFAILED;
                }
                else if (msgType.Equals(ServerMsgType.LOGOUTACK))
                {
                      string status = TextFollowing(msg, "STATUS:");
                    if (status.Equals("OK"))
                        globalMsg = GlobalMessage.LOGOUTACKOK;
                    else
                        globalMsg = GlobalMessage.LOGOUTACKFAILED;
                }
                else if (msgType.Equals(ServerMsgType.KEEPALIVE))
                    globalMsg = GlobalMessage.KEEPALIVE;
                else
                    globalMsg = GlobalMessage.DATA;
                serverPublishMessage(msg, serverSock, globalMsg);

                string newMsg = ServerMsgDelegate[index](msg, serverSock);
                if (newMsg == null)
                    return null;
         //       byte[] buffer = addHead(newMsg);

               // MsgPair msgPair = new MsgPair(buffer, globalMsg);
                 string flagStr = "-1-2-1-2-1-2-1-2";
                 string total = flagStr + newMsg;

                 MsgPair msgPair = new MsgPair(total, globalMsg);

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


        ///---------------------------------------------------------------------------------
        /// Sever消息处理
        ///---------------------------------------------------------------------------------
        string loginAck(string msg, Socket serverSock)
        {
            msg += "MYSQLIP:" + "192.168.10.203" + "\r\n";

            string status = TextFollowing(msg, "STATUS:");
            if (status.Equals("OK"))
            {
                string operatorLevel = TextFollowing(msg, "OPERATORLEVEL:");
                Fleck.IWebSocketConnection clientSock = myGlobals.ServerClient[serverSock];
                string operatorName = myGlobals.operatorData.getOperatorName(clientSock);
                myGlobals.operatorData.setOperatorLevel(operatorName, operatorLevel);
            }
            return msg;
        }
        string changePasswordAck(string msg, Socket sock)
        {
            return msg;
        }
        string setOperatorAck(string msg, Socket sock)
        {
            return msg;

        }
        string setOperatorNumberAck(string msg, Socket sock)
        {
            return msg;

        }
        string logoutAck(string msg, Socket sock)
        {
            return msg;

        }
        string createConfRoomAck(string msg, Socket sock)
        {
            string operatorNum = TextFollowing(msg, "OPERATORNUMBER:");
            string serverRoomId = TextFollowing(msg, "SERVERROOMID:");
            string status = TextFollowing(msg, "STATUS:");

            if (status.Equals("OK"))
            {
                foreach (var kv in myGlobals.myPlans.plans)
                {
                    if (kv.Key.Equals(operatorNum))
                        kv.Value.serverRoomId = serverRoomId;
                }

                foreach (var kv in myGlobals.myRooms.rooms)
                {
                    if (kv.Key.Equals(operatorNum))
                        kv.Value.serverRoomId = serverRoomId;
                }
            }
            else if (status.Equals("FAILED"))
            {
                myGlobals.myPlans.plans.Remove(operatorNum);
                myGlobals.myRooms.rooms.Remove(operatorNum);
            }
            return msg;

        }
        string newConfRoom(string msg, Socket sock)
        {
            return msg;

        }
        string stopConfRoomAck(string msg, Socket sock)
        {

            return msg;

        }
        string addConfMemberAck(string msg, Socket sock)
        {
            return msg;

        }
        string membering(string msg, Socket sock)
        {

            string number = TextFollowing(msg, "NUMBER:");
            if (myGlobals.gatewayData.ContainsNumber(number))
                return null;
            return msg;

        }

        string memberConnected(string msg, Socket sock)
        {
            string serverRoomId = TextFollowing(msg, "SERVERROOMID:");
            string localRoomId = TextFollowing(msg, "LOCALCONFROOMID:");
            string operatorName = TextFollowing(msg, "OPERATORNAME:");
            string number = TextFollowing(msg, "NUMBER:");



            // if (myGlobals.gatewaySip.sipNum.ContainsValue(number))
            if (myGlobals.gatewayData.ContainsNumber(number))
            {
                myGlobals.mre1.WaitOne();
                myGlobals.mre2.Reset();
                //foreach (KeyValuePair<string, string> kv in myGlobals.gatewaySip.sipNum)

                string type = myGlobals.gatewayData.getType(number);
                USER_INFO userInfo = new USER_INFO();
                userInfo.serverRoomId = serverRoomId;
                userInfo.type = type;
                USER_INFO[] userRecords = myGlobals.dbManager.GetUser(userInfo);

                // myGlobals.gatewaySip.active[type] = "1";
                myGlobals.gatewayData.gateway[type].isConnnected = true;
                myGlobals.gatewayData.gateway[type].serverRoomId = serverRoomId;
                myGlobals.mre2.Set();

                foreach (USER_INFO tmpUserInfo in userRecords)
                {
                    string msg2Client = "MEMBERCONNECTED\r\n"
                                        + "NUMBER:" + tmpUserInfo.phone + "\r\n"
                                        + "ISOPERATOR:0\r\n"
                                        + "OPERATORNAME:" + operatorName + "\r\n"
                                        + "LOCALCONFROOMID:" + localRoomId + "\r\n"
                                        + "SERVERROOMID:" + serverRoomId + "\r\n"
                                        + "ANSWERTIME:\r\n"
                                        + "ENABLEVIDEO:NO\r\n";
                    // byte[] bytes = System.Text.Encoding.UTF8.GetBytes(msg2Client);
                    //byte[] bytes = addHead(msg2Client);
                    //ServerUpdateMessage(bytes, sock, 2);
                    string flagStr = "-1-2-1-2-1-2-1-2";
                    string total = flagStr + msg2Client;
                    sendMsgToClient(total, sock, GlobalMessage.DATA);
                }
                return null;

            }
            return msg;

        }
        string memberCallFailed(string msg, Socket sock)
        {
            string serverRoomId = TextFollowing(msg, "SERVERROOMID:");
            string localRoomId = TextFollowing(msg, "LOCALCONFROOMID:");
            string operatorName = TextFollowing(msg, "OPERATORNAME:");

            string number = TextFollowing(msg, "NUMBER:");

            foreach (var kv in myGlobals.gatewayData.gateway)
            {
                if (kv.Value.sipNo == number)
                {
                    myGlobals.gatewayData.gateway[kv.Key].isOccupied = false;

                    string type = kv.Key;

                    USER_INFO userInfo = new USER_INFO();
                    userInfo.serverRoomId = serverRoomId;
                    userInfo.type = type;
                    USER_INFO[] userRecords = myGlobals.dbManager.GetUser(userInfo);

                    Dictionary<string, List<string>> tmpPhones = new Dictionary<string, List<string>>();
                    List<string> list = new List<string>();
                    foreach (USER_INFO tmpUserInfo in userRecords)
                    {
                        string msg2Client = "MEMBERCALLFAILED\r\n"
                                  + "CONFTYPE:0\r\n"
                                  + "OPERATORNAME:" + operatorName + "\r\n"
                                  + "ISOPERATOR:0\r\n"
                                  + "NUMBER:" + tmpUserInfo.phone + "\r\n"
                                  + "LOCALCONFROOMID:" + localRoomId + "\r\n"
                                  + "SERVERROOMID:" + serverRoomId + "\r\n"
                                  + "REASIONCODE:602\r\n";

                      //  byte[] bytes = addHead(msg2Client);
                        string flagStr = "-1-2-1-2-1-2-1-2";
                        string total = flagStr + msg2Client;
                        sendMsgToClient(total, sock, GlobalMessage.DATA);
                        list.Add(tmpUserInfo.phone);
                    }

                    tmpPhones.Add(type, list);
                    myGlobals.pocSendDeal.deleteMember(operatorName, localRoomId, serverRoomId, tmpPhones);
                }
                return null;
            }

            return msg;
        }
        string confRoomInfo(string msg, Socket sock)
        {

            string serverRoomId = TextFollowing(msg, "SERVERROOMID:");

            int i;
            i = msg.LastIndexOf("CONFMEMBER");
            int num = Convert.ToInt32(msg[i + 1]);
            int oldNum = num;

            USER_INFO userInfo = new USER_INFO();
            userInfo.serverRoomId = serverRoomId;
            USER_INFO[] userInfoArr = myGlobals.dbManager.GetUser(userInfo);

            foreach (USER_INFO u in userInfoArr)
            {
                string tmpStr = "CONFMEMBER" + num;
                msg += tmpStr + ":" + "123456;" + u.phone + ";TALKING;;" + u.phone + ";0;0;1;1;1;" + "\r\n";
                num++;
            }

            int newNum = num;
            msg = msg.Replace("CONFMEMBER" + oldNum, "CONFMEMBER" + newNum);
            return msg;

        }
        string operatorInfo(string msg, Socket sock)
        {
            return msg;

        }
        string kickConfMemberAck(string msg, Socket sock)
        {

            return msg;

        }
        string quitConfRoom(string msg, Socket sock)
        {

            string number = TextFollowing(msg, "NUMBER:");

            if (myGlobals.gatewayData.ContainsNumber(number))
            {
                string type = myGlobals.gatewayData.getType(number);
                myGlobals.gatewayData.gateway[type].isOccupied = false;
                myGlobals.gatewayData.gateway[type].isConnnected = false;
                myGlobals.gatewayData.gateway[type].serverRoomId = "0";
                return null;
            }

            foreach (var kv in myGlobals.myRooms.rooms)
            {
                if (kv.Value.sipNumbers.Contains(number))
                    kv.Value.sipNumbers.Remove(number);
            }

            return msg;

        }
        string serverReleaseConfRoom(string msg, Socket sock)
        {

            string serverRoomId = TextFollowing(msg, "SERVERROOMID:");
            string localRoomId = TextFollowing(msg, "LOCALCONFROOMID:");
            string operatorName = TextFollowing(msg, "OPERATORNAME:");
         //   string operatorNum = TextFollowing(msg, "OPERATORNUMBER:");

            foreach(var kv in myGlobals.myPlans.plans)
            {
                if (kv.Value.serverRoomId == serverRoomId)
                {
                    myGlobals.myPlans.plans.Remove(kv.Value.operatorNum);
                    break;
                }
            }

            foreach (var kv in myGlobals.myRooms.rooms)
            {
                if (kv.Value.serverRoomId == serverRoomId)
                {
                    myGlobals.myRooms.rooms.Remove(kv.Value.operatorNum);
                    break;
                }
            }


            USER_INFO userInfo = new USER_INFO();

            userInfo.localRoomId = localRoomId;
            userInfo.serverRoomId = serverRoomId;

            USER_INFO[] userArr = myGlobals.dbManager.GetUser(userInfo);

            if (userArr.Length > 0)
            {
                USER_INFO oldUser = new USER_INFO();
                oldUser.localRoomId = localRoomId;
                oldUser.serverRoomId = serverRoomId;

                USER_INFO newUser = new USER_INFO();
                newUser.localRoomId = "0";
                newUser.serverRoomId = "0";
                myGlobals.dbManager.UpdateUser(newUser, oldUser);
            }
            return msg;

        }
        string serverCreateConfRoom(string msg, Socket sock)
        {
            return msg;

        }
        string serverCreateIncomingConfRoom(string msg, Socket sock)
        {
            return msg;

        }
        string muteConfMemberAck(string msg, Socket sock)
        {
            return msg;

        }
        string unMuteConfMemberAck(string msg, Socket sock)
        {

            return msg;
        }
        string setConfBroadcastAck(string msg, Socket sock)
        {
            return msg;

        }
        string unSetConfBroadcastAck(string msg, Socket sock)
        {
            return msg;

        }
        string switchConfRoomAck(string msg, Socket sock)
        {
            return msg;

        }
        string setConfHostVideoAck(string msg, Socket sock)
        {
            return msg;

        }
        string keepAlive(string msg, Socket sock)
        {

            return msg;
        }
        string incomingCallQueueState(string msg, Socket sock)
        {
            return msg;

        }
        string setPriorNumberAck(string msg, Socket sock)
        {
            return msg;

        }
        string operatorNumberState(string msg, Socket sock)
        {
            return msg;

        }
        string operatorLogin(string msg, Socket sock)
        {
            return msg;

        }
        string operatorLogout(string msg, Socket sock)
        {
            return msg;

        }
        string opeartorNewState(string msg, Socket sock)
        {
            return msg;

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
