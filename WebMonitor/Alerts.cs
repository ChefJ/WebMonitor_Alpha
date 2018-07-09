using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMonitor
{
    public class Alerts : INotifyPropertyChanged
    {
        string _Domain = "http://test.com";
        string _Info = "Null";
        string _BornData = "";
        public event PropertyChangedEventHandler PropertyChanged;

        public Alerts()
        {
            _BornData = DateTime.Now.ToLocalTime().ToString();
        }

        public Alerts(string in_Domain,string in_Info)
        {
            Domain = in_Domain;
            Info = in_Info;
            _BornData = DateTime.Now.ToLocalTime().ToString();
        }
        public string Domain { get { return _Domain; } set { _Domain = value; OnPropertyChanged("Domain"); } }
        public string Info { get { return _Info; } set { _Info = value; OnPropertyChanged("Info"); } }
        public string BornData { get { return _BornData; } }

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));

            // With C# 6 this can be replaced with
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
