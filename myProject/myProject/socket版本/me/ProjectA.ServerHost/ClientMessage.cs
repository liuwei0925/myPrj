using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Communication.Sockets.Core;
using Communication.Sockets.Core.Server;
using Communication.Sockets.Core.Client;

namespace Aviad.ProjectA.ServerHost
{
    public enum ClientMsgType
    {
        LOGIN,
        CREATECONFROOM,
        STOPCONFROOM,
        ADDCONFMEMBER,
        KICKCONFMEMBER,
        CHANGEPASSWORD,
        SERVERCREATECONFROOMACK,
        SETOPERATORSTATE,
        SETOPERATORNUMBER,
        CHANGEOPERATORNUMBEER,
        SERVERCREATEINCOMINGCONFROOMACK,
        LOGOUT,
        GETCONFROOMINFO,
        GETOPERATORINFOR,
        KEEPALIVEACK,
        MUTECONFMEMBER,
        UNMUTECONFMEMBER,
        SETCONFBROADCAST,
        UNSETCONFBROADCAST,
        SWITCHCONFROOM,
        SETCONFHOSTVIDEO,
        RESTARTCONFMODEL,
        ADDMEMBERNOTIFY,
        DELETEMEMBERNOTIFY,

        ALIYUNDATA
        
    }

    public partial class ClientMsgProcess
    {
       

        public delegate string MsgProcessDelegate(string str, Socket sock);
        //public delegate void OnupdateMsg(string message);
        //public event OnupdateMsg updateMsg;

        public MsgProcessDelegate[] ClientMsgDelegate;
        public Dictionary<string, int> msgIndex;
        public int messageIndex;

        public string Host;
        public string Port;
        //public OperatorData operatorData;
      //  public SqlUtils sqlUtils;
        
        public ClientMsgProcess()
        {
            ClientMsgDelegate = new MsgProcessDelegate[24]
                            { 
                               new MsgProcessDelegate(login),
                               new MsgProcessDelegate(createConfRoom),
                               new MsgProcessDelegate(stopConfRoom),
                               new MsgProcessDelegate(addConfMember),
                               new MsgProcessDelegate(kickConfMember),
                               new MsgProcessDelegate(changePassword),
                               new MsgProcessDelegate(serverCreateConfRoomAck),
                               new MsgProcessDelegate(setOperatorState),
                               new MsgProcessDelegate(setOperatorNumber),
                               new MsgProcessDelegate(changeOperatorNumber),
                               new MsgProcessDelegate(serverCreateIncomingConfRoomAck),
                               new MsgProcessDelegate(logout),
                               new MsgProcessDelegate(getConfRoomInfo),
                               new MsgProcessDelegate(getOperatorInfo),
                               new MsgProcessDelegate(keepAliveAck),
                               new MsgProcessDelegate(muteConfMember),
                               new MsgProcessDelegate(unMuteConfMember),
                               new MsgProcessDelegate(setConfBroadcast),
                               new MsgProcessDelegate(unSetConfBroadcast),
                               new MsgProcessDelegate(switchConfRoom),
                               new MsgProcessDelegate(setConfHostVideo),
                               new MsgProcessDelegate(restartConfModel),
                               new MsgProcessDelegate(addMemberNotify),
                               new MsgProcessDelegate(deleteMemberNotify),
                             
                            };

            //  pd[(int)ClientMessageProcess.LOGIN]();
            msgIndex = new Dictionary<string, int>();
            initMsgIndex();

            //sqlUtils = new SqlUtils();
        }
        void initMsgIndex()
        {
            int i = 0;
            msgIndex.Add("LOGIN", i++);
            msgIndex.Add("CREATECONFROOM", i++);
            msgIndex.Add("STOPCONFROOM", i++);
            msgIndex.Add("ADDCONFMEMBER", i++);
            msgIndex.Add("KICKCONFMEMBER", i++);
            msgIndex.Add("CHANGEPASSWORD", i++);
            msgIndex.Add("SERVERCREATECONFROOMACK", i++);
            msgIndex.Add("SETOPERATORSTATE", i++);
            msgIndex.Add("SETOPERATORNUMBER", i++);
            msgIndex.Add("CHANGEOPERATORNUMBEER", i++);
            msgIndex.Add("SERVERCREATEINCOMINGCONFROOMACK", i++);
            msgIndex.Add("LOGOUT", i++);
            msgIndex.Add("GETCONFROOMINFO", i++);
            msgIndex.Add("GETOPERATORINFOR", i++);
            msgIndex.Add("KEEPALIVEACK", i++);
            msgIndex.Add("MUTECONFMEMBER", i++);
            msgIndex.Add("UNMUTECONFMEMBER", i++);
            msgIndex.Add("SETCONFBROADCAST", i++);
            msgIndex.Add("UNSETCONFBROADCAST", i++);
            msgIndex.Add("SWITCHCONFROOM", i++);
            msgIndex.Add("SETCONFHOSTVIDEO", i++);
            msgIndex.Add("RESTARTCONFMODEL", i++);
            msgIndex.Add("ADDMEMBERNOTIFY", i++);
            msgIndex.Add("DELETEMEMBERNOTIFY", i++);
        }

       
    }
}
