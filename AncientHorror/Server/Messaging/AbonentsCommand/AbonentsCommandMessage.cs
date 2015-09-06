using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AncientHorror.Server.Messaging.AbonentsCommand
{
    [Serializable]
    public class AbonentsCommandMessage
    {
        public AbonentsCommandType Type { get; set; }
        public String Message { get; set; }
        public ServerMessage GetServerMessage(Abonent ab)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AbonentsCommandType));

            ServerMessage sm = new ServerMessage() { Type = ServerMessageType.Info, Sender = ab.Gamer  };
            using (var stream = new MemoryStream())
            {
                serializer = new XmlSerializer(typeof(AbonentsCommandMessage));
                serializer.Serialize(stream, this);
                sm.Message = Encoding.UTF8.GetString(stream.ToArray());
            }
            return sm;
        }
        public object GetInnerMessage()
        {
            switch (Type)
            {
                case AbonentsCommandType.Authorization: 
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(AuthorizationMessage));
                        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Message)))
                        {
                            AuthorizationMessage msg = (AuthorizationMessage)serializer.Deserialize(stream);
                            return msg;
                        }
                    }
                case AbonentsCommandType.UnAuthorization:
                    {
                        return new UnAuthorizationMessage();
                    }
                case AbonentsCommandType.Exit:
                    {
                        return new ExitMessage();
                    }
                case AbonentsCommandType.ExitRoom:
                    {
                        return new ExitRoomMessage();
                    }
                case AbonentsCommandType.JoinRoom:
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(JoinRoomMessage));
                        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Message)))
                        {
                            JoinRoomMessage msg = (JoinRoomMessage)serializer.Deserialize(stream);
                            return msg;
                        }
                    }
                case AbonentsCommandType.CreateRoom:
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(CreateRoomMessage));
                        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Message)))
                        {
                            CreateRoomMessage msg = (CreateRoomMessage)serializer.Deserialize(stream);
                            return msg;
                        }
                    }
                case AbonentsCommandType.RequestRoomInfo:
                    {
                        return new RequestRoomInfoMessage();
                    }
                case AbonentsCommandType.StartRoom:
                    {
                        return new StartRoomMessage();
                    }

            }
            return null;
        }
    }
}
