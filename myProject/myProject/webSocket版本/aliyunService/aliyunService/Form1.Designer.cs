namespace aliyunService
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.aliyun = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPageReceive = new System.Windows.Forms.TabPage();
            this.clearReceive = new System.Windows.Forms.Button();
            this.ReceiveTextBox = new System.Windows.Forms.TextBox();
            this.tabPageSend = new System.Windows.Forms.TabPage();
            this.ClearSend = new System.Windows.Forms.Button();
            this.sendButton = new System.Windows.Forms.Button();
            this.SendTextBox = new System.Windows.Forms.TextBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.clearKeepAlive = new System.Windows.Forms.Button();
            this.keepaliveTextbox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.aliyun.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPageReceive.SuspendLayout();
            this.tabPageSend.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // aliyun
            // 
            this.aliyun.Controls.Add(this.tabControl2);
            this.aliyun.Controls.Add(this.groupBox1);
            this.aliyun.Location = new System.Drawing.Point(4, 22);
            this.aliyun.Name = "aliyun";
            this.aliyun.Padding = new System.Windows.Forms.Padding(3);
            this.aliyun.Size = new System.Drawing.Size(360, 400);
            this.aliyun.TabIndex = 0;
            this.aliyun.Text = "aliyun";
            this.aliyun.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPageReceive);
            this.tabControl2.Controls.Add(this.tabPageSend);
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(3, 87);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(354, 310);
            this.tabControl2.TabIndex = 1;
            // 
            // tabPageReceive
            // 
            this.tabPageReceive.Controls.Add(this.clearReceive);
            this.tabPageReceive.Controls.Add(this.ReceiveTextBox);
            this.tabPageReceive.Location = new System.Drawing.Point(4, 22);
            this.tabPageReceive.Name = "tabPageReceive";
            this.tabPageReceive.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageReceive.Size = new System.Drawing.Size(346, 284);
            this.tabPageReceive.TabIndex = 0;
            this.tabPageReceive.Text = "Receive";
            this.tabPageReceive.UseVisualStyleBackColor = true;
            // 
            // clearReceive
            // 
            this.clearReceive.Location = new System.Drawing.Point(259, 255);
            this.clearReceive.Name = "clearReceive";
            this.clearReceive.Size = new System.Drawing.Size(61, 23);
            this.clearReceive.TabIndex = 9;
            this.clearReceive.Text = "Clear";
            this.clearReceive.UseVisualStyleBackColor = true;
            this.clearReceive.Click += new System.EventHandler(this.clearReceive_Click);
            // 
            // ReceiveTextBox
            // 
            this.ReceiveTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReceiveTextBox.Location = new System.Drawing.Point(3, 3);
            this.ReceiveTextBox.Multiline = true;
            this.ReceiveTextBox.Name = "ReceiveTextBox";
            this.ReceiveTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ReceiveTextBox.Size = new System.Drawing.Size(340, 278);
            this.ReceiveTextBox.TabIndex = 4;
            // 
            // tabPageSend
            // 
            this.tabPageSend.Controls.Add(this.ClearSend);
            this.tabPageSend.Controls.Add(this.sendButton);
            this.tabPageSend.Controls.Add(this.SendTextBox);
            this.tabPageSend.Location = new System.Drawing.Point(4, 22);
            this.tabPageSend.Name = "tabPageSend";
            this.tabPageSend.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSend.Size = new System.Drawing.Size(346, 284);
            this.tabPageSend.TabIndex = 1;
            this.tabPageSend.Text = "Send";
            this.tabPageSend.UseVisualStyleBackColor = true;
            // 
            // ClearSend
            // 
            this.ClearSend.Location = new System.Drawing.Point(6, 245);
            this.ClearSend.Name = "ClearSend";
            this.ClearSend.Size = new System.Drawing.Size(61, 23);
            this.ClearSend.TabIndex = 8;
            this.ClearSend.Text = "Clear";
            this.ClearSend.UseVisualStyleBackColor = true;
            this.ClearSend.Click += new System.EventHandler(this.ClearSend_Click);
            // 
            // sendButton
            // 
            this.sendButton.Location = new System.Drawing.Point(262, 245);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(61, 23);
            this.sendButton.TabIndex = 7;
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // SendTextBox
            // 
            this.SendTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SendTextBox.Location = new System.Drawing.Point(3, 3);
            this.SendTextBox.Multiline = true;
            this.SendTextBox.Name = "SendTextBox";
            this.SendTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.SendTextBox.Size = new System.Drawing.Size(340, 278);
            this.SendTextBox.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.clearKeepAlive);
            this.tabPage1.Controls.Add(this.keepaliveTextbox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(346, 284);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "KeepAlive";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // clearKeepAlive
            // 
            this.clearKeepAlive.Location = new System.Drawing.Point(258, 256);
            this.clearKeepAlive.Name = "clearKeepAlive";
            this.clearKeepAlive.Size = new System.Drawing.Size(61, 22);
            this.clearKeepAlive.TabIndex = 9;
            this.clearKeepAlive.Text = "Clear";
            this.clearKeepAlive.UseVisualStyleBackColor = true;
            this.clearKeepAlive.Click += new System.EventHandler(this.clearKeepAlive_Click);
            // 
            // keepaliveTextbox
            // 
            this.keepaliveTextbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.keepaliveTextbox.Location = new System.Drawing.Point(3, 3);
            this.keepaliveTextbox.Multiline = true;
            this.keepaliveTextbox.Name = "keepaliveTextbox";
            this.keepaliveTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.keepaliveTextbox.Size = new System.Drawing.Size(340, 278);
            this.keepaliveTextbox.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.statusLabel);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(354, 84);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(69, 34);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(41, 12);
            this.statusLabel.TabIndex = 3;
            this.statusLabel.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "状态：";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.aliyun);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(368, 426);
            this.tabControl1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 426);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.aliyun.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPageReceive.ResumeLayout(false);
            this.tabPageReceive.PerformLayout();
            this.tabPageSend.ResumeLayout(false);
            this.tabPageSend.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage aliyun;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPageReceive;
        private System.Windows.Forms.TextBox ReceiveTextBox;
        private System.Windows.Forms.TabPage tabPageSend;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.TextBox SendTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox keepaliveTextbox;
        private System.Windows.Forms.Button clearReceive;
        private System.Windows.Forms.Button ClearSend;
        private System.Windows.Forms.Button clearKeepAlive;

    }
}

