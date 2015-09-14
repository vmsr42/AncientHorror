using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AncientHorrorClient.Commands
{
    public class AddAbonentToChatCmd: InterfaceCommand
    {
        public AddAbonentToChatCmd()
            : base("Написать")
        {
        }
        public override bool CanExecute(object parameter)
        {

            if (Abonent == null || Abonent.Name == "ssss")
                return false;
            return true;
        }

        public override void Execute(object parameter)
        {
        }
    }
}
