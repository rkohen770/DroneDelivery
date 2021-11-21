using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using DalObject;
using IDAL;
namespace BL
{
    public partial class BL :IBL.IBL
    {
        IDal dAL = new DalObject.DalObject();

        //public List<DroneForList> droneForLists=new List<DroneForList>();
        public BL()
        {
            
        }
    }
}
