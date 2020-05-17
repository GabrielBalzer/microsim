﻿using System;
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
        public static void initPCL()
        {
            DataStorage.programCounter = 0;
        }

        public static void initWReg()
        {
            DataStorage.w_register = 0;
        }

        public static void initStack()
        {
            Stack stack1 = new Stack();
        }
    }
}
