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
    public class StartRoomMessage: BaseMessage
    {
        public StartRoomMessage() : base(new DataContractSerializer(typeof(StartRoomMessage))) { }
        protected override TransportContainer TKCreation(string text)
        {
            var msg = new AbonentsCommandMessage() { Message = text, Type = AbonentsCommandType.StartRoom, MsgId = this.MsgId };
            return msg.GetTC();
        }
        protected override void CopyMessageField(BaseMessage msg)
        {
        }
        public override BaseMessage GetInnerMessage()
        {
            return null;
        }
    }
}
