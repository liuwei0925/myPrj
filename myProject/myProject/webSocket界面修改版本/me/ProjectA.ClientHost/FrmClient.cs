using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using Communication.Sockets.Core;
using Communication.Sockets.Core.Client;

namespace ProjectA.ClientHost
{
    public partial class FrmClient : Form
    {
        ClientTerminal m_ClientTerminal = new ClientTerminal();

        public FrmClient()
        {
            InitializeComponent();
            m_ClientTerminal.Connected += m_TerminalClient_Connected;
            m_ClientTerminal.Disconncted += m_TerminalClient_ConnectionDroped;
            m_ClientTerminal.MessageRecived += m_TerminalClient_MessageRecived;
        }

        private void cmdConnect_Click(object sender, EventArgs e)
        {
            try
            {
                string szIPSelected = txtIPAddress.Text;
                string szPort = txtPort.Text;
                int alPort = Convert.ToInt16(szPort, 10);
                IPAddress remoteIPAddress = IPAddress.Parse(szIPSelected);

                m_ClientTerminal.Connect(remoteIPAddress, alPort);
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }

        }

        private void cmdSendMessage_Click(object sender, EventArgs e)
        {
            try
            {
                string mes = txtData.Text;
                byte[] buffer = ConvertStrToBytes(mes);
                m_ClientTerminal.SendMessage(buffer);

            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }

        void m_TerminalClient_MessageRecived(Socket socket, byte[] bytes)
        {
            string message = ConvertBytesToString(bytes, bytes.Length);
            PublishMessage(listMessages, "Socket: " + message);
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            m_ClientTerminal.Close();
            
            cmdConnect.Enabled = true;
            cmdClose.Enabled = false;
        }

        private void m_btnNew_Click(object sender, EventArgs e)
        {
            new FrmClient().Show();
        }

        void m_TerminalClient_Connected(Socket socket)
        {
            byte[] buffer = ConvertStrToBytes("Hello There");
            m_ClientTerminal.SendMessage(buffer);

            cmdConnect.Enabled = false;
            cmdClose.Enabled = true;
            PublishMessage(listLog, "Connection Opened!");

            m_ClientTerminal.StartListen();
            PublishMessage(listLog, "Start listening to server messages");
        }

        void m_TerminalClient_ConnectionDroped(Socket socket)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new TCPTerminal_DisconnectDel(m_TerminalClient_ConnectionDroped), socket);
                return;
            }

            cmdConnect.Enabled = true;
            cmdClose.Enabled = false;

            PublishMessage(listLog, "Server has been disconnected!");
        }

        private byte[] ConvertStrToBytes(string p_mes)
        {
            return System.Text.Encoding.ASCII.GetBytes(p_mes);
        }
        
        private string ConvertBytesToString(byte[] bytes, int iRx)
        {
            char[] chars = new char[iRx + 1];
            System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
            d.GetChars(bytes, 0, iRx, chars, 0);
            string szData = new string(chars);

            return szData;
        }

        private void PublishMessage(ListBox listBox, string mes)
        {
            if (InvokeRequired)
            {
                BeginInvoke((ThreadStart)delegate { PublishMessage(listBox, mes); });
                return;
            }

            listBox.Items.Add(mes);
        }

    }
}