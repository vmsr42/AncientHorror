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
    public class RequestRoomInfoMessage: BaseMessage
    {
        public RequestRoomInfoMessage() : base(new DataContractSerializer(typeof(RequestRoomInfoMessage)),false) { }
        protected override TransportContainer TKCreation(string text)
        {
            var msg = new AbonentsCommandMessage() { Message = text, Type = AbonentsCommandType.RequestRoomInfo, MsgId = this.MsgId };
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
