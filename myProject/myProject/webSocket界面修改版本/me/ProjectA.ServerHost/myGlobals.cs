using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MysqlBasicClass;
using System.Threading;
using System.Net.Sockets;
using Fleck;
namespace Aviad.ProjectA.ServerHost
{
    public static class myGlobals
    {

        public static ManagerTables dbManager = new ManagerTables();

        public static OpeartorData operatorData = new OpeartorData();

        public static GatewayData gatewayData = new GatewayData();

        //public static Dictionary<Socket, Socket> ClientServer = new Dictionary<Socket, Socket>();
        //public static Dictionary<Socket, Socket> ServerClient = new Dictionary<Socket, Socket>();

        public static Dictionary<IWebSocketConnection, Socket> ClientServer = new Dictionary<IWebSocketConnection, Socket>();
        public static Dictionary<Socket, IWebSocketConnection> ServerClient = new Dictionary<Socket, IWebSocketConnection>();
        public static List<string> pocTypes = new List<string>();

        public static ManualResetEvent mre1 = new ManualResetEvent(true);
        public static ManualResetEvent mre2 = new ManualResetEvent(true);

        public static MessageBus messageBus = new MessageBus();

        public static PocSendDeal pocSendDeal = new PocSendDeal();
        public static PocReceiveDeal pocReceiveDeal = new PocReceiveDeal();

        public static Plan myPlans = new Plan();

        public static Room myRooms = new Room();


    }
}
