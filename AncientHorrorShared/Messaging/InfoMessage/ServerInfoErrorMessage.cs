using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AncientHorrorShared.Messaging.InfoMessage
{
    [DataContract]
    public class ServerInfoErrorMessage: BaseMessage
    {
        [DataMember]
        public String Error { get; set; }
        public ServerInfoErrorMessage() : base(new DataContractSerializer(typeof(ServerInfoErrorMessage)),false) { }
        protected override TransportContainer TKCreation(string text)
        {
            var simmsg = new ServerInfoMessage() { Message = text, Type = SIMessageType.Error, MsgId = this.MsgId };
            return simmsg.GetTC();
        }

        protected override void CopyMessageField(BaseMessage msg)
        {
            ServerInfoErrorMessage copymsg = (ServerInfoErrorMessage)msg;
            this.Error = copymsg.Error;
        }
        public override BaseMessage GetInnerMessage()
        {
            return null;
        }
    }
    
}
