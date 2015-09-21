using AncientHorror.Game.GameObjects;
using AncientHorror.Server;
using AncientHorrorShared.Messaging.PlayerCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AncientHorror.Game
{
    public class Game
    {
        private List<Player> Players { get; set; }
        public StageEnum State { get; set; }
        private List<int> waitingUsersId = new List<int>();
        private List<PCTypes> waitingtypes = new List<PCTypes>();
        public Game(List<Abonent> abonents )
        {
            Players = new List<Player>();
            foreach (var ab in abonents)
            {
                Players.Add(new Player(ab));
                waitingUsersId.Add(ab.Gamer.Id);                
            }
        }
        public bool AddUserMessage(int userId, PlayerCommandMessage msg)
        {
            if (!waitingUsersId.Contains(userId)|!waitingtypes.Contains(msg.Type))
                return false;
            var player = Players.FirstOrDefault(p=>p.Id==userId);
            if (player.PlayerMessage!=null)
                return false;
            player.PlayerMessage=msg;
            return true;
        }
        public void Start()
        {
            
        }
        private void GameLogic
        
    }
}
