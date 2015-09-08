﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AncientHorrorShared.Messaging.AbonentsCommand;
using AncientHorrorShared.Messaging.InfoMessage;
using AncientHorrorShared;
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
                        AuthorizationMessage amsg = (AuthorizationMessage)acMsg.GetInnerMessage();
                        break;
                    }
                case AbonentsCommandType.UnAuthorization:
                    {
                        ab.Gamer.Name = "Guest" + ab.Gamer.Id;
                        ab.Gamer.UserId = 0;
                        ab.Status = AbonentStatusEnum.Guest;
                        break;
                    }
                case AbonentsCommandType.Exit:
                    {
                        ab.Gamer.Name = "Guest" + ab.Gamer.Id;
                        ab.Gamer.UserId = 0;
                        ab.Status = AbonentStatusEnum.Disconnected;
                        break;
                    }
                case AbonentsCommandType.CreateRoom:
                    {
                        CreateRoomMessage crmsg = (CreateRoomMessage)acMsg.GetInnerMessage();
                        GameRoom room = new GameRoom(roomnumber, crmsg.Name, crmsg.Password, ab.Gamer);
                        if (room.AddAbonent(ab))
                        {
                            this.RemoveAbonent(ab);
                            Program.Rooms.Add(room);
                        }
                        else
                        {
                            ServerInfoErrorMessage error = new ServerInfoErrorMessage() { Error = "Не удалось создать комнату" };
                            var smsg = error.GetTC();
                            smsg.Sender = new GameAbonent() { Id = -1, Name = "Server" };
                            ab.SendMessage(smsg);
                        }
                        
                        break;
                    }
                case AbonentsCommandType.JoinRoom:
                    {
                        JoinRoomMessage jrmsg = (JoinRoomMessage)acMsg.GetInnerMessage();
                        if (jrmsg.RoomId > 0)
                        {
                            GameRoom room = (GameRoom)Program.Rooms.FirstOrDefault(r => r.Id == jrmsg.RoomId);
                            if (room.AddAbonent(ab, jrmsg.Password))
                                this.RemoveAbonent(ab);
                            else
                            {
                                ServerInfoErrorMessage error = new ServerInfoErrorMessage() { Error = "Не удалось подключиться к комнате" };
                                var smsg = error.GetTC();
                                smsg.Sender = new GameAbonent() { Id = -1, Name = "Server" };
                                ab.SendMessage(smsg);
                            }
                        }
                        break;
                    }
                case AbonentsCommandType.RequestRoomInfo:
                    {
                        ServerInfoRoomsMessage riMessage = new ServerInfoRoomsMessage() { RoomIds = new List<int>(), RoomNames = new List<String>(), Owners = new List<GameAbonent>() };
                        foreach (var room in Program.Rooms)
                            if (room.Id > 0)
                            {
                                riMessage.RoomIds.Add(room.Id);
                                riMessage.RoomNames.Add(room.Name);
                                riMessage.Owners.Add(room.Owner);
                            }
                        var smsg = riMessage.GetTC();
                        smsg.Sender = new GameAbonent() { Id = -1, Name = "Server" };
                        ab.SendMessage(smsg);
                        break;
                    }

            }
        }

        protected override void AfterRemoveOwner()
        {
            
        }
    }
}
