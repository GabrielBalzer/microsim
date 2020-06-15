using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microsim
{
    class PCL
    {
        private uint schema = 0xFF;
        public void addtoPCL()
        {
            if((DataStorage.programCounter & schema) < 255)
            {
                DataStorage.programCounter++;
            }
            else
            {
                DataStorage.programCounter = DataStorage.programCounter & 0b1111100000000;
            }
            Console.WriteLine("Wert des PCL: " + DataStorage.programCounter);
        }

        public void setPCL(uint value)
        {
            uint cache;
            cache = DataStorage.programCounter & 0b1111100000000;
            DataStorage.programCounter = cache | value;

        }

        public void subtractPCL(uint value)
        {
            uint currentlath = DataStorage.programCounter & 0b1111100000000;
            uint currentcounter = (DataStorage.programCounter & 0b0000011111111) - value;
            DataStorage.programCounter = currentlath | currentcounter;
        }

        public uint getPCL()
        {
            uint value = DataStorage.programCounter &0b0000011111111;
            return value;
        }
        
        public uint getPCLLath()
        {
            uint value = DataStorage.programCounter & 0b1111100000000;
            return value;
        }

    }
}
