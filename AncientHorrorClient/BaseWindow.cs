using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AncientHorrorClient
{
    public class BaseWindow: Window, INotifyPropertyChanged
    {
        private String busyMsg = String.Empty;
        public String BusyMessage
        {
            get 
            {
                return busyMsg;
            }
        }
        private Boolean isbusy = false;
        public Boolean IsBusy
        {
            get
            {
                return isbusy;
            }
        }
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
        }
        public BaseWindow(bool ismain)
        {
            if (Global.CurrentWindow != null)
                Global.CurrentWindow.CloseWindow();
            this.ismain = ismain;
            Global.CurrentWindow = this;
            this.Show();
        }
        public void CloseWindow()
        {
            this.Dispose();
            this.Close();
            Global.CurrentWindow = null;
        }
        public virtual void Dispose() { }
        protected void SetBusyStatus(bool isBusy, String bmsg)
        {
            if (!isBusy)
            {
                isbusy = false;
                busyMsg = String.Empty;
            }
            else
            {
                isbusy = true;
                busyMsg = bmsg;
            }
            OnPropertyChanged("IsBusy");
            OnPropertyChanged("BusyMessage");
        }
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
