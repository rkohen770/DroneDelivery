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
            XElement dalConfig = XElement.Load(@"dal-config.xml");
            DalName = dalConfig.Element("dal").Value;
            DalPackages = (from pkg in dalConfig.Element("dal-packages").Elements()
                           select pkg
                         ).ToDictionary(p => "" + p.Name, p => p.Value);
        }

        public class DalConfigExeption : Exception
        {
            public DalConfigExeption(string msg) : base(msg) { }
            public DalConfigExeption(string msg,Exception ex ) : base(msg, ex) { }
        }
    }
}
