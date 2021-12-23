 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;

namespace IBL
{
    /// <summary>
    /// Implement the design pattern of a factory
    /// </summary>
    public static class BLFactory
    {
        public static IBL GetBL()
        {
            return new BlObject();
        }
    }
}