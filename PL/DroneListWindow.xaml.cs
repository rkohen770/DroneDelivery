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

        public DroneListWindow()
        {
            InitializeComponent();
        }

        public DroneListWindow(IBL.IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            DronesListView.ItemsSource = bl.GetAllDronesBo();
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DroneStatus status = (DroneStatus)((ComboBox)sender).SelectedItem;
            List<DroneForList> list = bl.GetDronesByPredicat(d => d.DroneStatus == status).ToList();
            DronesListView.ItemsSource = list;
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WeightCategories weight = (WeightCategories)((ComboBox)sender).SelectedItem;
            List<DroneForList> list = bl.GetDronesByPredicat(d => d.MaxWeight == weight).ToList();
            DronesListView.ItemsSource = list;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
