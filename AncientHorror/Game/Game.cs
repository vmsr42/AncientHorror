using AncientHorror.Game.GameObjects;
using AncientHorror.Server;
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
        public Game(List<Abonent> abonents )
        {
            Players = new List<Player>();
            foreach (var ab in abonents)
            {
                Players.Add(new Player(ab));
                waitingUsersId.Add(ab.Gamer.Id);                
            }
            State = StageEnum.Intitialization;
        }
        
    }
}
