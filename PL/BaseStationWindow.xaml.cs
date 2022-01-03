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
            BaseStation station = bl.GetBaseStation(baseStationDetails.BaseStationId);
            ID.Text = station.BaseStationId.ToString();
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
            DroneForList drone= bl.CloneDrone(bl.GetDrone(droneDetails.DroneId));
            new DroneWindow(bl, drone, mainWindow).ShowDialog();
            LVDroneInChargings.ItemsSource = bl.GetAllDronesBo();
            LVDroneInChargings.Items.Refresh();
        }


    }
}
