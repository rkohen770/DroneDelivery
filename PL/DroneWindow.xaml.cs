﻿using BLApi;
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
    /// Interaction logic for DroneDisplayWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private IBL bl = BLFactory.GetBL();
        bool flag = false;
        private DroneForList drone { get; set; }
        private MainWindow mainWindow;

        public DroneWindow()
        {
            DialogResult = true;
            InitializeComponent();
        }
        /// <summary>
        /// This constractor is for adding a drone
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="mainWindow"></param>
        public DroneWindow(IBL bl, MainWindow mainWindow)
        {
            InitializeComponent();
            this.bl = bl;
            this.mainWindow = mainWindow;
            Height = 270;
            Close.Margin = new Thickness(372, 55, 0, 0);
            SaveClick.Margin = new Thickness(372, 105, 0, 0);
            CurrentLocation.Visibility = Visibility.Hidden;
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));

        }
        /// <summary>
        /// This constractor is for drone display
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="drone"></param>
        /// <param name="mainWindow"></param>
        public DroneWindow(IBL bl, DroneForList drone, MainWindow mainWindow)
        {
            InitializeComponent();
            this.bl = bl;
            this.drone = drone;
            this.mainWindow = mainWindow;
            CurrentLocation.Visibility = Visibility.Visible;
            ID.Text = drone.DroneID.ToString();
            Model.Text = drone.DroneModel;
            MaxWeight.Text = drone.MaxWeight.ToString();
            Battery.Text = drone.DroneBattery.ToString();
            Status.Text = drone.DroneStatus.ToString();
            stationID.Width = 200;
            stationID.Margin = new Thickness(30, 145, 0, 0);
            parcel_Num_Is_Transferred.Text = drone.ParcelNumIsTransferred.ToString();
            CurrentLocation.Text = drone.CurrentLocation.ToString();
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
            mainWindow.LVListDrones.Items.Refresh();
            flag = true;
            Close();
        }

        /// <summary>
        /// Button for closing a window
        /// </summary>
        /// <param name="sender">Button type</param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            flag = true;
            Close();
        }

        //Bouns.
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (flag.Equals(false)) e.Cancel = true;
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
                mainWindow.LVListDrones.ItemsSource = bl.GetAllDronesBo();
                mainWindow.LVListDrones.Items.Refresh();

            }
            catch (BadDroneIDException ex)
            {
                MessageBox.Show(ex.ID.ToString(), ex.Message);
            }
        }
        private void SendingButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.SendingDroneForCharging(int.Parse(ID.Text));
                MessageBox.Show("Sending the drone for charging was completed successfully");
                mainWindow.LVListDrones.ItemsSource = bl.GetAllDronesBo();
                mainWindow.LVListDrones.Items.Refresh();
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
                bl.UpdateReleaseDroneFromCharging(int.Parse(ID.Text), new TimeSpan(0, 5, 0));
                MessageBox.Show("Release the drone from charging was completed successfully");
                mainWindow.LVListDrones.ItemsSource = bl.GetAllDronesBo();
                mainWindow.LVListDrones.Items.Refresh();
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
                mainWindow.LVListDrones.Items.Refresh();
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
                mainWindow.LVListDrones.Items.Refresh();
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
                mainWindow.LVListDrones.Items.Refresh();
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

        private void parcel_Num_Is_Transferred_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (int.Parse(parcel_Num_Is_Transferred.Text) !=0)
            {
                ParcelForList parcelDetails = bl.CloneParcel(bl.GetParcel(int.Parse(parcel_Num_Is_Transferred.Text)));
                new ParcelWindow(bl, parcelDetails, mainWindow).ShowDialog();
            }
        }
    }
}