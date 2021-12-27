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
        ObservableCollection<BaseStation> listStations;
        /// <summary>
        /// collection of drones
        /// </summary>
        ObservableCollection<DroneForList> listDrone;
        
        public MainWindow(User user)
        {
            InitializeComponent();
            MyUser = user;
            //reset list of ststions
            listStations = new ObservableCollection<BaseStation>((IEnumerable<BaseStation>)bl.GetAllBaseStationsBo());

            //reset the list of the drones   
            listDrone = new ObservableCollection<DroneForList>(bl.GetAllDronesBo());
           
            userGrid.DataContext = MyUser;
            //check the status of the drones
            foreach (var d in listDrone) d.DroneStatus(1);

            LVListDrones.ItemsSource = listDrone;
            // userGrid.DataContext = MyUser;
        }
        public MainWindow()
        {
            InitializeComponent();

            //reset the list of the drones   
            listDrone = new ObservableCollection<DroneForList>(bl.GetAllDronesBo());
            LVListDrones.ItemsSource = listDrone;
            cmbDronesID.ItemsSource = listDrone;
        }
        /// <summary>
        /// A button that opens a window for adding a drone
        /// </summary>
        /// <param name="sender">Button type</param>
        /// <param name="e"></param>
        private void AddDroneButton_Click(object sender, RoutedEventArgs e)
        {
            new AddDroneWindow(bl, this).ShowDialog();
        }




        /// <summary>
        /// A button that opens a window of the drone details
        /// </summary>
        /// <param name="sender">Button type</param>
        /// <param name="e"></param>
        private void ShowDroneButton_Click(object sender, RoutedEventArgs e)
        {
            new DroneListWindow(bl).Show();
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

        private void ChangePassenger_Click(object sender, RoutedEventArgs e)
        {
            PassengerOpen = true;
            Client passenger = new (MyUser, this);
            passenger.Show();
        }

        #endregion
    }
}