using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AncientHorrorShared.Messaging.PlayerCommands
{
    [DataContract]
    public class InterfaceMessage: BaseMessage
    {
        [DataMember]
        public PCTypes Type { get; set; }
        [DataMember]
        public String Message { get; set; }
        public InterfaceMessage() : base(new DataContractSerializer(typeof(InterfaceMessage)), true) { }

        public override BaseMessage GetInnerMessage()
        {
            
            return null;
        }

        protected override void CopyMessageField(BaseMessage msg)
        {
            InterfaceMessage copymsg = (InterfaceMessage)msg;
            this.Message = copymsg.Message;
            this.Type = copymsg.Type;
        }

        protected override TransportContainer TKCreation(string text)
        {
            var msg = new TransportContainer() { Message = text, Type = TCTypes.PlayerCommand, MsgId = this.MsgId };
            return msg;
        }
    }
}
