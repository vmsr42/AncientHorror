using AncientHorror.Server.Messaging.AbonentsCommand;
using AncientHorror.Server.Messaging.InfoMessage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AncientHorror.Server.Messaging
{
    public class ServerMessage
    {
        public ServerMessageType Type { get; set; }
        public String Message { get; set; }
        public GameAbonent Sender { get; set; }
        public object GetInnerMessage()
        {
            switch (Type)
            {
                case ServerMessageType.Info:
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ServerInfoMessage));
                        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Message)))
                        {
                            ServerInfoMessage msg = (ServerInfoMessage)serializer.Deserialize(stream);
                            return msg;
                        }
                    }
                case ServerMessageType.AbonentCommand:
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(AbonentsCommandMessage));
                        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Message)))
                        {
                            AbonentsCommandMessage msg = (AbonentsCommandMessage)serializer.Deserialize(stream);
                            return msg;
                        }
                    }
                     
            }
            return null;
        }
    }
}
