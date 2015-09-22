using AncientHorror.Game.GameObjects;
using AncientHorror.Server;
using AncientHorrorShared.Messaging.PlayerCommands;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AncientHorror.Game.Enviroment;

namespace AncientHorror.Game.Logic
{
    public class Game
    {
        private DateTime gameTime = DateTime.Now;
        public DateTime GameTime
        {
            get
            {
                return gameTime;
            }
        }
        public GameEnviroment GameEnviroment { get; private set; }
        private List<ObjectController> Controllers { get; set; }
        private GameLogic GameLogic;
        public Game(List<Abonent> abonents, GameLogic logic )
        {
            logic = GameLogic;
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
                if (contrl.WaitngMessageTypes.Contains(msg.Type)&&contrl.PlayerMessage!=null)
                {
                    contrl.WaitngMessageTypes = new ConcurrentBag<PCTypes>();
                    contrl.PlayerMessage = msg;
                    res = true;
                }
            }
            return res;
        }
        public void Start()
        {
            GameEnviroment = GameLogic.InitGameObjects();
            
        }

        
        
        
    }
}
