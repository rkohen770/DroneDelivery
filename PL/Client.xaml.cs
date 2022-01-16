using BLApi;
using BO;
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
        public BO.User MyUser { get; set; }
        private readonly IBL bl = BLFactory.GetBL();
        private CustomerForList customerForList { get; set; }
        public ObservableCollection<PO.ViewTimes> times = new ObservableCollection<PO.ViewTimes>();
        public TimeSpan second = new TimeSpan(0, 0, 0, 1);
        MainWindow last = null;
        DispatcherTimer timer;

        /// <summary>
        /// collection of parcel that customers send
        /// </summary>
        ObservableCollection<ParcelAtCustomer> listForCustomers;
        /// <summary>
        /// collection of parcel that customers getting
        /// </summary>
        ObservableCollection<ParcelAtCustomer> listToCustomers;
        //public Client(User user)
        //{
        //    InitializeComponent();
        //    timer = new DispatcherTimer();
        //    timer.Interval = TimeSpan.FromSeconds(1);
        //    timer.Tick += timer_Tick;
        //    timer.Start();
        //    MyUser = user;
        //    lUser.DataContext = MyUser;
        //    lDate.Content = DateTime.Now.ToString("dd/MM/yy");
        //}

        public Client(User user, MainWindow w = default)
        {
            InitializeComponent();
            MyUser = user;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            lUser.DataContext = MyUser;
            lDate.Content = DateTime.Now.ToString("dd/MM/yy");
            last = w;

            customerForList = bl.GetAllCustomerByPredicate(c => c.Name == user.UserName).FirstOrDefault();

            Customer customer = bl.GetCustomer(customerForList.CustomerID);
            //reset list of parcel that customers send
            listForCustomers = new ObservableCollection<ParcelAtCustomer>(customer.FromCustomer);
            LVListForCustomers.ItemsSource = listForCustomers;
            //reset list of parcel that customers getting
            listToCustomers = new ObservableCollection<ParcelAtCustomer>(customer.ToCustomer);
            LVListToCustomers.ItemsSource = listToCustomers;

            ID.Text = customer.CustomerID.ToString();
            CustomersName.Text = customer.NameOfCustomer.ToString();
            CustomersPhone.Text = customer.PhoneOfCustomer.Substring(0, 3) + "-" + customer.PhoneOfCustomer.Substring(3, 7);
            CustomersLocation.Text = customer.LocationOfCustomer.ToString();
            CustomersLocation.Visibility = Visibility.Visible;




        }

        /// <summary>
        /// an event that shows the timer
        /// </summary>
        private void timer_Tick(object sender, EventArgs e)
        {
            lblTime.Content = DateTime.Now.ToLongTimeString();

            for (int i = 0; i < times.Count(); i++)
            {
                if (times[i].LastTime == TimeSpan.Zero)
                    times.Remove(times[i]);
                else times[i].LastTime -= second;
            }

        }

        private void ShowParcelDetails_Click(object sender, MouseButtonEventArgs e)
        {
            ParcelAtCustomer parcel = ((ListView)sender).SelectedItem as ParcelAtCustomer;
            new ParcelDetails(bl, bl.CloneParcel(bl.GetParcel(parcel.ParcelID))).ShowDialog();
        }

        /// <summary>
        /// A button that opens a window for adding a parcel
        /// </summary>
        /// <param name="sender">Button type</param>
        /// <param name="e"></param>
        private void AddParcelsButton_Click(object sender, RoutedEventArgs e)
        {
            new ParcelWindow(bl, last).ShowDialog();
        }

        /// <summary>
        /// A button that opens a window for updating a customer
        /// </summary>
        /// <param name="sender">Button type</param>
        /// <param name="e"></param>
        private void UpdateCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.UpdateCustomerData(int.Parse(ID.Text), CustomersName.Text, CustomersPhone.Text);
                MessageBox.Show("Update a customer was completed successfully");
            }
            catch (BadCustomerIDException ex)
            {
                MessageBox.Show(ex.ID.ToString(), ex.Message);
            }
        }
        /// <summary>
        /// an event show the login window
        /// </summary>
        private void ChangeUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Login enter = new();
                enter.Show();
                Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
