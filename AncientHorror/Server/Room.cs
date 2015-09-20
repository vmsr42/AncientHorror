using AncientHorrorShared;
using AncientHorrorShared.Messaging;
using AncientHorrorShared.Messaging.AbonentsCommand;
using AncientHorrorShared.Messaging.InfoMessage;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace AncientHorror.Server
{
    public abstract class Room
    {
        public GameAbonentInfo Owner { get; private set; }
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
        private int capacity = 0;
        private int Capability = 0;
        public Boolean CanAddAbonent
        {
            get
            {
                if (Capability == 0)
                    return true;
                if (capacity < Capability)
                    return true;
                return false;
            }
        }
        public Room(int capability, int id, string name, GameAbonentInfo own)
        {
            this.Capability = capability;
            this._id = id;
            this._name = name;
            Owner = own;
        }
        object listlocker = new object();
        private List<Abonent> abnsList = new List<Abonent>();
        protected void AddToAbList(Abonent ab)
        {
            lock(listlocker)
            {
                abnsList.Add(ab);
            }
        }
        protected List<Abonent> AbnsToList()
        {
            lock(listlocker)
            {
                return abnsList;
            }
        }
        protected void RemoveFromAbList(Abonent ab)
        {
            lock(listlocker)
            {
                abnsList.Remove(ab);
            }
        }
        public void SendMessage(TransportContainer msg)
        {
            msg.Room = this.GetGameRoomInfo();
            foreach (var abn in AbnsToList())
            {
                abn.SendMessage(msg);
            }
        }
        public bool AddAbonent(Abonent ab)
        {
            try
            {
                if (ab.Status != AbonentStatusEnum.Authorized && Id != 0)
                    return false;
                if (CanAddAbonent)
                {
                    AddToAbList(ab);
                    ab.CurrentRoom = this;
                    capacity++;
                    ab.Sender.Sock.BeginReceive(ab.Sender.buffer, 0, 4096, SocketFlags.None, new AsyncCallback(AfterRecieve), ab);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public void SendRoomStatusMessage()
        {
            var msg = new ServerInfoAbonentsMessage() { Abonents = new List<GameAbonentInfo>() };
            foreach (var abon in AbnsToList())
                msg.Abonents.Add(abon.Gamer);
            var smsg = msg.GetTC();
            this.SendMessage(smsg);
        }




        private void AfterRecieve(IAsyncResult ar)
        {
            Abonent ab = (Abonent)ar.AsyncState;

                try
                {
                    ab.Sender.Sock.EndReceive(ar);
                    if (ab.Status != AbonentStatusEnum.Disconnected)
                    {
                        
                            try
                            {
                                string utf8 = Encoding.UTF8.GetString(ab.Sender.buffer);

                                char[] removed = new char[1];
                                removed[0] = (char)0;
                                utf8 = utf8.Trim(removed);

                                TransportContainer msg = new TransportContainer();
                                msg.UTFDeSerialize(utf8);
                                switch (msg.Type)
                                {
                                    case TCTypes.AbonentCommand:
                                        {
                                            AbonentsCommandMessage acmsg = (AbonentsCommandMessage)msg.GetInnerMessage();
                                            AfterRecieveCommand(acmsg, ab);
                                            break;
                                        }
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                            ab.Sender.ClearBuffer();
                            if (this.AbnsToList().Contains(ab))
                            {
                                ab.Sender.ClearBuffer();
                                ab.Sender.Sock.BeginReceive(ab.Sender.buffer, 0, 4096, 0, new AsyncCallback(AfterRecieve), ab);
                                return;
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
                        ab.Sender.Sock.Close();
                        RemoveFromAbList(ab);
                        if (ab.Gamer.Id == this.Owner.Id)
                            AfterRemoveOwner();
                    }
                }
                catch { }

        }

        protected abstract void AfterRemoveOwner();
        public void RemoveAbonent(Abonent ab)
        {
            RemoveFromAbList(ab);
            
                ab.CurrentRoom = null;
                capacity--;
           
        }
        public GameRoomInfo GetGameRoomInfo()
        {
            var info = new GameRoomInfo();
            info.Id = this.Id;
            info.Name = this.Name;
            info.Owner = this.Owner;
            info.HavePassword = HavePassword();
            info.Capability = this.Capability;
            info.Capacity = this.capacity;
            return info;
        }
        protected abstract void AfterRecieveCommand(AbonentsCommandMessage acMsg, Abonent ab);
        protected abstract Boolean HavePassword();
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         