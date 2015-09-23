using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AncientHorrorShared.Messaging.GameLogic
{
    [DataContract]
    public class GameLogicMessage: BaseMessage
    {
        [DataMember]
        public GLTypes Type { get; set; }
        [DataMember]
        public String Message { get; set; }
        public GameLogicMessage() : base(new DataContractSerializer(typeof(GameLogicMessage)), true) { }

        public override BaseMessage GetInnerMessage()
        {

            switch (Type)
            {
                case GLTypes.StartGame:
                    {
                        return new StartGameMessage() { MsgId = this.MsgId };
                    }
                case GLTypes.EndGame:
                    {
                        EndGameMessage msg = new EndGameMessage();
                        msg.UTFDeSerialize(this.Message);
                        return msg;
                    }
                case GLTypes.State:
                    {
                        return new ExitRoomMessage() { MsgId = this.MsgId };
                    }
                
            }
            return null;
        }

        protected override void CopyMessageField(BaseMessage msg)
        {
            GameLogicMessage copymsg = (GameLogicMessage)msg;
            this.Message = copymsg.Message;
            this.Type = copymsg.Type;
        }

        protected override TransportContainer TKCreation(string text)
        {
            var msg = new TransportContainer() { Message = text, Type = TCTypes.PlayerCommand, MsgId = this.MsgId };
            return msg;
        }
    }
}
