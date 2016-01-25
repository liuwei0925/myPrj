using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MobilePocService;
using WebUnicomAction;
using ClassLibrary2;
using System.Configuration;
using System.Collections.Specialized;

namespace aliyunService
{
    public class SendDeal
    {
        public delegate void msgDelegate(string message, int param);
        public event msgDelegate msgEvent;

        public string MobileSeqno;
        public string MobileUserName;
        public string MobilePassword;
        public string MobileUrlIp;
        public string MobileUrlPort;
        public string MobileGroupid;
        public string MobileGroupNum;


        public string TelecomUserName;
        public string TelecomPassword;
        public string TelecomDeviceId;
        public string TelecomUrlIp;
        public string UnicomGroupid;
        public string UnicomGroupNum;
        public string UnicomUrlIp;

        public string TelecomGroupid;
        public string TelecomGroupNum;

        public string msgBusIP;
        public string msgBusPort;

        PocService mobileManager;
        WebUniconWin unicomManager;
        telecomService telecomManager;

        public SendDeal()
        {
            readConfigFile();
            mobileManager = new PocService();
            unicomManager = new WebUniconWin();
            telecomManager = new telecomService();
        }

        public void readConfigFile()
        {

            var mobileSection = ConfigurationManager.GetSection("mobileSetting") as NameValueCollection;

            MobileUserName = mobileSection["userName"].ToString();
            MobilePassword = mobileSection["password"].ToString();
            MobileUrlIp = mobileSection["ip"].ToString();
            MobileUrlPort = mobileSection["port"].ToString();
            MobileSeqno = mobileSection["seqno"].ToString();

            MobileGroupNum = "C6165&126410001@4gpoc.com";

            var telecomSection = ConfigurationManager.GetSection("telecomSetting") as NameValueCollection;

            TelecomUserName = telecomSection["userName"].ToString();
            TelecomPassword = telecomSection["password"].ToString();
            TelecomUrlIp = telecomSection["ip"].ToString();
            TelecomDeviceId = telecomSection["deviceId"].ToString();
            TelecomGroupNum = "huucxb491874";

            UnicomGroupid = "52231216";
            var msgBusSection = ConfigurationManager.GetSection("messageBus") as NameValueCollection;

            msgBusIP = msgBusSection["ip"].ToString();
            msgBusPort = msgBusSection["port"].ToString();
           // register();
        }

        public void mobileAdd(string oneLine, string operatorName, string localRoomId, string serverRoomId)
        {
            Account tmpAccount;
            int RetStr = 0;
            //string groupNum = TextFollowing(oneLine, "GROUPNUM:", ";");

            //增加成员到组中


            tmpAccount.seqno = MobileSeqno;
            tmpAccount.uri = MobileUserName;
            tmpAccount.password = MobilePassword;
            tmpAccount.ip = MobileUrlIp;
            tmpAccount.port = Convert.ToInt32(MobileUrlPort);

            mobileManager.account = tmpAccount;

            MobilePocGroupInfo tmpGroupInfo = new MobilePocGroupInfo();

            tmpGroupInfo.groupType = "4";
            tmpGroupInfo.groupLevel = "3";
            tmpGroupInfo.groupUri = MobileGroupNum;
            mobileManager.groupInfo = tmpGroupInfo;

            MobilePocUserInfo userInfo = new MobilePocUserInfo();
            userInfo.priority = "2";


            int index = oneLine.LastIndexOf(';');
            string[] phones = oneLine.Substring(0, index).Split(';');
            int i;
            for (i = 0; i < phones.Length; i++)
            {
                //stQchatGroupMember.number_mem = tempUserInfo[i];
                userInfo.userName = phones[i];
                userInfo.userUri = phones[i] + "@4gpoc.com";
                mobileManager.userInfo = userInfo;
                RetStr = mobileManager.AddGroupMember();
                if (RetStr == 0)
                {
                    //SaveLogInfo.WriteLog("Mobile ADDMEMBERS user " + tempUserInfo[i] + " to groupNum " + sGroupNum + " is success.");
                    //SetText("POC Server:" + "移动添加成员" + tempUserInfo[i] + "成功" + "\r\n");

                    string msg = string.Empty;
                    msg += "OPERATION:ADDMEMBERACK" + "\r\n"
                         + "OPERATORNAME:" + operatorName + "\r\n"
                         + "SERVERROOMID:" + serverRoomId + "\r\n"
                         + "LOCALCONFROOMID:" + localRoomId + "\r\n"
                         + "TYPE:MOBILE" + "\r\n"
                         + "STATUS:OK" + "\r\n"
                         + "NUMBER:" + phones[i] + "\r\n";
                    myGlobals.msgBus.sendMessage(msg);
                }
                else
                {
                    //SaveLogInfo.WriteLog("Mobile ADDMEMBERS user " + tempUserInfo[i] + " to groupNum " + sGroupNum + " is fail.");
                    //SetText("POC Server:" + "移动添加成员" + tempUserInfo[i] + "失败" + "\r\n");
                    string msg = string.Empty;
                    msg += "OPERATION:ADDMEMBERACK" + "\r\n"
                         + "OPERATORNAME:" + operatorName + "\r\n"
                         + "SERVERROOMID:" + serverRoomId + "\r\n"
                         + "LOCALCONFROOMID:" + localRoomId + "\r\n"
                         + "TYPE:MOBILE" + "\r\n"
                         + "STATUS:FAILED" + "\r\n"
                         + "NUMBER:" + phones[i] + "\r\n";
                    myGlobals.msgBus.sendMessage(msg);
                }
            }

            //SetText("\r\n");
        }

        public void unicomAdd(string oneLine, string operatorName, string localRoomId, string serverRoomId)
        {
            //获得需要添加到组的用户信息
            // string groupid = TextFollowing(oneLine, "GROUPID:", ";");
            int index = oneLine.LastIndexOf(';');
            string[] phones = oneLine.Substring(0, index).Split(';');
            int i;

            string xmlTemp = unicomManager.SomeUsersDistributionParameterToXml(phones, "In");
            string retstring = unicomManager.groupUserDistribution(UnicomGroupid, xmlTemp);

            //  string tmpStr = string.Join(";", phones);

            if (string.IsNullOrEmpty(retstring)
                || retstring.Equals("分配组内用户操作成功.")
                || retstring.IndexOf("分配组内用户操作成功.") >= 0)
            {
                //添加成功
                //SaveLogInfo.WriteLog("unicom ADDMEMBERS users:" + userInfos
                //    + " to groupid: " + groupid + " is success.");
                //SetText("POC Server:" + "联通添加成员成功" + "\r\n");

                foreach (string phone in phones)
                {
                    string msg = string.Empty;
                    msg += "OPERATION:ADDMEMBERACK" + "\r\n"
                         + "OPERATORNAME:" + operatorName + "\r\n"
                         + "SERVERROOMID:" + serverRoomId + "\r\n"
                         + "LOCALCONFROOMID:" + localRoomId + "\r\n"
                         + "TYPE:UNICOM" + "\r\n"
                         + "STATUS:OK" + "\r\n"
                         + "NUMBER:" + phone + "\r\n";
                    myGlobals.msgBus.sendMessage(msg);
                }
            }
            else
            {
                foreach (string phone in phones)
                {
                    string msg = string.Empty;
                    msg += "OPERATION:ADDMEMBERACK" + "\r\n"
                         + "OPERATORNAME:" + operatorName + "\r\n"
                         + "SERVERROOMID:" + serverRoomId + "\r\n"
                         + "LOCALCONFROOMID:" + localRoomId + "\r\n"
                         + "TYPE:UNICOM" + "\r\n"
                         + "STATUS:FAILED" + "\r\n"
                         + "NUMBER:" + phone + "\r\n";
                    myGlobals.msgBus.sendMessage(msg);
                }
            }
            //{
            //    SaveLogInfo.WriteLog("unicom ADDMEMBERS users:" + userInfos
            //        + " to groupid: " + groupid + " is fail.");
            //    SetText("POC Server:" + "联通添加成员失败" + "\r\n");


        }

        public void telecomAdd(string oneLine, string operatorName, string localRoomId, string serverRoomId)
        {
            QChat_GroupMember stQchatGroupMember = new QChat_GroupMember();
            string RetStr = null;
            string groupNum = TelecomGroupNum;


            stQchatGroupMember.username = TelecomUserName;
            stQchatGroupMember.password = TelecomPassword;
            stQchatGroupMember.deviceID = TelecomDeviceId;
            stQchatGroupMember.urlIp = TelecomUrlIp;
            stQchatGroupMember.memberSize = 2;
            stQchatGroupMember.groupCode = groupNum;
            stQchatGroupMember.type_mem = 1;
            stQchatGroupMember.lebel = 4;

            int index = oneLine.LastIndexOf(';');
            string[] phones = oneLine.Substring(0, index).Split(';');
            int i;

            for (i = 0; i < phones.Length; i++)
            {
                stQchatGroupMember.number_mem = phones[i];
                RetStr = telecomManager.AddGroupMember(stQchatGroupMember);
                if ((!string.IsNullOrEmpty(RetStr))
                    && (RetStr.IndexOf("成功") > 0))
                {
                    string msg = string.Empty;
                    msg += "OPERATION:ADDMEMBERACK" + "\r\n"
                         + "OPERATORNAME:" + operatorName + "\r\n"
                         + "SERVERROOMID:" + serverRoomId + "\r\n"
                         + "LOCALCONFROOMID:" + localRoomId + "\r\n"
                         + "TYPE:TELECOM" + "\r\n"
                         + "STATUS:OK" + "\r\n"
                         + "NUMBER:" + phones[i] + "\r\n";
                    myGlobals.msgBus.sendMessage(msg);
                }
                else
                {
                    string msg = string.Empty;
                    msg += "OPERATION:ADDMEMBERACK" + "\r\n"
                         + "OPERATORNAME:" + operatorName + "\r\n"
                         + "SERVERROOMID:" + serverRoomId + "\r\n"
                         + "LOCALCONFROOMID:" + localRoomId + "\r\n"
                         + "TYPE:TELECOM" + "\r\n"
                         + "STATUS:FAILED" + "\r\n"
                         + "NUMBER:" + phones[i] + "\r\n";
                    myGlobals.msgBus.sendMessage(msg);
                }
            }

            //SetText("\r\n");

        }

        //---------------------------------------------------------------------------------
        /// 运营商删除成员
        ///---------------------------------------------------------------------------------
        public void mobileDelete(string oneLine, string operatorName, string localRoomId, string serverRoomId)
        {
            Account tmpAccount;
            int RetStr = 0;
            string groupNum = MobileGroupNum;

            int index = oneLine.LastIndexOf(';');
            string[] phones = oneLine.Substring(0, index).Split(';');

            tmpAccount.seqno = MobileSeqno;
            tmpAccount.uri = MobileUserName;
            tmpAccount.password = MobilePassword;
            tmpAccount.ip = MobileUrlIp;
            tmpAccount.port = Convert.ToInt32(MobileUrlPort);

            mobileManager.account = tmpAccount;

            MobilePocGroupInfo tmpGroupInfo = new MobilePocGroupInfo();

            tmpGroupInfo.groupType = "4";
            tmpGroupInfo.groupLevel = "3";
            tmpGroupInfo.groupUri = groupNum;

            mobileManager.groupInfo = tmpGroupInfo;
            MobilePocUserInfo userInfo = new MobilePocUserInfo();
            userInfo.priority = "2";

            int i;
            for (i = 0; i < phones.Length; i++)
            {
                userInfo.userUri = phones[i] + "@4gpoc.com";
                mobileManager.userInfo = userInfo;
                mobileManager.DelGroupMember();
                if (RetStr == 0)
                {
                    string msg = string.Empty;
                    msg += "OPERATION:DELETEMEMBERACK" + "\r\n"
                         + "OPERATORNAME:" + operatorName + "\r\n"
                         + "SERVERROOMID:" + serverRoomId + "\r\n"
                         + "LOCALCONFROOMID:" + localRoomId + "\r\n"
                         + "TYPE:MOBILE" + "\r\n"
                         + "STATUS:OK" + "\r\n"
                         + "NUMBER:" + phones[i] + "\r\n";
                    myGlobals.msgBus.sendMessage(msg);
                }
                else
                {
                    string msg = string.Empty;
                    msg += "OPERATION:DELETEMEMBERACK" + "\r\n"
                         + "OPERATORNAME:" + operatorName + "\r\n"
                         + "SERVERROOMID:" + serverRoomId + "\r\n"
                         + "LOCALCONFROOMID:" + localRoomId + "\r\n"
                         + "TYPE:MOBILE" + "\r\n"
                         + "STATUS:FAILED" + "\r\n"
                         + "NUMBER:" + phones[i] + "\r\n";
                    myGlobals.msgBus.sendMessage(msg);
                }
            }

        }

        public void unicomDelete(string oneLine, string operatorName, string localRoomId, string serverRoomId)
        {
            string groupid = UnicomGroupid;

            int index = oneLine.LastIndexOf(';');
            string[] phones = oneLine.Substring(0, index).Split(';');
            //从组中删除成员
            string xmlTemp = unicomManager.SomeUsersDistributionParameterToXml(phones, "Out");
            string retstring = unicomManager.groupUserDistribution(groupid, xmlTemp);
            // string tmpStr = string.Join(";", phones);
            if (string.IsNullOrEmpty(retstring)
                || retstring.Equals("分配组内用户操作成功.")
                || retstring.IndexOf("分配组内用户操作成功.") >= 0)
            {
                foreach (string phone in phones)
                {
                    string msg = string.Empty;
                    msg += "OPERATION:DELETEMEMBERACK" + "\r\n"
                         + "OPERATORNAME:" + operatorName + "\r\n"
                         + "SERVERROOMID:" + serverRoomId + "\r\n"
                         + "LOCALCONFROOMID:" + localRoomId + "\r\n"
                         + "TYPE:UNICOM" + "\r\n"
                         + "STATUS:OK" + "\r\n"
                         + "NUMBER:" + phone + "\r\n";
                    myGlobals.msgBus.sendMessage(msg);
                }
            }
            else
            {
                foreach (string phone in phones)
                {
                    string msg = string.Empty;
                    msg += "OPERATION:DELETEMEMBERACK" + "\r\n"
                         + "OPERATORNAME:" + operatorName + "\r\n"
                         + "SERVERROOMID:" + serverRoomId + "\r\n"
                         + "LOCALCONFROOMID:" + localRoomId + "\r\n"
                         + "TYPE:UNICOM" + "\r\n"
                         + "STATUS:FAILED" + "\r\n"
                         + "NUMBER:" + phone + "\r\n";
                    myGlobals.msgBus.sendMessage(msg);
                }
            }

        }

        public void telecomDelete(string oneLine, string operatorName, string localRoomId, string serverRoomId)
        {
            QChat_GroupMember stQchatGroupMember = new QChat_GroupMember();
            string RetStr = null;
            string groupNum = TelecomGroupNum;

            int index = oneLine.LastIndexOf(';');
            string[] phones = oneLine.Substring(0, index).Split(';');

            stQchatGroupMember.username = TelecomUserName;
            stQchatGroupMember.password = TelecomPassword;
            stQchatGroupMember.deviceID = TelecomDeviceId;
            stQchatGroupMember.urlIp = TelecomUrlIp;
            stQchatGroupMember.memberSize = 2;
            stQchatGroupMember.groupCode = groupNum;
            stQchatGroupMember.type_mem = 1;
            stQchatGroupMember.lebel = 4;

            int i;
            for (i = 0; i < phones.Length; i++)
            {
                stQchatGroupMember.number_mem = phones[i];
                RetStr = telecomManager.DeleteGroupMember(stQchatGroupMember);
                if ((!string.IsNullOrEmpty(RetStr))
                    && (RetStr.IndexOf("成功") > 0))
                {
                    string msg = string.Empty;
                    msg += "OPERATION:DELETEMEMBERACK" + "\r\n"
                         + "OPERATORNAME:" + operatorName + "\r\n"
                         + "SERVERROOMID:" + serverRoomId + "\r\n"
                         + "LOCALCONFROOMID:" + localRoomId + "\r\n"
                         + "TYPE:TELECOM" + "\r\n"
                         + "STATUS:OK" + "\r\n"
                         + "NUMBER:" + phones[i] + "\r\n";
                    myGlobals.msgBus.sendMessage(msg);
                }
                else
                {
                    string msg = string.Empty;
                    msg += "OPERATION:DELETEMEMBERACK" + "\r\n"
                         + "OPERATORNAME:" + operatorName + "\r\n"
                         + "SERVERROOMID:" + serverRoomId + "\r\n"
                         + "LOCALCONFROOMID:" + localRoomId + "\r\n"
                         + "TYPE:TELECOM" + "\r\n"
                         + "STATUS:FAILED" + "\r\n"
                         + "NUMBER:" + phones[i] + "\r\n";
                    myGlobals.msgBus.sendMessage(msg);
                }
            }

        }
    }
}
