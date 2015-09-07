using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AncientHorrorShare.Messaging.InfoMessage
{
    [DataContract]
    public class ServerInfoRoomsMessage
    {
        [DataMember]
        public List<int> RoomIds { get; set; }
        [DataMember]
        public List<String> RoomNames { get; set; }
        [DataMember]
        public List<GameAbonent> Owners { get; set; }
        public ServerMessage GetServerMessage()
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(ServerInfoRoomsMessage));
            ServerInfoMessage sim = new ServerInfoMessage() { Message = String.Empty, Type = ServerInfoMessageType.Abonents };

            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, this);
                byte[] data = stream.ToArray();
                sim.Message = Encoding.UTF8.GetString(data, 0, data.Length);
            }
            return sim.GetServerMessage();
            
        }
    }
    
}
