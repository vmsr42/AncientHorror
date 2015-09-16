using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AncientHorrorClient.Helpers
{
    public enum BusyMessageEnum
    {
        [Description("")]
        None,
        [Description("Соединяемся с сервером")]
        Connecting,
        [Description("Идет авторизация")]
        Authorizing
    }
}
