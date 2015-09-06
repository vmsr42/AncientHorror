using AncientHorror.Server.Messaging;
using AncientHorror.Server.Messaging.AbonentsCommand;
using AncientHorror.Server.Messaging.InfoMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AncientHorror.Server
{
    public class GameRoom: Room
    {
        private String password;
        public GameRoom(int id,String name, string pass, GameAbonent owner)
            : base(8,id,name, owner)
        {
            this.password = pass;
        }
        public bool AddAbonent(Abonent ab, String pass)
        {
            if (this.password == pass)
                return this.AddAbonent(ab);
            else
                return false;
        }


        protected override void AfterRecieveCommand(AbonentsCommandMessage acMsg, Abonent ab)
        {
            switch (acMsg.Type)
            {
                case AbonentsCommandType.ExitRoom:
                    {
                        if (this.RemoveAbonent(ab))
                            Program.Lobby.AddAbonent(ab);
                        else
                        {
                            ServerInfoErrorMessage error = new ServerInfoErrorMessage() { Error = "Не удалось покинуть комнату...мухаха" };
                            ab.SendMessage(error.GetServerMessage(ab));
                        }
                        break;
                    }
                case AbonentsCommandType.Exit:
                    {
                        ab.Gamer.Name = "Guest" + ab.Gamer.Id;
                        ab.Gamer.UserId = 0;
                        ab.Status = AbonentStatusEnum.Disconnected;
                        break;
                    }
                case AbonentsCommandType.StartRoom:
                    {
                        break;
                    }
 
            }
        }
    }
}
