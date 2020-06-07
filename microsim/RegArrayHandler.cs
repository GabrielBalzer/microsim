using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microsim
{
    class RegArrayHandler
    {
        public void setRegArray(uint adress, uint value)
        {
            //Status Register
            if ((adress == 0x03) | (adress == 0x83) )
            {
                DataStorage.regArray[0x03] = value;
                DataStorage.regArray[0x83] = value;
            }
            else
            {
                DataStorage.regArray[adress] = value;
            }
            
        }

        public uint getRegArray(uint adress)
        {
            uint value = DataStorage.regArray[adress];
            return value;
        }

        public void setZeroFlag(bool set)
        {
            if (set)
            {
                setRegArray(0x03, getRegArray(0x03) | 0b00000100);
            }
            else
            {
               setRegArray(0x03, getRegArray(0x03) & 0b11111011);
            }
        }

        public void setCarryFlag(bool set)
        {
            if (set)
            {
               setRegArray(0x03, getRegArray(0x03) | 0b00000001);
            }
            else
            {
                setRegArray(0x03, getRegArray(0x03) & 0b11111110);
            }
        }

        public void setDigitCarryFlag(bool set)
        {
            if (set)
            {
                setRegArray(0x03, getRegArray(0x03) | 0b00000010);
            }
            else
            {
                setRegArray(0x03, getRegArray(0x03) & 0b11111101);
            }
        }
    }
}
