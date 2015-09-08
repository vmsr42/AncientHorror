using AncientHorrorShared;
using AncientHorrorShared.Messaging.AbonentsCommand;
using AncientHorrorShared.Messaging.InfoMessage;
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
                            if (ab.Gamer.Id == this.Owner.Id)
                            {
                                AfterRemoveOwner();
                            }
                        else
                        {
                            ServerInfoErrorMessage error = new ServerInfoErrorMessage() { Error = "Не удалось покинуть комнату...мухаха" };
                            var smsg = error.GetTC();
                            smsg.Sender = new GameAbonent() { Id = -1, Name = "Server" };
                            ab.SendMessage(smsg);
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
                case AbonentsCommandType.KickUser:
                    {
                        KickUserMessage kickmsg = (KickUserMessage)acMsg.GetInnerMessage();
                        Abonent kickedab = this.abntsList.FirstOrDefault(a => a.Gamer.Id==kickmsg.UserId);
                        if (kickedab!=null)
                        {
                            if (this.RemoveAbonent(kickedab))
                                Program.Lobby.AddAbonent(kickedab);
                        }
                        break;
                    }
 
            }
        }
        protected override void AfterRemoveOwner()
        {
            foreach (var remab in abntsList)
            {
                this.RemoveAbonent(remab);
                Program.Lobby.AddAbonent(remab);
            }
            Program.Rooms.Remove(this);
        }
    }
}
