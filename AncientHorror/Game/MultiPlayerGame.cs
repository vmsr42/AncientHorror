using AncientHorror.Game.GameObjects;
using AncientHorror.Server;
using AncientHorrorShared.Messaging.PlayerCommands;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using AncientHorror.Game.Logic;
using AncientHorrorShared.Messaging.GameLogic;

namespace AncientHorror.Game
{
    public class MultiPlayerGame
    {
        private DateTime gameTime = DateTime.Now;
        public DateTime GameTime
        {
            get
            {
                return gameTime;
            }
        }
        private List<ObjectController> Controllers { get; set; }
        private GameLogic Logic;
        public MultiPlayerGame(List<Abonent> abonents, GameLogic logic)
        {
            Logic = logic;
            Controllers = new List<ObjectController>();
            foreach (var ab in abonents)
                Controllers.Add(new ObjectController(ab));

        }
        public bool AddUserMessage(int userId, PlayerCommandMessage msg)
        {
            bool res = false;
            var contrl = Controllers.FirstOrDefault(c => c.Id == userId);
            if (contrl!=null)
            {
                if (contrl.HasType(msg.Type)&&contrl.PlayerMessage!=null)
                {
                    contrl.SetMsgTypes(null);
                    contrl.PlayerMessage = msg;
                    res = true;
                }
            }
            return res;
        }
        public void Start(Action<Task> after)
        {
            foreach (var contr in Controllers)
            {
                contr.SetMsgTypes(null);
                contr.SendMessage(new StartGameMessage());
            }
            Logic.InitGameObjects(Controllers);
            Task.Factory.StartNew(Logic.Run).ContinueWith(after);
            
        }

        
        
        
    }
}
