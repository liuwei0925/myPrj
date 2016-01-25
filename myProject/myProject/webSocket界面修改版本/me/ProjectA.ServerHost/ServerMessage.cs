using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Aviad.ProjectA.ServerHost
{ public enum ServerMsgType
        {
            LOGINACK,
            CHANGEPASSWORDACK,
            SETOPERATORSTATEACK,
            SETOPERATORNUMBERACK,
            LOGOUTACK,
            CREATECONFROOMACK,
            NEWCONFROOM,
            STOPCONFROOMACK,
            ADDCONFMEMBERACK,
            MEMBERRING,
            MEMBERCONNECTED,
            MEMBERCALLFAILED,
            CONFROOMINFO,
            OPERATORINFO,
            KICKCONFMEMBERACK,
            QUITCONFROOM,
            SERVERRELEASECONFROOM,
            SERVERCREATECONFROOM,
            SERVERCREATEINCOMINGCONFROOM,
            MUTECONFMEMBERACK,
            UNMUTECONFMEMBERACK,
            SETCONFBROADCASTACK,
            UNSETCONFBROADCASTACK,
            SWITCHCONFROOMACK,
            SETCONFHOSTVIDEOACK,
            KEEPALIVE,
            INCOMINGCALLQUEUESTATE,
            SPECIALINCOMINGCALLQUEUESTATE,
            SETPRIORNUMBERACK,
            OPERATORNUMBERSTATE,
            OPERATORLOGIN,
            OPERATORLOGOUT,
            OPERATORNEWSTATE,
            
            ALIYUNDATA
        }
   
    public partial class ServerMsgProcess
    {
       

        public delegate string MsgProcessDelegate(string str, Socket sock);
        public MsgProcessDelegate[] ServerMsgDelegate;
        //private SqlUtils sqlUtils;

        //public delegate void onUpdateMessage(string msg);
        //public event onUpdateMessage updateMessage;
        public ServerMsgProcess()
        {
           ServerMsgDelegate = new MsgProcessDelegate[32]
                            { 
                               new MsgProcessDelegate(loginAck),
                               new MsgProcessDelegate(changePasswordAck),
                               new MsgProcessDelegate(setOperatorAck),
                               new MsgProcessDelegate(setOperatorNumberAck),
                               new MsgProcessDelegate(logoutAck),
                               new MsgProcessDelegate(createConfRoomAck),
                               new MsgProcessDelegate(newConfRoom),
                               new MsgProcessDelegate(stopConfRoomAck),
                               new MsgProcessDelegate(addConfMemberAck),
                               new MsgProcessDelegate(membering),
                               new MsgProcessDelegate(memberConnected),
                               new MsgProcessDelegate(memberCallFailed),
                               new MsgProcessDelegate(confRoomInfo),
                               new MsgProcessDelegate(operatorInfo),
                               new MsgProcessDelegate(kickConfMemberAck),
                               new MsgProcessDelegate(quitConfRoom),
                               new MsgProcessDelegate(serverReleaseConfRoom),
                               new MsgProcessDelegate(serverCreateConfRoom),
                               new MsgProcessDelegate(serverCreateIncomingConfRoom),
                               new MsgProcessDelegate(muteConfMemberAck),
                               new MsgProcessDelegate(unMuteConfMemberAck),
                               new MsgProcessDelegate(setConfBroadcastAck),
                               new MsgProcessDelegate(unSetConfBroadcastAck),
                               new MsgProcessDelegate(switchConfRoomAck),
                               new MsgProcessDelegate(setConfHostVideoAck),
                               new MsgProcessDelegate(keepAlive),
                               new MsgProcessDelegate(incomingCallQueueState),
                               new MsgProcessDelegate(setPriorNumberAck),
                               new MsgProcessDelegate(operatorNumberState),
                               new MsgProcessDelegate(operatorLogin),
                               new MsgProcessDelegate(operatorLogout),
                               new MsgProcessDelegate(opeartorNewState),

                            };

           msgIndex = new Dictionary<string, int>();

           initMsgIndex();

           //sqlUtils = new SqlUtils();

           // myGlobals.pocManager.pocEvent += new pocManager.pocDelegates(pocMessage);
          // myGlobals.pocManager.msgArrival += new PocSendDeal.onMsgArrival(pocMessageArrival);
        }
        public Dictionary<string, int> msgIndex;


    // void  loginAck();
    // void  changePasswordAck();
    // void  setOperatorAck();
    // void  setOperatorNumberAck();
    // void  logoutAck();
    // void  createConfRoomAck();
    // void  newConfRoom();
    // void  stopConfRoomAck();
    // void  addConfMemberAck();
    // void  membering();
    // void  memberConnected();
    // void  memberCallFailed();
    // void  confRoomInfo();
    // void  operatorInfo();
    // void  kickConfMemberAck();
    // void  quitConfRoom();
    // void  serverReleaseConfRoom();
    // void  serverCreateConfRoom();
    // void  serverCreateIncomingConfRoom();
    // void  muteConfMemberAck();
    // void  unMuteConfMemberAck();
    // void  setConfBroadcastAck();
    // void  unSetConfBroadcastAck();
    // void  switchConfRoomAck();
    // void  setConfHostVideoAck();
    // void  keepAlive();
    //void  incomingCallQueueState();
    //void  setPriorNumberAck();
    //void  operatorNumberState();
    //void  operatorLogin();
    //void  operatorLogout();
    //void  opeartorNewState();
    
        void initMsgIndex()
        {
            int i = 0;
            msgIndex.Add("LOGINACK",i++);
            msgIndex.Add("CHANGEPASSWORDACK",i++);
            msgIndex.Add("SETOPERATORSTATEACK",i++);
            msgIndex.Add("SETOPERATORNUMBERACK",i++);
            msgIndex.Add("LOGOUTACK",i++);
            msgIndex.Add("CREATECONFROOMACK",i++);           
            msgIndex.Add("NEWCONFROOM",i++);
            msgIndex.Add("STOPCONFROOMACK",i++);           
            msgIndex.Add("ADDCONFMEMBERACK",i++);
            msgIndex.Add("MEMBERRING",i++);           
            msgIndex.Add("MEMBERCONNECTED",i++);
            msgIndex.Add("MEMBERCALLFAILED",i++);           
            msgIndex.Add("CONFROOMINFO",i++);
            msgIndex.Add("OPERATORINFO",i++);           
            msgIndex.Add("KICKCONFMEMBERACK",i++);
            msgIndex.Add("QUITCONFROOM",i++);           
            msgIndex.Add("SERVERRELEASECONFROOM",i++);
            msgIndex.Add("SERVERCREATECONFROOM",i++);           
            msgIndex.Add("SERVERCREATEINCOMINGCONFROOM",i++);
            msgIndex.Add("MUTECONFMEMBERACK",i++);           
            msgIndex.Add("UNMUTECONFMEMBERACK",i++);
            msgIndex.Add("SETCONFBROADCASTACK",i++);           
            msgIndex.Add("UNSETCONFBROADCASTACK",i++);
            msgIndex.Add("SWITCHCONFROOMACK",i++);           
            msgIndex.Add("SETCONFHOSTVIDEOACK",i++);
            msgIndex.Add("KEEPALIVE",i++);           
            msgIndex.Add("INCOMINGCALLQUEUESTATE",i++);
            msgIndex.Add("SPECIALINCOMINGCALLQUEUESTATE",i++);           
            msgIndex.Add("SETPRIORNUMBERACK",i++);
            msgIndex.Add("OPERATORNUMBERSTATE",i++);
            msgIndex.Add("OPERATORLOGIN",i++);           
            msgIndex.Add("OPERATORLOGOUT",i++);
            msgIndex.Add("OPERATORNEWSTATE",i++);
        }
       
    }
}
