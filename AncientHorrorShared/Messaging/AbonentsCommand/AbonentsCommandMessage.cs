using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AncientHorrorShared.Messaging.AbonentsCommand
{
    [DataContract]
    public class AbonentsCommandMessage: BaseMessage
    {
        [DataMember]
        public AbonentsCommandType Type { get; set; }
        [DataMember]
        public String Message { get; set; }
        public AbonentsCommandMessage() : base(new DataContractSerializer(typeof(AbonentsCommandMessage)), true) { }
        public override BaseMessage GetInnerMessage()
        {
            switch (Type)
            {
                case AbonentsCommandType.Authorization: 
                    {
                        AuthorizationMessage msg = new AuthorizationMessage();
                        msg.UTFDeSerialize(this.Message);
                        return msg;
                    }
                case AbonentsCommandType.UnAuthorization:
                    {
                        return new UnAuthorizationMessage() { MsgId = this.MsgId };
                    }
                case AbonentsCommandType.Exit:
                    {
                        return new ExitMessage() { MsgId = this.MsgId }; 
                    }
                case AbonentsCommandType.ExitRoom:
                    {
                        return new ExitRoomMessage() { MsgId = this.MsgId }; 
                    }
                case AbonentsCommandType.JoinRoom:
                    {
                        JoinRoomMessage msg = new JoinRoomMessage();
                        msg.UTFDeSerialize(this.Message);
                        return msg;
                    }
                case AbonentsCommandType.CreateRoom:
                    {
                        CreateRoomMessage msg = new CreateRoomMessage();
                        msg.UTFDeSerialize(this.Message);
                        return msg;
                    }
                case AbonentsCommandType.RequestRoomInfo:
                    {
                        return new RequestRoomInfoMessage() { MsgId = this.MsgId }; 
                    }
                case AbonentsCommandType.StartRoom:
                    {
                        return new StartRoomMessage() { MsgId = this.MsgId }; 
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

        protected override void CopyMessageField(BaseMessage msg)
        {
            AbonentsCommandMessage copymsg = (AbonentsCommandMessage)msg;
            this.Message = copymsg.Message;
            this.Type = copymsg.Type;
        }

        protected override TransportContainer TKCreation(string text)
        {
            var msg = new TransportContainer() { Message = text, Type = TCTypes.AbonentCommand, MsgId = this.MsgId };
            return msg;
        }
    }
}
