using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
namespace Aviad.ProjectA.ServerHost
{

    public class GatewayRecord
    {
        public string type;
        public string sipNo;
        public bool isOccupied;
        public bool isConnnected;
        public string serverRoomId;

        public GatewayRecord()
        {
            type = "";
            sipNo = "0";
            isOccupied = false;
            isConnnected = false;
            serverRoomId = "0";
        }
    }
    public class GatewayData
    {
        public Dictionary<string, GatewayRecord> gateway = new Dictionary<string, GatewayRecord>();


        public void Add(string type, string sipNo)
        {
            GatewayRecord gwRecord = new GatewayRecord();
            gwRecord.sipNo = sipNo;
            gateway.Add(type, gwRecord);
        }

        public void Remove(string type)
        {
            gateway.Remove(type);
        }

        public bool ContainsNumber(string number)
        {
            foreach (var kv in gateway)
            {
                if (kv.Value.sipNo == number)
                    return true;
            }
            return false;
        }

        public string getNumber(string type)
        {
            foreach (var kv in gateway)
            {
                if (kv.Key == type)
                    return kv.Value.sipNo;
            }
            return null;
        }

        public string getType(string number)
        {
            foreach (var kv in gateway)
            {
                if (kv.Value.sipNo == number)
                    return kv.Key;
            }
            return null;
        }
    }
}
