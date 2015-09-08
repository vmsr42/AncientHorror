using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AncientHorror.Server.Messaging.AbonentsCommand
{
    public class JoinRoomMessage
    {
        public int RoomId { get; set; }
        public string Password { get; set; }
        public ServerMessage GetServerMessage(Abonent ab)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(JoinRoomMessage));
            AbonentsCommandMessage ac = new AbonentsCommandMessage() { Message = String.Empty, Type = AbonentsCommandType.Exit };

            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, this);
                ac.Message = Encoding.UTF8.GetString(stream.ToArray());
            }
            return ac.GetServerMessage(ab);

        }
    }
}
