using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    public static class DalFactory
    {
        public static IDal GetDal(string type)
        {
            // get dal implementation name from config.xml according to <data> element
            string dalType = type;
            try // get dal package info according to <dal> element value in config file
            {
                if (type == "dalObject")
                    return new DalObject();
                else
                    throw new DLConfigException($"Wrong DL type: {dalType}");
            }
            catch (KeyNotFoundException ex)
            {
                // if package name is not found in the list - there is a problem in config.xml
                throw new DLConfigException($"Wrong DL type: {dalType}", ex);
            }
        }
    }
}