using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Net.Sockets;
namespace Aviad.ProjectA.ServerHost
{
    public class Heartbeat
    {
        Timer timer;
        public delegate void timerDelegate(Socket key);
        public event timerDelegate timeout;
        private Socket key;
        private int counter;
        public void keepalive()
        {
            counter = 36;
        }

        public void start()
        {
            timer.Start();
        }
        public void stop()
        {
            timer.Stop();
        }
        public Heartbeat()
        {
            //key = l;
            counter = 36;
            timer = new Timer();
            timer.Interval = 6000;
            timer.Enabled = false;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);

        }

        void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            counter -= 6;
            if (counter == 0)
            {
                timer.Stop();
                timeout(key);
            }
        }
    }
}
