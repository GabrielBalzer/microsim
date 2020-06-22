using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace microsim
{
    class Initializer
    {
        private static PCL pcl = new PCL();
        private static RegArrayHandler regArrayHandler = new RegArrayHandler();
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

        private static void initWatchdog()
        {
            DataStorage.watchdogValue = 0;
        }

        public static void InitTimer0()
        {
            DataStorage.prescalerValue = 256;
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
            initWatchdog();
            //InitWatchdog();
            //InitInterrupt();
        }

        public static void otherReset()
        {
            uint tmr0value = DataStorage.regArray[0x01];
            uint statusvalue = DataStorage.regArray[0x03] & 0b00000111;
            uint fsrvalue = DataStorage.regArray[0x04];
            uint portavalue = DataStorage.regArray[0x05];
            uint portbvalue = DataStorage.regArray[0x06];
            uint eedatavalue = DataStorage.regArray[0x08];
            uint eeadrvalue = DataStorage.regArray[0x09];
            uint intconvalue = DataStorage.regArray[0x0B] & 0b00000001;

            initPCL();
            for (int i = 0; i < DataStorage.regArray.Length; i++)
            {
                DataStorage.regArray[i] = 0b00000000;
            }

            regArrayHandler.setRegArray(0x01, tmr0value);
            regArrayHandler.setRegArray(0x03, statusvalue);
            regArrayHandler.setRegArray(0x04,fsrvalue);
            regArrayHandler.setRegArray(0x05, portavalue);
            regArrayHandler.setRegArray(0x06,portbvalue);
            regArrayHandler.setRegArray(0x08, eedatavalue);
            regArrayHandler.setRegArray(0x09, eeadrvalue);
            regArrayHandler.setRegArray(0x0B,intconvalue);
            regArrayHandler.setRegArray(0x81, 0b11111111);
            regArrayHandler.setRegArray(0x85,0b00011111);
            regArrayHandler.setRegArray(0x86,0b11111111);
            initStartCounter();
            initWatchdog();

        }
    }
}
