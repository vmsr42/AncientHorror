using AncientHorror.Net;
using AncientHorror.Server;
using AncientHorrorShared;
using AncientHorrorShared.Messaging.PlayerCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AncientHorror.Game.GameObjects
{
    public class Player: GameObject
    {
        
        public object msglocker = new object();
        private int atempts = 0;
        public String Name { get; set; }
        public SingleSender Sender { get; set; }
        private PlayerCommandMessage playerMessage = null;
        public PlayerCommandMessage PlayerMessage
        {
            get
            {
                return playerMessage;
            }
            set
            {
                lock(msglocker)
                {
                    if (value!=playerMessage)
                    {
                        playerMessage = value;
                    }
                }
            }
        }
        public Player(Abonent ab): base(ab.Gamer.Id)
        {
            this.Sender = ab.Sender;
            this.Name = ab.Gamer.Name;
        }
    }
}
