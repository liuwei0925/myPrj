using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Communication.Sockets.Core;
using Communication.Sockets.Core.Server;
using Communication.Sockets.Core.Client;
using log4net;
using log4net.Config;
using System.Collections.Generic;
using System.Timers;

namespace Aviad.ProjectA.ServerHost
{
    

    public partial class FrmMain : Form
    {
        private static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //ClientTerminal clientTerminal = new ClientTerminal();
        private Dictionary<Socket, ClientTerminal> toServer = new Dictionary<Socket, ClientTerminal>();
        ServerTerminal toClient = new ServerTerminal();

        public Dictionary<Socket, Heartbeat> clientTimers = new Dictionary<Socket, Heartbeat>();
        public Dictionary<Socket, Heartbeat> serverTimers = new Dictionary<Socket, Heartbeat>();

        ClientMsgProcess clientMsgProcess;
        ServerMsgProcess serverMsgProcess;

        private int messagesCount;

        private PocSendDeal pocService;
 
        public FrmMain()
        {
            InitializeComponent();
            
            XmlConfigurator.Configure();
            // Define the border style of the form to a dialog box.
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            // Set the MaximizeBox to false to remove the maximize box.
            this.MaximizeBox = false;
            // Set the MinimizeBox to false to remove the minimize box.
            this.MinimizeBox = false;
            // Set the start position of the form to the center of the screen.
            this.StartPosition = FormStartPosition.CenterScreen;
            // Display the form as a modal dialog box.
           // this.ShowDialog();


            clientMsgProcess = new ClientMsgProcess();
            serverMsgProcess = new ServerMsgProcess();

            int status = myGlobals.dbManager.CreateAllTable(ServerHost2.Text, "psx", "psx", "psx");

            if (status == -1)
            {
                MessageBox.Show("网络连接失败");
                System.Environment.Exit(0);
                //return;
            }

            myGlobals.pocTypes.Add("MOBILE");
            myGlobals.pocTypes.Add("UNICOM");
            myGlobals.pocTypes.Add("TELECOM");
            
            myGlobals.pocReceiveDeal.publishMessage += new PocReceiveDeal.onPublishMessage(pubAliyunMessage);
            myGlobals.pocReceiveDeal.sendMsgToClient += new PocReceiveDeal.onSendMsgToClient(sendMsgToClient);
            myGlobals.pocReceiveDeal.sendMsgToServer += new PocReceiveDeal.onSendMsgToServer(sendMsgToServer);

            clientMsgProcess.sendMsgToClient += new ClientMsgProcess.onSendMsgToClient(sendMsgToClient2);
            clientMsgProcess.sendMsgToServer += new ClientMsgProcess.onSendMsgToServer(sendMsgToServer);
            clientMsgProcess.clientPublishMessage += new ClientMsgProcess.onClientPublishMessage(clientPubMessage);
            serverMsgProcess.sendMsgToClient += new ServerMsgProcess.onSendMsgToClient(sendMsgToClient);
            serverMsgProcess.serverPublishMessage += new ServerMsgProcess.onServerPublishMessage(serverPubMessage);
            serverMsgProcess.sendMsgToServer += new ServerMsgProcess.onSendMsgToServer(sendMsgToServer);


            myGlobals.gatewayData.Add("MOBILE", "121");
            myGlobals.gatewayData.Add("UNICOM", "");
            myGlobals.gatewayData.Add("TELECOM", "");

            myGlobals.messageBus.init();


        }

        public void pubAliyunMessage(string msg, GlobalMessage msgType)
        {
            if (msgType.Equals(GlobalMessage.REGISTERED))
                updateLabelStatus(msgBusLabel, "注册成功");
            else if (msgType.Equals(GlobalMessage.MSGBUSKEEPALIVE))
                PublishMessage(mbKeepaliveTextBox, msg);
            else
                PublishMessage(mbReceiveTextBox, msg);
        }

        public void clientPubMessage(string msg,Socket clientSock, GlobalMessage msgType)
        {
            if (msgType.Equals(GlobalMessage.KEEPALIVEACK))
            {
                clientTimers[clientSock].keepalive();
                PublishMessage(keepalive1, msg);
            }
            else
                PublishMessage(messageBox1, msg);
        }

        public void serverPubMessage(string msg,Socket serverSock, GlobalMessage msgType)
        {
            if (msgType.Equals(GlobalMessage.KEEPALIVE))
            {
                serverTimers[serverSock].keepalive();
                PublishMessage(keepalive2, msg);
            }
            else
                PublishMessage(messageBox2, msg);
        }


        public void updateLabelStatus(Label label, string mes)
        {
            if (InvokeRequired)
            {
                BeginInvoke((ThreadStart)delegate { updateLabelStatus(label, mes); });
                return;
            }
            else
                label.Text = mes + "\r\n";
        }

///---------------------------------------------------------------------------------
/// Client
///---------------------------------------------------------------------------------

        public void closeClient(Socket clientSock)
        {
            if (!myGlobals.ClientServer.ContainsKey(clientSock))
                return;
            Socket serverSock = null;
            if (myGlobals.ClientServer.ContainsKey(clientSock))
                serverSock = myGlobals.ClientServer[clientSock];
            if (toClient.m_clients.ContainsKey(clientSock))
                toClient.Close(clientSock);
            if (myGlobals.ClientServer.ContainsKey(clientSock))
                myGlobals.ClientServer.Remove(clientSock);
            if (serverSock != null)
            {
                if (myGlobals.ServerClient.ContainsValue(serverSock))
                    myGlobals.ServerClient.Remove(serverSock);
            }
            string operatorName = myGlobals.operatorData.getOperatorName(clientSock);

            if (operatorName != null)
            {
                if (myGlobals.operatorData.operators.ContainsKey(operatorName))
                    myGlobals.operatorData.operators.Remove(operatorName);
            }
            if (clientTimers.ContainsKey(clientSock))
            {
                clientTimers[clientSock].stop();
                clientTimers.Remove(clientSock);
            }

            long sock = clientSock.Handle.ToInt64();
            PublishMessage(logBox1, string.Format("Client {0}(socket:{1}) has been disconnected!", clientSock.LocalEndPoint, sock));
        }

        public void sendMsgToServer(byte[] bytes, Socket clientSock, GlobalMessage msgType)
        {
            Socket value;
            if (msgType.Equals(GlobalMessage.PACKETERROR))
            {
                closeClient(clientSock);
                return;
            }
            else if (msgType == GlobalMessage.LOGIN)
            {
                ClientTerminal c = new ClientTerminal();
                c.Connected += new TCPTerminal_ConnectDel(ServerConnected);
                c.MessageRecived += new TCPTerminal_MessageRecivedDel(ServerMessageRecived);
                c.Disconncted += new TCPTerminal_DisconnectDel(ServerConnectionDroped);
                //clientTerminal = c;

                createSocket(c);
                value = c.m_socClient;
                toServer.Add(value, c);

                myGlobals.ClientServer.Add(clientSock, value);
                myGlobals.ServerClient.Add(value, clientSock);
                connect2Server(c);

            }
            else
                value = myGlobals.ClientServer[clientSock];

            //if (clientMsgProcess.messageType == clientMsgType.KEEPALIVEACK)
            //    clientHeartbeats[key].keepalive();

            toServer[value].SendMessage(bytes);
        }

        private void listenConnection(object sender, EventArgs e)
        {
            try
            {
               // listLog.Items.Clear();
                logBox1.Clear();
                string szPort = txtPort.Text;
                int alPort = Convert.ToInt16(szPort, 10);

                createTerminal(alPort);

                cmdConnect.Enabled = false;
                cmdClose.Enabled = true;
            }
            catch (Exception se)
            {
                MessageBox.Show(se.Message);
            }

        }

        private void closeConnection(object sender, EventArgs e)
        {
            //closeTerminal();

            //cmdConnect.Enabled = true;
            //cmdClose.Enabled = false;
        }

        public void clientTimeout(Socket clientSock)
        {
           // closeTerminal(key);
           // clientHeartbeats.Remove(key);
            closeClient(clientSock);
            Socket serverSock = myGlobals.ClientServer[clientSock];
            closeServer(serverSock);
        }

        void ClientMessageRecived(Socket socket, byte[] buffer)
        {
           // long key = socket.Handle.ToInt64();
            //analysisClient(buffer, key);
            clientMsgProcess.analyse(buffer, socket);
        }

        void ClientConnected(Socket socket)
        {
            long sock = socket.Handle.ToInt64();
            PublishMessage(logBox1, string.Format("Client {0}(socket:{1}) has been connected!", socket.LocalEndPoint, sock));

            //long key = socket.Handle.ToInt64();
            //Heartbeat clientTimer = new Heartbeat();
            //clientTimer.timeout += new Heartbeat.timerDelegate(clientTimeout);
           // clientHeartbeats.Add(key, clientTimer);

        }

        void ClientDisConnected(Socket socket)
        {
            //Heartbeat heartbeat = new Heartbeat(key);
           // heartbeats.Add(key, heartbeat);
            //long key = socket.Handle.ToInt64();
            //if (clientHeartbeats.ContainsKey(key))
            //    clientHeartbeats.Remove(key);
            long sock = socket.Handle.ToInt64();

            if (myGlobals.ClientServer.ContainsKey(socket))
            {
                Socket serverSock = myGlobals.ClientServer[socket];
                //PublishMessage(logBox1, string.Format("Client {0}(socket:{1}) has been disconnected!", socket.LocalEndPoint, sock));
                closeClient(socket);
                closeServer(serverSock);
            }
            else
            {
                //PublishMessage(logBox1, string.Format("Client {0}(socket:{1}) has been disconnected!", socket.LocalEndPoint, sock));
                closeClient(socket);
            }
        }


        private void PublishMessage(TextBox textBox, string msg)
        {
            if (InvokeRequired)
            {
                BeginInvoke((ThreadStart)delegate { PublishMessage(textBox, msg); });
                return;
            }

            msg = string.Format("{0}: \r\n{1}", DateTime.Now.ToString("HH:mm:ss"), msg);
            // listBox.Items.Add(mes);
            string separateSymbols = "----------------------------------------------";
            textBox.Text += msg + "\r\n" + separateSymbols + "\r\n";
            log.Debug(msg);
        }



///---------------------------------------------------------------------------------
/// Server
///---------------------------------------------------------------------------------

        public void closeServer(Socket serverSock)
        {

            if (!myGlobals.ServerClient.ContainsKey(serverSock))
                return;
            Socket clientSock = null;
            if (myGlobals.ClientServer.ContainsKey(clientSock))
                clientSock = myGlobals.ServerClient[serverSock];

            if (toServer.ContainsKey(serverSock))
            {
                toServer[serverSock].Close();
                toServer.Remove(serverSock);
            }
            if (myGlobals.ClientServer.ContainsKey(clientSock))
                myGlobals.ClientServer.Remove(clientSock);
            if (myGlobals.ServerClient.ContainsValue(serverSock))
                myGlobals.ServerClient.Remove(serverSock);

            string operatorName = myGlobals.operatorData.getOperatorName(clientSock);
            if (operatorName != null)
            {
                if (myGlobals.operatorData.operators.ContainsKey(operatorName))
                    myGlobals.operatorData.Remove(operatorName);
            }
            if (toServer.ContainsKey(serverSock))
                toServer.Remove(serverSock);
            if (serverTimers.ContainsKey(serverSock))
            {
                serverTimers[serverSock].stop();
                serverTimers.Remove(serverSock);
            }

           // long sock = serverSock.Handle.ToInt64();
            PublishMessage(logBox2, string.Format("Server has been disconnected!"));
        }

        public void sendMsgToClient2(byte[] bytes, Socket clientSock, GlobalMessage msgType)
        {
            long sock = clientSock.Handle.ToInt64();
            toClient.DistributeMessage(bytes, clientSock);
            PublishMessage(logBox1, string.Format("Client {0}(socket:{1}) has been disconnected!", clientSock.LocalEndPoint, sock));
            toClient.Close(clientSock);
        }


        public void sendMsgToClient(byte[] bytes, Socket serverSock, GlobalMessage msgType)
        {

            Socket clientSock = myGlobals.ServerClient[serverSock];
            //long sock = clientSock.Handle.ToInt64();
            if (msgType == GlobalMessage.LOGINACKFAILED)
            {
                toClient.DistributeMessage(bytes, clientSock);
                closeServer(serverSock);
                closeClient(clientSock);
            }
            else if (msgType.Equals(GlobalMessage.LOGINACKOK))
            {
                Heartbeat cTimer = new Heartbeat();
                cTimer.timeout += new Heartbeat.timerDelegate(clientTimeout);
                cTimer.start();
                clientTimers.Add(clientSock, cTimer);
                toClient.DistributeMessage(bytes, clientSock);
            }
            else if (msgType == GlobalMessage.LOGOUTACKOK)
            {
                toClient.DistributeMessage(bytes, clientSock);
                closeServer(serverSock);
                closeClient(clientSock);
            }
            else
                toClient.DistributeMessage(bytes, clientSock);
        }

      

       

        public void createSocket(ClientTerminal c)
        {
            string szIPSelected = ServerHost2.Text;
            string szPort = txtPort2.Text;
            int alPort = Convert.ToInt16(szPort, 10);
            IPAddress remoteIPAddress = IPAddress.Parse(szIPSelected);
            c.createSocket(remoteIPAddress, alPort);
        }

        public void connect2Server(ClientTerminal s)
        {
            try
            {
                // toSever.Connect(remoteIPAddress, alPort);
                s.Connect();
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
                System.Environment.Exit(0);
            }

        }

        private void createTerminal(int alPort)
        {
           // toClient = new ServerTerminal();

            toClient.MessageRecived += new TCPTerminal_MessageRecivedDel(ClientMessageRecived);
            toClient.ClientConnect += new TCPTerminal_ConnectDel(ClientConnected);
            toClient.ClientDisconnect += new TCPTerminal_DisconnectDel(ClientDisConnected);
            
            toClient.StartListen(alPort);
        }

        //private void closeTerminal(Socket clientSock)
        //{
        //    //toClient.MessageRecived -= new TCPTerminal_MessageRecivedDel(ClientMessageRecived);
        //    //toClient.ClientConnect -= new TCPTerminal_ConnectDel(ClientConnected);
        //    //toClient.ClientDisconnect -= new TCPTerminal_DisconnectDel(ClientDisConnected);

        //    toClient.Close(clientSock);
        //}

        public void serverTimeout(Socket serverSock)
        {
            //toServer[value].Close();
           // serverHeartbeats.Remove(value);
            closeServer(serverSock);
            Socket clientSock = myGlobals.ServerClient[serverSock];
            closeClient(clientSock);
        }

        public void ServerMessageRecived(Socket serverSock, byte[] buffer)
        {
            //long value = socket.Handle.ToInt64();
            //long key = myGlobals.ServerClient[value];
            //analyseServer(buffer, value);
            serverMsgProcess.analyse(buffer, serverSock);
        }
     
        public void ServerConnected(Socket serverSock)
        {
           // byte[] buffer = ConvertStrToBytes("Hello There");
         //   toSever.SendMessage(buffer);
          //  long value = socket.Handle.ToInt64();

            //cmdConnect2.Enabled = false;
            //cmdClose2.Enabled = true;
            PublishMessage(logBox2, "Connection Opened!");

            //Heartbeat hb = new Heartbeat();
            //hb.timeout += new Heartbeat.timerDelegate(clientTimeout);
            //serverHeartbeats.Add(value, hb);

            //long key = socket.Handle.ToInt64();
            toServer[serverSock].StartListen();
            //clientTerminal.StartListen();
            //long key = socket.Handle.ToInt64();
            //m_clients[key].StartListen();
           // myGlobals.psxClients[key].StartListen();
            PublishMessage(logBox2, "Start listening to server messages");

            Heartbeat sTimer = new Heartbeat();
            sTimer.timeout += new Heartbeat.timerDelegate(serverTimeout);
            sTimer.start();
            serverTimers.Add(serverSock, sTimer);
        }

        public void ServerConnectionDroped(Socket serverSock)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new TCPTerminal_DisconnectDel(ServerConnectionDroped), serverSock);
                return;
            }

          //  long l = socket.Handle.ToInt64();
            //if (serverHeartbeats.ContainsKey(l))
            //    serverHeartbeats.Remove(l);
            cmdConnect2.Enabled = true;
            cmdClose2.Enabled = false;

            if (serverTimers.ContainsKey(serverSock))
                serverTimers.Remove(serverSock);

            Socket clientSock = myGlobals.ServerClient[serverSock];

            closeServer(serverSock);
            closeClient(clientSock);

            PublishMessage(logBox2, "Server has been disconnected!");
        }

///-----------------------------------------------------------------------------------------
///清除消息
///-----------------------------------------------------------------------------------------
        private void msgClear1_Click(object sender, EventArgs e)
        {
            messageBox1.Clear();
        }

        private void msgClear2_Click(object sender, EventArgs e)
        {
            messageBox2.Clear();

        }

        private void logClear1_Click(object sender, EventArgs e)
        {
            logBox1.Clear();
        }

        private void logClear2_Click(object sender, EventArgs e)
        {
            logBox2.Clear();
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            string msg = "TEST" + mbSendTextBox.Text;
            myGlobals.messageBus.sendMessage(msg);
        }

        private void keepaliveClear1_Click(object sender, EventArgs e)
        {
            keepalive1.Clear();
        }

        private void keepaliveClear2_Click(object sender, EventArgs e)
        {
            keepalive2.Clear();
        }

        private void keepaliveClear_Click(object sender, EventArgs e)
        {
            mbKeepaliveTextBox.Text = "";
        }

        private void sendClear_Click(object sender, EventArgs e)
        {
            mbSendTextBox.Text = "";
        }

        private void receiveClear_Click(object sender, EventArgs e)
        {
            mbReceiveTextBox.Text = "";
        }


    }
}