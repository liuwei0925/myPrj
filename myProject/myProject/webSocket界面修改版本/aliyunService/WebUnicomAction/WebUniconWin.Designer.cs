namespace WebUnicomAction
{
    partial class WebUniconWin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.textBox_webIP = new System.Windows.Forms.TextBox();
            this.button_webChange = new System.Windows.Forms.Button();
            this.textBox_Condition = new System.Windows.Forms.TextBox();
            this.button13 = new System.Windows.Forms.Button();
            this.richTextBox_Xml = new System.Windows.Forms.RichTextBox();
            this.richTextBox_Result = new System.Windows.Forms.RichTextBox();
            this.button14 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(22, 188);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(108, 27);
            this.button1.TabIndex = 0;
            this.button1.Text = "查询组";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.QueryGroupList_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(22, 242);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(108, 27);
            this.button2.TabIndex = 1;
            this.button2.Text = "用户列表";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.QueryUserList_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(151, 188);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(108, 27);
            this.button3.TabIndex = 3;
            this.button3.Text = "增加组";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.AddGroup_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(151, 242);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(108, 27);
            this.button4.TabIndex = 2;
            this.button4.Text = "增加用户";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.AddUser_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(281, 188);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(108, 27);
            this.button5.TabIndex = 5;
            this.button5.Text = "修改组";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.ModifyGroup_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(281, 242);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(108, 27);
            this.button6.TabIndex = 4;
            this.button6.Text = "修改用户";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.ModifyUser_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(410, 188);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(108, 27);
            this.button7.TabIndex = 7;
            this.button7.Text = "删除组";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.DeleteGroup_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(410, 242);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(108, 27);
            this.button8.TabIndex = 6;
            this.button8.Text = "删除用户";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.DeleteUser_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(542, 188);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(108, 27);
            this.button9.TabIndex = 9;
            this.button9.Text = "查询组内用户";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.QueryGroupUserList_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(542, 242);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(108, 27);
            this.button10.TabIndex = 8;
            this.button10.Text = "查询用户权限";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.UserAuthorityQuery_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(667, 188);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(108, 27);
            this.button11.TabIndex = 11;
            this.button11.Text = "组内成员分配";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.GroupUserDistribution_Click);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(667, 242);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(108, 27);
            this.button12.TabIndex = 10;
            this.button12.Text = "修改用户权限";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.UserAuthorityModify_Click);
            // 
            // textBox_webIP
            // 
            this.textBox_webIP.Location = new System.Drawing.Point(43, 12);
            this.textBox_webIP.Name = "textBox_webIP";
            this.textBox_webIP.Size = new System.Drawing.Size(414, 25);
            this.textBox_webIP.TabIndex = 12;
            this.textBox_webIP.TextChanged += new System.EventHandler(this.textBox_webIP_TextChanged);
            // 
            // button_webChange
            // 
            this.button_webChange.Location = new System.Drawing.Point(542, 11);
            this.button_webChange.Name = "button_webChange";
            this.button_webChange.Size = new System.Drawing.Size(200, 23);
            this.button_webChange.TabIndex = 13;
            this.button_webChange.Text = "设置web地址";
            this.button_webChange.UseVisualStyleBackColor = true;
            this.button_webChange.Click += new System.EventHandler(this.webURLCompleted_Click);
            // 
            // textBox_Condition
            // 
            this.textBox_Condition.Location = new System.Drawing.Point(43, 54);
            this.textBox_Condition.Name = "textBox_Condition";
            this.textBox_Condition.Size = new System.Drawing.Size(414, 25);
            this.textBox_Condition.TabIndex = 14;
            this.textBox_Condition.TextChanged += new System.EventHandler(this.textBox_Condition_TextChanged);
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(543, 53);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(200, 23);
            this.button13.TabIndex = 15;
            this.button13.Text = "条件提交";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.ConditionCompleted_Click);
            // 
            // richTextBox_Xml
            // 
            this.richTextBox_Xml.Location = new System.Drawing.Point(43, 102);
            this.richTextBox_Xml.Name = "richTextBox_Xml";
            this.richTextBox_Xml.Size = new System.Drawing.Size(700, 64);
            this.richTextBox_Xml.TabIndex = 16;
            this.richTextBox_Xml.Text = "";
            this.richTextBox_Xml.TextChanged += new System.EventHandler(this.richTextBox_Xml_TextChanged);
            // 
            // richTextBox_Result
            // 
            this.richTextBox_Result.Location = new System.Drawing.Point(12, 349);
            this.richTextBox_Result.Name = "richTextBox_Result";
            this.richTextBox_Result.Size = new System.Drawing.Size(775, 135);
            this.richTextBox_Result.TabIndex = 17;
            this.richTextBox_Result.Text = "";
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(22, 287);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(108, 27);
            this.button14.TabIndex = 18;
            this.button14.Text = "获取用户信息";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.userInfo_Click);
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(151, 287);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(108, 27);
            this.button15.TabIndex = 19;
            this.button15.Text = "获取GPS信息";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.getGPS_Click);
            // 
            // button16
            // 
            this.button16.Location = new System.Drawing.Point(281, 287);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(108, 27);
            this.button16.TabIndex = 20;
            this.button16.Text = "获取用户状态";
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.getState_Click);
            // 
            // WebUniconWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 496);
            this.Controls.Add(this.button16);
            this.Controls.Add(this.button15);
            this.Controls.Add(this.button14);
            this.Controls.Add(this.richTextBox_Result);
            this.Controls.Add(this.richTextBox_Xml);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.textBox_Condition);
            this.Controls.Add(this.button_webChange);
            this.Controls.Add(this.textBox_webIP);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "WebUniconWin";
            this.Text = "WebUniconWin";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.TextBox textBox_webIP;
        private System.Windows.Forms.Button button_webChange;
        private System.Windows.Forms.TextBox textBox_Condition;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.RichTextBox richTextBox_Xml;
        private System.Windows.Forms.RichTextBox richTextBox_Result;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button16;

    }
}