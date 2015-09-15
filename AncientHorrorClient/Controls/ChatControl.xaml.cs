﻿using AncientHorrorClient.Commands;
using AncientHorrorShared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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


namespace AncientHorrorClient.Controls
{
    /// <summary>
    /// Логика взаимодействия для ChatControl.xaml
    /// </summary>
    public partial class ChatControl : HeaderedControl
    {
        private RemoveFromChatCmd removeUserCmd = null;
        public RemoveFromChatCmd RemoveUserCmd
        {
            get
            {
                if (removeUserCmd == null)
                    removeUserCmd = new RemoveFromChatCmd(Chanels);
                return removeUserCmd;
            }
        }

        public static readonly DependencyProperty MessagesProperty =
DependencyProperty.Register("Messages", typeof(ObservableCollection<ChatMessage>), typeof(ChatControl));
        public ObservableCollection<ChatMessage> Messages
        {
            get
            {
                return (ObservableCollection<ChatMessage>)this.GetValue(MessagesProperty);
            }
            set
            {
                this.SetValue(MessagesProperty, value);
            }
        }

        private ObservableCollection<GameAbonentInfo> chanels = null;
        public ObservableCollection<GameAbonentInfo> Chanels
        {
            get
            {
                if (chanels == null)
                {
                    chanels = new ObservableCollection<GameAbonentInfo>();
                    chanels.Add(new GameAbonentInfo() { Name = "Общий", Id = -1 });
                    chanels.Add(new GameAbonentInfo() { Name = "Общий2", Id = 1 });
                    chanels.Add(new GameAbonentInfo() { Name = "Общий3", Id = 2 });
                    chanels.Add(new GameAbonentInfo() { Name = "Общий4", Id = 3 });
                }
                return chanels;
            }
        }
        private GameAbonentInfo selected = null;
        public GameAbonentInfo Selected 
        {
            get
            {
                if (selected == null)
                    selected = Chanels.First();
                return selected;
            }
            set
            {
                if (value!=selected)
                {
                    selected = value;
                    OnPropertyChanged("Selected");
                }
            }
        }
        private String msgToSend = String.Empty;
        public String MsgToSend
        {
            get
            {
                return msgToSend;
            }
            set
            {
                if (value != msgToSend)
                {
                    msgToSend = value;
                    OnPropertyChanged("MsgToSend");
                }
            }
        }
        public ChatControl()
        {
            InitializeComponent();
        }
        public void AddToChanel(GameAbonentInfo ab)
        {
            if (!Chanels.Contains(ab))
                Chanels.Add(ab);
            Selected = ab;
        }

        private void AbonentLabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
