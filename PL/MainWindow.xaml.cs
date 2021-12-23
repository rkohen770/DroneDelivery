﻿using System;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// collection of lines
        /// </summary>
        ObservableCollection<DroneForList> listDrones;

        private IBL bl = BLFactory.GetBL();
        public MainWindow()
        {
            InitializeComponent();
            //reset the lines list
            listDrones = new ObservableCollection<DroneForList>(bl.GetAllDronesBo());


            //show the lines
            cmbDronesID.ItemsSource = listDrones;
            cmbDronesID.DisplayMemberPath = "DroneId";
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










        ///// <summary>
        ///// A button that opens a window of the drone details
        ///// </summary>
        ///// <param name="sender">Button type</param>
        ///// <param name="e"></param>
        //private void ShowDroneButton_Click(object sender, RoutedEventArgs e)
        //{
        //    new DroneListWindow(bl).Show();
        //}

    }
}