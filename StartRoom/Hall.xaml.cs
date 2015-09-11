using System.Collections.Generic;
using System.Windows;

namespace StartRoom
{
    /// <summary>
    /// Interaction logic for Hall.xaml 
    /// </summary>
    public partial class Hall : Window
    {
        public Hall()
        {
            InitializeComponent();
            // я создала 2 фейковых метода чтобы продемострировать работу всех кнопок
            AddRoom(Help(string.Empty));
            Status();
        }


        private void Status()
        {
            if (!ListBox3.HasItems)
            {
                Button3.IsEnabled = false;
            }

            if (Account.AccountName != null)
            {
                ListBox2.Items.Add(Account.AccountName + " зашла в игру");
            }
        }

        private void Button1Click(object sender, RoutedEventArgs e) // отправить сообщение в чат
        {
            if (TextBox1.Text != string.Empty)
            {
                ListBox1.Items.Add(Account.AccountName + ":" + TextBox1.Text);
                TextBox1.Clear();
            }
        }

        private void Button2Click(object sender, RoutedEventArgs e) // создать комнату
        {
            Account.YouAreLeader = true;
            TabItem2.IsEnabled = true;
            TabItem2.IsSelected = true;
            TabItem1.IsEnabled = false;

            if (Account.YouAreLeader)
            {
                ListBox2.Items.Add(Account.AccountName + " создала новую комнату");
            }

            // спросить Мишу как вынимать список ЛистВох4 (коллекция??)
            // чтоб можно было его куда сохранить например

            ListBox4.Items.Add(Account.AccountName); //ввести ограничение но не сюда, на кнопку "войти в комнату"
            ListBox4.Items.Add("Dasha"); // test
            ListBox4.Items.Add("Masha"); // test
            ListBox4.Items.Add("Ksusha"); // test
            ListBox4.Items.Add("Nusha"); // test
            ListBox4.Items.Add("Asha"); // test
            ListBox4.Items.Add("Usha"); // test
            ListBox4.Items.Add("Lusha"); // test

            TextBox3.Text = ListBox4.Items.Count + " / 8";

            ListBox3.Items.Add("Комната № " + (ListBox3.Items.Count + 1) + " " + Account.AccountName + " " + ListBox4.Items.Count + " / 8"); // правильно не менять
        }

        private void Button6Click(object sender, RoutedEventArgs e) // выход
        {
            Application startRoom = Application.Current;
            startRoom.Shutdown();
        }

        private void Button4Click(object sender, RoutedEventArgs e) // играть
        {
            var game = new Game();
            game.Show();
            Close();
        }

        private void Button3Click(object sender, RoutedEventArgs e) // войти в комнату
        {
            if (ListBox3.SelectedItem != null & Help(string.Empty).Count < 8)
            {
                TabItem2.IsEnabled = true;
                TabItem2.IsSelected = true;
                TabItem1.IsEnabled = false;
                Button4.Visibility = Visibility.Hidden;


                ListBox2.Items.Add(Account.AccountName + " зашла в комнату");

                foreach (string s in Help(Account.AccountName))
                {
                    ListBox4.Items.Add(s);
                }

                TextBox3.Text = ListBox4.Items.Count + " / 8";
            }
            else
            {
                Button3.IsEnabled = false;
            }
        }

        private List<string> Help(string accountName)
        {
            var irishaRoom = new List<string>();
            irishaRoom.Add("Irisha");
            irishaRoom.Add("Anusha");
            irishaRoom.Add("Uliasha");
            irishaRoom.Add("Olusha");
            irishaRoom.Add("Natasha");
            irishaRoom.Add("Yniasha");
            irishaRoom.Add("Katusha");
         //   irishaRoom.Add("Arisha");
            if (accountName != string.Empty & irishaRoom.Count < 8)
            {
                irishaRoom.Add(accountName);
            }

            return irishaRoom;
        }

        private void AddRoom(List<string> name)
        {
            ListBox3.Items.Add("Комната № " + (ListBox3.Items.Count + 1) + " " + name[0] + " "
        + name.Count + " / 8");
        }

        private void Button5Click(object sender, RoutedEventArgs e)
        {
            ListBox2.Items.Add(Account.AccountName + " вышла из комнаты");

            if (Account.YouAreLeader)
            {
               // TabItem2.IsEnabled = false;
                TabItem1.IsSelected = true;
                TabItem1.IsEnabled = true;

                ListBox4.Items.Clear();

                ListBox3.Items.Remove("Комната № " + (ListBox3.Items.Count + 1) + " " + Account.AccountName + " " + ListBox4.Items.Count + " / 8");
 
                // поудалять все что насоздавала
            }



        }
    }
}
