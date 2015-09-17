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
        Authorizing,
        [Description("Идет создание комнаты")]
        CreatingRoom,
        [Description("Присоединяемся к комнате")]
        JoiningRoom,
        [Description("Выходим из комнаты")]
        ExitingRoom

    }
}
