using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class CustomerInParcel
    {
        public int Id;
        public string Name;

        public override string ToString()
        {
            return $"Id: {Id} \nName: {Name}";
        }
    }
}
