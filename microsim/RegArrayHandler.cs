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
            else if (adress == 0x01)
            {
                DataStorage.regArray[0x01] = value;
                DataStorage.prescalerCount = 0;
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
            else if (adress == 0x05)
            {
                uint beforeRA4 = DataStorage.regArray[0x05] & 0b00010000;
                uint afterRA4 = value & 0b00010000;
                if ((beforeRA4 == 0) && (afterRA4 > 0))
                {
                    DataStorage.lowHighFlankRA4 = true;
                }
                else if ((afterRA4 == 0) && (beforeRA4 > 0))
                {
                    DataStorage.highLowFlankRA4 = true;
                }
                DataStorage.regArray[0x05] = value;
            }
            else if (adress == 0x06)
            {
                uint beforeRB0 = DataStorage.regArray[0x06] & 0b00000001;
                uint afterRB0 = value & 0b00000001;
                //low high
                if ((beforeRB0 == 0) && (afterRB0 > 0))
                {
                    if (((DataStorage.regArray[0x81] & 0b01000000) == 0b01000000))
                    {
                        setRegArray(0x0B, (getRegArray(0x0B) | 0b00000010));
                    }
                }
                //high low
                else if ((afterRB0 == 0) && (beforeRB0 > 0))
                {
                    if ((DataStorage.regArray[0x81] & 0b01000000) != 0b01000000)
                    {
                        setRegArray(0x0B, (getRegArray(0x0B) | 0b00000010));
                    }
                }

                uint beforeRB7 = DataStorage.regArray[0x06] & 0b10000000;
                uint afterRB7 = value & 0b10000000;
                if ((((beforeRB7 == 0) && (afterRB7 > 0)) || (afterRB7 == 0) && (beforeRB7 > 0)) &&
                    ((getRegArray(0x86) & 0b10000000) == 0b10000000))
                {
                    setRegArray(0x0B, (getRegArray(0x0B) | 0b00000001));
                }

                uint beforeRB6 = DataStorage.regArray[0x06] & 0b01000000;
                uint afterRB6 = value & 0b01000000;
                if ((((beforeRB6 == 0) && (afterRB6 > 0)) || (afterRB6 == 0) && (beforeRB6 > 0)) &&
                    ((getRegArray(0x86) & 0b01000000) == 0b01000000))
                {
                    setRegArray(0x0B, (getRegArray(0x0B) | 0b00000001));
                }

                uint beforeRB5 = DataStorage.regArray[0x06] & 0b00100000;
                uint afterRB5 = value & 0b00100000;
                if ((((beforeRB5 == 0) && (afterRB5 > 0)) || (afterRB5 == 0) && (beforeRB5 > 0)) &&
                    ((getRegArray(0x86) & 0b00100000) == 0b00100000))
                {
                    setRegArray(0x0B, (getRegArray(0x0B) | 0b00000001));
                }

                uint beforeRB4 = DataStorage.regArray[0x06] & 0b00010000;
                uint afterRB4 = value & 0b00010000;
                if ((((beforeRB4 == 0) && (afterRB4 > 0)) || (afterRB4 == 0) && (beforeRB4 > 0)) &&
                    ((getRegArray(0x86) & 0b00010000) == 0b00010000))
                {
                    setRegArray(0x0B, (getRegArray(0x0B) | 0b00000001));
                }

                DataStorage.regArray[0x06] = value;
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
                    DataStorage.prescalerValue = (uint) Math.Pow(2, (DataStorage.regArray[0x81] & 0b00000111));
                }
                else
                {
                    DataStorage.prescalerValue = (uint) Math.Pow(2, (DataStorage.regArray[0x81] & 0b00000111)) * 2;

                }

                DataStorage.prescalerCount = -1;
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
