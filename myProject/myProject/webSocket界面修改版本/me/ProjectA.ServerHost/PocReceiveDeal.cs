using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MysqlBasicClass;
using System.Net;
using System.Net.Sockets;
using Fleck;
namespace Aviad.ProjectA.ServerHost
{
    public class PocReceiveDeal
    {

        public delegate void onSendMsgToServer(byte[] buffer, Fleck.IWebSocketConnection sock, GlobalMessage type);
        public event onSendMsgToServer sendMsgToServer;

        public delegate void onSendMsgToClient(string msg, Socket sock, GlobalMessage type);
        public event onSendMsgToClient sendMsgToClient;

        public delegate void onPublishMessage(string msg, GlobalMessage type);
        public event onPublishMessage publishMessage;


        public PocReceiveDeal()
        {
            myGlobals.messageBus.aliyunMsgArrival += new MessageBus.onAliyunMsgArrival(aliyunMsgReceived);
        }

        public void aliyunMsgReceived(string msg, GlobalMessage msgType)
        {
            publishMessage(msg, msgType);
            if (msgType.Equals(GlobalMessage.REGISTERED))
                return;
            if (msgType.Equals(GlobalMessage.TEST))
                return;
            if (msgType.Equals(GlobalMessage.MSGBUSKEEPALIVE))
                return;
            string operation = TextFollowing(msg, "OPERATION:");

            if (operation == "ADDMEMBERACK")
                addAckDeal(msg);
            if (operation == "DELETEMEMBERACK")
                delAckDeal(msg);
        }

        public void addAckDeal(string msg)
        {
            string serverRoomId = TextFollowing(msg, "SERVERROOMID:");
            string localRoomId = TextFollowing(msg, "LOCALCONFROOMID:");
            string type = TextFollowing(msg, "TYPE:");
            string number = TextFollowing(msg, "NUMBER:");
            string status = TextFollowing(msg, "STATUS:");
            string operatorName = TextFollowing(msg, "OPERATORNAME:");


            Fleck.IWebSocketConnection clientSock = myGlobals.operatorData.getOperatorSock(operatorName);
            Socket serverSock = myGlobals.ClientServer[clientSock];

            foreach (var kv in myGlobals.myPlans.plans.Values)
            {
                if (kv.Contains(number))
                    serverRoomId = kv.serverRoomId;
            }

            if (status == "OK")
            {
                myGlobals.mre2.WaitOne();
                myGlobals.mre1.Reset();
               // if (myGlobals.gatewaySip.active[type] == "1")
                if (myGlobals.gatewayData.gateway[type].isConnnected)
                {
                    string msg2Client = "MEMBERCONNECTED\r\n"
                                             + "NUMBER:" + number + "\r\n"
                                             + "ISOPERATOR:0\r\n"
                                             + "OPERATORNAME:" + operatorName + "\r\n"
                                             + "LOCALCONFROOMID:" + localRoomId + "\r\n"
                                             + "SERVERROOMID:" + serverRoomId + "\r\n"
                                             + "ANSWERTIME:\r\n"
                                             + "ENABLEVIDEO:NO\r\n";
                    //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(msg2Client);
                   // byte[] bytes = addHead(msg2Client);
                    //ServerUpdateMessage(bytes, serverSock, 2);
                    string flagStr = "-1-2-1-2-1-2-1-2";
                    string total = flagStr + msg2Client;
                    sendMsgToClient(total, serverSock, GlobalMessage.ALIYUNDATA);
                }
                else
                {

                }
                USER_INFO oldUser = new USER_INFO();
                oldUser.phone = number;

                USER_INFO newUser = new USER_INFO();

                newUser.localRoomId = localRoomId;
                newUser.serverRoomId = serverRoomId;
                myGlobals.dbManager.UpdateUser(newUser, oldUser);
                myGlobals.mre1.Set();
            }
            else if (status == "FAILED")
            {
                //long l = myGlobals.ServerClient[sock];
                // string operatorName = myGlobals.operators[l];
                string msg2Client = "MEMBERCALLFAILED\r\n"
                                  + "CONFTYPE:0\r\n"
                                  + "OPERATORNAME:" + operatorName + "\r\n"
                                  + "ISOPERATOR:0\r\n"
                                  + "NUMBER:" + number + "\r\n"
                                  + "LOCALCONFROOMID:" + localRoomId + "\r\n"
                                  + "SERVERROOMID:" + serverRoomId + "\r\n"
                                  + "REASIONCODE:602\r\n";
                //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(msg2Client);
                //byte[] bytes = addHead(msg2Client);
                string flagStr = "-1-2-1-2-1-2-1-2";
                string total = flagStr + msg2Client;
                sendMsgToClient(total, serverSock, GlobalMessage.ALIYUNDATA);
               // ServerUpdateMessage(bytes, serverSock, 2);
            }

        }
        public void delAckDeal(string msg)
        {
            string serverRoomId = TextFollowing(msg, "SERVERROOMID:");
            string localRoomId = TextFollowing(msg, "LOCALCONFROOMID:");
            string type = TextFollowing(msg, "TYPE:");
            string number = TextFollowing(msg, "NUMBER:");
            string status = TextFollowing(msg, "STATUS:");
            string operatorName = TextFollowing(msg, "OPERATORNAME:");

            Fleck.IWebSocketConnection clientSock = myGlobals.operatorData.getOperatorSock(operatorName);
            Socket serverSock = myGlobals.ClientServer[clientSock];

            //foreach (var kv in myGlobals.myPlans.plans.Values)
            //{
            //    if (kv.Contains(number))
            //        serverRoomId = kv.serverRoomId;
            //}

            if (status == "OK")
            {

                USER_INFO oldUser = new USER_INFO();
                USER_INFO newUser = new USER_INFO();

                oldUser.phone = number;
                newUser.localRoomId = "0";
                newUser.serverRoomId = "0";
                myGlobals.dbManager.UpdateUser(newUser, oldUser);



                string msg2Client = "QUITCONFROOM\r\n"
                                  + "ISOPERATOR:0\r\n"
                                  + "OPERATORNAME:" + operatorName + "\r\n"
                                  + "LOCALCONFROOMID:" + localRoomId + "\r\n"
                                  + "SERVERROOMID:" + serverRoomId + "\r\n"
                                  + "NUMBER:" + number + "\r\n";
                // byte[] bytes = System.Text.Encoding.UTF8.GetBytes(msg2Client);
               // byte[] bytes = addHead(msg2Client);
                string flagStr = "-1-2-1-2-1-2-1-2";
                string total = flagStr + msg2Client;
                //ServerUpdateMessage(bytes, serverSock, 2);
                sendMsgToClient(total, serverSock, GlobalMessage.ALIYUNDATA);

                if (myGlobals.gatewayData.gateway[type].isConnnected == true)
                {
                    USER_INFO tmpUserInfo = new USER_INFO();
                    tmpUserInfo.type = type;
                    tmpUserInfo.localRoomId = localRoomId;
                    tmpUserInfo.serverRoomId = serverRoomId;

                    USER_INFO[] userRecords = myGlobals.dbManager.GetUser(tmpUserInfo);

                    if (userRecords.Length == 0)
                    {
                        string sipNum = myGlobals.gatewayData.getNumber(type);
                        string msg2Server = "KICKCONFMEMBER\r\n"
                                          + "OPERATORNAME:" + operatorName + "\r\n"
                                          + "LOCALCONFROOMID:" + localRoomId + "\r\n"
                                          + "SERVERROOMID:" + serverRoomId + "\r\n"
                                          + "NUMBER:" + sipNum + "\r\n";

                        // byte[] byteMessage = System.Text.Encoding.UTF8.GetBytes(msg2Server);
                        byte[] buffer = addHead(msg2Server);
                        sendMsgToServer(buffer, clientSock, GlobalMessage.ALIYUNDATA);

                    }
                }

            }
        }

        public byte[] addHead(string message)
        {
            byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(message);
            byte[] PsxMsgFlag = new byte[] { 0xff, 0xfe, 0xff, 0xfe, 0xff, 0xfe, 0xff, 0xfe };

            byte[] bytesInt = BitConverter.GetBytes((Int32)IPAddress.HostToNetworkOrder((Int32)dataBytes.Length));

            byte[] fullBytes = new byte[PsxMsgFlag.Length + bytesInt.Length + dataBytes.Length];
            Array.Copy(PsxMsgFlag, 0, fullBytes, 0, PsxMsgFlag.Length);
            Array.Copy(bytesInt, 0, fullBytes, PsxMsgFlag.Length, bytesInt.Length);
            Array.Copy(dataBytes, 0, fullBytes, PsxMsgFlag.Length + bytesInt.Length, dataBytes.Length);
            return fullBytes;
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
