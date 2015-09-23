using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AncientHorrorShared.Messaging.GameLogic
{
    [DataContract]
    public class EndGameMessage : BaseMessage
    {
        [DataMember]
        public bool Result { get; set; }
        [DataMember]
        public String Text { get; set; }
        public EndGameMessage() : base(new DataContractSerializer(typeof(EndGameMessage)), false) { }
        protected override TransportContainer TKCreation(string text)
        {
            var msg = new GameLogicMessage() { Message = text, Type = GLTypes.EndGame, MsgId = this.MsgId };
            return msg.GetTC();
        }
        protected override void CopyMessageField(BaseMessage msg)
        {
            EndGameMessage copymsg = (EndGameMessage)msg;
            this.Result = copymsg.Result;
            this.Text = copymsg.Text;
        }
        public override BaseMessage GetInnerMessage()
        {
            return null;
        }
    }
}
