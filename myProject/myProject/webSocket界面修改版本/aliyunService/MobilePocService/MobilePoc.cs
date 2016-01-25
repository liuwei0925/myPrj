using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading;
using System.Collections;
using System.Security;

namespace MobilePocService
{
    public struct Account
    {
        public string seqno;
        public string uri;
        public string password;
        public string ip;
        public int port;
    }

    public struct AuthorizationReturnInfo
    {
        public string identify;
        public string enid;
        public string seqno;
    }

    public struct MobilePocGroupInfo
    {
        public string groupid;
        public string groupName;
        public string groupNickname;
        public string groupType;
        public string groupLevel;
        public string groupUri;
    }

    public struct MobilePocUserInfo
    {
        public string userName;
        public string userUri;
        public string phone_number;
        public string priority;
    }

    public struct MobilePocGroupUser
    {
        public List<string> users;
    }

    public class PocService
    {
        //异常定义

        public static int SocketNull = -1;
        public static int SocketException = -2;
        public static int IndexException = -3;
        public static int ParsingError = -4;
        public static int XmlLoadError = -5;

        public Account account;
        public AuthorizationReturnInfo authReturnInfo;
        public MobilePocGroupInfo groupInfo;
        public MobilePocUserInfo userInfo;
        public MobilePocGroupUser groupUser;

        public PocService()
        {
            account = new Account();
            authReturnInfo = new AuthorizationReturnInfo();
            groupInfo = new MobilePocGroupInfo();
            userInfo = new MobilePocUserInfo();
            groupUser = new MobilePocGroupUser();
            account.seqno = "123";
            authReturnInfo.seqno = "123";
        }

        // This method requests the home page content for the specified server. 

        public int Receive(Socket socket, byte[] buffer, int offset, int size, int timeout)
        {
            int startTickCount = Environment.TickCount;
            int received = 0;  // how many bytes is already received
            int bytesCount = 0;
            do
            {
                if (Environment.TickCount > startTickCount + timeout)
                    throw new Exception("receiveTimeout");
                try
                {
                    bytesCount = socket.Receive(buffer, offset + received, size - received, SocketFlags.None);
                    received += bytesCount;
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.WouldBlock ||
                        ex.SocketErrorCode == SocketError.IOPending ||
                        ex.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
                    {
                        // socket buffer is probably empty, wait and try again
                        Thread.Sleep(30);
                    }
                    if (ex.SocketErrorCode == SocketError.ConnectionReset)
                    {
                        break;
                    }
                    if (ex.SocketErrorCode == SocketError.HostDown)
                    {

                    }
                    else
                        throw ex; // any serious error occurr
                }

            } while (bytesCount > 0);
            return received;
        }


        public int Send(Socket socket, byte[] buffer, int offset, int size, int timeout)
        {
            int startTickCount = Environment.TickCount;
            int sent = 0;  // how many bytes is already sent
            do
            {
                if (Environment.TickCount > startTickCount + timeout)
                    throw new Exception("SendTimeout");

                try
                {
                    sent += socket.Send(buffer, offset + sent, size - sent, SocketFlags.None);
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.WouldBlock ||
                        ex.SocketErrorCode == SocketError.IOPending ||
                        ex.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
                    {
                        // socket buffer is probably full, wait and try again
                        Thread.Sleep(30);
                    }
                    else
                        throw ex;  // any serious error occurr
                }
            } while (sent < size);
            return 0;
        }

        public int SocketSendReceive(string xml, ref string result)
        {
            //string server = "221.130.33.220";
            //int port = 8090;
            //string server = "112.33.0.187";
            //int port = 19998;
            string server = account.ip;
            int port = account.port;
            string request = "";
            Byte[] tmp = Encoding.UTF8.GetBytes(xml);
            request = "GET /Provisioning/ClientServlet HTTP/1.1\r\n";
            request += "Host:";
            request += server;
            request += "\r\n";
            request += "Content-Type: application/x-www-form-urlencoded\r\n";
            request += "Content-Length: ";
            // request += xml.Length.ToString();
            request += tmp.Length;
            request += "\r\n\r\n";
            request += xml;
            request += "\r\n\r\n";

            //Byte[] bytesSent = Encoding.ASCII.GetBytes(request);
            Byte[] bytesSent = Encoding.UTF8.GetBytes(request);
            Byte[] bytesReceived = new Byte[2048];

            string page = "";
            // Create a socket connection with the specified server and port;
            Socket s = ConnectSocket(server, port);

            if (s == null)
                //return ("Connection failed");
                return SocketNull;
            // Send request to the server.
            int sendTimeout = 10000;
            int receiveTimeout = 10000;

            try
            { // sends the text with timeout 10s
                Send(s, bytesSent, 0, bytesSent.Length, sendTimeout);
            }
            catch (Exception ex)
            {
                return SocketException;
            }

            try
            { // receive data with timeout 10s
                int bytesLen = Receive(s, bytesReceived, 0, bytesReceived.Length, receiveTimeout);
                page = Encoding.UTF8.GetString(bytesReceived, 0, bytesLen);
            }
            catch (Exception ex)
            {
                return SocketException;
            }

            int i;
            i = page.IndexOf('<');
            if (i == -1)
                return IndexException;
            result = page.Substring(i, page.Length - i);

            s.Shutdown(SocketShutdown.Both);
            s.Close();
            Console.WriteLine(result);

            return 0;
        }

        public bool SocketConnected(Socket s)
        {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }
        public Socket ConnectSocket(string server, int port)
        {
            Socket s = null;
            IPAddress[] ips;

            ips = Dns.GetHostAddresses(server);

            // Loop through the AddressList to obtain the supported AddressFamily. This is to avoid 
            // an exception that occurs when the host IP Address is not compatible with the address family 
            // (typical in the IPv6 case). 
            //  foreach (IPAddress address in hostEntry.AddressList)
            foreach (IPAddress address in ips)
            {
                IPEndPoint ipe = new IPEndPoint(address, port);
                Socket tempSocket =
                    new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                tempSocket.Connect(ipe);

                if (tempSocket.Connected)
                {
                    s = tempSocket;
                    break;
                }
                else
                {
                    continue;
                }
            }
            return s;
        }




        public List<string> xmlParsing(string xmlString, string nodeName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);
            string xPath = "//*";
            //XmlNode root = xmlDoc.DocumentElement;
            XmlNodeList nodes = xmlDoc.SelectNodes(xPath);
            List<string> result = new List<string>();

            foreach (XmlNode node in nodes)
            {
                if (node.Name == nodeName)
                {
                    if (nodeName == "user")
                    {
                        string target = node.Attributes["mobile"].Value;
                        ParsingError = 0;
                        result.Add(target);

                    }
                    else if (nodeName == "enid" || nodeName == "identify")
                    {
                        string target = node.InnerText;
                        result.Add(target);
                        return result;
                    }
                    else if (nodeName == "group")
                    {
                        string target = node.Attributes["groupUri"].Value;
                        result.Add(target);
                        return result;
                    }
                    else if (nodeName == "response")
                    {
                        string target = node.Attributes["code"].Value;
                        result.Add(target);
                        return result;
                    }
                }
            }
            return result;
        }

        public string getOperationStatus(string xmlString)
        {
            string OperationStatus;
            string nodeName = "response";
            List<string> tmp = new List<string>();

            tmp = xmlParsing(xmlString, nodeName);
            OperationStatus = tmp[0].ToString();
            return OperationStatus;
        }

        //鉴权
        public int Authorization()
        {
            string hash;
            using (MD5 md5Hash = MD5.Create())
            {
                Md5Hash md5 = new Md5Hash();
                hash = md5.GetMd5Hash(md5Hash, account.password);

                Console.WriteLine("The MD5 hash of " + account.password + " is: " + hash + ".");

                Console.WriteLine("Verifying the hash...");

                if (md5.VerifyMd5Hash(md5Hash, account.password, hash))
                {
                    Console.WriteLine("The hashes are the same.");
                }
                else
                {
                    Console.WriteLine("The hashes are not same.");
                }
            }
            string xml = string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<request target=\"Authorization\" opercode=\"1\" seqno=\"{0}\">" +
                "<uri>{1}</uri>" +
                "<password encryptType=\"MD5\">{2}</password>" +
                "<ver>2.0</ver>" +
                "</request>", account.seqno, account.uri, hash);

            string result = "";
            int operationStatus;
            operationStatus = SocketSendReceive(xml, ref result);
            if (operationStatus < 0)
                return operationStatus;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);

            try
            {
                if (!(getOperationStatus(result) == "200"))
                {
                    operationStatus = -3;
                    return operationStatus;
                    //return -3;
                }

                string NodeName;
                NodeName = "identify";
                List<string> tmp1 = new List<string>();
                tmp1 = xmlParsing(result, NodeName);
                authReturnInfo.identify = tmp1[0].ToString();

                NodeName = "enid";
                List<string> tmp2 = new List<string>();
                tmp2 = xmlParsing(result, NodeName);
                authReturnInfo.enid = tmp2[0].ToString();
            }
            catch (NullReferenceException e)
            {
                return ParsingError;
            }
            catch (XmlException e)
            {
                return XmlLoadError;
            }
            return operationStatus;
        }

        //1.获取指定群组的所有用户
        public int GetUserList()
        {
            int operationStatus;
            // AuthorizationReturnInfo tmp = new AuthorizationReturnInfo();
            //operationStatus = Authorization(account, ref tmp);
            operationStatus = Authorization();
            // authReturnInfo = tmp;
            // MobilePocGroupInfo = tmpMobilePocGroupInfo;
            if (operationStatus < 0)
                return operationStatus;

            string xml = string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<request target=\"GetGroupList\" opercode=\"2\" seqno=\"{0}\">" +
                "<identify>{1}</identify>" +
                "<enid>{2}</enid>" +
                "</request>", authReturnInfo.seqno, authReturnInfo.identify, authReturnInfo.enid);

            string result = "";
            //  int operationCode;
            operationStatus = SocketSendReceive(xml, ref result);
            if (operationStatus < 0)
                return operationStatus;
            try
            {
                if (!(getOperationStatus(result) == "200"))
                    return -3;

                string nodeName = "user";
                List<string> users = new List<string>();
                users = xmlParsing(result, nodeName);
                groupUser.users = users;
            }
            catch (NullReferenceException e)
            {
                return ParsingError;
            }
            catch (XmlException e)
            {
                return XmlLoadError;
            }
            return 0;
        }

        //2.创建一个组
        public int CreateGroup()
        {
            //   int num = Int32.Parse(groupInfo.groupType);
            string xml = "";
            int operationStatus;
            // AuthorizationReturnInfo tmp = new AuthorizationReturnInfo();
            //operationStatus = Authorization(account, ref tmp);
            operationStatus = Authorization();
            // authReturnInfo = tmp;
            // MobilePocGroupInfo = tmpMobilePocGroupInfo;
            if (operationStatus < 0)
                return operationStatus;
            //    return -1;

            xml = string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<request target=\"CreateGroup\" opercode=\"5\" seqno=\"{0}\">" +
                "<identify>{1}</identify>" +
                "<enid>{2}</enid>" +
                "<group groupName=\"{3}\" groupNickname=\"{4}\" groupType=\"{5}\"  level=\"{6}\">" +

              //  "<user userUri=\"{7}\" userName=\"{8}\" isOwner=\"{9}\" priority=\"{10}\"/>" +
                "</group>" +
                "</request>", authReturnInfo.seqno,
                                authReturnInfo.identify,
                                    authReturnInfo.enid,
                                        groupInfo.groupName,
                                            groupInfo.groupNickname,
                                                groupInfo.groupType,
                                                    groupInfo.groupLevel);

                                                            //userInfo.userUri,
                                                            //    userInfo.userName,
                                                            //        "1",
                                                            //            userInfo.priority);

            string result = "";
            //  int operationCode;
            operationStatus = SocketSendReceive(xml, ref result);
            if (operationStatus < 0)
                return operationStatus;

            try
            {
                if (!(getOperationStatus(result) == "200"))
                    return -3;

                string nodeName;
                nodeName = "group";
                List<string> tmp = new List<string>();
                tmp = xmlParsing(result, nodeName);
                groupInfo.groupUri = tmp[0].ToString();
            }
            catch (NullReferenceException e)
            {
                return ParsingError;
            }
            catch (XmlException e)
            {
                return XmlLoadError;
            }
            return 0;
        }

        //3.删除一个群组
        public int DelGroup()
        {
            int operationStatus;
            //  AuthorizationReturnInfo tmp = new AuthorizationReturnInfo();
            //  operationStatus = Authorization(account, ref tmp);
            operationStatus = Authorization();
            //    authReturnInfo = tmp;
            if (operationStatus < 0)
                return operationStatus;

            int i = groupInfo.groupUri.IndexOf('&');
            string tmpGroupUri = groupInfo.groupUri.Substring(0, i + 1) + "amp;" + groupInfo.groupUri.Substring(i + 1);
            string xml = string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<request target=\"DelGroup\" opercode=\"9\" seqno=\"{0}\">" +
                "<identify>{1}</identify>" +
                "<enid>{2}</enid>" +
                "<group groupUri=\"{3}\" groupType=\"{4}\"/>" +
                "</request>", authReturnInfo.seqno, authReturnInfo.identify, authReturnInfo.enid, tmpGroupUri, groupInfo.groupType);

            string result = "";
            //  int operationCode;
            operationStatus = SocketSendReceive(xml, ref result);
            if (operationStatus < 0)
                return operationStatus;

            try
            {
                if (!(getOperationStatus(result) == "200"))
                    return -3;
            }
            catch (NullReferenceException e)
            {
                return ParsingError;
            }
            catch (XmlException e)
            {
                return XmlLoadError;
            }
            return 0;
        }

        //4.添加群组成员
        public int AddGroupMember()
        {
            int operationStatus;
            //  AuthorizationReturnInfo tmp = new AuthorizationReturnInfo();
            // operationStatus = Authorization(account, ref tmp);
            operationStatus = Authorization();
            // authReturnInfo = tmp;
            if (operationStatus < 0)
                return operationStatus;
            int i = groupInfo.groupUri.IndexOf('&');
            string tmpGroupUri = groupInfo.groupUri.Substring(0, i + 1) + "amp;" + groupInfo.groupUri.Substring(i + 1);
            string xml = string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<request target=\"AddGroupMember\" opercode=\"7\" seqno=\"{0}\">" +
                "<identify>{1}</identify>" +
                "<enid>{2}</enid>" +
                "<group groupUri=\"{3}\" groupType=\"{4}\">" +
                "<user userUri=\"{5}\" userName=\"{6}\"  priority=\"{7}\"/>" +
                "</group>" +
                "</request>", authReturnInfo.seqno, authReturnInfo.identify, authReturnInfo.enid, tmpGroupUri, groupInfo.groupType, userInfo.userUri, userInfo.userName, userInfo.priority);

            string result = "";
            //  int operationCode;
            operationStatus = SocketSendReceive(xml, ref result);
            if (operationStatus < 0)
                return operationStatus;
            try
            {
                if (!(getOperationStatus(result) == "200"))
                    return -3;
            }
            catch (NullReferenceException e)
            {
                return ParsingError;
            }
            catch (XmlException e)
            {
                return XmlLoadError;
            }
            return 0;
        }

        //5.删除群组成员
        public int DelGroupMember()
        {
            int operationStatus;
            //   AuthorizationReturnInfo tmp = new AuthorizationReturnInfo();
            //operationStatus = Authorization(account, ref tmp);
            operationStatus = Authorization();
            //  authReturnInfo = tmp;

            if (operationStatus < 0)
                return operationStatus;
            int i = groupInfo.groupUri.IndexOf('&');
            string tmpGroupUri = groupInfo.groupUri.Substring(0, i + 1) + "amp;" + groupInfo.groupUri.Substring(i + 1);
            string xml = string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<request target=\"DelGroupMember\" opercode=\"8\" seqno=\"{0}\">" +
                "<identify>{1}</identify>" +
                "<enid>{2}</enid>" +
                "<group groupUri=\"{3}\" groupType=\"{4}\">" +
                "<user userUri=\"{5}\"/>" +
                "</group>" +
                "</request>", authReturnInfo.seqno, authReturnInfo.identify, authReturnInfo.enid, tmpGroupUri, groupInfo.groupType, userInfo.userUri);

            string result = "";
            //  int operationCode;
            operationStatus = SocketSendReceive(xml, ref result);
            if (operationStatus < 0)
                return operationStatus;
            try
            {
                if (!(getOperationStatus(result) == "200"))
                    return -3;
            }
            catch (NullReferenceException e)
            {
                return ParsingError;
            }
            catch (XmlException e)
            {
                return XmlLoadError;
            }
            return 0;

        }
    }
}
