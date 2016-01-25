using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
namespace aliyunService
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            // Set the MaximizeBox to false to remove the maximize box.
            this.MaximizeBox = false;
            // Set the MinimizeBox to false to remove the minimize box.
            this.MinimizeBox = false;
            // Set the start position of the form to the center of the screen.
            this.StartPosition = FormStartPosition.CenterScreen;


            //myGlobals.msgBus.messageArrival += new MessageBus.onMessageArrival(messageReceived);
            myGlobals.recvDeal.publishMessage += new ReceiveDeal.onPublishMessage(messageReceived);
            myGlobals.msgBus.init();
        }

        public void PublishMessage(TextBox textBox, string mes)
        {
            if (InvokeRequired)
            {
                BeginInvoke((ThreadStart)delegate { PublishMessage(textBox, mes); });
                return;
            }
            else
            {
                string separateSymbols = "----------------------------------------------";
                mes = string.Format("{0}: \r\n{1}", DateTime.Now.ToString("HH:mm:ss"), mes);
                textBox.Text += mes + "\r\n" + separateSymbols + "\r\n";
            }
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

        public void messageReceived(string message, messageType msgType)
        {
            if (msgType.Equals(messageType.REGISTERED))
                updateLabelStatus(statusLabel, "注册成功");
            else if (msgType.Equals(messageType.KEEPALIVE))
                PublishMessage(keepaliveTextbox, message);
            else 
                PublishMessage(ReceiveTextBox, message);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //pocManager.msgBusService.sendMessage(Send.Text);
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            //pocManager.msgBusService.sendMessage(Send.Text);
            string msg = "TEST" + SendTextBox.Text;
            myGlobals.msgBus.sendMessage(msg);
        }

        private void clearReceive_Click(object sender, EventArgs e)
        {
            ReceiveTextBox.Text = "";
        }

        private void ClearSend_Click(object sender, EventArgs e)
        {
            SendTextBox.Text = "";
        }

        private void clearKeepAlive_Click(object sender, EventArgs e)
        {
            keepaliveTextbox.Text = "";
        }
    }
}
