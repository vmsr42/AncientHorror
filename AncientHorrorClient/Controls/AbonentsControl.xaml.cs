using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using AncientHorrorClient.Commands;
using AncientHorrorClient.Controls;
using AncientHorrorShared;

namespace AncientHorrorClient.Controls
{
    /// <summary>
    /// Логика взаимодействия для AbonentsControl.xaml
    /// </summary>
    public partial class AbonentsControl : HeaderedControl
    {
        #region Свойства
        private String filtertext = String.Empty;
        public String FilterText
        {
            get
            {
                return filtertext;
            }
            set
            {
                if (value!=filtertext)
                {
                    filtertext = value;
                    OnPropertyChanged("FilterText");
                    this.AbnList.Items.Filter = null;
                    this.AbnList.Items.Filter = delegate(object obj)
                    {
                        GameAbonentInfo ab = (GameAbonentInfo)obj;
                        if (ab.Name.ToUpper().Trim().Contains(filtertext.ToUpper().Trim()))
                            return true;
                        else
                            return false;
                    };
                }
            }
        }
        private GameAbonentInfo selected;
        public GameAbonentInfo Selected 
        {
            get
            {
                return selected;
            }
            set
            {
                if (value!=selected)
                {
                    selected = value;
                    OnPropertyChanged("Selected");
                }
            }
        }

        public static readonly DependencyProperty AbonentsProperty =
DependencyProperty.Register("Abonents", typeof(ObservableCollection<GameAbonentInfo>), typeof(AbonentsControl));
        public ObservableCollection<GameAbonentInfo> Abonents
        {
            get
            {
                return (ObservableCollection<GameAbonentInfo>)this.GetValue(AbonentsProperty);
            }
            set
            {
                this.SetValue(AbonentsProperty, value);
            }
        }
        #endregion Свойства
        public AbonentsControl()
        {
            InitializeComponent();
        }


       private void ListboxClicked(object sender, MouseButtonEventArgs e)
        {
            if (sender!=null&&Selected!=null)
            {
                ListBox lbx = (ListBox)sender;
                var lbi = lbx.ItemContainerGenerator.ContainerFromItem(Selected) as ListBoxItem;
                if (!IsMouseOverTarget(lbi, e.GetPosition((IInputElement)lbi)))
                {
                    Selected = null;
                }

            }
        }

        private bool IsMouseOverTarget(Visual target, Point point)
        {
            var bounds = VisualTreeHelper.GetDescendantBounds(target);
            return bounds.Contains(point);
        }


          




    }
}
