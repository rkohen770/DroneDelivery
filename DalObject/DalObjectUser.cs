using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DalApi;

namespace DL
{
    partial class DalObject : IDal
    {

        /// <summary>
        /// returns user by the user name from the file
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User GetUser(string userName, string password = null)
        {
            //List<User> users = Tools.LoadListFromXMLSerializer<User>(UseresPath);
            if (password != null)
            {
                if (!DataSource.users.Exists(u => u.UserName == userName && u.Password == password))
                {
                    throw new BadUserNameException(userName, "the user not exists in the list of users");
                }
            }
            //find user in the array of users
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
            return from user in DataSource.users
                   where predicate(user)
                   select user.Clone();

        }
        /// <summary>
        /// adds new user to the file
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(User user)
        {
            if (DataSource.users.Exists(u => u.UserName == user.UserName && u.Admin == user.Admin && u.Password == user.Password))
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
            if (!DataSource.users.Exists(u => u.UserName == userName))
            {
                throw new BadUserNameException(userName, $"bad user name: {userName}");
            }
            else
            {
                //We will go through the entire list of the drone, to find a available drone
                for (int uIndex = 0; uIndex < DataSource.parcels.Count; uIndex++)
                {
                    if (DataSource.users[uIndex].UserName == userName)
                    {
                        User user = DataSource.users[uIndex];//Obtain an index for the location where the user is located
                        user.Available = false;//Update the avilabl field in the drone package found
                        DataSource.users[uIndex] = user;
                    }
                }
            }
        }
        /// <summary>
        /// updates user in the file
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(DO.User user)
        {
            if (!DataSource.users.Exists(u => u.UserName == user.UserName))
            {
                throw new BadUserNameException(user.UserName, $"bad user name: {user.UserName}");
            }
            else
            {
                //We will go through the entire list of the drone, to find a available drone
                for (int uIndex = 0; uIndex < DataSource.parcels.Count; uIndex++)
                {
                    if (DataSource.users[uIndex].UserName == user.UserName)
                    {
                        User user1 = new()
                        {
                            UserName = user.UserName,
                            Password = user.Password,
                            Admin = user.Admin,
                            Available = user.Available
                        };
                        DataSource.users[uIndex] = user1;
                    }
                }
            }
        }

    }
}
