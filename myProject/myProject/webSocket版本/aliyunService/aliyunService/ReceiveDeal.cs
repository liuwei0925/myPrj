using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace aliyunService
{
    public class ReceiveDeal
    {
        public delegate void onPublishMessage(string msg, messageType type);
        public event onPublishMessage publishMessage;

        public Dictionary<string, int> addIndex;
        public Dictionary<string, int> deleteIndex;

        public delegate void addMember(string oneLine, string operatorName, string localRoomId, string serverRoomId);
        public addMember[] addDelegates;

        public delegate void deleteMember(string oneLine, string operatorName, string localRoomId, string serverRoomId);
        public deleteMember[] deleteDelegates;


        public void initIndex()
        {
            int i = 0;
            addIndex.Add("MOBILE", i++);
            addIndex.Add("UNICOM", i++);
            addIndex.Add("TELECOM", i++);

            i = 0;
            deleteIndex.Add("MOBILE", i++);
            deleteIndex.Add("UNICOM", i++);
            deleteIndex.Add("TELECOM", i++);
        }

        public ReceiveDeal()
        {
            myGlobals.msgBus.messageArrival += new MessageBus.onMessageArrival(messageReceived);

            addIndex = new Dictionary<string, int>();
            deleteIndex = new Dictionary<string, int>();

            addDelegates = new addMember[3]
                            { 
                               new addMember(myGlobals.sendDeal.mobileAdd),
                               new addMember(myGlobals.sendDeal.unicomAdd),
                               new addMember(myGlobals.sendDeal.telecomAdd),
                            };

            deleteDelegates = new deleteMember[3]
                            { 
                               new deleteMember(myGlobals.sendDeal.mobileDelete),
                               new deleteMember(myGlobals.sendDeal.unicomDelete),
                               new deleteMember(myGlobals.sendDeal.telecomDelete),
                            };

            initIndex();
        }


        public void messageReceived(string message, messageType msgType)
        {
           
            publishMessage(message, msgType);
            if (msgType.Equals(messageType.REGISTERED))
                return;
            if (msgType.Equals(messageType.TEST))
                return;
            if (msgType.Equals(messageType.KEEPALIVE))
                return;

            string operationType = TextFollowing(message, "OPERATION:", "");
            string localRoomId = TextFollowing(message, "LOCALCONFROOMID:", "");
            string serverRoomId = TextFollowing(message, "SERVERROOMID:", "");
            string operatorName = TextFollowing(message, "OPERATORNAME:", "");

            if (operationType == "ADDMEMBER")
            {
                foreach (KeyValuePair<string, int> kv in addIndex)
                {
                    if (message.Contains(kv.Key))
                    {
                        string oneLine = TextFollowing(message, kv.Key + ":", "");
                        string type = kv.Key;
                        int index = addIndex[type];
                        addDelegates[index](oneLine, operatorName, localRoomId, serverRoomId);
                    }
                }
            }

            else if (operationType == "DELETEMEMBER")
            {
                foreach (KeyValuePair<string, int> kv in deleteIndex)
                {
                    if (message.Contains(kv.Key))
                    {
                        string oneLine = TextFollowing(message, kv.Key + ":", "");
                        string type = kv.Key;
                        int index = deleteIndex[type];
                        deleteDelegates[index](oneLine, operatorName, localRoomId, serverRoomId);
                    }
                }
            }
        }

        public string TextFollowing(string searchTxt, string value, string separator)
        {
            string separateString = string.Empty;
            if (string.IsNullOrEmpty(separator))
                separateString = "\r\n";
            else
                separateString = separator;
            if (!String.IsNullOrEmpty(searchTxt) && !String.IsNullOrEmpty(value))
            {
                int index = searchTxt.IndexOf(value);
                if (-1 < index)
                {
                    int separateIndex = searchTxt.IndexOf(separateString, index);
                    if (-1 < separateIndex)
                    {
                        int start = index + value.Length;
                        return searchTxt.Substring(start, separateIndex - start);
                    }
                }
            }
            return null;
        }
    }
}
