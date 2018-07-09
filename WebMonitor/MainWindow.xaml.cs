using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
namespace WebMonitor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        bool MonitorFlag = false;
        ObservableCollection<UrlInfo> _UrlList = new ObservableCollection<UrlInfo>();

        ObservableCollection<Alerts> _AlertList = new ObservableCollection<Alerts>();
        public ObservableCollection<Alerts> AlertList
        {
            get { return _AlertList; }
            set
            {
                if (_AlertList != value)
                {
                    _AlertList = value;
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("AlertList"));
                }
            }
        }
        public ObservableCollection<UrlInfo> UrlList
        {
            get { return _UrlList; }
            set
            {
                if (_UrlList != value)
                {
                    _UrlList = value;
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("PluginList"));
                }
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            //  listUrl.ItemsSource = UrlList;
            rgvUrlList.ItemsSource = UrlList;
            rgvAlertList.ItemsSource = AlertList;
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 50000;
            aTimer.Enabled = true;

           // Console.WriteLine("Press \'q\' to quit the sample.");
           // while (Console.Read() != 'q') ;
        }
        
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            //AlertList.Add(new Alerts());
            if (MonitorFlag)
            {
                foreach (UrlInfo ainfo in UrlList)
                {
                    Task UpdateStatus = new Task(() => {
                        try
                        {
                            ainfo.Latency += 1;
                            string tmp = ainfo.Domain.Replace("http://", "");
                            ainfo.IpAddr = Dns.GetHostAddresses(tmp)[0].ToString();

                            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(ainfo.Domain);
                            webRequest.AllowAutoRedirect = false;
                            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
                            //Returns "MovedPermanently", not 301 which is what I want.
                            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                            string content = reader.ReadToEnd();
                            string header = Regex.Match(content, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;
                            if(header!= "")
                                ainfo.SiteTitle = header;
                            // MessageBox.Show(header);
                            if( (ainfo.Status != response.StatusCode.ToString())&&(ainfo.Status!="-1"))
                                AlertList.Add(new Alerts(ainfo.Domain, "Status changes from " + ainfo.Status + " To " + response.StatusCode.ToString()));
                            ainfo.Status = response.StatusCode.ToString();
                        }
                        catch (Exception exx)
                        {
                            ainfo.Status = exx.Message;

                            //MessageBox.Show(exx.Message);
                        }
                    });
                    UpdateStatus.Start();
                }

            }


        }
        private void btnAddUrl_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Excel Files (*.*)|*.*"
            };
            var result = openFileDialog.ShowDialog();
            if (result == true && openFileDialog.FileName != "")
            {//此处卡顿。
                this.tbxUrlFilePath.Text = openFileDialog.FileName;
                Task AddTask = new Task(
                () =>
                {
                    List<string> tmpList = Tools.ReadFileByLine(openFileDialog.FileName);
                    foreach (string url in tmpList)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            UrlList.Add(new UrlInfo(url));
                        });
                    }
                });
                AddTask.Start();
              }
             // PrepareScanTaskList = tmpUrlList;
            
            else
                return;
        }

        private void btnStartStop_Click(object sender, RoutedEventArgs e)
        {
            if(MonitorFlag)
            {
                btnStartStop.Content = "Start";
                MonitorFlag = false;
            }
            else
            {
                btnStartStop.Content = "Stop";
                MonitorFlag = true;
            }
        }
    }
}
