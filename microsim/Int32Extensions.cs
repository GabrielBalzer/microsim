using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microsim
{
    public static class Int32Extensions
    {
        public static Boolean[] ToBooleanArray(this Int32 i)
        {
            return Convert.ToString(i, 2 ).PadLeft(8, '0').Select(s => s.Equals('1')).ToArray();
        }
    }
}
