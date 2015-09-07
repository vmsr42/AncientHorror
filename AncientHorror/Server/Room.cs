using AncientHorror.Server.Messaging;
using AncientHorror.Server.Messaging.AbonentsCommand;
using AncientHorror.Server.Messaging.InfoMessage;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AncientHorror.Server
{
    public abstract class Room
    {
        public GameAbonent Owner { get; private set; }
        private String _name;
        private int _id;
        public int Id
        {
            get
            {
                return _id;
            }
        }
        public String Name
        {
            get
            {
                return _name;
            }
        }

        private int Capability = 0;
        public Boolean CanAddAbonent
        {
            get
            {
                if (Capability == 0)
                    return true;
                if (abntsList.Count < Capability)
                    return true;
                return false;
            }
        }
        public Room(int capability, int id, string name, GameAbonent own)
        {
            this.Capability = capability;
            this._id = id;
            this._name = name;
            Owner = own;
        }
        protected BlockingCollection<Abonent> abntsList = new BlockingCollection<Abonent>();
        public void SendMessage(ServerMessage msg)
        {
            foreach (var abn in abntsList)
            {
                XmlSerializer writer = new XmlSerializer(typeof(ServerMessage));
                using (MemoryStream ms = new MemoryStream())
                {
                    writer.Serialize(ms, msg);
                    abn.Sock.Send(ms.ToArray());
                }
            }
        }
        public bool AddAbonent(Abonent ab)
        {
            if (ab.Status != AbonentStatusEnum.Authorized)
                return false;
            if (CanAddAbonent)
            {
                abntsList.Add(ab);
                var msg = new ServerInfoAbonentsMessage() { };
                foreach (var abon in abntsList)
                    msg.Abonents.Add(abon.Gamer);
                this.SendMessage(msg.GetServerMessage(ab));
                ab.Sock.BeginReceive(ab.buffer, 0, 4096, SocketFlags.None, new AsyncCallback(AfterRecieve), ab);
                return true;
            }
            else
                return false;

        }

        private void AfterRecieve(IAsyncResult ar)
        {
            Abonent ab = (Abonent)ar.AsyncState;
            try
            {
                ab.Sock.EndReceive(ar);
                if (ab.Status != AbonentStatusEnum.Disconnected)
                {
                    using (MemoryStream ms = new MemoryStream(ab.buffer))
                    {
                        XmlSerializer reader = new XmlSerializer(typeof(ServerMessage));
                        ServerMessage msg = (ServerMessage)reader.Deserialize(ms);
                        switch(msg.Type)
                        { 
                            case ServerMessageType.AbonentCommand:
                                {
                                    AbonentsCommandMessage acmsg = (AbonentsCommandMessage)msg.GetInnerMessage();
                                    AfterRecieveCommand(acmsg, ab);
                                    break;
                                }
                        }
                        if (abntsList.Contains(ab))
                        {
                            ab.Sock.BeginReceive(ab.buffer, 0, 4096, 0, new AsyncCallback(AfterRecieve), ab);
                        }
                    }
                }
            }
            catch 
            {

                    ab.Status = AbonentStatusEnum.Disconnected;
                    ab.Gamer.Name = String.Empty;
                    ab.Gamer.Id = 0;
            }
            try
            {
                if (ab.Status == AbonentStatusEnum.Disconnected)
                {
                    ab.Sock.Close();
                    this.abntsList.TryTake(out ab);
                    if (ab.Gamer.Id==this.Owner.Id)
                        AfterRemoveOwner()
                }
            }
            catch { }

        }

        protected abstract void AfterRemoveOwner();
        public bool RemoveAbonent(Abonent ab)
        {
            if (abntsList.TryTake(out ab))
            {
                var msg = new ServerInfoAbonentsMessage() { };
                foreach (var abon in abntsList)
                    msg.Abonents.Add(abon.Gamer);
                this.SendMessage(msg.GetServerMessage(ab));
                return true;
            }
            else
                return false;
        }
        protected abstract void AfterRecieveCommand(AbonentsCommandMessage acMsg, Abonent ab);
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         