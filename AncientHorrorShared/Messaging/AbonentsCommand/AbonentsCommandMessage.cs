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
    public class AbonentsCommandMessage
    {
        [DataMember]
        public AbonentsCommandType Type { get; set; }
        [DataMember]
        public String Message { get; set; }
        public ServerMessage GetServerMessage()
        {
            ServerMessage sm = new ServerMessage() { Type = ServerMessageType.Info };
            using (var stream = new MemoryStream())
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(AbonentsCommandMessage));
                serializer.WriteObject(stream, this);
                byte[] data = stream.ToArray();
                sm.Message = Encoding.UTF8.GetString(data,0,data.Length);
            }
            return sm;
        }
        public object GetInnerMessage()
        {
            switch (Type)
            {
                case AbonentsCommandType.Authorization: 
                    {
                        DataContractSerializer serializer = new DataContractSerializer(typeof(AuthorizationMessage));
                        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Message)))
                        {
                            AuthorizationMessage msg = (AuthorizationMessage)serializer.ReadObject(stream);
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
                        DataContractSerializer serializer = new DataContractSerializer(typeof(JoinRoomMessage));
                        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Message)))
                        {
                            JoinRoomMessage msg = (JoinRoomMessage)serializer.ReadObject(stream);
                            return msg;
                        }
                    }
                case AbonentsCommandType.CreateRoom:
                    {
                        DataContractSerializer serializer = new DataContractSerializer(typeof(CreateRoomMessage));
                        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Message)))
                        {
                            CreateRoomMessage msg = (CreateRoomMessage)serializer.ReadObject(stream);
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
                    case AbonentsCommandType.KickUser:
                    {
                        DataContractSerializer serializer = new DataContractSerializer(typeof(KickUserMessage));
                        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Message)))
                        {
                            KickUserMessage msg = (KickUserMessage)serializer.ReadObject(stream);
                            return msg;
                        }
                    }

            }
            return null;
        }
    }
}
