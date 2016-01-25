using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aliyunService
{
    public static class myGlobals
    {
        public static MessageBus msgBus = new MessageBus();
        public static SendDeal sendDeal = new SendDeal();
        public static ReceiveDeal recvDeal = new ReceiveDeal();
    }
}
