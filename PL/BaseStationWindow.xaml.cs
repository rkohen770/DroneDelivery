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

        public BaseStationWindow(IBL bl, BaseStationForList baseStationDetails, MainWindow mainWindow)
        {
            InitializeComponent();
            this.bl = bl;
            this.baseStationDetails = baseStationDetails;
            this.mainWindow = mainWindow;

           BaseStation station = bl.GetBaseStation(baseStationDetails.BaseStationId);
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


    }
}
