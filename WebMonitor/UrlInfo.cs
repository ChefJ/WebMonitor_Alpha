using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMonitor
{
    public class UrlInfo: INotifyPropertyChanged
    {
        string _Domain = "http://test.com";
        string _IpAddr = "0.0.0.0";
        int _Latency = 0;
        string _Status = "-1";
        bool _OtherProblem = false;
        string _OtherInfo = "";
        string _SiteTitle = "";
        public event PropertyChangedEventHandler PropertyChanged;
        public UrlInfo(string inDomain)
        {
            Domain = inDomain;
        }
        public string SiteTitle {
            get {
                return _SiteTitle;
            }
            set {
                _SiteTitle = value;
                OnPropertyChanged("SiteTitle");
            } }
        public string Domain { get; set; }
        public string IpAddr { get { return _IpAddr; } set { _IpAddr = value; OnPropertyChanged("IpAddr"); } }
        public string LatencyStr { get { return _Latency.ToString(); } }

        public int Latency {
            get { return _Latency; }
            set { _Latency = value;
                OnPropertyChanged("Latency");
            }
        }
        public string Status {
            get { return _Status; }

            set { _Status = value; }
        }

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
