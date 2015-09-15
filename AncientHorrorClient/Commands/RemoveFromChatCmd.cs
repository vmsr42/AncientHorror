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
        public delegate void removeAbDelegate(GameAbonentInfo abon);
        private removeAbDelegate CmdAction; 
        public RemoveFromChatCmd( removeAbDelegate action)
            : base("Закрыть вкладку")
        {
            CmdAction = action;
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
            CmdAction(ab);

        }
    }
}
