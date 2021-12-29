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
        
        public MainWindow(User user)
        {
            InitializeComponent();
            MyUser = user;
            //reset list of ststions
            listStations = new ObservableCollection<BaseStationForList>(bl.GetAllBaseStationsBo());

            //reset the list of the drones   
            listDrone = new ObservableCollection<DroneForList>(bl.GetAllDronesBo());

            ChangeClient.DataContext = MyUser;
            //check the status of the drones
            //foreach (var d in listDrone) d.DroneStatus(1);
            LVListDrones.ItemsSource = listDrone;
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
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            // cmbDronesID.ItemsSource = listDrone;
        }
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
        /// an event to show the details line window
        /// </summary>

        private void ShowDroneDetails_Click(object sender, MouseButtonEventArgs e)
        {
            DroneForList droneDetails =((ListView)sender).SelectedItem as DroneForList;
            new DroneWindow(bl, droneDetails, this).ShowDialog();
            LVListDrones.ItemsSource = bl.GetAllDronesBo();
            LVListDrones.Items.Refresh();
        }
        #region User 
        /// <summary>
        /// an event show the login window
        /// </summary>

        private void ChangeUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Login enter = new ();
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
            Client passenger = new (MyUser, this);
            passenger.Show();
        }

        #endregion

        private void statusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DroneStatus status = (DroneStatus)((ComboBox)sender).SelectedItem;
            List<DroneForList> list = bl.GetDronesByPredicat(d => d.DroneStatus == status).ToList();
            LVListDrones.ItemsSource = list;
        }

        private void weightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WeightCategories weight = (WeightCategories)((ComboBox)sender).SelectedItem;
            List<DroneForList> list = bl.GetDronesByPredicat(d => d.MaxWeight == weight).ToList();
            LVListDrones.ItemsSource = list;
        }
    }
}