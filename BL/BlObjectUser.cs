using System;
using System.Collections.Generic;
using System.Linq;
using BO;
using DO;
using System.Threading;
using BLApi;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;
namespace BO
{
    public partial class BlObject: IBL
    {
      
        DO.User UserBoDoAdapter(User userBO)
        {
            DO.User userDO = new DO.User();
            userBO.CopyPropertiesTo(userDO);
            return userDO;
        }
        User UserDoBoAdapter(DO.User userDO)
        {
            User userBO = new User();
            userDO.CopyPropertiesTo(userBO);
            return userBO;
        }
        public void AddUser(User user)
        {
            try
            {
                DO.User user1 = new ()
                {
                    UserName = user.UserName,
                    Admin = (DO.Permission)user.Admin,
                    password = user.password,
                };
                //Add baseStation in DAL to data source.
                dal.AddUser(user1);
            }
            catch (DO.BadUserNameException ex)
            {
                throw new BadUserNameException(ex.Message, ex);
            }
            catch { }
        }
        public void UpdateUser(User user)
        {
            try
            {
                dal.UpdateUser(UserBoDoAdapter(user));
            }
            catch (DO.BadUserNameException ex)
            {
                throw new BadUserNameException(ex.Message, ex);
            }
        }
        public void DeleteUser(string userName)
        {

            try
            {
                dal.DeleteUser(userName);
            }
            catch (DO.BadUserNameException ex)
            {
                throw new BadUserNameException(ex.Message, ex);
            }
        }
        public User GetUser(string userName)
        {
            DO.User userDO;
            try
            {
                userDO = dal.GetUser(userName);
            }
            catch (DO.BadUserNameException ex)
            {
                throw new BadUserNameException(ex.Message, ex);
            }
            return UserDoBoAdapter(userDO);
        }

    }
}
