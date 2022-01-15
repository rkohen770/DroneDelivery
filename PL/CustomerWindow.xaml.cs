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
            CustomersLocation.Visibility = Visibility.Hidden;
            longttitude.Visibility = Visibility.Visible;
            Longttitude.Visibility = Visibility.Visible;
            lattitude.Visibility = Visibility.Visible;
            Lattitude.Visibility = Visibility.Visible;
            Update.Visibility = Visibility.Hidden;

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
            ID.Text = customer.CustomerID.ToString();
            CustomersName.Text = customer.NameOfCustomer.ToString();
            CustomersPhone.Text = customer.PhoneOfCustomer.Substring(0, 3) + "-" + customer.PhoneOfCustomer.Substring(3, 7);
            CustomersLocation.Text = customer.LocationOfCustomer.ToString();

            //reset list of parcel that customers send
            listForCustomers = new ObservableCollection<ParcelAtCustomer>(customer.FromCustomer);
            LVListForCustomers.ItemsSource = listForCustomers;
            //reset list of parcel that customers getting
            listToCustomers = new ObservableCollection<ParcelAtCustomer>(customer.ToCustomer);
            LVListToCustomers.ItemsSource = listToCustomers;
            SaveClick.Visibility = Visibility.Hidden;

        }

        private void CustomerIdTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Button for saving the details of the new customer
        /// </summary>
        /// <param name="sender">button type</param>
        /// <param name="e"></param>
        private void SaveCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ID.Text == null || CustomersName.Text == null || CustomersPhone.Text == null)
                    MessageBox.Show("Not all detalis are set");
                else if (CustomersPhone.Text.Length > 10)
                    MessageBox.Show("Phone number longs then 5 letters");
                else
                {
                    Location location = new() { Lattitude = double.Parse(Lattitude.Text), Longitude = double.Parse(Longttitude.Text) };
                    bl.AddCustomerBo(int.Parse(ID.Text), CustomersName.Text, CustomersPhone.Text, location);
                }
                MessageBox.Show("Adding a customer was completed successfully");

            }
            catch (BadCustomerIDException ex)
            {
                MessageBox.Show(ex.ID.ToString(), ex.Message + "\nAdding a customer was not completed successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            mainWindow.LVListCustomers.Items.Refresh();
            Close();
        }

        /// <summary>
        /// Button for closing a window
        /// </summary>
        /// <param name="sender">Button type</param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
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
                mainWindow.LVListCustomers.ItemsSource = bl.GetAllCustomersBo();
                mainWindow.LVListCustomers.Items.Refresh();

            }
            catch (BadCustomerIDException ex)
            {
                MessageBox.Show(ex.ID.ToString(), ex.Message);
            }
        }

        private void ShowParcelDetails_Click(object sender, MouseButtonEventArgs e)
        {
            ParcelAtCustomer parcel = ((ListView)sender).SelectedItem as ParcelAtCustomer;
            new ParcelWindow(bl, bl.CloneParcel(bl.GetParcel(parcel.ParcelID)), mainWindow).ShowDialog();
            Customer c = bl.GetCustomer(customer.CustomerID);
            LVListForCustomers.ItemsSource = c.FromCustomer;
            LVListForCustomers.Items.Refresh();
            LVListToCustomers.ItemsSource = c.ToCustomer;
            LVListToCustomers.Items.Refresh();
        }
    }
}
