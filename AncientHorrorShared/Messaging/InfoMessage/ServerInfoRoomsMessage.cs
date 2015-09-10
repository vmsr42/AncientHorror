﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AncientHorrorShared.Messaging.InfoMessage
{
    [DataContract]
    public class ServerInfoRoomsMessage: BaseMessage
    {
        [DataMember]
        public List<int> RoomIds { get; set; }
        [DataMember]
        public List<String> RoomNames { get; set; }
        [DataMember]
        public List<GameAbonent> Owners { get; set; }
        public ServerInfoRoomsMessage() : base(new DataContractSerializer(typeof(ServerInfoRoomsMessage)),false) { }
        protected override TransportContainer TKCreation(string text)
        {
            var simmsg = new ServerInfoMessage() { Message = text, Type = SIMessageType.Rooms, MsgId = this.MsgId };
            return simmsg.GetTC();
        }
        protected override void CopyMessageField(BaseMessage msg)
        {
            ServerInfoRoomsMessage copymsg = (ServerInfoRoomsMessage)msg;
            this.RoomIds = copymsg.RoomIds;
            this.RoomNames = copymsg.RoomNames;
            this.Owners = copymsg.Owners;
        }
        public override BaseMessage GetInnerMessage()
        {
            return null;
        }
    }
    
}
