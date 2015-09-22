using AncientHorror.Net;
using AncientHorrorShared.Messaging;
using AncientHorrorShared.Messaging.PlayerCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AncientHorror.Game.GameObjects
{
    public abstract class ControlledObject: GameObject
    {
        public ControlStatusEnum ControlStatus { get; set; }
        private ObjectController control;
        public ControlledObject(ObjectController contr)
            : base(contr.Id)
        {
            this.control = contr;
            ControlStatus = ControlStatusEnum.Controlled;
        }
        public void SendMessage(BaseMessage msg)
        {
            if (ControlStatus == ControlStatusEnum.Controlled)
            {
                try
                {
                    control.SendMessage(msg);
                }
                catch
                {
                    ControlStatus =  ControlStatusEnum.LostControl;
                }
            }
        }
        public void AskQuestion(BaseMessage msg, List<PCTypes> answerTypes)
        {
            if (ControlStatus == ControlStatusEnum.Controlled)
            {
                try
                {
                    control.SetMsgTypes(answerTypes);
                    control.PlayerMessage = null;
                    control.SendMessage(msg);
                }
                catch
                {
                    ControlStatus = ControlStatusEnum.LostControl;
                }
            }
        }
        public Task<PlayerCommandMessage> WaitAnswer(int timeout)
        {
            if (ControlStatus == ControlStatusEnum.Controlled)
                return control.WaitAnswer(timeout);
            else
                return new Task<PlayerCommandMessage>(() => { return null; });
        }
        public void RestoreConnect(SingleSender connect)
        {
            if (ControlStatus == ControlStatusEnum.LostControl)
                control.Reconnect(connect);
        }
        
    }
}
