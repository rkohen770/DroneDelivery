using IBL;
using IBL.BO;
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
    /// Interaction logic for DroneDisplayWindow.xaml
    /// </summary>
    public partial class AddDroneWindow : Window
    {
        private IBL.IBL bl = BLFactory.GetBL();
        public DroneForList drone { get; set; }
        private MainWindow mainWindow;
        public bool isAdd;
        public bool isUpdate;

    
        public AddDroneWindow ()
        {
            DialogResult = true;
            InitializeComponent();
        }
     
        public AddDroneWindow(IBL.IBL bl,DroneListWindow droneListWindow)
        {
            InitializeComponent();
            this.bl = bl;
            this.mainWindow = mainWindow;
            isAdd = true;
            isUpdate = false;
            SaveClick.Visibility = Visibility.Visible;
            Update.Visibility = Visibility.Hidden;
            status.Visibility = Visibility.Hidden;
            Status.Visibility = Visibility.Hidden;
            Battery.Visibility = Visibility.Hidden;
            battery.Visibility = Visibility.Hidden;
            WeightSelector.Visibility = Visibility.Visible;
            MaxWeight.Visibility = Visibility.Hidden;
            longitude.Visibility = Visibility.Hidden;
            Longitude.Visibility = Visibility.Hidden;
            lattitude.Visibility = Visibility.Hidden;
            Lattitude.Visibility = Visibility.Hidden;
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));

        }
        public AddDroneWindow(IBL.IBL bl, DroneForList drone, DroneListWindow droneListWindow)
        {
            InitializeComponent();
            this.bl = bl;
            this.drone = drone;
            this.mainWindow = mainWindow;
            isUpdate = true;
            isAdd = false;
            //DroneGrid.DataContext = drone;
            SaveClick.Visibility = Visibility.Hidden;
            Update.Visibility = Visibility.Visible;
            Sending.Visibility = Visibility.Visible;
            Release.Visibility = Visibility.Visible;
            Send_Delivery.Visibility = Visibility.Visible;
            Collection.Visibility = Visibility.Visible;
            Parcel_Delivery.Visibility = Visibility.Visible;
            status.Visibility = Visibility.Visible;
            Status.Visibility = Visibility.Visible;
            longitude.Visibility = Visibility.Visible;
            Longitude.Visibility = Visibility.Visible;
            lattitude.Visibility = Visibility.Visible;
            Lattitude.Visibility = Visibility.Visible;
            WeightSelector.Visibility = Visibility.Hidden;
            MaxWeight.Visibility = Visibility.Visible;
            ID.IsReadOnly = true;
            ID.Text = drone.DroneId.ToString();
            Model.Text = drone.DroneModel;
            MaxWeight.Text = drone.MaxWeight.ToString();
            Battery.Text = drone.DroneBattery.ToString();
            Status.IsReadOnly = true;
            Status.Text = drone.DroneStatus.ToString();
            stationID.Width = 200;
            stationID.Margin = new Thickness(30, 145, 0, 0);
            stationID.Content = "Parcel Num Is Transferred";
            StationID.Text = drone.ParcelNumIsTransferred.ToString();
            Longitude.Text = drone.CurrentLocation.Longitude.ToString();
            Lattitude.Text = drone.CurrentLocation.Lattitude.ToString();
            
        }

        /// <summary>
        /// text box that allows only numbers to be entered
        /// </summary>
        /// <param name="sender">TextBox type</param>
        /// <param name="e"></param>
        private void DroneIdTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Button for saving the details of the new drone
        /// </summary>
        /// <param name="sender">button type</param>
        /// <param name="e"></param>
        private void SaveDroneButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ID.Text == null || Model.Text == null || WeightSelector.SelectedItem == null || StationID.Text == null)
                    MessageBox.Show("Not all detalis are set");
                else if (ID.Text.Length > 4)
                    MessageBox.Show("Drone id longs then 4 letters");
                else if (StationID.Text.Length > 5)
                    MessageBox.Show("Station id longs then 5 letters");
                else
                {
                    bl.AddDroneBo(int.Parse(ID.Text), Model.Text,
                        (WeightCategories)WeightSelector.SelectedItem, int.Parse(StationID.Text));
                }
                MessageBox.Show("Adding a drone was completed successfully");

            }
            catch (BadDroneIDException ex)
            {
                MessageBox.Show(ex.ID.ToString(), ex.Message + "\nAdding a drone was not completed successfully");
            }
            catch (BadBaseStationIDException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            mainWindow.DronesListView.Items.Refresh();
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
        /// A button that opens a window for updating a drone
        /// </summary>
        /// <param name="sender">Button type</param>
        /// <param name="e"></param>
        private void UpdateDroneButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.UpdateNameOfDrone(int.Parse(ID.Text), Model.Text);
                MessageBox.Show("Update a drone was completed successfully");
                mainWindow.DronesListView.DataContext = bl.GetAllDronesBo();
                mainWindow.DronesListView.Items.Refresh();
            }
            catch(BadDroneIDException ex)
            {
                MessageBox.Show(ex.ID.ToString(), ex.Message );
            }
        }
         private void SendingButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.SendingDroneForCharging(int.Parse(ID.Text));
                MessageBox.Show("Sending the drone for charging was completed successfully");
                mainWindow.DronesListView.Items.Refresh();
            }
            catch (StatusDroneNotAllowException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (BadBaseStationIDException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (BadDroneIDException ex)
            {
                MessageBox.Show(ex.ID.ToString(), ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ReleaseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.UpdateReleaseDroneFromCharging(int.Parse(ID.Text), new TimeSpan(1, 0, 0));
                MessageBox.Show("Release the drone from charging was completed successfully");
                mainWindow.DronesListView.Items.Refresh();
            }
            catch (StatusDroneNotAllowException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (BadDroneIDException ex)
            {
                MessageBox.Show(ex.ID.ToString(), ex.Message);
            }
            catch (BadBaseStationIDException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void SendDeliveryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.UpdateAssignParcelToDrone(int.Parse(ID.Text));
                MessageBox.Show("Sending the drone for delivery was completed successfully");
                mainWindow.DronesListView.Items.Refresh();
            }
            catch (BatteryOfDroneNotAllowException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CollectionButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.UpdateCollectionParcelByDrone(int.Parse(ID.Text));
                MessageBox.Show("Collection parcel by drone was completed successfully");
                mainWindow.DronesListView.Items.Refresh();
            }
            catch (BadParcelIDException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (BadDroneIDException ex)
            {
                MessageBox.Show(ex.ID.ToString(), ex.Message);
            }
            catch (BadCustomerIDException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ParcelDeliveryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.UpdateDeliveryParcelByDrone(int.Parse(ID.Text));
                MessageBox.Show("Delivery parcel by drone was completed successfully");
                mainWindow.DronesListView.Items.Refresh();
            }
            catch (BadParcelIDException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (BadDroneIDException ex)
            {
                MessageBox.Show(ex.ID.ToString(), ex.Message);
            }
            catch (BadCustomerIDException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }




        private void closing(object sender, ToolTipEventArgs e)
        {

        }
    }
}
