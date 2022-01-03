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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BO;
using BLApi;
using System.Collections.ObjectModel;
using System.IO;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IBL bl = BLFactory.GetBL();
        public User MyUser { get; set; }
        public bool PassengerOpen { get; set; }
        /// <summary>
        /// collection of stations
        /// </summary>
        ObservableCollection<BaseStationForList> listStations;
        /// <summary>
        /// collection of drones
        /// </summary>
        ObservableCollection<DroneForList> listDrone;
        /// <summary>
        /// collection of customers
        /// </summary>
        ObservableCollection<CustomerForList> listCustomers;

        public MainWindow(User user)
        {
            InitializeComponent();
            MyUser = user;
            //reset list of ststions
            listStations = new ObservableCollection<BaseStationForList>(bl.GetAllBaseStationsBo());
            LVListBaseStations.ItemsSource = listStations;

            //reset the list of the drones   
            listDrone = new ObservableCollection<DroneForList>(bl.GetAllDronesBo());
            LVListDrones.ItemsSource = listDrone;

            //reset list of customers
            listCustomers = new ObservableCollection<CustomerForList>(bl.GetAllCustomersBo());
            LVListCustomers.ItemsSource = listCustomers;

            ChangeClient.DataContext = MyUser;
            //check the status of the drones
            //foreach (var d in listDrone) d.DroneStatus(1);

            // cmbDronesID.ItemsSource = listDrone;
            userGrid.DataContext = MyUser;

            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }
        public MainWindow()
        {
            InitializeComponent();

            //reset the list of the drones   
            listDrone = new ObservableCollection<DroneForList>(bl.GetAllDronesBo());
            LVListDrones.ItemsSource = listDrone;

            //reset the list of the base station  
            listStations = new ObservableCollection<BaseStationForList>(bl.GetAllBaseStationsBo());
            LVListBaseStations.ItemsSource = listStations;

            //reset the list of the customers   
            listCustomers = new ObservableCollection<CustomerForList>(bl.GetAllCustomersBo());
            LVListCustomers.ItemsSource = listCustomers;

            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            // cmbDronesID.ItemsSource = listDrone;
        }

        #region Drones
        /// <summary>
        /// A button that opens a window for adding a drone
        /// </summary>
        /// <param name="sender">Button type</param>
        /// <param name="e"></param>
        private void AddDroneButton_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(bl, this).ShowDialog();
            LVListDrones.ItemsSource = bl.GetAllDronesBo();
            LVListDrones.Items.Refresh();
        }

        /// <summary>
        /// A button that refresh the list of drones.
        /// </summary>
        /// <param name="sender">Button type</param>
        /// <param name="e"></param>
        private void RefreshDronesButton_Click(object sender, RoutedEventArgs e)
        {
            LVListDrones.ItemsSource = bl.GetAllDronesBo().OrderBy(d => d.DroneStatus);
            LVListDrones.Items.Refresh();
        }

        /// <summary>
        /// an event to show the details drone window
        /// </summary>

        private void ShowDroneDetails_Click(object sender, MouseButtonEventArgs e)
        {
            DroneForList droneDetails = ((ListView)sender).SelectedItem as DroneForList;
            new DroneWindow(bl, droneDetails, this).ShowDialog();
            LVListDrones.ItemsSource = bl.GetAllDronesBo();
            LVListDrones.Items.Refresh();
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DroneStatus status = (DroneStatus)((ComboBox)sender).SelectedItem;
            List<DroneForList> list = bl.GetDronesByPredicat(d => d.DroneStatus == status).ToList();
            LVListDrones.ItemsSource = list;
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WeightCategories weight = (WeightCategories)((ComboBox)sender).SelectedItem;
            List<DroneForList> list = bl.GetDronesByPredicat(d => d.MaxWeight == weight).ToList();
            LVListDrones.ItemsSource = list;
        }
        #endregion

        #region Base Station

        /// <summary>
        /// an event to show the details base station window
        /// </summary>
        private void ShowBaseStationDetails_Click(object sender, MouseButtonEventArgs e)
        {
            BaseStationForList baseStationDetails = ((ListView)sender).SelectedItem as BaseStationForList;
            new BaseStationWindow(bl, baseStationDetails, this).ShowDialog();
            LVListBaseStations.ItemsSource = bl.GetAllBaseStationsBo();
            LVListBaseStations.Items.Refresh();
        }

        #endregion

        #region Customer
        private void ShowCustomerDetails_Click(object sender, MouseButtonEventArgs e)
        {
            CustomerForList customerDetails = ((ListView)sender).SelectedItem as CustomerForList;
            new CustomerWindow(bl, customerDetails, this).ShowDialog();
            LVListCustomers.ItemsSource = bl.GetAllCustomersBo();
            LVListCustomers.Items.Refresh();
        }

        #endregion

        #region User 
        /// <summary>
        /// an event show the login window
        /// </summary>

        private void ChangeUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Login enter = new();
                enter.Show();
                Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// an event to show the passenger window
        /// </summary>

        private void ChangeClient_Click(object sender, RoutedEventArgs e)
        {
            PassengerOpen = true;
            Client passenger = new(MyUser, this);
            passenger.Show();
        }

        #endregion


    }
}