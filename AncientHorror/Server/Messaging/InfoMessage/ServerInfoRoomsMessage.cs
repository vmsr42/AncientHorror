using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AncientHorror.Server.Messaging.InfoMessage
{
    [Serializable]
    public class ServerInfoRoomsMessage
    {
        public List<int> RoomIds { get; set; }
        public List<String> RoomNames { get; set; }
        public List<GameAbonent> Owners { get; set; }
        public ServerMessage GetServerMessage(Abonent ab)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ServerInfoRoomsMessage));
            ServerInfoMessage sim = new ServerInfoMessage() { Message = String.Empty, Type = ServerInfoMessageType.Abonents };
                
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, this);
                sim.Message = Encoding.UTF8.GetString(stream.ToArray());
            }
            return sim.GetServerMessage(ab);
            
        }
    }
    
}
