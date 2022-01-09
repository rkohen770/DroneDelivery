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
    /// Interaction logic for BaseStationWindow.xaml
    /// </summary>
    public partial class BaseStationWindow : Window
    {
        private IBL bl;
        private BaseStationForList baseStationDetails;
        private MainWindow mainWindow;

        public BaseStationWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// constractor for update the base station
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="baseStationDetails"></param>
        /// <param name="mainWindow"></param>
        public BaseStationWindow(IBL bl, MainWindow mainWindow)
        {
            InitializeComponent();
            this.bl = bl;
            this.mainWindow = mainWindow;
            CurrentLocation.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// constractor for dysplay a base station
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="baseStationDetails"></param>
        /// <param name="mainWindow"></param>
        public BaseStationWindow(IBL bl, BaseStationForList baseStationDetails, MainWindow mainWindow)
        {
            InitializeComponent();
            this.bl = bl;
            this.baseStationDetails = baseStationDetails;
            this.mainWindow = mainWindow;
            BaseStation station = bl.GetBaseStation(baseStationDetails.BaseStationID);
            ID.Text = station.BaseStationID.ToString();
            Name.Text = station.NameBaseStation.ToString();
            CurrentLocation.Text = station.Location.ToString();
            NumOfAvailableChargingPositions.Text = station.NumOfAvailableChargingPositions.ToString();

            if (station.DroneInChargings.Count > 0)
                LVDroneInChargings.ItemsSource = station.DroneInChargings;

        }

        /// <summary>
        /// text box that allows only numbers to be entered
        /// </summary>
        /// <param name="sender">TextBox type</param>
        /// <param name="e"></param>
        private void StationIdTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ShowDroneDetails_Click(object sender, MouseButtonEventArgs e)
        {
            DroneInCharging droneDetails = ((ListView)sender).SelectedItem as DroneInCharging;
            DroneForList drone= bl.CloneDrone(bl.GetDrone(droneDetails.DroneID));
            new DroneWindow(bl, drone, mainWindow).ShowDialog();
            LVDroneInChargings.ItemsSource = bl.GetAllDronesBo();
            LVDroneInChargings.Items.Refresh();
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
        /// Button for saving the details of the new drone
        /// </summary>
        /// <param name="sender">button type</param>
        /// <param name="e"></param>
        private void SaveBaseStationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ID.Text == null || Name.Text == null || CurrentLocation.Text == null || NumOfAvailableChargingPositions.Text == null)
                    MessageBox.Show("Not all detalis are set");
                else if (ID.Text.Length > 4)
                    MessageBox.Show("Base Station ID longs then 4 letters");
                else if (Name.Text.Length > 5)
                    MessageBox.Show("Base Station Name longs then 5 letters");
                //else
                //{
                //    bl.AddBaseStationBo(int.Parse(ID.Text), Name.Text,
                //        CurrentLocation.Text, int.Parse(StationID.Text));
                //}
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
            mainWindow.LVListDrones.Items.Refresh();
            Close();
        }

    }
}
