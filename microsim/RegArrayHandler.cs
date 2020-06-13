using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microsim
{
    class RegArrayHandler
    {
        private PCL pcl = new PCL();
        public void setRegArray(uint adress, uint value)
        {
            
            if ((adress == 0x03) | (adress == 0x83))
            {
                //Status Register
                DataStorage.regArray[0x03] = value;
                DataStorage.regArray[0x83] = value;
            }
            else if ((adress == 0x02) | (adress == 0x82))
            {
                //PCL
                DataStorage.regArray[0x02] = value;
                DataStorage.regArray[0x82] = value;
            }
            else if ((adress == 0x04) | (adress == 0x84))
            {
                //FSR
                DataStorage.regArray[0x04] = value;
                DataStorage.regArray[0x84] = value;
            }
            else if ((adress == 0x0A) | (adress == 0x8A))
            {
                //PCLLATH
                DataStorage.regArray[0x0A] = value;
                DataStorage.regArray[0x8A] = value;
            }
            else if ((adress == 0x0B) | (adress == 0x8B))
            {
                //INTCON
                DataStorage.regArray[0x0B] = value;
                DataStorage.regArray[0x8B] = value;
            }
            else
            {
                DataStorage.regArray[adress] = value;
            }
            if (adress == 0x81)
            {
                if ((DataStorage.regArray[0x81] & 0x08) == 0x08)
                {
                    DataStorage.prescalerValue =
                        2 ^ (DataStorage.regArray[0x81] & 0xF8);  
                }
                else
                {
                    DataStorage.prescalerValue =
                        2 ^ (DataStorage.regArray[0x81] & 0xF8) * 2; /* *2 da Timer 0 */ 
                }
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
