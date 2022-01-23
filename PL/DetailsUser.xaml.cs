using BLApi;
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
    /// Interaction logic for DetailsUser.xaml
    /// </summary>
    public partial class DetailsUser : Window
    {
        private IBL bl = BLFactory.GetBL();
        bool flag = false;
        public DetailsUser(User user)
        {
            InitializeComponent();
            CustomersName.Text = user.UserName;
        }

        private void CustomerIdTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Button for saving the details of the new customer
        /// </summary>
        /// <param name="sender">button type</param>
        /// <param name="e"></param>
        private void SaveCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ID.Text == null || CustomersName.Text == null || CustomersPhone.Text == null)
                    MessageBox.Show("Not all detalis are set");
                else if (CustomersPhone.Text.Length > 10)
                    MessageBox.Show("Phone number longs then 5 letters");
                else
                {
                    Location location = new() { Lattitude = double.Parse(Lattitude.Text), Longitude = double.Parse(Longttitude.Text) };
                    bl.AddCustomerBo(int.Parse(ID.Text), CustomersName.Text, CustomersPhone.Text, location);
                }
                MessageBox.Show("Adding a customer was completed successfully");

            }
            catch (BadCustomerIDException ex)
            {
                MessageBox.Show(ex.ID.ToString(), ex.Message + "\nAdding a customer was not completed successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

    }
}
