using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using WebSocketSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.IO;

namespace Aviad.ProjectA.ServerHost
{
    public class MessageBus
    {

        public delegate void onAliyunMsgArrival(string msg, GlobalMessage msgType);
        public event onAliyunMsgArrival aliyunMsgArrival;

        private WebSocket ws = new WebSocket("ws://121.40.30.88:8080");
        private System.Timers.Timer heartbeat;

        private string session_id;
        private string guid = Guid.NewGuid().ToString();

        private int mbSynNumber = 0;

        public MessageBus()
        {
        }

         public void init()
        {

            heartbeat = new System.Timers.Timer(12000);
            // Hook up the Elapsed event for the timer. 
            heartbeat.Elapsed += OnTimedEvent;
            heartbeat.Enabled = false;

            ws.OnOpen += (sender, e) =>
            {

            };
            ws.OnMessage += (sender, e) =>
            {
                //MessageBox.Show(e.Data);
                string response = e.Data;
                JObject jo = JObject.Parse(response);
                if (jo["method"].ToString() == "REGISTER" && jo["type"].ToString() == "register_resp")
                {
                    if (jo["info"]["result"].ToString() == "0")
                    {
                        heartbeat.Start();

                        
                        session_id = jo["info"]["session_id"].ToString();

                        string subscribeStr = @"{
	                                            'method': 'REGISTER',
	                                            'type': 'subscribe_req',
	                                            'from': '',
	                                            'call_id': 'xxx',
	                                            'info': {
		                                                 'subscribe': '@emergency_cmd_msg@'
                                                        }
	                                        }";

                        JObject subObj = JObject.Parse(subscribeStr);
                        subObj["from"] = session_id;
                        subObj["call_id"] = guid;

                        subscribeStr = subObj.ToString();

                        subscribeStr = subscribeStr.Replace("\r\n", "");

                        ws.Send(subscribeStr);

                    }
                }
              
                else if (jo["method"].ToString() == "REGISTER" && jo["type"].ToString() == "subscribe_resp")
                {
                    aliyunMsgArrival("", GlobalMessage.REGISTERED);
                }
                else if (jo["method"].ToString() == "NOTIFY" && jo["type"].ToString() == "@emergency_cmd_msg@")
                {
                   // textBox2.Text += jo["info"]["data"];
                    string mes = jo["info"]["data"].ToString();
                    string headFlag = "-1-2-1-2-1-2-1-2";
                    if (!mes.StartsWith(headFlag))
                        return;

                    string content = mes.Substring(headFlag.Length);
                    if (content.StartsWith("TEST"))
                        aliyunMsgArrival(content.Substring(4), GlobalMessage.TEST);
                    else
                        aliyunMsgArrival(content, GlobalMessage.ALIYUNDATA);
                }
                else if (jo["method"].ToString() == "INFO" && jo["type"].ToString() == "heart_beat")
                {
                    mbSynNumber++;
                    string mes = "heart_beat" + "\r\n" + "SYN " + mbSynNumber.ToString();
                    aliyunMsgArrival(mes, GlobalMessage.MSGBUSKEEPALIVE);
                }
            };

            ws.OnError += (sender, e) =>
            {

            };
            ws.OnClose += (sender, e) =>
            {

            };

            ws.Connect();
            register();
        }
       
        public void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            string heartbeatFormat = @"{
	                                'method': 'INFO',
	                                'from': '客户端ID',
	                                'type': 'heart_beat',
	                                'call_id': 'xxx'
                                    }";

            JObject hbObj = JObject.Parse(heartbeatFormat);

            hbObj["call_id"] = guid;
            hbObj["from"] = session_id;
            string heartbeatStr = hbObj.ToString();
            heartbeatStr = heartbeatStr.Replace("\r\n", "");
            ws.Send(heartbeatStr);
        }

        public void register()
        {
            string cerFile = "certificate.txt";


            JObject cerObj = JObject.Parse(File.ReadAllText(cerFile));
            string RegFormat = @"{
	                                'method': 'REGISTER',
	                                'type': 'register_req',
	                                'call_id': 'xxx',
                                    'info': 'certificate'
	                             }";

            JObject regObj = JObject.Parse(RegFormat);

            regObj["call_id"] = guid;
            regObj["info"] = cerObj;
            string registerStr = regObj.ToString();
            registerStr = registerStr.Replace("\r\n", "");

            ws.Send(registerStr);
        }

        public void sendMessage(string mes)
        {
           
            string notiFormat = @"{
	                            'method': 'NOTIFY',
	                            'type': '@emergency_cmd_msg@',
	                            'from': '客户端ID',
	                            'call_id': 'xxx',
	                            'info': {
		                                'data': ''
                                        }
	                            }";

            JObject notiObj = JObject.Parse(notiFormat);


            notiObj["from"] = session_id;

            string headFlag = "-1-2-1-2-1-2-1-2";
            string totalMes = "";
            totalMes += headFlag;
            totalMes += mes;

            notiObj["info"]["data"] = totalMes;
            string notifyStr = notiObj.ToString();
            notifyStr = notifyStr.Replace("\r\n", "");

            ws.Send(notifyStr);

        }
    }
}
