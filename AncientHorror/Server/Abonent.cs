using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AncientHorror.Server
{
    public class Abonent
    {
        public GameAbonent Gamer { get; set; }
        public AbonentStatusEnum Status { get; set; }
        public Socket Sock { get; set; }
        public byte[] buffer = new byte[4096];
        public StringBuilder sb = new StringBuilder();
        public int RoomId { get; set; }
    }
}
