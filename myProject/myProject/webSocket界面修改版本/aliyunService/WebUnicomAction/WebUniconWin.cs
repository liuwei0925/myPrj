using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace WebUnicomAction
{
    public partial class WebUniconWin : Form
    {
        public WebUniconWin()
        {
            InitializeComponent();
            webUrlStr = "";
            btnIndex = 0;
            webNum = "";
            webXml = "";
            webObject = new WebUnicomAction.UnicomService.WebServicePtyt();
            webObject.Url = "http://localhost:8098/services/WebServicePtyt?wsdl";
        }
        //属性
        private WebUnicomAction.UnicomService.WebServicePtyt webObject;//
        public string webUrlStr;//输入的IP地址
        private int btnIndex;//标示操作
        private string webNum;//号码参数
        private string webXml;//内容参数
        public string ActNumber;//号码参数,对外
        public string ActXml;//内容参数,对外
        //方法
        
        //组表
        public string queryGroupList()
        {
            string str1_Temp = null;
            try
            {
                if (!string.IsNullOrEmpty(webUrlStr))
                {
                    webObject.Url = "http://" + webUrlStr + ":8098/services/WebServicePtyt?wsdl";
                }
                str1_Temp = webObject.QueryGroupList();
            }
            catch (WebException e)
            {
                str1_Temp="1";
            }
            catch (System.Web.Services.Protocols.SoapException e)
            {
                str1_Temp = "1";
            }
            return str1_Temp;
        }
        //用户表
        public string queryUserList()
        {
            string str1_Temp = null;
            try
            {
                if (!string.IsNullOrEmpty(webUrlStr))
                {
                    webObject.Url = "http://" + webUrlStr + ":8098/services/WebServicePtyt?wsdl";
                }
                str1_Temp = webObject.QueryUserList();
            }
            catch (WebException e)
            {
                str1_Temp = "1";
            }
            catch (System.Web.Services.Protocols.SoapException e)
            {
                str1_Temp = "1";
            }
            return str1_Temp;
        }
        //组内用户表
        public string queryGroupUserList(string groupNum)
        {
            string str1_Temp = null;
            try
            {
                if (!string.IsNullOrEmpty(webUrlStr))
                {
                    webObject.Url = "http://" + webUrlStr + ":8098/services/WebServicePtyt?wsdl";
                }
                str1_Temp = webObject.QueryGroupUserList(groupNum);
            }
            catch (WebException e)
            {
                str1_Temp = "1";
            }
            catch (System.Web.Services.Protocols.SoapException e)
            {
                str1_Temp = "1";
            }
            return str1_Temp;
        }
        //增加组
        public string addGroup(string groupNum, string groupXml)
        {
            string str1_Temp = null;
            try
            {
                if (!string.IsNullOrEmpty(webUrlStr))
                {
                    webObject.Url = "http://" + webUrlStr + ":8098/services/WebServicePtyt?wsdl";
                }
                str1_Temp = webObject.AddGroup(groupNum, groupXml);
            }
            catch (WebException e)
            {
                str1_Temp = "1";
            }
            catch (System.Web.Services.Protocols.SoapException e)
            {
                str1_Temp = "1";
            }
            return str1_Temp;
        }
        //修改组
        public string modifyGroup(string groupNum, string groupXml)
        {
            string str1_Temp = null;
            try
            {
                if (!string.IsNullOrEmpty(webUrlStr))
                {
                    webObject.Url = "http://" + webUrlStr + ":8098/services/WebServicePtyt?wsdl";
                }
                str1_Temp = webObject.ModifyGroup(groupNum, groupXml);
            }
            catch (WebException e)
            {
                str1_Temp = "1";
            }
            catch (System.Web.Services.Protocols.SoapException e)
            {
                str1_Temp = "1";
            }
            return str1_Temp;
        }
        //删除组
        public string deleteGroup(string groupNum)
        {
            string str1_Temp = null;
            try
            {
                if (!string.IsNullOrEmpty(webUrlStr))
                {
                    webObject.Url = "http://" + webUrlStr + ":8098/services/WebServicePtyt?wsdl";
                }
                str1_Temp = webObject.DeleteGroup(groupNum);
            }
            catch (WebException e)
            {
                str1_Temp = "1";
            }
            catch (System.Web.Services.Protocols.SoapException e)
            {
                str1_Temp = "1";
            }
            return str1_Temp;
        }
        //增加用户
        public string addUser(string userNum, string userXml)
        {
            string str1_Temp = null;
            try
            {
                if (!string.IsNullOrEmpty(webUrlStr))
                {
                    webObject.Url = "http://" + webUrlStr + ":8098/services/WebServicePtyt?wsdl";
                }
                str1_Temp = webObject.AddUser(userNum, userXml);
            }
            catch (WebException e)
            {
                str1_Temp = "1";
            }
            catch (System.Web.Services.Protocols.SoapException e)
            {
                str1_Temp = "1";
            }
            return str1_Temp;
        }
        //修改用户
        public string modifyUser(string userNum, string userXml)
        {
            string str1_Temp = null;
            try
            {
                if (!string.IsNullOrEmpty(webUrlStr))
                {
                    webObject.Url = "http://" + webUrlStr + ":8098/services/WebServicePtyt?wsdl";
                }
                str1_Temp = webObject.ModifyUser(userNum, userXml);
            }
            catch (WebException e)
            {
                str1_Temp = "1";
            }
            catch (System.Web.Services.Protocols.SoapException e)
            {
                str1_Temp = "1";
            }
            return str1_Temp;
        }
        //删除用户
        public string deleteUser(string userNum)
        {
            string str1_Temp = null;
            try
            {
                if (!string.IsNullOrEmpty(webUrlStr))
                {
                    webObject.Url = "http://" + webUrlStr + ":8098/services/WebServicePtyt?wsdl";
                }
                str1_Temp = webObject.DeleteUser(userNum);
            }
            catch (WebException e)
            {
                str1_Temp = "1";
            }
            catch (System.Web.Services.Protocols.SoapException e)
            {
                str1_Temp = "1";
            }
            return str1_Temp;
        }
        //用户权限查询
        public string UserAuthorityQuery(string userNum)
        {
            string str1_Temp = null;
            try
            {
                if (!string.IsNullOrEmpty(webUrlStr))
                {
                    webObject.Url = "http://" + webUrlStr + ":8098/services/WebServicePtyt?wsdl";
                }
                str1_Temp = webObject.UserAuthorityQuery(userNum);
            }
            catch (WebException e)
            {
                str1_Temp = "1";
            }
            catch (System.Web.Services.Protocols.SoapException e)
            {
                str1_Temp = "1";
            }
            return str1_Temp;
        }
        //用户权限修改
        public string userAuthorityModify(string userNum, string authorityXml)
        {
            string str1_Temp = null;
            try
            {
                if (!string.IsNullOrEmpty(webUrlStr))
                {
                    webObject.Url = "http://" + webUrlStr + ":8098/services/WebServicePtyt?wsdl";
                }
                str1_Temp = webObject.UserAuthorityModify(userNum, authorityXml);
            }
            catch (WebException e)
            {
                str1_Temp = "1";
            }
            catch (System.Web.Services.Protocols.SoapException e)
            {
                str1_Temp = "1";
            }
            return str1_Temp;
        }
        //组内成员分配
        public string groupUserDistribution(string groupNum, string userXml)
        {
            string str1_Temp = null;
            try
            {
                if (!string.IsNullOrEmpty(webUrlStr))
                {
                    webObject.Url = "http://" + webUrlStr + ":8098/services/WebServicePtyt?wsdl";
                }
                str1_Temp = webObject.GroupUserDistribution(groupNum, userXml);
            }
            catch (WebException e)
            {
                str1_Temp = "1";
            }
            catch (System.Web.Services.Protocols.SoapException e)
            {
                str1_Temp = "1";
            }
            return str1_Temp;
        }
        //根据号码查询当前GPS轨迹信息
        public string gpsCurQuery(string tempNum)
        {
            string str1_Temp = null;
            try
            {
                if (!string.IsNullOrEmpty(webUrlStr))
                {
                    webObject.Url = "http://" + webUrlStr + ":8098/services/WebServicePtyt?wsdl";
                }
                //str1_Temp = webObject.GpsCurQuery(tempNum);
            }
            catch (WebException e)
            {
                str1_Temp = "1";
            }
            catch (System.Web.Services.Protocols.SoapException e)
            {
                str1_Temp = "1";
            }
            return str1_Temp;
        }
        //根据号码查询历史GPS轨迹信息
        public string gpsHisQuery(string tempNum, string dtStart, string dtEnd)
        {
            string str1_Temp = null;
            try
            {
                if (!string.IsNullOrEmpty(webUrlStr))
                {
                    webObject.Url = "http://" + webUrlStr + ":8098/services/WebServicePtyt?wsdl";
                }
                //str1_Temp = webObject.GpsHisQuery(tempNum, dtStart, dtEnd);
            }
            catch (WebException e)
            {
                str1_Temp = "1";
            }
            catch (System.Web.Services.Protocols.SoapException e)
            {
                str1_Temp = "1";
            }
            return str1_Temp;
        }
        //根据号码查询用户详细信息
        public string userInfoQuery(string tempNum)
        {
            string str1_Temp = null;
            try
            {
                if (!string.IsNullOrEmpty(webUrlStr))
                {
                    webObject.Url = "http://" + webUrlStr + ":8098/services/WebServicePtyt?wsdl";
                }
                //str1_Temp = webObject.UserInfoQuery(tempNum);
            }
            catch (WebException e)
            {
                str1_Temp = "1";
            }
            catch (System.Web.Services.Protocols.SoapException e)
            {
                str1_Temp = "1";
            }
            return str1_Temp;
        }
        //根据号码查询用户状态信息
        public string userStatusQuery(string tempNum)
        {
            string str1_Temp = null;
            try
            {
                if (!string.IsNullOrEmpty(webUrlStr))
                {
                    webObject.Url = "http://" + webUrlStr + ":8098/services/WebServicePtyt?wsdl";
                }
                //str1_Temp = webObject.UserStatusQuery(tempNum);
            }
            catch (WebException e)
            {
                str1_Temp = "1";
            }
            catch (System.Web.Services.Protocols.SoapException e)
            {
                str1_Temp = "1";
            }
            return str1_Temp;
        }
        //将组的字符传装成xml格式,增加和修改
        public string GroupParameterToXml(string groupNum,string groupName,string groupParNum,int kindTemp)
        {
            string xmlReturn = null;
            string xmlHead = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>";
                string xmlStart = "<Root>";
                string xmlEnd = "</Root>";
            if (kindTemp==1)//增加
            {
                //<?xml version="1.0" encoding="utf-8"?>
                //<Root>
                //     <Group GroupTelno="组号码" GroupName="组名称" ParGrpTelno="父组号码"/>
                //</Root>
                string xmlData = "<Group " + "  GroupTelno=\"" + groupNum + "\"   GroupName=\"" + groupName + "\"   ParGrpTelno=\"" + groupParNum + "\"   />";
                xmlReturn = xmlHead + xmlStart + xmlData + xmlEnd;
            }
            else if (kindTemp == 2)//修改
            {
                //<?xml version="1.0" encoding="utf-8"?>
                //<Root>
                //     <Group GroupTelno="组号码" GroupName="组名称" />
                //</Root>
                string xmlData = "<Group " + "  GroupTelno=\"" + groupNum + "\"   GroupName=\"" + groupName +"\"   />";
                xmlReturn = xmlHead + xmlStart + xmlData + xmlEnd;
            }
            return xmlReturn;
        }
        //将用户的字符传装成xml格式,增加和修改
        public string UserParameterToXml(string userNum, string userName, string userType,string userDesc,string userGrpList, int kindTemp)
        {
            string xmlReturn = null;
            string xmlHead = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>";
                string xmlStart = "<Root>";
                string xmlEnd = "</Root>";
            if (kindTemp==1)//增加
            {
                //<?xml version="1.0" encoding="utf-8"?>
                //<Root>
                //     <User UserTelno="用户号码" UserName="用户名称" UserType="用户类型" Desc="用户描述"  GrpList="父组号码"/>
                //</Root>
                string xmlData = "<User " + "   UserTelno=\"" + userNum + "\"    UserName=\"" + userName 
                    + "\"    UserType=\"" + userType + "\"    Desc=\"" + userDesc+ "\"    GrpList=\"" + userGrpList+ "\"    />";
                xmlReturn = xmlHead + xmlStart + xmlData + xmlEnd;
            }
            else if (kindTemp == 2)//修改
            {
                //<?xml version="1.0" encoding="utf-8"?>
                //<Root>
                //     <User UserTelno="用户号码" UserName="用户名称" UserType="用户类型" Desc="用户描述"/>
                //</Root>
                string xmlData = "<User " + "   UserTelno=\"" + userNum + "\"    UserName=\"" + userName
                    + "\"    UserType=\"" + userType + "\"    Desc=\"" + userDesc + "\"    />";
                xmlReturn = xmlHead + xmlStart + xmlData + xmlEnd;
            }
            return xmlReturn;
        }
        //将用户权限的字符传装成xml格式
        public string UserAuthorityParameterToXml(string voiceTemp, string mediaTemp, string messageTemp, string gpsTemp)
        {
            //<?xml version="1.0" encoding="utf-8"?>
            //<Root>
            //      <Authority voice="语音权限" media="视频权限" message="即时信息" gps="定位"/>
            //</Root>
            string xmlReturn = null;
            string xmlHead = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>";
            string xmlStart = "<Root>";
            string xmlEnd = "</Root>";
            string xmlData = "<Authority " + "    voice=\"" + voiceTemp + "\"     media=\"" + mediaTemp
                    + "\"     message=\"" + messageTemp + "\"     gps=\"" + gpsTemp + "\"     />";
            xmlReturn = xmlHead + xmlStart + xmlData + xmlEnd;
            return xmlReturn;
        }
        //将组内用户分配的字符传装成xml格式
        public string UserDistributionParameterToXml(string userNum, string InOutTemp)
        {
            //<?xml version="1.0" encoding="utf-8"?>
            //<Root>
            //     <User UserTelno="用户号码" InOut="加入或退出" />
            //        …
            //     <User UserTelno="用户号码" InOut="加入或退出" />
            //</Root>
            //加入或退出: In表示加入组, Out 表示退出组
            string xmlReturn = null;
            string xmlHead = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>";
            string xmlStart = "<Root>";
            string xmlEnd = "</Root>";
            string xmlData = "<User " + "    UserTelno=\"" + userNum + "\"      InOut=\"" + InOutTemp + "\"      />";
            xmlReturn = xmlHead + xmlStart + xmlData + xmlEnd;
            return xmlReturn;
        }
        //将组内多个用户分配的字符传装成xml格式
        public string SomeUsersDistributionParameterToXml(string[] userNums, string InOutTemp)
        {
            //<?xml version="1.0" encoding="utf-8"?>
            //<Root>
            //     <User UserTelno="用户号码" InOut="加入或退出" />
            //        …
            //     <User UserTelno="用户号码" InOut="加入或退出" />
            //</Root>
            //加入或退出: In表示加入组, Out 表示退出组

            string xmlReturn = null;
            string xmlHead = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>";
            string xmlStart = "<Root>";
            string xmlEnd = "</Root>";
            string xmlData = null;
            if (userNums == null || userNums.Length <= 0)
                xmlData = "<User />";
            else
            {
                for (int i = 0; i < userNums.Length;i++ )
                {
                    string temNumber = userNums[i];
                    xmlData += "<User " + "    UserTelno=\"" + temNumber + "\"      InOut=\"" + InOutTemp + "\"      />";
                }
            }
            xmlReturn = xmlHead + xmlStart + xmlData + xmlEnd;
            return xmlReturn;
        }
        //按钮事件
        private void QueryGroupList_Click(object sender, EventArgs e)
        {
            string groupList = queryGroupList();
            richTextBox_Result.AppendText(groupList + "\r\n");
        }

        private void QueryUserList_Click(object sender, EventArgs e)
        {
            string groupList = queryUserList();
            richTextBox_Result.AppendText(groupList + "\r\n");
        }

        private void AddGroup_Click(object sender, EventArgs e)
        {
            btnIndex = 1;
            textBox_Condition.Text = "4588";
            string numContent = textBox_Condition.Text;
            string textTemp = GroupParameterToXml(numContent, numContent, "", (int)1);
            richTextBox_Xml.Text = textTemp;
            //richTextBox_Xml.Text = textTemp; "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>"
            //     + "<Root><Group GroupTelno=\"组号码\" GroupName=\"组名称\" ParGrpTelno=\"父组号码\" /></Root>";
        }

        private void AddUser_Click(object sender, EventArgs e)
        {
            btnIndex = 2;
            textBox_Condition.Text = "请输入用户号码";
            string numContent = textBox_Condition.Text;
            string textTemp = UserParameterToXml(numContent, numContent, "0", numContent, "", (int)1);
            richTextBox_Xml.Text = textTemp;
            //richTextBox_Xml.Text = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>"
            //    + "<Root><User UserTelno=\"用户号码\" UserName=\"用户名称\" UserType=\"用户类型\" Desc=\"用户描述\" GrpList=\"父组号码\" /></Root>";
        }

        private void ModifyGroup_Click(object sender, EventArgs e)
        {
            btnIndex = 3;
            //textBox_Condition.Text = "请输入组号码";
            //richTextBox_Xml.Text = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>"
            //    + "<Root><Group GroupTelno=\"组号码\" GroupName=\"组名称\" /></Root>";
            textBox_Condition.Text = "请输入组号码";
            string numContent = textBox_Condition.Text;
            string textTemp = GroupParameterToXml(numContent, numContent, "", (int)2);
            richTextBox_Xml.Text = textTemp;
        }

        private void ModifyUser_Click(object sender, EventArgs e)
        {
            btnIndex = 4;
            //textBox_Condition.Text = "请输入用户号码";
            //richTextBox_Xml.Text = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>"
            //    + "<Root><User UserTelno=\"用户号码\" UserName=\"用户名称\" UserType=\"用户类型\" Desc=\"用户描述\" /></Root>";
            textBox_Condition.Text = "请输入用户号码";
            string numContent = textBox_Condition.Text;
            string textTemp = UserParameterToXml(numContent, numContent, "0", numContent, "", (int)2);
            richTextBox_Xml.Text = textTemp;
        }

        private void DeleteGroup_Click(object sender, EventArgs e)
        {
            btnIndex = 5;
            textBox_Condition.Text = "请输入组号码";
            richTextBox_Xml.Text = "";
        }

        private void DeleteUser_Click(object sender, EventArgs e)
        {
            btnIndex = 6;
            textBox_Condition.Text = "请输入用户号码";
            richTextBox_Xml.Text = "";
        }

        private void QueryGroupUserList_Click(object sender, EventArgs e)
        {
            btnIndex = 7;
            textBox_Condition.Text = "请输入组号码";
            richTextBox_Xml.Text = "";
        }

        private void UserAuthorityQuery_Click(object sender, EventArgs e)
        {
            btnIndex = 8;
            textBox_Condition.Text = "请输入用户号码";
            richTextBox_Xml.Text = "";
        }

        private void GroupUserDistribution_Click(object sender, EventArgs e)
        {
            btnIndex = 9;
            //textBox_Condition.Text = "请输入组号码";
            //richTextBox_Xml.Text = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>"
            //    +"<Root><User UserTelno=\"用户号码\" InOut=\"加入或退出\" /></Root>";
            textBox_Condition.Text = "请输入组号码";
            string numContent = textBox_Condition.Text;
            string textTemp = UserDistributionParameterToXml(numContent, "In"); ;
            richTextBox_Xml.Text = textTemp;
        }

        private void UserAuthorityModify_Click(object sender, EventArgs e)
        {
            btnIndex = 10;
            //textBox_Condition.Text = "请输入用户号码";
            //richTextBox_Xml.Text = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>"
            //    + "<Root><Authority voice=\"语音权限\" media=\"视频权限\" message=\"即时信息\" gps=\"定位\" /></Root>";
            textBox_Condition.Text = "请输入用户号码";
            string numContent = textBox_Condition.Text;
            string textTemp = UserAuthorityParameterToXml("true", "true", "true", "true");
            richTextBox_Xml.Text = textTemp;
        }

        private void ConditionCompleted_Click(object sender, EventArgs e)
        {
            webNum = textBox_Condition.Text;
            webXml = richTextBox_Xml.Text;
            if (btnIndex==1)
            {
                string groupList = addGroup(webNum, webXml);
                richTextBox_Result.AppendText(groupList + "\r\n");
            }
            else if (btnIndex == 2)
            {
                string groupList = addUser(webNum, webXml);
                richTextBox_Result.AppendText(groupList+"\r\n");
            }
            else if (btnIndex == 3)
            {
                string groupList = modifyGroup(webNum, webXml);
                richTextBox_Result.AppendText(groupList + "\r\n");
            }
            else if (btnIndex == 4)
            {
                string groupList = modifyUser(webNum, webXml);
                richTextBox_Result.AppendText(groupList + "\r\n");
            }
            else if (btnIndex == 5)
            {
                string groupList = deleteGroup(webNum);
                richTextBox_Result.AppendText(groupList + "\r\n");
            }
            else if (btnIndex == 6)
            {
                string groupList = deleteUser(webNum);
                richTextBox_Result.AppendText(groupList + "\r\n");
            }
            else if (btnIndex == 7)
            {
                string groupList = queryGroupUserList(webNum);
                richTextBox_Result.AppendText(groupList + "\r\n");
            }
            else if (btnIndex == 8)
            {
                string groupList = UserAuthorityQuery(webNum);
                richTextBox_Result.AppendText(groupList + "\r\n");
            }
            else if (btnIndex == 9)
            {
                string groupList = groupUserDistribution(webNum,webXml);
                richTextBox_Result.AppendText(groupList + "\r\n");
            }
            else if (btnIndex == 10)
            {
                string groupList = userAuthorityModify(webNum, webXml);
                richTextBox_Result.AppendText(groupList + "\r\n");
            }
            btnIndex = 0;
        }

        private void webURLCompleted_Click(object sender, EventArgs e)
        {
            webUrlStr = textBox_webIP.Text;
        }

        private void userInfo_Click(object sender, EventArgs e)
        {
            webNum = textBox_Condition.Text;
            string groupList = userInfoQuery(webNum);
            richTextBox_Result.AppendText(groupList + "\r\n");
        }

        private void getGPS_Click(object sender, EventArgs e)
        {
            webNum = textBox_Condition.Text;
            string trm123 = gpsCurQuery(webNum);
            richTextBox_Result.AppendText(trm123 + "\r\n");
        }

        private void getState_Click(object sender, EventArgs e)
        {
            webNum = textBox_Condition.Text;
            string trm123 = userStatusQuery(webNum);
            richTextBox_Result.AppendText(trm123 + "\r\n");
        }

        private void textBox_webIP_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox_Xml_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_Condition_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
