using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microsim
{
    class Initializer
    {
        public static void initRegArray()
        {
            for (int i = 0; i < DataStorage.regArray.Length; i++)
            {
                DataStorage.regArray[i] = 0xFF;
            }
        }
    }
}
