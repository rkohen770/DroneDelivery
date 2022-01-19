using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DalApi
{
    /// <summary>
    /// Class for processing config.xml file and getting from there information which is relevant for initialization of DalApi
    /// </summary>
    class DalConfig
    {
        internal static string DalName;
        internal static Dictionary<string, string> DalPackages;
        static DalConfig()
        {
            XElement dalConfig = XElement.Load(@"xml\dal-config.xml");
            DalName = dalConfig.Element("dal").Value;
            DalPackages = (from pkg in dalConfig.Element("dal_packages").Elements()
                           select pkg
                         ).ToDictionary(p => "" + p.Name, p => p.Value);
        }

       
    }
}
