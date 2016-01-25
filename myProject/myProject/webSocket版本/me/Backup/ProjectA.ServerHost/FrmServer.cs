using System;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Communication.Sockets.Core;
using Communication.Sockets.Core.Server;
using log4net;
using log4net.Config;

namespace ProjectA.ServerHost
{
    public partial class FrmServer : Form
    {
        private static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        ServerTerminal m_ServerTerminal;
        private int messagesCount;
        public FrmServer()
        {
            InitializeComponent();
            
            XmlConfigurator.Configure();
        }

        private void cmdConnect_Click(object sender, EventArgs e)
        {
            try
            {
                listLog.Items.Clear();

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

        private void cmdClose_Click(object sender, EventArgs e)
        {
            closeTerminal();

            cmdConnect.Enabled = true;
            cmdClose.Enabled = false;
        }

        void m_Terminal_ClientDisConnected(Socket socket)
        {
            PublishMessage(listLog, string.Format("Client {0} has been disconnected!", socket.LocalEndPoint));
        }

        void m_Terminal_ClientConnected(Socket socket)
        {
            PublishMessage(listLog, string.Format("Client {0} has been connected!", socket.LocalEndPoint));
        }

        void m_Terminal_MessageRecived(Socket socket, byte[] buffer)
        {
            string message = ConvertBytesToString(buffer, buffer.Length);

            PublishMessage(listMessages, string.Format("Sockets: {0}", message));

            // Send Echo
            m_ServerTerminal.DistributeMessage(buffer);
        }


        private string ConvertBytesToString(byte[] bytes, int iRx)
        {
            char[] chars = new char[iRx + 1];
            System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
            d.GetChars(bytes, 0, iRx, chars, 0);
            string szData = new string(chars);

            return szData;
        }

        private void createTerminal(int alPort)
        {
            m_ServerTerminal = new ServerTerminal();
            
            m_ServerTerminal.MessageRecived += new TCPTerminal_MessageRecivedDel(m_Terminal_MessageRecived);
            m_ServerTerminal.ClientConnect += new TCPTerminal_ConnectDel(m_Terminal_ClientConnected);
            m_ServerTerminal.ClientDisconnect += new TCPTerminal_DisconnectDel(m_Terminal_ClientDisConnected);
            
            m_ServerTerminal.StartListen(alPort);
        }

        private void closeTerminal()
        {
            m_ServerTerminal.MessageRecived -= new TCPTerminal_MessageRecivedDel(m_Terminal_MessageRecived);
            m_ServerTerminal.ClientConnect -= new TCPTerminal_ConnectDel(m_Terminal_ClientConnected);
            m_ServerTerminal.ClientDisconnect -= new TCPTerminal_DisconnectDel(m_Terminal_ClientDisConnected);

            m_ServerTerminal.Close();
        }

        private void PublishMessage(ListBox listBox, string mes)
        {
            if (InvokeRequired)
            {
                BeginInvoke((ThreadStart) delegate { PublishMessage(listBox, mes); });
                return;
            }

            mes = string.Format("{0}: {1}", DateTime.Now.ToString("hh:mm:ss:ffff"), mes);
            listBox.Items.Add(mes);
            lblMessages.Text = messagesCount++.ToString();
            
            log.Debug(mes);
        }

        private void m_btnNew_Click(object sender, EventArgs e)
        {
            new FrmServer().Show();
        }

    }
}