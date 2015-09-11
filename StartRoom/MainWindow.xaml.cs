using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Windows;

namespace StartRoom
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
            // еще раз погрызть биндинги

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (Test(TextBox.Text))
            {
                Account.AccountName = TextBox.Text;
                
                var hall = new Hall();
                hall.Show();
                Close();
            }
            else
            {
                TextBox.Clear();
                MessageBox.Show("Имя должно содержать буквы и/или цифры");
            }
        }

        private static bool Test(string account) // надо почистить текстбокс не факт что здесь
        {
            Regex badString = new Regex(@"\W");

            var check = badString.IsMatch(account);

            if (!check)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
