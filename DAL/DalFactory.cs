using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{  
    /// <summary>
   /// Static Factory class for creating Dal tier implementation object according to
   /// configuration in file config.xml
   /// </summary>
    public static class DalFactory
    {
        /// <summary>
        /// The function creates Dal tier implementation object according to Dal type
        /// as appears in "dal" element in the configuration file config.xml.<br/>
        /// The configuration file also includes element "dal-packages" with list
        /// of available packages (.dll files) per Dal type.<br/>
        /// Each Dal package must use "Dal" namespace and it must include internal access
        /// singleton class with the same name as package's name.<br/>
        /// The singleton class must include public static property called "Instance"
        /// which must contain the single instance of the class.
        /// </summary>
        /// <returns>Dal tier implementation object</returns>
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