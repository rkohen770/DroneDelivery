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
using BL;
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IBL.IBL bl = new BlObject();
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void ShowDroneButton_Click(object sender, RoutedEventArgs e)
        {
            new DroneListWindow(bl).Show();

        }

    }
}
