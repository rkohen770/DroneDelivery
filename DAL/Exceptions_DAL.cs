using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL.DO
{
    /// <summary>
    ///A class inherits from an "Exception" so we can throw an error
    /// </summary>
    class ExistingFigureException : Exception
    {
        public ExistingFigureException() { }
        public ExistingFigureException(string exe) : base(exe) { }

    }
    class NoDataExistsException : Exception
    {
        public NoDataExistsException() { }
        public NoDataExistsException(string exe) : base(exe) { }
    }
}
