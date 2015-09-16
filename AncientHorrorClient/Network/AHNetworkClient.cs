using AncientHorrorShared;
using AncientHorrorShared.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization;
using AncientHorrorShared.Messaging.InfoMessage;
using AncientHorrorShared.Messaging.ConfirmMessage;
using System.Threading;
using AncientHorrorShared.Messaging.AbonentsCommand;
using AncientHorrorClient.Helpers;

namespace AncientHorrorClient.Network
{
    public class AHNetworkClient
    {
        public delegate void disconnectDelegate();
        public event disconnectDelegate Disconnected;
        private void OnDisconnected()
        {
            var handler = Disconnected;
            if (handler != null)
            {
                handler();
            }
        }
        private Socket client;
        private BackgroundWorker bWorker = new BackgroundWorker();
        private GameAbonentInfo abon = new GameAbonentInfo();
        public GameAbonentInfo Abonent
        {
            get
            {
                return abon;
            }
            private set
            {
                if (value==null||abon==null)
                {
                    if (abon!=value)
                    {
                        abon = value;
                        OnAbonentChanged(abon);
                    }
                }
                if (!value.Equals(abon))
                {
                    abon = value;
                    OnAbonentChanged(abon);
                }
            }
        }
        public delegate void AbnChgDelegate(GameAbonentInfo ab);
        public event AbnChgDelegate AbonentChanged;
        private void OnAbonentChanged(GameAbonentInfo ab)
        {
            var handler = AbonentChanged;
            if (handler != null)
            {
                handler(ab);
            }
        }
        private GameRoomInfo room = new GameRoomInfo() { Id = -2 };
        public GameRoomInfo Room
        {
            get
            {
                return room;
            }
            private set
            {
                if (value == null || room == null)
                {
                    if (room != value)
                    {
                        room = value;
                        OnRoomChanged(room);
                    }
                }
                if (!value.Equals(room))
                {
                    room = value;
                    OnRoomChanged(room);
                }
            }
        }
        public delegate void RoomChgDelegate(GameRoomInfo ab);
        public event RoomChgDelegate RoomChanged;
        private void OnRoomChanged(GameRoomInfo rm)
        {
            var handler = RoomChanged;
            if (handler != null)
            {
                handler(rm);
            }
        }
        private void OnRoomsMessageRecieved(BaseMessage msg)
        {
            var handler = RoomsMessageRecieved;
            if (handler != null)
            {
                handler(msg);
            }
        }
        public delegate void RoomsMsgDelegate(BaseMessage msg);
        public event RoomsMsgDelegate RoomsMessageRecieved;
        private ConcurrentBag<TimedConfirmMessage> ConfirmList = new ConcurrentBag<TimedConfirmMessage>();
        private int timeout = 2;
        private bool connected = false;
        public bool IsConnected
        {
            get
            {
                return connected;
            }
        }
        public AHNetworkClient(int waitingtime)
        {
            timeout = waitingtime;
        }
        public async Task<ClientAnswer> SendMessage(BaseMessage msg)
        {
            string errmsg = String.Empty;
            bool res = false;
            if (IsConnected)
            {
                try
                {
                    var tc = msg.GetTC();
                    tc.User = Abonent;
                    tc.Room = Room;
                    string utf8 = tc.UTFSerialize();
                    byte[] data = Encoding.UTF8.GetBytes(utf8);
                    client.Send(data);
                    if (msg.NeedConfirm)
                    {
                        res = await Task.Run<bool>(() =>
                        {
                            return GetConfirmationWithWaiting(timeout, msg.MsgId);
                        });
                    }
                    else
                        res = true;
                }
                //Добавить обнаружение дисконекта
                catch 
                {
                }
            }
            return new ClientAnswer() { Message=errmsg, Result=res };

        }
        private bool FindMessageByRefWithClearingOverdue(Guid refId, out bool result)
        {
            bool find = false;
            result = false;
            for (int i = 0; i < ConfirmList.Count;i++ )
            {
                bool needdel = false;
                var cm = ConfirmList.ElementAt(i);
                if (cm.Referer == refId)
                {
                    find = true;
                    needdel = true;
                    result = cm.Confirmation;
                }
                if (!needdel)
                {
                    var time = DateTime.Now - cm.RecievedTime;
                    var waittime = TimeSpan.FromSeconds(timeout);
                    if (time > waittime)
                    {
                        needdel = true;
                    }
                }
                if (needdel)
                    ConfirmList.TryTake(out cm);
            }
            return find;
        }
        private bool GetConfirmationWithWaiting(int seconds, Guid refId)
        {
            DateTime start = DateTime.Now;
            DateTime current = DateTime.Now;
            bool hasconfirm = false;
            bool answer = false;
            var waittime = TimeSpan.FromSeconds(seconds);
            while (current-start<waittime&&!hasconfirm)
            {
                hasconfirm = FindMessageByRefWithClearingOverdue(refId, out answer);
                Thread.Sleep(100);
                current = DateTime.Now;
            }
            return answer;
        }
        public async Task<ClientAnswer> ConnectToServer(string login, string password)
        {
            try
            {
                Room = new GameRoomInfo() { Id = -2 };
                Abonent = new GameAbonentInfo();
                Global.Status = BusyMessageEnum.Connecting;
                var cl = new TcpClient(Global.servLink, Global.servPort);
                cl.ReceiveTimeout = timeout * 1000;
                client = cl.Client;
                connected = true;
            }
            catch(Exception ex)
            {
                Global.Status = BusyMessageEnum.None;
                return new ClientAnswer() { Result=false, Message=ex.Message };
            }
            bWorker.DoWork += bWorker_DoWork;
            bWorker.ProgressChanged += bWorker_ProgressChanged;
            bWorker.WorkerReportsProgress = true;
            bWorker.RunWorkerAsync();
            Global.Status = BusyMessageEnum.Authorizing;
            var answ = await this.SendMessage(new AuthorizationMessage() { Login = login, Password = password });
            string msg = String.Empty;
            Global.Status = BusyMessageEnum.None;
            if (!answ.Result)
            {
                this.Disconnect();
                answ.Message = "Неправильный логин или пароль";
            }
            return answ;
        }
        public async void Disconnect()
        {
            if (IsConnected)
            {
                await this.SendMessage(new ExitMessage());
                connected = false;
                bWorker.DoWork -= bWorker_DoWork;
                bWorker.ProgressChanged -= bWorker_ProgressChanged;
                try
                {
                    client.Disconnect(false);
                }
                finally
                {
                    client.Dispose();
                    Room = new GameRoomInfo() { Id = -2 };
                    Abonent = new GameAbonentInfo();
                }
                client = null;
                OnDisconnected();
            }
        }

        private void bWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (this.IsConnected == true)
            {
                byte[] buffer = new byte[4096];
                try
                {

                    client.Receive(buffer);
                    
                    string utf8 = Encoding.UTF8.GetString(buffer);
                    TransportContainer msg = new TransportContainer();
                    msg.UTFDeSerialize(utf8);
                    bWorker.ReportProgress(0, msg);

                }
                    //Добавить обнаружение дисконекта
                catch
                {

                }
            }
        }

        private void bWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            TransportContainer tc = (TransportContainer)e.UserState;
            this.Abonent = tc.User;
            this.Room = tc.Room;
            this.Room.Capacity = tc.Room.Capacity;
            switch(tc.Type)
            {
                case TCTypes.Confirm:
                    {
                        ServerConfirmMessage cm = (ServerConfirmMessage)tc.GetInnerMessage();
                        TimedConfirmMessage tcm = new TimedConfirmMessage(cm);
                        ConfirmList.Add(tcm);
                        break;
                    }
                case TCTypes.Info:
                    {
                        ServerInfoMessage msg = (ServerInfoMessage)tc.GetInnerMessage();
                        OnRoomsMessageRecieved(msg);
                        break;
                    }

            }
            
        }
    }
}
