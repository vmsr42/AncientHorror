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
    public abstract class BaseMessage
    {
        private DataContractSerializer serializer;
        [DataMember]
        public Guid MsgId { get; set; }
        public Boolean NeedConfirm { get; private set; }
        public BaseMessage(DataContractSerializer ser, bool needConf)
        {
            MsgId = Guid.NewGuid();
            serializer = ser;
            NeedConfirm = needConf;
        }
        public TransportContainer GetTC()
        {
            var msg = TKCreation(UTFSerialize());
            msg.MsgId = this.MsgId;
            return msg;
        }
        public String UTFSerialize()
        {
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, this);
                byte[] data = stream.ToArray();
                return Encoding.UTF8.GetString(data, 0, data.Length);
            }
        }
        public void UTFDeSerialize(String text)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(text)))
            {
                var copymsg = (BaseMessage)serializer.ReadObject(stream);
                this.MsgId = copymsg.MsgId;
                CopyMessageField(copymsg);
            }
        }
        protected abstract void CopyMessageField(BaseMessage msg);
        protected abstract TransportContainer TKCreation(String text);
        public abstract BaseMessage GetInnerMessage();
    }
}
