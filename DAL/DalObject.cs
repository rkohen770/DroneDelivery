using IDAL;
using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DalObject
{
    public partial class DalObject :IDal
    {
        public DalObject()
        {
            DataSource.Initialize();
        }


    }
}
