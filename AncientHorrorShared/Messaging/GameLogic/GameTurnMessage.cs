using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AncientHorrorShared.Messaging.GameLogic
{
    [DataContract]
    public class GameTurnMessage : BaseMessage
    {
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public String Text { get; set; }
        [DataMember]
        public DateTime EndTime { get; set; }
        public GameTurnMessage() : base(new DataContractSerializer(typeof(GameTurnMessage)), false) { }
        protected override TransportContainer TKCreation(string text)
        {
            var msg = new GameLogicMessage() { Message = text, Type = GLTypes.Turn, MsgId = this.MsgId };
            return msg.GetTC();
        }
        protected override void CopyMessageField(BaseMessage msg)
        {
            GameTurnMessage copymsg = (GameTurnMessage)msg;
            this.UserId = copymsg.UserId;
            this.EndTime = copymsg.EndTime;
            this.Text = copymsg.Text;
        }
        public override BaseMessage GetInnerMessage()
        {
            return null;
        }
    }
}
