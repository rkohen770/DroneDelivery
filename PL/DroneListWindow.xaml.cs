using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using IBL;
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        private IBL.IBL bl = BLFactory.GetBL();
        List<DroneForList> droneCollection;
      
        public DroneListWindow()
        {
            InitializeComponent();
        }

        public DroneListWindow(IBL.IBL _bl)
        {
            InitializeComponent();
            droneCollection = new List<DroneForList>(bl.GetAllDronesBo());
            bl = _bl;
            DronesListView.ItemsSource = bl.GetAllDronesBo();
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        private void statusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DroneStatus status = (DroneStatus)((ComboBox)sender).SelectedItem;
            List<DroneForList> list = bl.GetDronesByPredicat(d => d.DroneStatus == status).ToList();
            DronesListView.ItemsSource = list;
        }

        private void weightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WeightCategories weight = (WeightCategories)((ComboBox)sender).SelectedItem;
            List<DroneForList> list = bl.GetDronesByPredicat(d => d.MaxWeight == weight).ToList();
            DronesListView.ItemsSource = list;
        }

        /// <summary>
        /// A button that opens a window for adding a drone
        /// </summary>
        /// <param name="sender">Button type</param>
        /// <param name="e"></param>
        private void AddDroneButton_Click(object sender, RoutedEventArgs e)
        {
            new AddDroneWindow(bl, this).ShowDialog();
            DronesListView.Items.Refresh();
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
        /// A button that opens a window of the drone details
        /// </summary>
        /// <param name="sender">Button type</param>
        /// <param name="e"></param>
        private void ShowDronesButtom_Click(object sender, MouseButtonEventArgs e)
        {
            DroneForList drone = droneCollection[((ListView)sender).SelectedIndex];
            new AddDroneWindow( bl, drone).Show();
            DronesListView.Items.Refresh();
        }

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
