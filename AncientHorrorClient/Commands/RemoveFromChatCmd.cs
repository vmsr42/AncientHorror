using AncientHorrorShared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AncientHorrorClient.Commands
{
    public class RemoveFromChatCmd: InterfaceCommand
    {
        
        private ObservableCollection<GameAbonentInfo> chatusers = null;
        public RemoveFromChatCmd(ObservableCollection<GameAbonentInfo> users)
            : base("Закрыть вкладку")
        {
            chatusers = users;
        }
        
        public override bool CanExecute(object parameter)
        {
            var ab = (GameAbonentInfo)parameter;
            if (ab.Id>0)
                return true;
            return false;
        }

        public override void Execute(object parameter)
        {
            GameAbonentInfo ab = (GameAbonentInfo)parameter;
            if (chatusers.Contains(ab))
                chatusers.Remove(ab);
        }
    }
}
