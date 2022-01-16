using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DalApi;

namespace DalApi
{
     partial class DalObject: IDal 
    {
        /// <summary>
        /// users XElement
        /// </summary>
        string UseresPath = @"UseresXml.xml";

        /// <summary>
        /// returns user by the user name from the file
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User GetUser(string userName, string password)
        {
            //List<User> users = Tools.LoadListFromXMLSerializer<User>(UseresPath);
            if (!DataSource.users.Exists(u => u.UserName == userName&&u.Password==password))
            {
                throw new BadUserNameException(userName, "the user not exists in the list of users");
            }
            //find the place of the customer in the array of customers
            return DataSource.users.Find(c => c.UserName == userName);

        }
        /// <summary>
        /// returns all users from the file
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DO.User> GetAllUseres()
        {
            //List<User> users = Tools.LoadListFromXMLSerializer<User>(UseresPath);
            return DataSource.users.AsEnumerable();
        }
        /// <summary>
        /// returns all users by predicate from the file
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<DO.User> GetAllUseresBy(Predicate<DO.User> predicate)
        {
            List<User> users = Tools.LoadListFromXMLSerializer<User>(UseresPath);
            return from u1 in users
                   where predicate(u1) && u1.Available == true
                   select u1;
        }
        /// <summary>
        /// adds new user to the file
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(User user)
        {
            if (DataSource.users.Exists(u=>u.UserName==user.UserName && u.Admin==user.Admin && u.Password==user.Password))
            {
                throw new BadUserNameException(user.UserName, "the user exists allready");
            }
            else
            {
                User u = new()
                {
                    UserName = user.UserName,
                    Admin = user.Admin,
                    Password = user.Password
                };
                DataSource.users.Add(u); //Adding the new user to the list
            }

        }
        /// <summary>
        /// deletes user from the file
        /// </summary>
        /// <param name="userName"></param>
        public void DeleteUser(string userName)
        {
            List<User> users = Tools.LoadListFromXMLSerializer<User>(UseresPath);

            User? user = (from u in users
                         where u.UserName == userName
                         select u).FirstOrDefault();

            if (user != null)
            {
                User user1 = (User)user;
                user1.Available = false;
                users.Add(user1);
                Tools.SaveListToXMLSerializer(users, UseresPath);
            }
            else
                throw new BadUserNameException(userName, $"bad user name: {userName}");
        }
        /// <summary>
        /// updates user in the file
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(DO.User user)
        {
            //List<User> users = Tools.LoadListFromXMLSerializer<User>(UseresPath);
            List<User> users = DataSource.users;
            User ? user1 = (from u in users
                          where u.UserName == user.UserName
                          select u).FirstOrDefault();

            if (user1 != null)
            {
                users.Remove((User)user1);
                users.Add(user);
                Tools.SaveListToXMLSerializer(users, UseresPath);
            }
            else
                throw new DO.BadUserNameException(user.UserName, $"bad user name: {user.UserName}");
        }

    }
}
