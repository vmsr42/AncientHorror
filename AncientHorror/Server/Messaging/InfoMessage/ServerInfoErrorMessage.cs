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
    public class ServerInfoErrorMessage
    {
        public String Error { get; set; }
        public ServerMessage GetServerMessage(Abonent ab)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ServerInfoErrorMessage));
            ServerInfoMessage sim = new ServerInfoMessage() { Message = String.Empty, Type = ServerInfoMessageType.Error };
                
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, this);
                sim.Message = Encoding.UTF8.GetString(stream.ToArray());
            }
            return sim.GetServerMessage(ab);
            
        }
    }
    
}
