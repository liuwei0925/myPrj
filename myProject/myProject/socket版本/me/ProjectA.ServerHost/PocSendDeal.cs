using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using WebSocketSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Timers;
using MysqlBasicClass;

namespace Aviad.ProjectA.ServerHost
{

    public class PocSendDeal
    {
        public void addMember(string operatorName, string localRoomId, string serverRoomId, Dictionary<string, List<string>> phoneNumbers)
        {
            string msg = string.Empty;
            msg += "OPERATION:ADDMEMBER\r\n";
            msg += "OPERATORNAME:" + operatorName + "\r\n";
            msg += "LOCALCONFROOMID:" + localRoomId + "\r\n";
            msg += "SERVERROOMID:" + serverRoomId + "\r\n";

            if (phoneNumbers.Count > 0)
            {
                foreach (KeyValuePair<string, List<string>> kv in phoneNumbers)
                {
                    msg += kv.Key + ":";
                    foreach (string s in kv.Value)
                    {
                        msg += s + ";";
                    }
                    msg += "\r\n";
                }
                //ws.Send(msg);
               myGlobals.messageBus.sendMessage(msg);
            }

        }

        public void deleteMember(string operatorName, string localRoomId, string serverRoomId, Dictionary<string, List<string>> phoneNumbers)
        {
            string msg = string.Empty;
            msg += "OPERATION:DELETEMEMBER\r\n";
            msg += "OPERATORNAME:" + operatorName + "\r\n";
            msg += "LOCALCONFROOMID:" + localRoomId + "\r\n";
            msg += "SERVERROOMID:" + serverRoomId + "\r\n";

            if (phoneNumbers.Count > 0)
            {
                foreach (KeyValuePair<string, List<string>> kv in phoneNumbers)
                {
                    msg += kv.Key + ":";
                    foreach (string s in kv.Value)
                    {
                        msg += s + ";";
                    }
                    msg += "\r\n";
                }
                // ws.Send(msg);
                myGlobals.messageBus.sendMessage(msg);
            }


        }

        public string TextFollowing(string searchTxt, string value)
        {
            string separateString = "\r\n";
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
