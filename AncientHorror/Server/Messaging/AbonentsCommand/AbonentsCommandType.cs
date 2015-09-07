using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AncientHorror.Server.Messaging.AbonentsCommand
{
    public enum AbonentsCommandType
    {
        Authorization,
        UnAuthorization,
        Exit,
        ExitRoom,
        JoinRoom,
        CreateRoom,
        RequestRoomInfo,
        StartRoom,
        KickUser

    }
}
