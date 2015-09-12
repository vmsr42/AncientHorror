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
    public class KickUserMessage: BaseMessage
    {
        [DataMember]
        public int UserId { get; set; }

        public KickUserMessage() : base(new DataContractSerializer(typeof(KickUserMessage)), true) { }
        protected override TransportContainer TKCreation(string text)
        {
            var msg = new AbonentsCommandMessage() { Message = text, Type = AbonentsCommandType.KickUser, MsgId = this.MsgId };
            return msg.GetTC();
        }
        protected override void CopyMessageField(BaseMessage msg)
        {
            KickUserMessage copymsg = (KickUserMessage)msg;
            this.UserId = copymsg.UserId;
        }
        public override BaseMessage GetInnerMessage()
        {
            return null;
        }
    }
}
