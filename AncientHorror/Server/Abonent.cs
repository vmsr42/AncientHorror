﻿using AncientHorrorShared;
using AncientHorrorShared.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AncientHorror.Server
{
    public class Abonent
    {
        public GameAbonent Gamer { get; set; }
        public AbonentStatusEnum Status { get; set; }
        public Socket Sock { get; set; }
        public byte[] buffer = new byte[4096];
        public StringBuilder sb = new StringBuilder();
        public int RoomId { get; set; }
        public void SendMessage(TransportContainer msg)
        {

            DataContractSerializer writer = new DataContractSerializer(typeof(TransportContainer));
                using (MemoryStream ms = new MemoryStream())
                {
                    writer.WriteObject(ms, msg);
                    this.Sock.Send(ms.ToArray());
                }
        }
    }
}
