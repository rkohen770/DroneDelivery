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
        public Client()
        {
            InitializeComponent();
        }

        public Client(BO.User user, Window win = default)
        {
            InitializeComponent();
            _user = user;
            //cmbStations.ItemsSource = from BO.Station s in bl.GetAllStations()
            //                          select new BO.Stat(s.Code, s.Name);
            //lUser.DataContext = MyUser;
            //lDate.Content = DateTime.Now.ToString("dd/MM/yy");
            //cmbStations.SelectedIndex = 0;
            last = win;
        }

    }
}
