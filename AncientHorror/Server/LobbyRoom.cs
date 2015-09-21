using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AncientHorrorShared.Messaging.AbonentsCommand;
using AncientHorrorShared.Messaging.InfoMessage;
using AncientHorrorShared;
using AncientHorrorShared.Messaging.ConfirmMessage;
namespace AncientHorror.Server
{
    public class LobbyRoom: Room
    {
        int usrid = 1;
        private int roomnumber = 1;
        public LobbyRoom(): base(0,0,"Lobby", null)
        { }



        protected override void AfterRecieveCommand(AbonentsCommandMessage acMsg, Abonent ab)
        {
            switch (acMsg.Type)
            {
                case AbonentsCommandType.Authorization:
                    {
                        bool done = false;
                        AuthorizationMessage amsg = (AuthorizationMessage)acMsg.GetInnerMessage();
                        //реализация логики авторизации пока пусть будет типа авторизован
                        ab.Status = AbonentStatusEnum.Authorized;
                        ab.Gamer.Id = usrid;
                        ab.Gamer.UserId = usrid;
                        usrid++;
                        ab.Gamer.Name = amsg.Login;
                        done = true;
                        if (amsg.NeedConfirm)
                        {
                            ServerConfirmMessage confirm = new ServerConfirmMessage() { Accept = done, RefMsgId = acMsg.MsgId };
                            var smsg = confirm.GetTC();
                            ab.SendMessage(smsg);
                        }
                        SendRoomStatusMessage();

                        break;
                    }
                case AbonentsCommandType.UnAuthorization:
                    {
                        bool done = false; ;

                        ab.Gamer.Name = "Guest" + ab.Gamer.Id;
                        ab.Gamer.UserId = 0;
                        ab.Status = AbonentStatusEnum.Guest;
                        done = true;
                        if (acMsg.GetInnerMessage().NeedConfirm)
                        {
                            ServerConfirmMessage confirm = new ServerConfirmMessage() { Accept = done, RefMsgId = acMsg.MsgId };
                            var smsg = confirm.GetTC();
                            ab.SendMessage(smsg);
                        }
                        SendRoomStatusMessage();
                        break;
                    }
                case AbonentsCommandType.Exit:
                    {
                        RemoveAbonent(ab);
                        ab.Gamer.Name = "Guest" + ab.Gamer.Id;
                        ab.Gamer.UserId = 0;
                        ab.Status = AbonentStatusEnum.Disconnected;
                        SendRoomStatusMessage();
                        break;
                    }
                case AbonentsCommandType.CreateRoom:
                    {
                        bool done = false;
                        CreateRoomMessage crmsg = (CreateRoomMessage)acMsg.GetInnerMessage();
                        GameRoom room = new GameRoom(roomnumber, crmsg.Name, crmsg.Password, ab.Gamer, crmsg.Capability);
                        Program.Rooms.Add(room);
                        RemoveAbonent(ab);
                        room.AddAbonent(ab);
                            done = true;
                        if (acMsg.GetInnerMessage().NeedConfirm)
                        {
                            ServerConfirmMessage confirm = new ServerConfirmMessage() { Accept = done, RefMsgId = acMsg.MsgId };
                            var smsg = confirm.GetTC();
                            ab.SendMessage(smsg);
                        }
                        ab.CurrentRoom.SendRoomStatusMessage();
                        SendRoomStatusMessage();
                        var roomsmsg = new ServerInfoRoomsMessage() { Rooms = Program.GetRoomsInfo() };
                        Program.Lobby.SendMessage(roomsmsg.GetTC());
                        break;
                    }
                case AbonentsCommandType.JoinRoom:
                    {
                        bool done = false;
                        JoinRoomMessage jrmsg = (JoinRoomMessage)acMsg.GetInnerMessage();
                        if (jrmsg.RoomId > 0)
                        {
                            GameRoom room = (GameRoom)Program.Rooms.FirstOrDefault(r => r.Id == jrmsg.RoomId);
                            if (room != null)
                            {
                                this.RemoveAbonent(ab);
                                    room.AddAbonent(ab, jrmsg.Password);
                                    done = true;
                            }
                        }
                        if (acMsg.GetInnerMessage().NeedConfirm)
                        {
                            ServerConfirmMessage confirm = new ServerConfirmMessage() { Accept = done, RefMsgId = acMsg.MsgId };
                            var smsg = confirm.GetTC();
                            ab.SendMessage(smsg);
                        }
                        SendRoomStatusMessage();
                        break;
                    }
                case AbonentsCommandType.RequestRoomInfo:
                    {
                        ServerInfoRoomsMessage riMessage = new ServerInfoRoomsMessage() { Rooms = Program.GetRoomsInfo() };
                        var smsg = riMessage.GetTC();
                        ab.SendMessage(smsg);
                        break;
                    }

            }
        }

        protected override void AfterRemoveOwner()
        {
            
        }

        protected override bool HavePassword()
        {
            return false;
        }
    }
}
