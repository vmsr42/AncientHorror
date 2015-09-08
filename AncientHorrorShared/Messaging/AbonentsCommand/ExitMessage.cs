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
    public class ExitMessage: BaseMessage
    {

        public ExitMessage() : base(new DataContractSerializer(typeof(ExitMessage))) { }
        protected override TransportContainer TKCreation(string text)
        {
            var msg = new AbonentsCommandMessage() { Message = text, Type = AbonentsCommandType.Exit, MsgId = this.MsgId };
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
