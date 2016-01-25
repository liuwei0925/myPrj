using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aviad.ProjectA.ServerHost
{
    public class RoomRecord
    {
        public List<string> sipNumbers = new List<string>();
        public string serverRoomId;
        public string operatorNum;
        public RoomRecord()
        {
            serverRoomId = "0";
            operatorNum = "0";
        }

 
        //public bool Contains(string number)
        //{
        //    foreach (var kv in pocNumbers)
        //    {
        //        if (kv.Value.Contains(number))
        //            return true;
        //    }
        //    return false;
        //}
    }
    public class Room
    {
        public Dictionary<string, RoomRecord> rooms = new Dictionary<string, RoomRecord>();
        public Room()
        {

        }

        public string getOperatorNum(string serverRoomId)
        {
            foreach(var kv in rooms)
            {
                if (kv.Value.serverRoomId == serverRoomId)
                {
                    return kv.Value.operatorNum;
                }
            }
            return "0";
        }
    }
}
