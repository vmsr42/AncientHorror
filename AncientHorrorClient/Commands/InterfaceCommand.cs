using AncientHorrorShared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AncientHorrorClient.Commands
{
    public abstract class InterfaceCommand : ICommand, INotifyPropertyChanged 
    {
        public bool CanDo
        {
            get
            {
                return CanExecute(Abonent);
            }
        }
        private GameAbonentInfo abonent = null;
        public GameAbonentInfo Abonent
        {
            get
            {
                return abonent;
            }
            set
            {
                if (value!=abonent)
                {
                    abonent = value;
                    OnPropertyChanged("Abonent");
                    OnPropertyChanged("CanDo");
                }
            }
        }
        public  String CmdName { get;private set; }
        public abstract bool CanExecute(object parameter);
        
        
 
        public event EventHandler CanExecuteChanged;


        public abstract void Execute(object parameter);
        public InterfaceCommand(String cmdName)
        {
            this.CmdName = cmdName;
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



    }
}
