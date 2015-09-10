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
    public class ServerInfoMessage: BaseMessage
    {
        [DataMember]
        public SIMessageType Type { get; set; }
        [DataMember]
        public String Message { get; set; }
        public ServerInfoMessage() : base(new DataContractSerializer(typeof(ServerInfoMessage)),false) { }

        public override BaseMessage GetInnerMessage()
        {
            switch (Type)
            {
                case SIMessageType.Abonents:
                    {
                        ServerInfoAbonentsMessage msg = new ServerInfoAbonentsMessage();
                        msg.UTFDeSerialize(this.Message);
                        return msg;
                    }
                case SIMessageType.Rooms:
                    {
                        ServerInfoRoomsMessage msg = new ServerInfoRoomsMessage();
                        msg.UTFDeSerialize(this.Message);
                        return msg;
                    }
                case SIMessageType.Error:
                    {
                        ServerInfoErrorMessage msg = new ServerInfoErrorMessage();
                        msg.UTFDeSerialize(this.Message);
                        return msg;
                    }
            }
            return null;
        }

        protected override void CopyMessageField(BaseMessage msg)
        {
            ServerInfoMessage copymsg = (ServerInfoMessage)msg;
            this.Message = copymsg.Message;
            this.Type = copymsg.Type;
        }

        protected override TransportContainer TKCreation(string text)
        {
            var msg = new TransportContainer() { Message = text, Type = TCTypes.Info, MsgId = this.MsgId };
            return msg;
        }
    }
}
