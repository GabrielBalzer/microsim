using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microsim
{
    class Initializer
    {
        private static void initRegArray()
        {
            for (int i = 0; i < DataStorage.regArray.Length; i++)
            {
                DataStorage.regArray[i] = 0b00000000;
            }


            //OPTION_REG
            DataStorage.regArray[0x81] = 0b11111111;

            //STATUS
            DataStorage.regArray[0x03] = 0b00011000;
            DataStorage.regArray[0x83] = 0b00011000;

            //PORTA
            DataStorage.regArray[0x05] = 0b00011111;

            //TRISA
            DataStorage.regArray[0x85] = 0b00011111;

            //TRISB
            DataStorage.regArray[0x86] = 0b11111111;

            //EECON1
            DataStorage.regArray[0x88] = 0b00001000;

            //INTCON
            DataStorage.regArray[0x0B] = 0b00000001;
            DataStorage.regArray[0x8B] = 0b00000001;
        }
        private static void initPCL()
        {
            DataStorage.programCounter = 0;
        }

        private static void initStartCounter()
        {
            DataStorage.startCounter = 0;
        }

        private static void initWReg()
        {
            DataStorage.w_register = 0;
        }

        private static void initStack()
        {
            DataStorage.stack1 = new Stack();
        }

        private static void initCycle()
        {
            DataStorage.cycleCount = 0;
        }

        private static void InitTimer0()
        {
            DataStorage.tim0 = new Timer0(); 
        }

        private static void InitWatchdog()
        {
            DataStorage.watchdog1 = new Watchdog();
        }

        public static void fullReset()
        {
            initRegArray();
            initPCL();
            initStartCounter();
            initWReg();
            initStack();
            initCycle();
            InitTimer0();
            InitWatchdog();
        }
    }
}
