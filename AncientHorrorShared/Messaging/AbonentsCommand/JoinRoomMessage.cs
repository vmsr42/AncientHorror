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
    public class JoinRoomMessage: BaseMessage
    {
        [DataMember]
        public int RoomId { get; set; }
        [DataMember]
        public string Password { get; set; }

        public JoinRoomMessage() : base(new DataContractSerializer(typeof(JoinRoomMessage))) { }
        protected override TransportContainer TKCreation(string text)
        {
            var msg = new AbonentsCommandMessage() { Message = text, Type = AbonentsCommandType.JoinRoom, MsgId = this.MsgId };
            return msg.GetTC();
        }
        protected override void CopyMessageField(BaseMessage msg)
        {
            JoinRoomMessage copymsg = (JoinRoomMessage)msg;
            this.RoomId = copymsg.RoomId;
            this.Password = copymsg.Password;
        }
        public override BaseMessage GetInnerMessage()
        {
            return null;
        }
    }
}
