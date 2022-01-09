using BLApi;
using BO;
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
        /// <param name="baseStationDetails"></param>
        /// <param name="mainWindow"></param>
        public ParcelWindow(IBL bl, MainWindow mainWindow)
        {
            InitializeComponent();
            this.bl = bl;
            this.mainWindow = mainWindow;
        }

        /// <summary>
        /// constractor for dysplay a 
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="baseStationDetails"></param>
        /// <param name="mainWindow"></param>
        public ParcelWindow(IBL bl, ParcelForList parcelDetails, MainWindow mainWindow)
        {
            InitializeComponent();
            this.bl = bl;
            this.parcelDetails = parcelDetails;
            this.mainWindow = mainWindow;
            Parcel parcel = bl.GetParcel(parcelDetails.ParcelID);
            //ID.Text = station.BaseStationID.ToString();
            //Name.Text = station.NameBaseStation.ToString();
            //CurrentLocation.Visibility = Visibility.Visible;
            //CurrentLocation.Text = station.Location.ToString();
            //NumOfAvailableChargingPositions.Text = station.NumOfAvailableChargingPositions.ToString();

            //if (station.DroneInChargings.Count > 0)
            //    LVDroneInChargings.ItemsSource = station.DroneInChargings;

        }



    }
}
