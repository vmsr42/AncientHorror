using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AncientHorrorShared.Messaging.ConfirmMessage
{
    [DataContract]
    public class ServerConfirmMessage: BaseMessage
    {
        [DataMember]
        public Guid RefMsgId { get; set; }
        [DataMember]
        public Boolean Accept { get; set; }
        public ServerConfirmMessage() : base(new DataContractSerializer(typeof(ServerConfirmMessage)), false) { }
        protected override TransportContainer TKCreation(string text)
        {
            return new TransportContainer() { Message = text, Type =TCTypes.Confirm, MsgId = this.MsgId };
        }

        protected override void CopyMessageField(BaseMessage msg)
        {
            ServerConfirmMessage copymsg = (ServerConfirmMessage)msg;
            this.RefMsgId = copymsg.RefMsgId;
            this.Accept = copymsg.Accept;
        }
        public override BaseMessage GetInnerMessage()
        {
            return null;
        }
    }
    
}
