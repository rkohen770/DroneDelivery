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
using System.Windows.Threading;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IBL bl = BLFactory.GetBL();
        public User MyUser { get; set; }
        public bool ClientOpen { get; set; }
        public ObservableCollection<PO.ViewTimes> times = new ObservableCollection<PO.ViewTimes>();
        public TimeSpan second = new TimeSpan(0, 0, 0, 1);
        DispatcherTimer timer;
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
        /// <summary>
        /// collection of parcels
        /// </summary>
        ObservableCollection<ParcelForList> listParcels;

        public MainWindow(User user)
        { 
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            lDate.Content = DateTime.Now.ToString("dd/MM/yy");

            
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

            //reset list of parcels
            listParcels = new ObservableCollection<ParcelForList>(bl.GetAllParcelsBo());
            LVListParcels.ItemsSource = listParcels;

            ChangeClient.DataContext = MyUser;
            //check the status of the drones
            //foreach (var d in listDrone) d.DroneStatus(1);

            // cmbDronesID.ItemsSource = listDrone;
            userGrid.DataContext = MyUser;

            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));

            ParcelStatusSelector.ItemsSource = Enum.GetValues(typeof(ParcelStatus));
            ParcelPrioritiesSelector.ItemsSource = Enum.GetValues(typeof(Priorities));

        }
        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            lDate.Content = DateTime.Now.ToString("dd/MM/yy");

            //reset the list of the drones   
            listDrone = new ObservableCollection<DroneForList>(bl.GetAllDronesBo());
            LVListDrones.ItemsSource = listDrone;

            //reset the list of the base station  
            listStations = new ObservableCollection<BaseStationForList>(bl.GetAllBaseStationsBo());
            LVListBaseStations.ItemsSource = listStations;

            //reset the list of the customers   
            listCustomers = new ObservableCollection<CustomerForList>(bl.GetAllCustomersBo());
            LVListCustomers.ItemsSource = listCustomers;

            //reset the list of the parcels   
            listParcels = new ObservableCollection<ParcelForList>(bl.GetAllParcelsBo());
            LVListParcels.ItemsSource = listParcels;

            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            // cmbDronesID.ItemsSource = listDrone;

            ParcelStatusSelector.ItemsSource = Enum.GetValues(typeof(ParcelStatus));
            ParcelPrioritiesSelector.ItemsSource = Enum.GetValues(typeof(Priorities));
        }

        #region Time
        /// <summary>
        /// an event that shows the timer
        /// </summary>
        private void timer_Tick(object sender, EventArgs e)
        {
            lblTime.Content = DateTime.Now.ToLongTimeString();

            for (int i = 0; i < times.Count(); i++)
            {
                if (times[i].LastTime == TimeSpan.Zero)
                    times.Remove(times[i]);
                else times[i].LastTime -= second;
            }

        }
        #endregion

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

        private void DroneStatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedItem == null)
            {
                LVListDrones.ItemsSource = listDrone;

            }
            else
            {
                DroneStatus status = (DroneStatus)((ComboBox)sender).SelectedItem;
                List<DroneForList> list = bl.GetDronesByPredicat(d => d.DroneStatus == status).ToList();
                LVListDrones.ItemsSource = list;
            }

        }
        private void ClearFilledStatusSelectorComboBox_Click(object sender, RoutedEventArgs e)
        {
            StatusSelector.SelectedItem = null;
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedItem == null)
            {
                LVListDrones.ItemsSource = listDrone;

            }
            else
            {
                WeightCategories weight = (WeightCategories)((ComboBox)sender).SelectedItem;
                List<DroneForList> list = bl.GetDronesByPredicat(d => d.MaxWeight == weight).ToList();
                LVListDrones.ItemsSource = list;
            }
        }
        private void ClearFilledWeightSelectorComboBox_Click(object sender, RoutedEventArgs e)
        {
            WeightSelector.SelectedItem = null;
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

        /// <summary>
        /// A button that opens a window for adding a base station
        /// </summary>
        /// <param name="sender">Button type</param>
        /// <param name="e"></param>
        private void AddBaseStationButton_Click(object sender, RoutedEventArgs e)
        {
            new BaseStationWindow(bl, this).ShowDialog();
            LVListBaseStations.ItemsSource = bl.GetAllBaseStationsBo();
            LVListBaseStations.Items.Refresh();
        }

        /// <summary>
        /// A button that refresh the list of base station.
        /// </summary>
        /// <param name="sender">Button type</param>
        /// <param name="e"></param>
        private void RefreshBaseStationButton_Click(object sender, RoutedEventArgs e)
        {
            LVListBaseStations.ItemsSource = bl.GetAllBaseStationsBo().OrderBy(b => b.NumOfAvailableChargingPositions);
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
        /// <summary>
        /// A button that opens a window for adding a customer
        /// </summary>
        /// <param name="sender">Button type</param>
        /// <param name="e"></param>
        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(bl, this).ShowDialog();
            LVListCustomers.ItemsSource = bl.GetAllCustomersBo();
            LVListCustomers.Items.Refresh();
        }
        #endregion

        #region Parcel
        /// <summary>
        /// A button that opens a window for adding a parcel
        /// </summary>
        /// <param name="sender">Button type</param>
        /// <param name="e"></param>
        private void AddParcelsButton_Click(object sender, RoutedEventArgs e)
        {
            new ParcelWindow(bl, this).ShowDialog();
            LVListParcels.ItemsSource = bl.GetAllParcelsBo();
            LVListParcels.Items.Refresh();
        }
        /// <summary>
        /// an event to show the details parcel window
        /// </summary>
        private void ShowParcelDetails_Click(object sender, MouseButtonEventArgs e)
        {
            ParcelForList parcelDetails = ((ListView)sender).SelectedItem as ParcelForList;
            new ParcelWindow(bl, parcelDetails, this).ShowDialog();
            LVListBaseStations.ItemsSource = bl.GetAllBaseStationsBo();
            LVListBaseStations.Items.Refresh();
        }

        /// <summary>
        /// A button that refresh the list of parcels order by target customer
        /// </summary>
        /// <param name="sender">Button type</param>
        /// <param name="e"></param>
        private void SortParcelByTargetButton_Click(object sender, RoutedEventArgs e)
        {
            LVListParcels.ItemsSource = bl.GetAllParcelsBo().OrderBy(p => p.CustomerNameTarget);
            LVListParcels.Items.Refresh();
        }
        /// <summary>
        /// A button that refresh the list of parcels order by send customer
        /// </summary>
        /// <param name="sender">Button type</param>
        /// <param name="e"></param>
        private void SortParcelBySenderButton_Click(object sender, RoutedEventArgs e)
        {
            LVListParcels.ItemsSource = bl.GetAllParcelsBo().OrderBy(p => p.CustomerNameSend);
            LVListParcels.Items.Refresh();
        }
        /// <summary>
        /// Filter a list by criterion - parcel priorities
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParcelPrioritiesSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedItem == null)
                LVListParcels.ItemsSource = listParcels;
            else
            {
                Priorities priority = (Priorities)((ComboBox)sender).SelectedItem;
                List<ParcelForList> lists = bl.GetParcelsByPredicat(p => p.Priorities == priority).ToList();
                LVListParcels.ItemsSource = lists;
            }
        }
        /// <summary>
        /// Filter a list by criterion - parcel status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParcelStatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (((ComboBox)sender).SelectedItem == null)
                LVListParcels.ItemsSource = listParcels;
            else
            {
                ParcelStatus status = (ParcelStatus)((ComboBox)sender).SelectedItem;
                List<ParcelForList> list = bl.GetParcelsByPredicat(p => p.ParcelStatus == status).ToList();
                LVListParcels.ItemsSource = list;
            }
        }
        private void ClearFilledParcelStatusSelectorComboBox_Click(object sender, RoutedEventArgs e)
        {
            ParcelStatusSelector.SelectedItem = null;
        }
        private void ClearFilledPrioritySelectorComboBox_Click(object sender, RoutedEventArgs e)
        {
            ParcelPrioritiesSelector.SelectedItem = null;
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
            ClientOpen = true;
            Client passenger = new(MyUser, this);
            passenger.Show();
        }

        #endregion


    }
}