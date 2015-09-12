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
    public class ServerInfoAbonentsMessage: BaseMessage
    {
        [DataMember]
        public List<GameAbonentInfo> Abonents { get; set; }
        public ServerInfoAbonentsMessage() : base(new DataContractSerializer(typeof(ServerInfoAbonentsMessage)), false) { }
        protected override TransportContainer TKCreation(string text)
        {
            var simmsg = new ServerInfoMessage() { Message = text, Type = SIMessageType.Abonents, MsgId = this.MsgId };
            return simmsg.GetTC();
        }

        protected override void CopyMessageField(BaseMessage msg)
        {
            ServerInfoAbonentsMessage copymsg = (ServerInfoAbonentsMessage)msg;
            this.Abonents = copymsg.Abonents;
        }

        public override BaseMessage GetInnerMessage()
        {
            return null;
        }
    }
    
}
