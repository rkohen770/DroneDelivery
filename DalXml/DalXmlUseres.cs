using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DalApi;
using DO;

namespace DL
{
    public sealed partial class DalXml : IDal
    {
        #region User
        /// <summary>
        /// returns user by the user name from the file
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public User GetUser(string userName, string password = null)
        {

            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(dir + UseresPath);

            User user = users.Find(u => u.UserName == userName && u.Available == true);
            if (user.UserName != null)
                return user; //no need to Clone()
            else
                throw new BadUserNameException(userName, $"bad User Name: {userName}");

        }

        /// <summary>
        /// returns all users from the file
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetAllUseres()
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(dir + UseresPath);
            return users.AsEnumerable();
        }
        /// <summary>
        /// returns all users by predicate from the file
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<User> GetAllUseresBy(Predicate<DO.User> predicate)
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(dir + UseresPath);
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
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(dir + UseresPath);
            DO.User user1 = (from u in users
                             where u.UserName == user.UserName
                             select u).FirstOrDefault();

            if (user1.UserName != null)
                throw new BadUserNameException(user.UserName, "Duplicate user name");
            users.Add(user);
            XMLTools.SaveListToXMLSerializer(users, UseresPath);

        }
        /// <summary>
        /// deletes user from the file
        /// </summary>
        /// <param name="userName"></param>
        public void DeleteUser(string userName)
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(dir + UseresPath);

            User user = (from u in users
                         where u.UserName == userName
                         select u).FirstOrDefault();

            if (user.UserName != null)
            {
                user.Available = false;
                users.Add(user);
                XMLTools.SaveListToXMLSerializer(users,  UseresPath);
            }
            else
                throw new BadUserNameException(userName, $"bad user name: {userName}");
        }
        /// <summary>
        /// updates user in the file
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(User user)
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(dir + UseresPath);

            User user1 = (from u in users
                          where u.UserName == user.UserName
                          select u).FirstOrDefault();

            if (user1.UserName != null)
            {
                users.Remove(user1);
                users.Add(user);
                XMLTools.SaveListToXMLSerializer(users, dir + UseresPath);
            }
            else
                throw new BadUserNameException(user.UserName, $"bad user name: {user.UserName}");
        }
        #endregion
    }
}
