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
                        ab.Gamer.Id = 1;
                        ab.Gamer.UserId = 1;
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
                        if (this.RemoveAbonent(ab))
                        {
                            room.AddAbonent(ab);
                            done = true;
                        }
                        else
                        {
                            this.AddAbonent(ab);
                            done = false;
                        }
                        if (acMsg.GetInnerMessage().NeedConfirm)
                        {
                            ServerConfirmMessage confirm = new ServerConfirmMessage() { Accept = done, RefMsgId = acMsg.MsgId };
                            var smsg = confirm.GetTC();
                            ab.SendMessage(smsg);
                        }
                        ab.CurrentRoom.SendRoomStatusMessage();
                        SendRoomStatusMessage();
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
                                if (this.RemoveAbonent(ab))
                                {
                                    room.AddAbonent(ab, jrmsg.Password);
                                    done = true;
                                }
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
                        ServerInfoRoomsMessage riMessage = new ServerInfoRoomsMessage() { Rooms = new List<GameRoomInfo>() };
                        foreach (var room in Program.Rooms)
                            if (room.Id > 0)
                            {
                                riMessage.Rooms.Add(room.GetGameRoomInfo());
                            }
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
