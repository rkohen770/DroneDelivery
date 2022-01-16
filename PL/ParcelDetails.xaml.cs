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
    /// Interaction logic for ParcelDetails.xaml
    /// </summary>
    public partial class ParcelDetails : Window
    {
        private IBL bl;
        private ParcelForList parcelDetails;
        /// <summary>
        /// constractor for dysplay a parcel
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="parcelDetails"></param>
        public ParcelDetails(IBL bl, ParcelForList parcelDetails)
        {

            InitializeComponent();
            this.bl = bl;
            this.parcelDetails = parcelDetails;
            DataContext = bl.GetParcel(parcelDetails.ParcelID);
            Parcel parcel = bl.GetParcel(parcelDetails.ParcelID);
            ID.Text = parcel.ParcelID.ToString();
            Sender_Id.Text = parcel.SenderOfParcel.CustomerID.ToString();
            Sender_Name.Text = parcel.SenderOfParcel.CustomerName;
            Target_Id.Text = parcel.TargetToParcel.CustomerID.ToString();
            Target_Name.Text = parcel.TargetToParcel.CustomerName;
            if (parcel.DroneInParcel.DroneID != 0)
            {
                Drone_Id.Text = parcel.DroneInParcel.DroneID.ToString();
                Drone_Battery.Text = parcel.DroneInParcel.DroneBattery.ToString() + "%";
                Drone_Location.Text = parcel.DroneInParcel.CurrentLocation.ToString();
            }
            else
            {
                Drone_Id.Text = "0";
                Drone_Battery.Text = "0%";
                Drone_Location.Text = null;
            }
            Requested.Text = parcel.Requested.ToString();
            Scheduled.Text = parcel.Scheduled.ToString();
            PickedUp.Text = parcel.PickedUp.ToString();
            ParcelDelivery.Text = parcel.ParcelDelivery.ToString();

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
    }
}
