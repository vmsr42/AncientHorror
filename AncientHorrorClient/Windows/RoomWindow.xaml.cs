using AncientHorrorShared;
using AncientHorrorShared.Messaging;
using AncientHorrorShared.Messaging.InfoMessage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AncientHorrorClient.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class RoomWindow : BaseWindow
    {
        
        private string error = String.Empty;
        public String Error
        {
            get
            {
                return error;
            }
            set
            {
                if (error!=value)
                {
                    error = value;
                    OnPropertyChanged("Error");
                }
            }
        }
        private GameAbonentInfo abon = Global.NetworkClient.Abonent;
        public GameAbonentInfo Abonent
        {
            get
            {
                return abon;
            }
            set
            {
                if (abon!=value)
                {
                    abon = value;
                    OnPropertyChanged("Abonent");
                }
            }
        }
        private GameRoomInfo room = Global.NetworkClient.Room;
        public GameRoomInfo Room
        {

            get
            {
                return room;
            }
            set
            {
                if (room != value)
                {
                    room = value;
                    OnPropertyChanged("Room");
                    OnPropertyChanged("IsLobby");
                }
            }
        }
        public ObservableCollection<GameRoomInfo> Rooms { get; set; }
        public ObservableCollection<GameAbonentInfo> Abonents { get; set; }
        public ObservableCollection<ChatMessage> Messages { get; set; }
        public RoomWindow()
            : base(false)
        {
            Messages = new ObservableCollection<ChatMessage>();
            Rooms = new ObservableCollection<GameRoomInfo>();
            Rooms.Add(new GameRoomInfo() { Name = "ssss", Id = 11, HavePassword = true, Owner = new GameAbonentInfo() { Name = "dddd", Id = 4 } });
            Abonents = new ObservableCollection<GameAbonentInfo>();
            Global.NetworkClient.AbonentChanged += NetworkClient_AbonentChanged;
            Global.NetworkClient.RoomChanged+=NetworkClient_RoomChanged;
            Global.NetworkClient.RoomsMessageRecieved+=NetworkClient_RoomsMessageRecieved;
            Rooms.CollectionChanged+=Rooms_CollectionChanged;
            Abonents.CollectionChanged+=Abonents_CollectionChanged;
            InitializeComponent();
        }

        private void Abonents_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems!=null)
                foreach (var item in e.NewItems)
                {
                    GameAbonentInfo abn = (GameAbonentInfo)item;
                    if (abn.Id > 0 && abn.Id != Abonent.Id)
                        Messages.Add(new ChatMessage() { Abonent = abn, Roomid = Room.Id, RoomName = Room.Name, Time = DateTime.Now, Message = "вошел в комнату " });
                }
            if (e.OldItems != null)
                foreach (var item in e.OldItems)
                {
                    GameAbonentInfo abn = (GameAbonentInfo)item;
                    if (abn.Id > 0 && abn.Id != Abonent.Id)
                        Messages.Add(new ChatMessage() { Abonent = abn, Roomid = Room.Id, RoomName = Room.Name, Time = DateTime.Now, Message = "вышел из комнаты "  });
                }
        }

        private void Rooms_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (Room.IsLobby)
            {
                foreach (var item in e.NewItems)
                {
                    GameRoomInfo rm = (GameRoomInfo)item;
                    Messages.Add(new ChatMessage() { Abonent = rm.Owner, Roomid = rm.Id, RoomName = rm.Name, Time = DateTime.Now, Message = "создал комнату "  });
                }
                foreach (var item in e.OldItems)
                {
                    GameRoomInfo rm = (GameRoomInfo)item;
                    Messages.Add(new ChatMessage() { Abonent = rm.Owner, Roomid = rm.Id, RoomName = rm.Name, Time = DateTime.Now, Message = "удалил комнату "  });
                }
            }
        }

        private void NetworkClient_RoomChanged(GameRoomInfo rm)
        {
            if (Room.Id>-1)
                Messages.Add(new ChatMessage() { Abonent = Abonent, Roomid = Room.Id, RoomName = Room.Name, Time = DateTime.Now, Message = "вышел из комнаты" });
            Room = rm;
            if (Room.Id > -1)
                Messages.Add(new ChatMessage() { Abonent = Abonent, Roomid = Room.Id, RoomName = Room.Name, Time = DateTime.Now, Message = "вошел в комнату" });
        }

        private void NetworkClient_AbonentChanged(GameAbonentInfo ab)
        {
            if (Abonent.Id > 0&&ab!=null)
                Messages.Add(new ChatMessage(){ Abonent = Abonent, Roomid = Room.Id, RoomName = Room.Name, Time = DateTime.Now, Message = "сменил имя на " + ab.Name });
            Abonent = ab;    
        }
        private void AbonentChanged(GameAbonentInfo ab, GameAbonentInfo newab)
        {
            bool needupdate = false;
            if (ab.Name != newab.Name)
            {
                needupdate = true;
                Messages.Add(new ChatMessage() { Abonent = new GameAbonentInfo() { Id = ab.Id, Name = ab.Name, UserId = ab.UserId }, Roomid = Room.Id, RoomName = Room.Name, Time = DateTime.Now, Message = "сменил имя на "+ newab.Name });
            }
            if (ab.UserId!=newab.UserId)
            {
                needupdate = true;
            }
            if (needupdate)
            {
                ab.Name = newab.Name;
                ab.UserId = newab.UserId;
            }
        }
        private void NetworkClient_RoomsMessageRecieved(BaseMessage msg)
        {
            ServerInfoMessage simsg = (ServerInfoMessage)msg;
            switch (simsg.Type)
            {
                case SIMessageType.Error:
                    {
                        var errmsg = (ServerInfoErrorMessage)msg.GetInnerMessage();
                        Error = errmsg.Error;
                        break;
                    }
                case SIMessageType.Abonents:
                    {
                        var abnmsg = (ServerInfoAbonentsMessage)msg.GetInnerMessage();
                        AfterAbonentsInfoRecieved(abnmsg);
                        break;
                    }
                case SIMessageType.Rooms:
                    {
                        var rmsmsg = (ServerInfoRoomsMessage)msg.GetInnerMessage();
                        AfterRoomsInfoRecived(rmsmsg);
                        break;
                    }
            }
        }

        private void AfterRoomsInfoRecived(ServerInfoRoomsMessage rmsmsg)
        {
            var oldRooms = Rooms.ToList();
            var newRooms = new List<GameRoomInfo>();
            foreach (var rm in rmsmsg.Rooms)
                if (oldRooms.Contains(rm))
                    oldRooms.Remove(rm);
                else
                    newRooms.Add(rm);
            foreach (var rm in oldRooms)
                Rooms.Remove(rm);
            foreach (var rm in newRooms)
                Rooms.Add(rm);
        }

        private void AfterAbonentsInfoRecieved(ServerInfoAbonentsMessage abnmsg)
        {
            var oldAbns = Abonents.ToList();
            var newAbns = new List<GameAbonentInfo>();
            foreach (var abn in abnmsg.Abonents)
            {
                var ab = Abonents.FirstOrDefault(a => a.Id == abn.Id);
                if (ab != null)
                {
                    AbonentChanged(ab, abn);
                    oldAbns.Remove(abn);
                }
                else
                    newAbns.Add(abn);
            }
            foreach (var rm in oldAbns)
                Abonents.Remove(rm);
            foreach (var rm in newAbns)
                Abonents.Add(rm);
        }

        public override void Dispose()
        {
            Global.NetworkClient.AbonentChanged -= NetworkClient_AbonentChanged;
            Global.NetworkClient.RoomChanged -= NetworkClient_RoomChanged;
            Global.NetworkClient.RoomsMessageRecieved -= NetworkClient_RoomsMessageRecieved;
            Rooms.CollectionChanged -= Rooms_CollectionChanged;
            Abonents.CollectionChanged -= Abonents_CollectionChanged;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Messages.Add(new ChatMessage() { Abonent = Abonent, Roomid = Room.Id, RoomName = Room.Name, Time = DateTime.Now, Message = "вошел в комнату" });
        }
    }
}
