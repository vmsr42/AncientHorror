﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AncientHorrorClient.Windows
{
    public class BaseWindow: Window, INotifyPropertyChanged
    {
        
        private Boolean ismain;
        public Boolean IsMain
        {
            get
            {
                return ismain;
            }
        }
        public BaseWindow()
        {
            ismain = false;
            this.Hide();
        }
        public BaseWindow(bool ismain)
        {
            
            this.ismain = ismain;
        }
        public void ShowWindow()
        {
            if (Global.CurrentWindow != null)
                Global.CurrentWindow.CloseWindow();
            Global.CurrentWindow = this;
            this.Show();
        }
        public void CloseWindow()
        {
            this.Dispose();
            this.Close();
            if (Global.CurrentWindow==this)
                Global.CurrentWindow = null;
        }
        public virtual void Dispose() { }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
                var handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(property));
                }
        }
        #endregion INotifyPropertyChanged

    }
}
