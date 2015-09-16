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
    public class GameRoomInfo : INotifyPropertyChanged
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public GameAbonentInfo Owner { get; set; }
        [DataMember]
        public bool HavePassword { get; set; }
        private int capacity = 0;
        [DataMember]
        public int Capacity 
        { 
            get
            {
                return capacity;
            }
            set
            {
                if (capacity!=value)
                {
                    capacity = value;
                    OnPropertyChanged("Capacity");
                }
            }
        }
        [DataMember]
        public int Capability { get; set; }
        [IgnoreDataMember]
        public Boolean IsLobby
        {
            get
            {
                if (this.Id == 0)
                    return true;
                else
                    return false;
            }
        }
        public override bool Equals(object obj)
        {
            return this.GetHashCode() == obj.GetHashCode();
        }
        public override int GetHashCode()
        {
            return this.Id;
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
