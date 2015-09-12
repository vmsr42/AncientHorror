using System.Windows;

namespace StartRoom
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        public Game()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Application startRoom = Application.Current;
            startRoom.Shutdown();
        }
    }
}
