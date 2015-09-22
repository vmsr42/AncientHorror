using AncientHorror.Net;
using AncientHorror.Server;
using AncientHorrorShared;
using AncientHorrorShared.Messaging.PlayerCommands;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AncientHorror.Game.GameObjects
{
    public class ObjectController
    {
        public int Id { get; set; }
        public object msglocker = new object();
        private int atempts = 0;
        public String Name { get; set; }
        public SingleSender Sender { get; set; }
        private PlayerCommandMessage playerMessage = null;
        public PlayerCommandMessage PlayerMessage
        {
            get
            {
                lock (msglocker)
                {
                    return playerMessage;
                }
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
        private ConcurrentBag<PCTypes> waitngMessageTypes = new ConcurrentBag<PCTypes>();
        public ConcurrentBag<PCTypes> WaitngMessageTypes
        {
            get
            {
                lock (msglocker)
                {
                    return waitngMessageTypes;
                }
            }
            set
            {
                lock (msglocker)
                {
                    if (value != waitngMessageTypes)
                    {
                        waitngMessageTypes = value;
                    }
                }
            }
        }
        public ObjectController(Abonent ab)
        {
            this.Id = ab.Gamer.UserId;
            this.Sender = ab.Sender;
            this.Name = ab.Gamer.Name;
        }
        public async Task<PlayerCommandMessage> WaitAnswer(int timeout)
        {
            PlayerMessage = null;
            Func<int, PlayerCommandMessage> action = new Func<int, PlayerCommandMessage>(GetPCM);
            Task<PlayerCommandMessage>.Factory.StartNew(action(timeout));
            
            
        }
        private PlayerCommandMessage GetPCM(int timeout)
        {
            DateTime end = DateTime.Now.AddMilliseconds(timeout);
            bool res = false;
            PlayerCommandMessage pm = null;
            while (DateTime.Now<end&&!res)
            {
                if (PlayerMessage != null)
                {
                    pm = PlayerMessage;
                    res = true;
                }
                else
                    Thread.Sleep(100);
            }
            return pm;
        }
    }
}
