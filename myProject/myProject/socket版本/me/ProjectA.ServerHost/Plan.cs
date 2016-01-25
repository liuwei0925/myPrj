using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aviad.ProjectA.ServerHost
{
    public class PlanRecord
    {
        public Dictionary<string, List<string>> pocNumbers = new Dictionary<string, List<string>>();
        public string serverRoomId;
        public string operatorNum;

        public PlanRecord()
        {
            serverRoomId = "0";
            operatorNum = "0";
        }
        public bool Contains(string number)
        {
            foreach (var kv in pocNumbers)
            {
                if (kv.Value.Contains(number))
                    return true;                 
            }
            return false;
        }
    }
    public class Plan
    {
        public Dictionary<string, PlanRecord> plans = new Dictionary<string, PlanRecord>();
        public Plan()
        {

        }
    }
}
