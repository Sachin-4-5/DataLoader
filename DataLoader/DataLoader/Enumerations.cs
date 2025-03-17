using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLoader
{
    public class Enumerations
    {
        public enum InputType
        {
            FIXEDLENGTH = 1,
            DELIMITED = 2,
            EXCEL = 3,
            XML = 4,
            JSON = 5,
            DB = 6
        }
    }
}
