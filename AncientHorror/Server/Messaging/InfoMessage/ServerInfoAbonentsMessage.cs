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
    public class ServerInfoAbonentsMessage
    {
        public List<GameAbonent> Abonents { get; set; }
        public ServerMessage GetServerMessage(Abonent ab)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ServerInfoAbonentsMessage));
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
