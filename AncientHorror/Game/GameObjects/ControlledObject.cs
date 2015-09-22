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
        private ObjectController control;
        public ControlledObject(ObjectController contr)
            : base(contr.Id)
        {
            this.control = contr;
        }
        protected void SendMessage()
        {

        }
        protected async Task<PlayerCommandMessage> RecieveMessage()
        {
            
        }
        
    }
}
