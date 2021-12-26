using BLApi;
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
namespace PL
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        IBL bl = BLFactory.GetBL();
        BO.User user;
        public Login()
        {
            InitializeComponent();
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            //if the user did not fill in the name or password 
            if (password.Password.Length == 0 || userName.Text == "")
                MessageBox.Show("Please enter user name and password");
            else
            {
                try
                {
                    user = bl.GetUser(userName.Text);
                    if (user.UserName == userName.Text && user.password == password.Password)
                    {
                        if (user.Admin == BO.Permission.Managment)
                        {
                            MainWindow main = new MainWindow(user);
                            main.Show();
                        }
                        else
                        {
                            Client passenger = new Client(user);
                            passenger.Show();
                        }
                        Close();
                    }
                    else MessageBox.Show("Incorrect username or password");
                }
                catch (BO.BadUserNameException ex)
                {
                    MessageBox.Show(ex.Message);
                    userName.Text = "";
                    password.Password = "";
                }
            }
        }

        /// <summary>
        /// an event to enter to the main window
        /// </summary>
        private void SaveEnter_Click(object sender, RoutedEventArgs e)
        {

            if (passwordBox.Password.Length == 0 || username1.Text == "")
                MessageBox.Show("Please enter user name and password");
            else
            {
                if (user == null)
                    user = new BO.User();
                try
                {
                    user.password = passwordBox.Password;
                    user.UserName = username1.Text;
                    if ((checkM.IsChecked == true) && (managerP.Password == "buses"))
                    {
                        user.Admin = BO.Permission.Managment;

                        bl.AddUser(user);
                        MessageBox.Show("Manager user:" + user.UserName + " added succsesfully!");
                        MainWindow main = new MainWindow(user);
                        main.Show();
                        checkM.IsChecked = false;
                        passwordBox.Password = "";
                        username1.Text = "";
                        Close();
                    }
                    else if (checkM.IsChecked == true) MessageBox.Show("Incorrect manager pin");
                    else
                    {
                        user.Admin = BO.Permission.Passenger;
                        user.password = passwordBox.Password;
                        bl.AddUser(user);
                        MessageBox.Show("Passenger user:" + user.UserName + " added succsesfully!");
                        checkM.IsChecked = false;
                        passwordBox.Password = "";
                        Client passenger = new Client(user);
                        passenger.Show();
                        Close();
                    }
                }
                catch (BO.BadUserNameException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}

