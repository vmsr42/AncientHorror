using AncientHorrorShared.Messaging.AbonentsCommand;
using AncientHorrorShared.Messaging.InfoMessage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AncientHorrorShared.Messaging
{
    [DataContract]
    public class TransportContainer: BaseMessage
    {
        [DataMember]
        public TCTypes Type { get; set; }
        [DataMember]
        public String Message { get; set; }
        
        public TransportContainer() : base(new DataContractSerializer(typeof(TransportContainer)),false) { }
        [DataMember]
        public GameAbonent User { get; set; }
        public override BaseMessage GetInnerMessage()
        {
            switch (Type)
            {
                case TCTypes.Info:
                    {
                        ServerInfoMessage msg = new ServerInfoMessage();
                        msg.UTFDeSerialize(this.Message);
                        return msg;
                    }
                case TCTypes.AbonentCommand:
                    {
                        AbonentsCommandMessage msg = new AbonentsCommandMessage();
                        msg.UTFDeSerialize(this.Message);
                        return msg;
                    }
                     
            }
            return null;
        }

        protected override void CopyMessageField(BaseMessage msg)
        {
            TransportContainer copymsg = (TransportContainer)msg;
            this.Message = copymsg.Message;
            this.Type = copymsg.Type;
            this.User = copymsg.User;
            this.MsgId = this.MsgId;
        }

        protected override TransportContainer TKCreation(string text)
        {
            return new TransportContainer() { Message = this.Message, User = this.User, MsgId = this.MsgId, Type = this.Type };
        }
    }
}
