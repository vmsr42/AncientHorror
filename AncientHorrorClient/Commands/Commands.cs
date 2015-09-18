using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AncientHorrorClient.Commands
{
    public static class Commands
    {
        public static readonly RoutedUICommand AddToChat = new RoutedUICommand
                       (
                               "Добавить в чат", "AddToChat", typeof(Commands)
                       );
        public static readonly RoutedUICommand KickUser = new RoutedUICommand
                       (
                               "Выкинуть из комнаты", "KickUser", typeof(Commands)
                       );
    }
}
