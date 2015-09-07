using AncientHorrorShare;
using AncientHorrorShare.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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
        public void SendMessage(ServerMessage msg)
        {
            
                XmlSerializer writer = new XmlSerializer(typeof(ServerMessage));
                using (MemoryStream ms = new MemoryStream())
                {
                    writer.Serialize(ms, msg);
                    this.Sock.Send(ms.ToArray());
                }
        }
    }
}
