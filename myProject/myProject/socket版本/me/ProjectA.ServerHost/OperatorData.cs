using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
namespace Aviad.ProjectA.ServerHost
{
    public class Numbers
    {
        public List<string> sipNumbers;
        public Dictionary<string, List<string>> pocNumbers;
        public void addNumber(string type, string phoneNo)
        {
            if (type == "SIP")
                sipNumbers.Add(phoneNo);
            else if (pocNumbers.ContainsKey(type))
            {
                pocNumbers[type].Add(phoneNo);
            }
            else
            {
                List<string> list = new List<string>();
                pocNumbers.Add(type, list);
                pocNumbers[type].Add(phoneNo);
            }
        }
        public Numbers()
        {
            sipNumbers = new List<string>();
            pocNumbers = new Dictionary<string, List<string>>();
        }
    }


    public class OperatorRecord
    {
        //public string operatorName;
        public string operatorLevel;
        public Socket sock;
    }

    public class OpeartorData
    {

        public Dictionary<string, OperatorRecord> operators = new Dictionary<string, OperatorRecord>();
        public OpeartorData()
        {

        }

        public void addOpeartor(string operatorName, Socket s)
        {
            OperatorRecord opRecord = new OperatorRecord();
            opRecord.sock = s;
            operators.Add(operatorName, opRecord);
        }

        public string getOperatorName(Socket s)
        {
            foreach (var item in operators)
            {
                if (item.Value.sock == s)
                    return item.Key;
            }
            return null;
        }

        public Socket getOperatorSock(string operatorName)
        {
            foreach (var item in operators)
            {
                if (item.Key == operatorName)
                    return item.Value.sock;
            }
            return null;
        }

        public string getOperatorLevel(string operatorName)
        {
            foreach (var item in operators)
            {
                if (item.Key == operatorName)
                    return item.Value.operatorLevel;
            }
            return null;
        }

        public void Remove(string opeartorName)
        {
            operators.Remove(opeartorName);
        }

        public void setOperatorLevel(string operatorName, string operatorLevel)
        {
           operators[operatorName].operatorLevel = operatorLevel;
        }
    }
}
