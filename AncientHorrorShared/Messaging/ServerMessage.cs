using AncientHorrorShare.Messaging.AbonentsCommand;
using AncientHorrorShare.Messaging.InfoMessage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AncientHorrorShare.Messaging
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
                        DataContractSerializer serializer = new DataContractSerializer(typeof(ServerInfoMessage));
                        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Message)))
                        {
                            ServerInfoMessage msg = (ServerInfoMessage)serializer.ReadObject(stream);
                            return msg;
                        }
                    }
                case ServerMessageType.AbonentCommand:
                    {
                        DataContractSerializer serializer = new DataContractSerializer(typeof(AbonentsCommandMessage));
                        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Message)))
                        {
                            AbonentsCommandMessage msg = (AbonentsCommandMessage)serializer.ReadObject(stream);
                            return msg;
                        }
                    }
                     
            }
            return null;
        }
    }
}
