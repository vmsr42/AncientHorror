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
    public class CreateRoomMessage: BaseMessage
    {
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public string Password { get; set; }
        public CreateRoomMessage() : base(new DataContractSerializer(typeof(CreateRoomMessage))) { }
        protected override TransportContainer TKCreation(string text)
        {
            var msg = new AbonentsCommandMessage() { Message = text, Type = AbonentsCommandType.CreateRoom, MsgId = this.MsgId };
            return msg.GetTC();
        }
        protected override void CopyMessageField(BaseMessage msg)
        {
            AuthorizationMessage copymsg = (AuthorizationMessage)msg;
            this.Name = copymsg.Login;
            this.Password = copymsg.Password;
        }
        public override BaseMessage GetInnerMessage()
        {
            return null;
        }
    }
}
