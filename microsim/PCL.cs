using System;

namespace microsim
{
    internal class PCL
    {
        private readonly uint schema = 0xFF;

        public void addtoPCL()
        {
            if ((DataStorage.programCounter & schema) < 255)
                DataStorage.programCounter++;
            else
                DataStorage.programCounter = DataStorage.programCounter & 0b1111100000000;
            Console.WriteLine("Wert des PCL: " + DataStorage.programCounter);
        }

        public void setPCL(uint value)
        {
            uint cache;
            cache = DataStorage.programCounter & 0b1111100000000;
            DataStorage.programCounter = cache | value;
        }


        public uint getPCL()
        {
            var value = DataStorage.programCounter & 0b0000011111111;
            return value;
        }

        public uint getPCLLath()
        {
            var value = DataStorage.programCounter & 0b1111100000000;
            return value;
        }
    }
}