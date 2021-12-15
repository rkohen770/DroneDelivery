using IBL;
using IBL.BO;
using System;
using System.Collections.Generic;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneDisplayWindow.xaml
    /// </summary>
    public partial class AddDroneWindow : Window
    {
        private IBL.IBL bl = BLFactory.GetBL();
        public DroneForList drone { get; set; }
        private DroneListWindow droneListWindow;
      
        public AddDroneWindow ()
        {
            DialogResult = true;
            InitializeComponent();
        }
        
        public AddDroneWindow(IBL.IBL bl,DroneListWindow droneListWindow)
        {
            InitializeComponent();
            this.bl = bl;
            SaveClick.Visibility = Visibility.Visible;
            Update.Visibility = Visibility.Hidden;
            status.Visibility = Visibility.Hidden;
            Status.Visibility = Visibility.Hidden;
            WeightSelector.Visibility = Visibility.Visible;
            MaxWeight.Visibility = Visibility.Hidden;
            this.droneListWindow = droneListWindow;
            longitude.Visibility = Visibility.Hidden;
            Longitude.Visibility = Visibility.Hidden;
            lattitude.Visibility = Visibility.Hidden;
            Lattitude.Visibility = Visibility.Hidden;
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));

        }
        public AddDroneWindow(IBL.IBL bl, DroneForList drone)
        {
            InitializeComponent();
            this.bl = bl;
            this.drone = drone;
            SaveClick.Visibility = Visibility.Hidden;
            Update.Visibility = Visibility.Visible;
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
            stationID.Margin = new Thickness(10, 225, 0, 0);
            stationID.Content = "Parcel Num Is Transferred";
            StationID.Text = drone.ParcelNumIsTransferred.ToString();
            Longitude.Text = drone.CurrentLocation.Longitude.ToString();
            Lattitude.Text = drone.CurrentLocation.Lattitude.ToString();
            
        }

        /// <summary>
        /// text box that allows only numbers to be entered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_OnlyNumbers_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            System.Windows.Controls.TextBox text = sender as TextBox;

            if (e.Key == Key.D0 || e.Key == Key.D1 || e.Key == Key.D2 || e.Key == Key.D3 || e.Key == Key.D4 || e.Key == Key.D5 || e.Key == Key.D6 || e.Key == Key.D7 || e.Key == Key.D8 || e.Key == Key.D9 || e.Key == Key.Enter || e.Key == Key.Delete || e.Key == Key.Escape || e.Key == Key.Back || e.Key == Key.Delete ||
                e.Key == Key.CapsLock || e.Key == Key.LeftShift || e.Key == Key.Home
                || e.Key == Key.End || e.Key == Key.Insert || e.Key == Key.Down || e.Key == Key.Right)
                return;
            e.Handled = true;
        }

        private void Save(object sender, RoutedEventArgs e)
        {

            try
            {
                bool flag = false;
                while (!flag)
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
                    flag = true;
                }
            }
            catch (BadDroneIDException ex)
            {
                MessageBox.Show(ex.ID.ToString(), ex.Message + "\nAdding a drone was not completed successfully");
            }
            catch (BadBaseStationIDException ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            droneListWindow.DronesListView.Items.Refresh();
            Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            bl.UpdateNameOfDrone(int.Parse(ID.Text), Model.Text);
            MessageBox.Show("Update a drone was completed successfully");
            
            Close();
        }

    }
}
