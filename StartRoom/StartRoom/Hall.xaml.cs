using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;

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
            Status();
        }

        private void Status()
        {
            if (Account.AccountName != null)
            {
                TextBox2.Text = Account.AccountName + " зашла в игру";
            }
        }

        private void Button1Click(object sender, RoutedEventArgs e)
        {
            var log = new List<string>();
            if (TextBox1.Text != string.Empty)
            {
                var message = Account.AccountName + ":" + TextBox1.Text;
                log.Add(message);
                
                ListBox1.Items.Add(message);
                
                TextBox1.Clear();
            }
        }

        private void Button2Click(object sender, RoutedEventArgs e)
        {

        }




        // разобраться с листом(гридбоксом)
    }
}
