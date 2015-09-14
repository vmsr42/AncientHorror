using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace AncientHorrorClient.Controls
{
    [ContentProperty("Content")]
    public class HeaderedControl : UserControl, INotifyPropertyChanged 
    {
        public Visibility MinimizeBtnVisibility { get; set; }
        public Visibility ExpandBtnVisibility { get; set; }
        public Visibility ReverseExpandBtnVisibility { get; set; }
        public bool Reverse { get; set; }

        private double opacityWind = 0.8;
        public double OpacityWind
        {
            get
            {
                return opacityWind;
            }
            set
            {
                if (value!=opacityWind)
                {
                    opacityWind = value;
                    OnPropertyChanged("OpacityWind");
                }
            }
        }
        private int windSize = 3;
        public int WindowSize
        {
            get
            {
                return windSize;
            }
            set
            {
                if (value < 0)
                    value = 3;
                if (value > 3)
                    value = 0;
                if (value != windSize)
                {
                    windSize = value;
                    OnPropertyChanged("WindowSize");
                    OnPropertyChanged("HeaderRow");
                    OnPropertyChanged("ContentRow");
                    OnPropertyChanged("ContentVisibility");
                    OnPropertyChanged("ContentSpan");
                }
            }
        }
        public int HeaderRow
        {
            get
            {
                if (Reverse)
                    return  0;
                return 3 - WindowSize;
            }
        }
        public int ContentRow
        {
            get
            {
                if (Reverse)
                    return 1;
                return HeaderRow + 1;
            }
        }
        public int ContentSpan
        {
            get
            {
                if (WindowSize==0)
                    return WindowSize + 1;
                return WindowSize;
            }
        }
        public Visibility ContentVisibility
        {
            get
            {
                if (WindowSize < 1)
                    return Visibility.Hidden;
                return Visibility.Visible;
            }
        }

        private void MinimizeCmd(object sender, RoutedEventArgs e)
        {
            WindowSize = 0;
        }
        private void ChangeSizeCmd(object sender, RoutedEventArgs e)
        {
            if (Reverse)
                WindowSize--;
            else
                WindowSize++;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
        
        static HeaderedControl()
        {
            
            DefaultStyleKeyProperty.OverrideMetadata(
                    typeof(HeaderedControl),
                    new FrameworkPropertyMetadata(typeof(HeaderedControl)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Button but = this.Template.FindName("MinimizeBtn", this) as Button;
            but.Click += MinimizeCmd;
            but = this.Template.FindName("ChangeSizeBtn", this) as Button;
            but.Click += ChangeSizeCmd;
            but = this.Template.FindName("ReverseChangeSizeBtn", this) as Button;
            but.Click += ChangeSizeCmd;

        }

    }
}
