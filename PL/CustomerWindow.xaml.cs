using BLApi;
using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        /// <summary>
        /// collection of parcel that customers send
        /// </summary>
        ObservableCollection<ParcelAtCustomer> listForCustomers;
        /// <summary>
        /// collection of parcel that customers getting
        /// </summary>
        ObservableCollection<ParcelAtCustomer> listToCustomers;


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
           
        }
        /// <summary>
        /// This constractor is for customer display
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="drone"></param>
        /// <param name="mainWindow"></param>
        public CustomerWindow(IBL bl, CustomerForList customerDetails, MainWindow mainWindow)
        {
            InitializeComponent();
            this.bl = bl;
            this.customer = customerDetails;
            this.mainWindow = mainWindow;
            Customer customer = bl.GetCustomer(customerDetails.CustomerID);
            ID.Text = customer.CustomerId.ToString();
            NameOfCostomer.Text = customer.NameOfCustomer;
            PhoneOfCustomer.Text = customer.PhoneOfCustomer.ToString();
            LocationOfCustomer.Text = customer.LocationOfCustomer.ToString();
            //reset list of parcel that customers send
            listForCustomers = new ObservableCollection<ParcelAtCustomer>(customer.FromCustomer);
            LVListForCustomers.ItemsSource = listForCustomers;
            //reset list of parcel that customers getting
            listToCustomers = new ObservableCollection<ParcelAtCustomer>(customer.ToCustomer);
            LVListToCustomers.ItemsSource = listToCustomers;


            ID.Visibility = Visibility.Visible;
            NameOfCostomer.Visibility = Visibility.Visible;
            PhoneOfCustomer.Visibility = Visibility.Visible;
            LocationOfCustomer.Visibility = Visibility.Visible;



        }

        private void CustomerIdTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

    }
}
