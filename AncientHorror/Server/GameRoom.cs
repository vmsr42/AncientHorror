using AncientHorrorShared;
using AncientHorrorShared.Messaging.AbonentsCommand;
using AncientHorrorShared.Messaging.InfoMessage;
using AncientHorrorShared.Messaging.ConfirmMessage;
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
        public GameRoom(int id,String name, string pass, GameAbonentInfo owner, int capability)
            : base(capability, id, name, owner)
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
                        bool done = false;

                        RemoveAbonent(ab);
                            Program.Lobby.AddAbonent(ab);
                            if (ab.Gamer.Id == this.Owner.Id)
                            {
                                AfterRemoveOwner();
                            }
                            done = true;
                        if (acMsg.GetInnerMessage().NeedConfirm)
                        {
                            ServerConfirmMessage confirm = new ServerConfirmMessage() { Accept = done, RefMsgId = acMsg.MsgId };
                            var smsg = confirm.GetTC();
                            ab.SendMessage(smsg);
                        }
                        SendRoomStatusMessage();
                        Program.Lobby.SendRoomStatusMessage();
                        break;
                    }
                case AbonentsCommandType.Exit:
                    {
                        this.RemoveAbonent(ab);
                        ab.Gamer.Name = "Guest" + ab.Gamer.Id;
                        ab.Gamer.UserId = 0;
                        ab.Status = AbonentStatusEnum.Disconnected;
                        if (ab.Gamer.Id == this.Owner.Id)
                        {
                            AfterRemoveOwner();
                        }
                        SendRoomStatusMessage();
                        break;
                    }
                case AbonentsCommandType.StartRoom:
                    {
                        bool done = false;
                        if (acMsg.GetInnerMessage().NeedConfirm)
                        {
                            ServerConfirmMessage confirm = new ServerConfirmMessage() { Accept = done, RefMsgId = acMsg.MsgId };
                            var smsg = confirm.GetTC();
                            ab.SendMessage(smsg);
                        }
                        break;
                    }
                case AbonentsCommandType.KickUser:
                    {
                        bool done = false;
                        KickUserMessage kickmsg = (KickUserMessage)acMsg.GetInnerMessage();
                        Abonent kickedab = this.AbnsToList().FirstOrDefault(a => a.Gamer.Id==kickmsg.UserId);
                        if (kickedab!=null)
                        {
                            this.RemoveAbonent(kickedab);
                                Program.Lobby.AddAbonent(kickedab);
                                done = true;
                        }
                        if (acMsg.GetInnerMessage().NeedConfirm)
                        {
                            ServerConfirmMessage confirm = new ServerConfirmMessage() { Accept = done, RefMsgId = acMsg.MsgId };
                            var smsg = confirm.GetTC();
                            ab.SendMessage(smsg);
                        }
                        SendRoomStatusMessage();
                        Program.Lobby.SendRoomStatusMessage();
                        break;
                    }
 
            }
        }
        protected override void AfterRemoveOwner()
        {
            foreach (var remab in AbnsToList())
            {
                this.RemoveAbonent(remab);
                Program.Lobby.AddAbonent(remab);
            }
            Program.Rooms.Remove(this);
            Program.Lobby.SendRoomStatusMessage();

            var roomsmsg = new ServerInfoRoomsMessage() { Rooms = Program.GetRoomsInfo() };
            Program.Lobby.SendMessage(roomsmsg.GetTC());
            
        }

        protected override bool HavePassword()
        {
            return !String.IsNullOrWhiteSpace(password);
        }
    }
}
