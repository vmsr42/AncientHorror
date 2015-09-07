using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AncientHorrorShare.Messaging.AbonentsCommand
{
    [DataContract]
    public class JoinRoomMessage
    {
        [DataMember]
        public int RoomId { get; set; }
        [DataMember]
        public string Password { get; set; }
        public ServerMessage GetServerMessage()
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(JoinRoomMessage));
            AbonentsCommandMessage ac = new AbonentsCommandMessage() { Message = String.Empty, Type = AbonentsCommandType.Exit };

            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, this);
                byte[] data = stream.ToArray();
                ac.Message = Encoding.UTF8.GetString(data, 0, data.Length);
            }
            return ac.GetServerMessage();

        }
    }
}
