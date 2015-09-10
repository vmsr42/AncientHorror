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

namespace AncientHorrorClient.Network
{
    public class AHNetworkClient
    {
        private Socket client;
        private BackgroundWorker bWorker = new BackgroundWorker();
        private GameAbonent abon = new GameAbonent();
        public GameAbonent Abonent
        {
            get
            {
                return abon;
            }
            private set
            {
                if (value!=abon)
                {
                    abon = value;
                    OnAbonentChanged(abon);
                }
            }
        }
        public delegate void AbnChgDelegate(GameAbonent ab);
        public event AbnChgDelegate AbonentChanged;
        private void OnAbonentChanged(GameAbonent abn)
        {
            var handler = AbonentChanged;
            if (handler != null)
            {
                handler(abn);
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
        public bool connected = false;
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
        public async Task<bool> SendMessage(BaseMessage msg)
        {
            bool res = false;
            if (IsConnected)
            {
                try
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(TransportContainer));
                    var tc = msg.GetTC();
                    using (var ms = new MemoryStream())
                    {
                        serializer.WriteObject(ms, tc);
                        client.Send(ms.ToArray());
                    }
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
            return res;

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
        public bool ConnectToServer(string ip, int port,out String error)
        {
            error = String.Empty;
            try
            {
                var cl = new TcpClient(ip, port);
                cl.ReceiveTimeout = timeout * 1000;
                client = cl.Client;
                connected = true;
            }
            catch(Exception ex)
            {
                error = "Не удалось соединиться с сервером по причине: " + ex.Message;
                return false;
            }
            bWorker.DoWork += bWorker_DoWork;
            bWorker.ProgressChanged += bWorker_ProgressChanged;
            bWorker.WorkerReportsProgress = true;
            bWorker.RunWorkerAsync();
            return true;
        }
        public void Disconnect()
        {
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
            }            
            client = null;
        }

        private void bWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (this.IsConnected == true)
            {
                try
                {
                    byte[] buffer = new byte[4096];

                    client.Receive(buffer);
                    using (MemoryStream ms = new MemoryStream(buffer))
                    {
                        DataContractSerializer reader = new DataContractSerializer(typeof(TransportContainer));
                        var msg = (TransportContainer)reader.ReadObject(ms);
                        bWorker.ReportProgress(0, msg);
                    }

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
            this.Abonent = tc.Sender;
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
