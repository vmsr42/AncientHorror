using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AncientHorrorShared.Messaging.GameLogic
{
    [DataContract]
    public class GameStateMessage : BaseMessage
    {
        [DataMember]
        public String StateName { get; set; }
        [DataMember]
        public String Text { get; set; }
        public GameStateMessage() : base(new DataContractSerializer(typeof(GameStateMessage)), false) { }
        protected override TransportContainer TKCreation(string text)
        {
            var msg = new GameLogicMessage() { Message = text, Type = GLTypes.State, MsgId = this.MsgId };
            return msg.GetTC();
        }
        protected override void CopyMessageField(BaseMessage msg)
        {
            GameStateMessage copymsg = (GameStateMessage)msg;
            this.StateName = copymsg.StateName;
            this.Text = copymsg.Text;
        }
        public override BaseMessage GetInnerMessage()
        {
            return null;
        }
    }
}
