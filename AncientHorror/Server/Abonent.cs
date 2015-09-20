using AncientHorror.Net;
using AncientHorrorShared;
using AncientHorrorShared.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AncientHorror.Server
{
    public class Abonent
    {
        public Room CurrentRoom{get;set;}
        public GameAbonentInfo Gamer { get; set; }
        public AbonentStatusEnum Status { get; set; }
        public SingleSender Sender { get; set; }
        public void SendMessage(TransportContainer msg)
        {
            if (this.CurrentRoom != null)
            {
                msg.Room = this.CurrentRoom.GetGameRoomInfo();
                msg.User = this.Gamer;
                Sender.SendMessage(msg);
            }
        }
    }
}
