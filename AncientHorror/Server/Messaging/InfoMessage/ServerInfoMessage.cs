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
    public class ServerInfoMessage
    {
        public ServerInfoMessageType Type { get; set; }
        public String Message { get; set; }

        public ServerMessage GetServerMessage(Abonent ab)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ServerInfoAbonentsMessage));

            ServerMessage sm = new ServerMessage() { Type = ServerMessageType.Info, Sender = ab.Gamer  };
            using (var stream = new MemoryStream())
            {
                serializer = new XmlSerializer(typeof(ServerInfoMessage));
                serializer.Serialize(stream, this);
                sm.Message = Encoding.UTF8.GetString(stream.ToArray());
            }
            return sm;
        }
        public object GetInnerMessage()
        {
            switch (Type)
            {
                case ServerInfoMessageType.Abonents:
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ServerInfoAbonentsMessage));
                        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Message)))
                        {
                            ServerInfoAbonentsMessage msg = (ServerInfoAbonentsMessage)serializer.Deserialize(stream);
                            return msg;
                        }
                    }
                case ServerInfoMessageType.Rooms:
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ServerInfoRoomsMessage));
                        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Message)))
                        {
                            ServerInfoRoomsMessage msg = (ServerInfoRoomsMessage)serializer.Deserialize(stream);
                            return msg;
                        }
                    }
                case ServerInfoMessageType.Error:
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ServerInfoErrorMessage));
                        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Message)))
                        {
                            ServerInfoErrorMessage msg = (ServerInfoErrorMessage)serializer.Deserialize(stream);
                            return msg;
                        }
                    }


            }
            return null;
        }
    }
}
