using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fleck;
namespace Aviad.ProjectA.ServerHost
{
    public class Server
    {
        public IWebSocketServer server;
        public List<IWebSocketConnection> allSockets = new List<IWebSocketConnection>();

        public delegate void onClientConnected(IWebSocketConnection sock);
        public delegate void onClientDisconneted(IWebSocketConnection sock);
        public delegate void onCliengMessageReceived(IWebSocketConnection sock, string message);

        private int port;

        public event onClientConnected ClientConnect;
        public event onClientDisconneted ClientDisconnect;
        public event onCliengMessageReceived MessageRecived;

        public Server()
        {
            port = 0;
            //var input = Console.ReadLine();
            //while (input != "exit")
            //{
            //    foreach (var socket in allSockets.ToList())
            //    {
            //        socket.Send(input);
            //    }
            //    input = Console.ReadLine();
            //}
            server = new WebSocketServer("ws://0.0.0.0:" + port.ToString());
        }

        public void startListening(int port)
        {
            FleckLog.Level = LogLevel.Debug;
            //var allSockets = new List<IWebSocketConnection>();
            //var server = new WebSocketServer("ws://0.0.0.0:8181");
            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    //  Console.WriteLine("Open!");
                    allSockets.Add(socket);
                   // socket.Send("Connected");
                    ClientConnect(socket);
                };
                socket.OnClose = () =>
                {
                    // Console.WriteLine("Close!");
                    allSockets.Remove(socket);
                    ClientDisconnect(socket);
                };
                socket.OnMessage = message =>
                {
                    //Console.WriteLine(message);
                    // allSockets.ToList().ForEach(s => s.Send("Echo: " + message));
                    MessageRecived(socket, message);
                };
            });

        }            

        public void sendMessage(IWebSocketConnection sock, string msg)
        {
            foreach (var s in allSockets)
            {
                if (s == sock)
                    s.Send(msg);
            }
        }
    }
}
