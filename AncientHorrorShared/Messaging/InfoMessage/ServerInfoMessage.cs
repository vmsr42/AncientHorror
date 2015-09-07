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
    public class ServerInfoMessage
    {
        [DataMember]
        public ServerInfoMessageType Type { get; set; }
        [DataMember]
        public String Message { get; set; }

        public ServerMessage GetServerMessage()
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(ServerInfoMessage));

            ServerMessage sm = new ServerMessage() { Type = ServerMessageType.Info  };
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, this);
                byte[] data = stream.ToArray();
                sm.Message = Encoding.UTF8.GetString(data, 0, data.Length);
            }
            return sm;
        }
        public object GetInnerMessage()
        {
            switch (Type)
            {
                case ServerInfoMessageType.Abonents:
                    {
                        DataContractSerializer serializer = new DataContractSerializer(typeof(ServerInfoAbonentsMessage));
                        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Message)))
                        {
                            ServerInfoAbonentsMessage msg = (ServerInfoAbonentsMessage)serializer.ReadObject(stream);
                            return msg;
                        }
                    }
                case ServerInfoMessageType.Rooms:
                    {
                        DataContractSerializer serializer = new DataContractSerializer(typeof(ServerInfoRoomsMessage));
                        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Message)))
                        {
                            ServerInfoRoomsMessage msg = (ServerInfoRoomsMessage)serializer.ReadObject(stream);
                            return msg;
                        }
                    }
                case ServerInfoMessageType.Error:
                    {
                        DataContractSerializer serializer = new DataContractSerializer(typeof(ServerInfoErrorMessage));
                        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Message)))
                        {
                            ServerInfoErrorMessage msg = (ServerInfoErrorMessage)serializer.ReadObject(stream);
                            return msg;
                        }
                    }


            }
            return null;
        }
    }
}
