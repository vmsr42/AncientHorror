using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AncientHorrorClient.Commands
{
    public class KickUserCmd: InterfaceCommand
    {
        public KickUserCmd()
            : base("Выкинуть из комнаты")
        {
        }
        public override bool CanExecute(object parameter)
        {
            if (Abonent == null)
                return false;
            return true;
        }

        public override void Execute(object parameter)
        {
            
        }

    }
}
