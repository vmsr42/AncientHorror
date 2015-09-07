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
            // вставить проверку на ник чтоб только буквы и цифры
            // избавиться от зависания программы

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (Test(TextBox.Text))
            {
                Account.AccountName = TextBox.Text;
                
                var room = new Hall();
                room.Show();
                Hide();
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

        //private static bool Test(string account)
        //{
        //    foreach (var letter in account.ToCharArray())
        //    {
        //        var check1 = char.IsDigit(letter);
        //        var check2 = char.IsLetter(letter); // ищет только знаки. знак + букво-цифру пропускает
        //        if (check1 | check2)  // неправильно написана проверка может пропустить знак в имени
        //        {
        //            return true;
        //        }
        //    }

        //    MessageBox.Show("Имя должно содержать буквы или цифры");
        //    return false;
        //}
    }
}
