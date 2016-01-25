using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aviad.ProjectA.ServerHost
{
    public enum GlobalMessage
    {
        LOGIN,
        LOGINACKOK,
        LOGINACKFAILED,
        KEEPALIVEACK,

        LOGOUT,
        LOGOUTACKOK,
        LOGOUTACKFAILED,
        LOGOUTACK,
        KEEPALIVE,

        DATA,

        ALIYUNDATA,
        REGISTERED,
        MSGBUSERROR,
        MSGBUSKEEPALIVE,
        TEST,
        RETURN,
        PACKETERROR
    }
}
