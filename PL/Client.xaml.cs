using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PL
{
    /// <summary>
    /// Interaction logic for Client.xaml
    /// </summary>
    public partial class Client : Window
    {
        public BO.User _user { get; set; }
        public TimeSpan second = new TimeSpan(0, 0, 0, 1);
        Window last = null;
        DispatcherTimer timer;
        public Client()
        {
            InitializeComponent();
        }

        public Client(BO.User user, Window win = default)
        {
            InitializeComponent();
            this._user = user;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            //cmbStations.ItemsSource = from BO.Station s in bl.GetAllStations()
            //                          select new BO.Stat(s.Code, s.Name);
            //lUser.DataContext = MyUser;
            //lDate.Content = DateTime.Now.ToString("dd/MM/yy");
            //cmbStations.SelectedIndex = 0;
            last = win;
        }
        /// <summary>
        /// an event that shows the timer
        /// </summary>

        private void timer_Tick(object sender, EventArgs e)
        {
            //lblTime.Content = DateTime.Now.ToLongTimeString();

            //for (int i = 0; i < times.Count(); i++)
            //{
            //    if (times[i].LastTime == TimeSpan.Zero)
            //        times.Remove(times[i]);
            //    else times[i].LastTime -= second;
            //}

            //if (DateTime.Now.Second % 5 == 0)
            //{
            //    ObservableCollection<PO.ViewTimes> listO = new ObservableCollection<PO.ViewTimes>(
            //         from item in PO.ViewTimes.times.TimesOfStation((int)(cmbStations.SelectedValue))
            //         where times.ToList().Find(x => x.UpLineID == item.UpLineID) == null
            //         select item
            //     );
            //    foreach (var item in listO)
            //    {
            //        times.Add(item);
            //    }
            //}
            //ObservableCollection<PO.ViewTimes> listObserv = new ObservableCollection<PO.ViewTimes>(times);
            //TimesList.ItemsSource = listObserv;
        }

    }
}
