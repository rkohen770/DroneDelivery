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
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        private IBL bl;
        private ParcelForList parcelDetails;
        private MainWindow mainWindow;

        public ParcelWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// constractor for Add the parcels
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="mainWindow"></param>
        public ParcelWindow(IBL bl, MainWindow mainWindow)
        {
            InitializeComponent();
            this.bl = bl;
            this.mainWindow = mainWindow;
           // id.Content = "Sender ID";
            //sender_Of_Parcel.Content = "Target ID";
            //target_To_Parcel.Content = "Wight";
            //weight.Content = "priority";
            Weight_Selector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            Priorities_Selector.ItemsSource = Enum.GetValues(typeof(Priorities));
            senderParcel.Visibility = Visibility.Hidden;
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
            this.bl = bl;
            this.parcelDetails = parcelDetails;
            this.mainWindow = mainWindow;
            DataContext = parcelDetails;
            if(parcelDetails.ParcelStatus==ParcelStatus.Associated)
            {
                CollectionParcelClick.IsEnabled = true;
            }
            else
            {
                CollectionParcelClick.IsEnabled = false;
            }
            Parcel parcel = bl.GetParcel(parcelDetails.ParcelID);
            ID.Text = parcel.ParcelID.ToString();
            Sender_Id.Text = parcel.SenderOfParcel.CustomerID.ToString();
            Sender_Name.Text = parcel.SenderOfParcel.CustomerName;
            Target_Id.Text = parcel.TargetToParcel.CustomerID.ToString();
            Target_Name.Text = parcel.TargetToParcel.CustomerName;
            if(parcel.DroneInParcel.DroneID!=0)
            {
                Drone_Id.Text = parcel.DroneInParcel.DroneID.ToString();
                Drone_Battery.Text = parcel.DroneInParcel.DroneBattery.ToString()+"%";
                Drone_Location.Text = parcel.DroneInParcel.CurrentLocation.ToString();
            }
            Requested.Text =parcel.Requested.ToString();
            Scheduled.Text = parcel.Scheduled.ToString();
            PickedUp.Text = parcel.PickedUp.ToString();
            ParcelDelivery.Text = parcel.ParcelDelivery.ToString();

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
                if (Sender_Id.Text==null || Target_Id.Text == null || Weight_Selector.SelectedItem == null || Priorities_Selector.SelectedItem == null)
                    MessageBox.Show("Not all detalis are set");
                else if (Sender_Id.Text.Length !=9 )
                    MessageBox.Show("Sender ID longer or shorter than 9 letters");
                else if (Target_Id.Text.Length != 9 )
                    MessageBox.Show("Target ID longer or shorter than 9 letters");
                else
                {
                    bl.AddParcelBo(int.Parse(Sender_Id.Text), int.Parse(Target_Id.Text),
                         (WeightCategories)Weight_Selector.SelectedItem, (Priorities)Priorities_Selector.SelectedItem);
                    MessageBox.Show("Adding a parcel was completed successfully");
                }
            }
            catch (BadCustomerIDException ex)
            {
                MessageBox.Show(ex.ID.ToString(), ex.Message + "\nThe customer does not exist in the system");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            mainWindow.LVListParcels.ItemsSource = bl.GetAllParcelsBo();
            mainWindow.LVListParcels.Items.Refresh();
            Close();
        }
        /// <summary>
        /// A button that opens a window for updating a Base Station
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
        }
    }
}
