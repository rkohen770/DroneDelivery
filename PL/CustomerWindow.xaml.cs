using BLApi;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        private IBL bl = BLFactory.GetBL();
        private CustomerForList customer { get; set; }
        private MainWindow mainWindow;

        public CustomerWindow()
        {
            DialogResult = true;
            InitializeComponent();
        }
        /// <summary>
        /// this constractor is for adding a customer
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="mainWindow"></param>
        public CustomerWindow(IBL bl, MainWindow mainWindow)
        {
            InitializeComponent();
            this.bl = bl;
            this.mainWindow = mainWindow;

            ID.Visibility = Visibility.Visible;
            NameOfCostomer.Visibility = Visibility.Visible;
            PhoneOfCustomer.Visibility = Visibility.Visible;
            LocationOfCustomer.Visibility = Visibility.Visible;
            //FromCustomer.Visibility = Visibility.Hidden;
            //ToCustomer.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// This constractor is for customer display
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="drone"></param>
        /// <param name="mainWindow"></param>
        public CustomerWindow(IBL bl, CustomerForList customerForList, MainWindow mainWindow)
        {
            InitializeComponent();
            this.bl = bl;
            this.customer = customerForList;
            this.mainWindow = mainWindow;
            
            
            ID.Visibility = Visibility.Visible;
            NameOfCostomer.Visibility = Visibility.Visible;
            PhoneOfCustomer.Visibility = Visibility.Visible;
            LocationOfCustomer.Visibility = Visibility.Visible;
           //FromCustomer.Visibility = Visibility.Hidden;
           //ToCustomer.Visibility = Visibility.Hidden;

            //ID.IsReadOnly = true;
            //ID.Text = drone.DroneId.ToString();
            //Model.Text = drone.DroneModel;
            //MaxWeight.Text = drone.MaxWeight.ToString();
            //Battery.Text = drone.DroneBattery.ToString();
            //Status.IsReadOnly = true;
            //Status.Text = drone.DroneStatus.ToString();
            //stationID.Width = 200;
            //stationID.Margin = new Thickness(30, 145, 0, 0);
            //stationID.Content = "Parcel Num Is Transferred";
            //StationID.Text = drone.ParcelNumIsTransferred.ToString();
            //CurrentLocation.Text = drone.CurrentLocation.ToString();
        }

        private void CustomerIdTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
