using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AncientHorrorShared.Messaging.GameLogic
{
    [DataContract]
    public class StartGameMessage: BaseMessage
    {
        public StartGameMessage():base(new DataContractSerializer(typeof(StartGameMessage)), false) {}
        protected override TransportContainer TKCreation(string text)
        {
            var msg = new GameLogicMessage() { Message = text, Type = GLTypes.StartGame, MsgId = this.MsgId };
            return msg.GetTC();
        }
        protected override void CopyMessageField(BaseMessage msg)
        {
        }
        public override BaseMessage GetInnerMessage()
        {
            return null;
        }
    }
}
