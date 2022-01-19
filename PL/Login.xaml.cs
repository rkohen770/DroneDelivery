using BLApi;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Serialization;

namespace PL
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        readonly IBL bl = BLFactory.GetBL();
        BO.User user;
        public Login()
        {
            InitializeComponent();
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            //if the user did not fill in the name or password 
            if (password.Password.Length == 0 || userName.Text == "")
                MessageBox.Show("Please enter user name and Password");
            else
            {
                try
                {
                    user = bl.GetUser(userName.Text, password.Password);
                    if (user.UserName == userName.Text && user.Password == password.Password)
                    {
                        if (user.Admin == BO.Permission.Managment)
                        {
                            MainWindow main = new (user);
                            main.Show();
                        }
                        else
                        {
                            Client client = new (user);
                            client.Show();
                        }
                        Close();
                    }
                    else MessageBox.Show("Incorrect username or Password");
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
                MessageBox.Show("Please enter user name and Password");
            else
            {
                if (user == null)
                    user = new BO.User();
                try
                {
                    user.Password = passwordBox.Password;
                    user.UserName = username1.Text;
                    if ((checkM.IsChecked == true) && (managerP.Password == "drones"))
                    {
                        user.Admin = BO.Permission.Managment;

                        bl.AddUser(user);
                        MessageBox.Show("Manager user:" + user.UserName + " added succsesfully!");
                        new MainWindow(user).Show();
                        checkM.IsChecked = false;
                        passwordBox.Password = "";
                        username1.Text = "";
                        Close();
                    }
                    else if (checkM.IsChecked == true) MessageBox.Show("Incorrect manager pin");
                    else
                    {
                        user.Admin = BO.Permission.Client;
                        user.Password = passwordBox.Password;
                        bl.AddUser(user);
                        MessageBox.Show("Client user:" + user.UserName + " added succsesfully!");
                        checkM.IsChecked = false;
                        passwordBox.Password = "";
                        DetailsUser details = new DetailsUser(user);
                        details.ShowDialog();
                        Client client = new (user);
                        client.Show();
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

        private void Button_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string dir = Directory.GetCurrentDirectory();

            List<DO.Drone> list2 = new(DL.DalObject.Instance.GetAllDrones());
            FileStream file2 = new FileStream(dir + @"\Data\DronesXml.xml", FileMode.Create);
            XmlSerializer x2 = new XmlSerializer(list2.GetType());
            x2.Serialize(file2, list2);
            file2.Close();

            List<DO.User> list3 = new(DL.DalObject.Instance.GetAllUseres());
            FileStream file3 = new FileStream(dir + @"\Data\UseresXml.xml", FileMode.Create);
            XmlSerializer x3 = new XmlSerializer(list3.GetType());
            x3.Serialize(file3, list3);
            file3.Close();

           List<DO.Customer> list4 = new(DL.DalObject.Instance.GetAllCustomers());
           FileStream file4 = new FileStream(dir + @"\Data\CustomersXml.xml", FileMode.Create);
           XmlSerializer x4 = new XmlSerializer(list4.GetType());
           x4.Serialize(file4, list4);
           file4.Close();

            //List<DO.DroneCharge> list5 = new(DL.DalObject.Instance.GetDronesInChargingsAtStation(x => x.DroneId == x.DroneId));
            //FileStream file5 = new FileStream(dir + @"\Data\DroneCharges.xml", FileMode.Create);
            //XmlSerializer x5 = new XmlSerializer(list5.GetType());
            //x5.Serialize(file5, list5);
            //file5.Close();

            List<DO.Parcel> list6 = new(DL.DalObject.Instance.GetAllParcels());
            FileStream file6 = new FileStream(dir + @"\Data\ParcelsXml.xml", FileMode.Create);
            XmlSerializer x6 = new XmlSerializer(list6.GetType());
            x6.Serialize(file6, list6);
            file6.Close();

            List<DO.Station> list7 = new(DL.DalObject.Instance.GetAllBaseStations());
            FileStream file7 = new FileStream(dir + @"\Data\StationsXml.xml", FileMode.Create);
            XmlSerializer x7 = new XmlSerializer(list7.GetType());
            x7.Serialize(file7, list7);
            file7.Close();

        }
    }
}

