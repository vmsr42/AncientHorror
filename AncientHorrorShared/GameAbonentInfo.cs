using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AncientHorrorShared
{
    [DataContract]
    public class GameAbonentInfo: INotifyPropertyChanged
    {
        [DataMember]
        public int Id { get; set; }
        private int userId;
        [DataMember]
        public int UserId
        {
            get
            {
                return userId;
            }
            set
            {
                if (userId != value)
                {
                    userId = value;
                    OnPropertyChanged("UserId");
                }
            }
        }
        [DataMember]
        private String name = string.Empty;
        public String Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name!=value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        public override int GetHashCode()
        {
            return this.Id;
        }
        public override bool Equals(object obj)
        {
            return this.GetHashCode() == obj.GetHashCode();
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
