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
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        private IBL bl;
        private ParcelForList parcelDetails;
        private MainWindow mainWindow;
        private Client client;
        private Customer customerDetails;
    
        /// <summary>
        /// constractor for Add the parcels
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="mainWindow"></param>
        public ParcelWindow(IBL bl, MainWindow mainWindow)
        {
            InitializeComponent();
            DataContext = false;

            this.bl = bl;
            this.mainWindow = mainWindow;
            Weight_Selector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            Priorities_Selector.ItemsSource = Enum.GetValues(typeof(Priorities));
            senderParcel.Visibility = Visibility.Hidden;
            Height = 400;
         
        }

        /// <summary>
        /// constractor for dysplay a parcel
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="parcelDetails"></param>
        /// <param name="mainWindow"></param>
        public ParcelWindow(IBL bl, ParcelForList parcelDetails, MainWindow mainWindow)
        {
            InitializeComponent();
            DataContext = false;

            this.bl = bl;
            this.parcelDetails = parcelDetails;
            this.mainWindow = mainWindow;
            DataContext = bl.GetParcel(parcelDetails.ParcelID);
            if (parcelDetails.ParcelStatus == ParcelStatus.Associated)
            {
                CollectionParcelClick.IsEnabled = true;
            }
            else
            {
                CollectionParcelClick.IsEnabled = false;
            }

            if (parcelDetails.ParcelStatus == ParcelStatus.WasCollected)
            {
                DeliveryParcelByDrone.IsEnabled = true;
            }
            else
            {
                DeliveryParcelByDrone.IsEnabled = false;
            }

            Parcel parcel = bl.GetParcel(parcelDetails.ParcelID);
            ID.Text = parcel.ParcelID.ToString();
            Sender_Id.Text = parcel.SenderOfParcel.CustomerID.ToString();
            Sender_Name.Text = parcel.SenderOfParcel.CustomerName;
            Target_Id.Text = parcel.TargetToParcel.CustomerID.ToString();
            Target_Name.Text = parcel.TargetToParcel.CustomerName;
            if(parcel.DroneInParcel.DroneID!=0)
            {
                ShowDroneDetails.IsEnabled = true;
                Drone_Id.Text = parcel.DroneInParcel.DroneID.ToString();
                Drone_Battery.Text = parcel.DroneInParcel.DroneBattery.ToString()+"%";
                Drone_Location.Text = parcel.DroneInParcel.CurrentLocation.ToString();
            }
            else
            {
                ShowDroneDetails.IsEnabled = false;
                Drone_Id.Text = "0";
                Drone_Battery.Text = "0%";
                Drone_Location.Text = null;
            }
            Requested.Text =parcel.Requested.ToString();
            Scheduled.Text = parcel.Scheduled.ToString();
            PickedUp.Text = parcel.PickedUp.ToString();
            ParcelDelivery.Text = parcel.ParcelDelivery.ToString();

        }
        /// <summary>
        /// constactor for add parcel in client detalis
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="parcelDetails"></param>
        /// <param name="client"></param>
        public ParcelWindow(IBL bl, Customer customer, Client client)
        {
            InitializeComponent();
            DataContext = false;

            this.bl = bl;
            this.client = client;
            this.customerDetails = customer;
            Weight_Selector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            Priorities_Selector.ItemsSource = Enum.GetValues(typeof(Priorities));
            senderParcel.Visibility = Visibility.Hidden;
            Height = 400;
         
            Sender_Id_Add.Text = customer.CustomerID.ToString();
            Sender_Name_Add.Text = customer.NameOfCustomer;
            Sender_Id_Add.IsReadOnly = true;
            Sender_Name_Add.IsReadOnly = true;
            

        }


        /// <summary>
        /// text box that allows only numbers to be entered
        /// </summary>
        /// <param name="sender">TextBox type</param>
        /// <param name="e"></param>
        private void ParcelTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Button for closing a window
        /// </summary>
        /// <param name="sender">Button type</param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = true;

            Close();
        }

        /// <summary>
        /// Button for saving the details of the new parcel
        /// </summary>
        /// <param name="sender">button type</param>
        /// <param name="e"></param>
        private void SaveParcelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Sender_Id_Add.Text==null || Target_Id_Add.Text == null || Weight_Selector.SelectedItem== null || Priorities_Selector.SelectedItem == null)
                    MessageBox.Show("Not all detalis are set");
                 else
                {
                    int.TryParse(Sender_Id_Add.Text, out int sender_Id);
                    int.TryParse(Target_Id_Add.Text, out int target_Id);
                    bl.AddParcelBo(sender_Id,target_Id,
                         (WeightCategories)Weight_Selector.SelectedItem, (Priorities)Priorities_Selector.SelectedItem);
                    MessageBox.Show("Adding a parcel was completed successfully");
                }
            }
            catch (BadCustomerIDException ex)
            {
                MessageBox.Show(ex.ID.ToString(), ex.Message + "\nThe customer does not exist in the system");
            }

            if (mainWindow != null)
            {
                mainWindow.LVListParcels.ItemsSource = bl.GetAllParcelsBo();
                mainWindow.LVListParcels.Items.Refresh();
            }
            if (client != null)
            {
                customerDetails = bl.GetCustomer(customerDetails.CustomerID);
                client.LVListForCustomers.ItemsSource = new ObservableCollection<ParcelAtCustomer>(customerDetails.FromCustomer);
                client.LVListForCustomers.Items.Refresh();
                client.LVListToCustomers.ItemsSource = new ObservableCollection<ParcelAtCustomer>(customerDetails.ToCustomer);
                client.LVListToCustomers.Items.Refresh();
            }

            Close();
        }

        private void ShowDroneDetails_Click(object sender, RoutedEventArgs e)
        {
            DroneForList drone = bl.CloneDrone(bl.GetDrone(int.Parse(Drone_Id.Text)));
            new DroneWindow(bl, drone, mainWindow).ShowDialog();
        }

        private void ShowSenderDetails_Click(object sender, RoutedEventArgs e)
        {
            CustomerForList SenderCustomer = bl.CloneCustomer(bl.GetCustomer(int.Parse(Sender_Id.Text)));
            new CustomerWindow(bl, SenderCustomer, mainWindow).ShowDialog();

        }
        private void ShowTargetDetails_Click(object sender, RoutedEventArgs e)
        {
            CustomerForList TargetCustomer = bl.CloneCustomer(bl.GetCustomer(int.Parse(Target_Id.Text)));
            new CustomerWindow(bl, TargetCustomer, mainWindow).ShowDialog();

        }
        /// <summary>
        /// A button that opens a window for updating parcel
        /// </summary>
        /// <param name="sender">Button type</param>
        /// <param name="e"></param>
        private void UpdateColectionParcelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.UpdateCollectionParcelByDrone(int.Parse(Drone_Id.Text));
                MessageBox.Show("Collection the parcel by drone was completed successfully");
                mainWindow.LVListParcels.ItemsSource = bl.GetAllParcelsBo();
                mainWindow.LVListParcels.Items.Refresh();
            }
            catch (BadParcelIDException ex)
            {
                MessageBox.Show(ex.ID.ToString(), ex.Message);
            }
        }/// <summary>
         /// A button that opens a window for updating a parcel
         /// </summary>
         /// <param name="sender">Button type</param>
         /// <param name="e"></param>
        private void UpdateDeliveryParcelByDroneButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.UpdateDeliveryParcelByDrone(int.Parse(Drone_Id.Text));
                MessageBox.Show("Delivery the parcel by drone was completed successfully");
                mainWindow.LVListParcels.ItemsSource = bl.GetAllParcelsBo();
                mainWindow.LVListParcels.Items.Refresh();
            }
            catch (BadParcelIDException ex)
            {
                MessageBox.Show(ex.ID.ToString(), ex.Message);
            }
        }
        //Bouns.
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext.Equals(false)) e.Cancel = true;
        }
    }
}
