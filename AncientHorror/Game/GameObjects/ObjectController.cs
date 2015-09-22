using AncientHorror.Net;
using AncientHorror.Server;
using AncientHorrorShared;
using AncientHorrorShared.Messaging;
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
        public object senderlocker = new object();
        private int atempts = 0;
        public String Name { get; set; }
        private SingleSender sender;
        private SingleSender Sender 
        {
            get
            {
                lock(senderlocker)
                {
                    return sender;
                }
            }
            set
            {
                if (value != sender)
                    sender = value;
            }
        }
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
                lock (msglocker)
                {
                    if (value != playerMessage)
                    {
                        playerMessage = value;
                    }
                }
            }
        }
        public void SendMessage(BaseMessage msg)
        {
            Sender.SendMessage(msg.GetTC());
        }
        private ConcurrentBag<PCTypes> waitngMessageTypes = new ConcurrentBag<PCTypes>();
        public ObjectController(Abonent ab)
        {
            this.Id = ab.Gamer.UserId;
            this.Sender = ab.Sender;
            this.Name = ab.Gamer.Name;
        }
        public Task<PlayerCommandMessage> WaitAnswer(int timeout)
        {
            Func<object, PlayerCommandMessage> action = new Func<object, PlayerCommandMessage>(GetPCM);
            return Task<PlayerCommandMessage>.Factory.StartNew(action, timeout);

        }
        private PlayerCommandMessage GetPCM(object time)
        {
            int timeout = (int)time;
            DateTime end = DateTime.Now.AddMilliseconds(timeout);
            bool res = false;
            PlayerCommandMessage pm = null;
            while (DateTime.Now < end && !res)
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
        public void SetMsgTypes(List<PCTypes> list)
        {
            lock (msglocker)
            {
                waitngMessageTypes = new ConcurrentBag<PCTypes>();
                if (list != null)
                {
                    foreach (var item in list)
                        waitngMessageTypes.Add(item);
                }
            }
        }
        public bool HasType(PCTypes typ)
        {
            lock (msglocker)
            {
                return waitngMessageTypes.Contains(typ);
            }
        }
        public void Reconnect(SingleSender cnct)
        {
            Sender = cnct;
        }
    }
}
