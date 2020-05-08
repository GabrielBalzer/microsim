using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microsim
{
    public class Stack
    {
        // variables
        private uint stckptr = 0;
        private uint[] entryStk = new uint[8];

        public Stack()
        {
            // init stack to default values
            InitStck();

            // set stckptr to 0xFF
            stckptr = 0;

            for (uint i = 0; i < entryStk.Length; i++)
            {
                Console.WriteLine(entryStk[i]);
            }
            Console.WriteLine("Stackpointer0" + stckptr);

            return;

        }
        uint freePlce = 0;
        public void SetValueToStck(uint value)
        {
            // set given value to next free space on stack
            freePlce = GetNxtFreeStckPlce();
            entryStk[freePlce] = value;

            // increment stckptr
            SetStckptrToStckPlce(freePlce);

            for (uint i = 0; i < entryStk.Length; i++)
            {
                Console.WriteLine(entryStk[i]);
            }
            Console.WriteLine("Stackpointer1" + stckptr);

            return;
        }

        public uint GetValueFromStck()
        {
            // return value from actual stckptr
            uint returnVal = entryStk[stckptr];
            DeleteStckPlce();

            for (uint i = 0; i < entryStk.Length; i++)
            {
                Console.WriteLine(entryStk[i]);
            }
            Console.WriteLine("Stackpointer2" + stckptr);

            return (returnVal);
        }

        private void InitStck()
        {
            // set stack values to 0
            for (uint i = 0; i < entryStk.Length; i++)
            {
                entryStk[i] = 0;
            }

            return;
        }
        int passedonce = 0;
        private uint GetNxtFreeStckPlce()
        {
            if (stckptr == 7)
            {
                // last index -> jump to index 0
                freePlce = 0;
            }
            if (passedonce == 0)
            {
                if (stckptr == 0)
                {
                    freePlce = 0;
                }
                passedonce = 1;
            }
            else
            {
                // next incremented index
                freePlce++;
            }

            return freePlce;
        }

        private void DeleteStckPlce()//(uint stckptr)
        {
            // get actual stckptr and delete stack value
            switch(stckptr)
            {
                case 0x0:
                    entryStk[0x00] = 0x0;
                    break;
                case 0x1:
                    entryStk[0x01] = 0x0;
                    break;
                case 0x2:
                    entryStk[0x02] = 0x0;
                    break;
                case 0x3:
                    entryStk[0x03] = 0x0;
                    break;
                case 0x4:
                    entryStk[0x04] = 0x0;
                    break;
                case 0x5:
                    entryStk[0x05] = 0x0;
                    break;
                case 0x6:
                    entryStk[0x06] = 0x0;
                    break;
                case 0x7:
                    entryStk[0x07] = 0x0;
                    break;
            }
            SetStckptrToStckPlce(stckptr);
            return;
        }
        
        private void SetStckptrToStckPlce(uint index)
        {
            // get last written index and set stckptr to new index
            switch(index)
            {
                case 0x0:
                    stckptr = 0x0;
                    break;
                case 0x1:
                    stckptr = 0x1;
                    break;
                case 0x2:
                    stckptr = 0x2;
                    break;
                case 0x3:
                    stckptr = 0x3;
                    break;
                case 0x4:
                    stckptr = 0x4;
                    break;
                case 0x5:
                    stckptr = 0x5;
                    break;
                case 0x6:
                    stckptr = 0x6;
                    break;
                case 0x7:
                    stckptr = 0x7;
                    break;
            }
            return;
        }
    }
}
