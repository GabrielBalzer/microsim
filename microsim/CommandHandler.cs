using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace microsim
{
    class CommandHandler
    {
        private DataStorage.Command command_element;
        private PCL PCL = new PCL();
        private RegArrayHandler regArrayHandler = new RegArrayHandler();
        private Timer0 timer0 = new Timer0();
        private Interrupts interrupts = new Interrupts();

        public void nextCommand()
        {
            
            uint oldCycles;
            uint cycleDiff;
            if (PCL.getPCL() < DataStorage.commandList.Count)
            {

                Console.WriteLine(DataStorage.commandList.ElementAt((int) PCL.getPCL()).command);
                oldCycles = DataStorage.cycleCount;
                handleCommand();
                cycleDiff = DataStorage.cycleCount - oldCycles;
                timer0.timerCount(cycleDiff);
                interrupts.checkInterrupt();
            }
            regArrayHandler.setRegArray(0x02, PCL.getPCL());
        }

        private void handleCommand()
        {
            if (DataStorage.startCounter != 0)
            {
                PCL.addtoPCL();
            }
            else
            {
                DataStorage.startCounter = 1;
            }

            command_element = DataStorage.commandList.ElementAt((int) DataStorage.programCounter);
            checkBreakpoint();



 
   

            Console.WriteLine("Handler :" + command_element.command);
            switch (command_element.command)
            {
                case "MOVLW":
                    MOVLW();
                    break;
                case "ANDLW":
                    ANDLW();
                    break;
                case "IORLW":
                    IORLW();
                    break;
                case "SUBLW":
                    SUBLW();
                    break;
                case "XORLW":
                    XORLW();
                    break;
                case "ADDLW":
                    ADDLW();
                    break;
                case "GOTO":
                    GOTO();
                    break;
                case "CALL":
                    CALL();
                    break;
                case "NOP":
                    NOP();
                    break;
                case "RETURN":
                    RETURN();
                    break;
                case "RETLW":
                    RETLW();
                    break;
                case "MOVWF":
                    MOVWF();
                    break;
                case "ADDWF":
                    ADDWF();
                    break;
                case "ANDWF":
                    ANDWF();
                    break;
                case "CLRF":
                    CLRF();
                    break;
                case "COMF":
                    COMF();
                    break;
                case "DECF":
                    DECF();
                    break;
                case "INCF":
                    INCF();
                    break;
                case "MOVF":
                    MOVF();
                    break;
                case "IORWF":
                    IORWF();
                    break;
                case "SUBWF":
                    SUBWF();
                    break;
                case "SWAPF":
                    SWAPF();
                    break;
                case "XORWF":
                    XORWF();
                    break;
                case "CLRW":
                    CLRW();
                    break;
                case "RLF":
                    RLF();
                    break;
                case "RRF":
                    RRF();
                    break;
                case "DECFSZ":
                    DECFSZ();
                    break;
                case "INCFSZ":
                    INCFSZ();
                    break;
                case "BSF":
                    BSF();
                    break;
                case "BCF":
                    BCF();
                    break;
                case "BTFSC":
                    BTFSC();
                    break;
                case "BTFSS":
                    BTFSS();
                    break;
                case "SLEEP":
                    SLEEP();
                    break;
                case "CLRWDT":
                    CLRWDT();
                    break;
                case "RETFIE":
                    RETFIE();
                    break;
                default:
                    Console.WriteLine("Unbekannter Befehl");
                    break;
            }
        }



        private void MOVLW()
        {
            Console.WriteLine("MOVLW gefunden");
            if (command_element.data <= 255)
            {
                DataStorage.w_register = command_element.data;
                Console.WriteLine("w-register: " + DataStorage.w_register);
                DataStorage.addCycle(1);
            }
        }

        private void ANDLW()
        {
            Console.WriteLine("ANDLW gefunden");
            DataStorage.w_register = DataStorage.w_register & command_element.data;
                if (DataStorage.w_register == 0)
                {
                    regArrayHandler.setZeroFlag(true);
                }
                else
                {
                    regArrayHandler.setZeroFlag(false);
                }

                Console.WriteLine("w-register: " + DataStorage.w_register);
                DataStorage.addCycle(1);

        }

        private void IORLW()
        {
            Console.WriteLine("IORLW gefunden");
            if (command_element.data <= 255)
            {
                DataStorage.w_register = DataStorage.w_register | command_element.data;
                if (DataStorage.w_register == 0)
                {
                    regArrayHandler.setZeroFlag(true);
                }
                else
                {
                    regArrayHandler.setZeroFlag(false);
                }

                Console.WriteLine("w-register: " + DataStorage.w_register);
                DataStorage.addCycle(1);
            }

        }

        private void SUBLW()
        {
            int lowbitf;
            int lowbitw;
            uint d = command_element.data & 0b10000000;
            uint f = getFAddr(command_element.data);
            Console.WriteLine("SUBLW gefunden");
            int result;
            lowbitf = (int) (command_element.data & 0b00001111);
            lowbitw = (int) (~DataStorage.w_register + 1) & 0b00001111;
            bool dc = (lowbitf + lowbitw) > 15;

            regArrayHandler.setDigitCarryFlag(dc);
 

                    //DC-Flag missing
                result = (int) command_element.data - (int) DataStorage.w_register;
                if (result > 0)
                {
                    DataStorage.w_register = (uint) result;
                    regArrayHandler.setCarryFlag(true);

                    regArrayHandler.setZeroFlag(false);

                }
                else if (result == 0)
                {
                    DataStorage.w_register = (uint) result;
                    regArrayHandler.setCarryFlag(true);
                    regArrayHandler.setZeroFlag(true);
                }
                else
                {
                    result =  (256 - Math.Abs(result));

                    regArrayHandler.setZeroFlag(false);
                    regArrayHandler.setCarryFlag(false);
                }

                if (d == 0)
                {
                    DataStorage.w_register = (uint)result;
                }
                else
                {
                    regArrayHandler.setRegArray(f, (uint)result);
                }
            DataStorage.addCycle(1);
        }

        private void XORLW()
        {
            Console.WriteLine("XORLW gefunden");
            if (command_element.data <= 255)
            {
                DataStorage.w_register = DataStorage.w_register ^ command_element.data;
                if (DataStorage.w_register == 0)
                {
                    regArrayHandler.setZeroFlag(true);
                }
                else
                {
                    regArrayHandler.setZeroFlag(false);
                }

                Console.WriteLine("w-register: " + DataStorage.w_register);
                DataStorage.addCycle(1);
            }
        }

        private void ADDLW()
        {
            uint result;
            Console.WriteLine("ADDLW gefunden");
            if (command_element.data <= 255)
            {
                result = DataStorage.w_register + command_element.data;

                //DC-Flag
                if (((DataStorage.w_register & 0b00001111) + (command_element.data & 0b00001111)) > 15)
                {
                    regArrayHandler.setDigitCarryFlag(true);
                }
                else
                {
                    regArrayHandler.setDigitCarryFlag(false);
                }

                if (result <= 255)
                {
                    DataStorage.w_register = result;

                    regArrayHandler.setCarryFlag(false);
                }
                else
                {
                    regArrayHandler.setCarryFlag(true);

                    DataStorage.w_register = result - 256;
                }

                if (DataStorage.w_register == 0)
                {
                    regArrayHandler.setZeroFlag(true);
                }
                else
                {
                    regArrayHandler.setZeroFlag(false);
                }
                DataStorage.addCycle(1);
            }
        }

        private void GOTO()
        {
            uint pclath;
            uint pclath3;
            uint pclath4;
            Console.WriteLine("GOTO gefunden");
            if (command_element.data <= 2047)
            {
                pclath3 = regArrayHandler.getRegArray(0x0A) & 0b00000100;
                pclath4 = regArrayHandler.getRegArray(0x0A) & 0b00001000;

                pclath = pclath4;
                pclath = pclath << 1;
                pclath = pclath | pclath3;
                pclath = pclath << 11;
                pclath = pclath | command_element.data;
                Console.WriteLine("PCLTEST : " + pclath);
                if (pclath < 1)
                {
                    PCL.setPCL(0);
                }
                else
                {
                    PCL.setPCL(pclath - 1);
                }
                DataStorage.addCycle(2);
            }
        }

        private void CALL()
        {
            uint pclath;
            uint pclath3;
            uint pclath4;
            Console.WriteLine("CALL gefunden");
            if (command_element.data <= 2047)
            {
                // save pc to stack
                DataStorage.stack1.SetValueToStck(DataStorage.programCounter + 1);

                pclath3 = regArrayHandler.getRegArray(0x0A) & 0b00000100;
                pclath4 = regArrayHandler.getRegArray(0x0A) & 0b00001000;

                pclath = pclath4;
                pclath = pclath << 1;
                pclath = pclath | pclath3;
                pclath = pclath << 11;
                pclath = pclath | command_element.data;
                Console.WriteLine("PCLTEST CALL : " + pclath);
                PCL.setPCL(pclath - 1);
                Console.WriteLine("W-reg : " + DataStorage.w_register);
                DataStorage.addCycle(2);
            }
        }

        private void NOP()
        {
            Console.WriteLine("NOP gefunden");
            DataStorage.addCycle(1);
        }

        private void RETURN()
        {
            Console.WriteLine("RETURN gefunden");
            PCL.setPCL(DataStorage.stack1.GetValueFromStck() - 1);
            Console.WriteLine("Stack WERT : " + (DataStorage.programCounter));
            Console.WriteLine("W-reg : " + DataStorage.w_register);
            DataStorage.addCycle(2);
        }

        private void RETLW()
        {
            Console.WriteLine("RETLW gefunden");
            if (command_element.data <= 255)
            {
                DataStorage.w_register = command_element.data;
                PCL.setPCL(DataStorage.stack1.GetValueFromStck() - 1);
                Console.WriteLine("W-reg : " + DataStorage.w_register);
                DataStorage.addCycle(2);
            }
        }

        private void MOVWF()
        {
            Console.WriteLine("MOVWF gefunden");

            uint f = getFAddr(command_element.data);
            regArrayHandler.setRegArray(f, DataStorage.w_register);
            DataStorage.addCycle(1);
        }

        private void ADDWF()
        {
            uint f = getFAddr(command_element.data);
            uint d;
            uint result;
            Console.WriteLine("ADDWF gefunden");
            d = command_element.data & 0b10000000;
            Console.WriteLine("F Wert: " + f);
            Console.WriteLine("D Wert: " + d);
            if (f <= 127)
            {
                result = DataStorage.w_register + regArrayHandler.getRegArray(f);

                //DC-Flag
                if (((DataStorage.w_register & 0b00001111) + (regArrayHandler.getRegArray(f) & 0b00001111) > 15))
                {
                    regArrayHandler.setDigitCarryFlag(true);
                }
                else
                {
                    regArrayHandler.setDigitCarryFlag(false);
                }

                if (result <= 255)
                {
                    regArrayHandler.setCarryFlag(false);
                }
                else
                {
                    regArrayHandler.setCarryFlag(true);
                    result = result - 256;
                }

                if (result == 0)
                {
                    regArrayHandler.setZeroFlag(true);
                }
                else
                {
                    regArrayHandler.setZeroFlag(false);
                }

                if (d == 0)
                {
                    DataStorage.w_register = result;
                }
                else
                {
                    regArrayHandler.setRegArray(f, result);
                }
                DataStorage.addCycle(1);
            }
        }

        private void ANDWF()
        {
            Console.WriteLine("ANDWF gefunden");
            uint d;
            uint result;
            uint f = getFAddr(command_element.data);
            d = command_element.data & 0b10000000;

            if (f <= 127)
            {
                result = DataStorage.w_register & regArrayHandler.getRegArray(f);
                if (d == 0)
                {
                    DataStorage.w_register = result;
                }
                else
                {
                    regArrayHandler.setRegArray(f, result);
                }

                if (result == 0)
                {
                    regArrayHandler.setZeroFlag(true);
                }
                else
                {
                    regArrayHandler.setZeroFlag(false);
                }
                DataStorage.addCycle(1);
            }

        }

        private void CLRF()
        {
            Console.WriteLine("CLRF gefunden");
            uint f = getFAddr(command_element.data);

            regArrayHandler.setRegArray(f, 0);
            regArrayHandler.setZeroFlag(true);

            DataStorage.addCycle(1);
        }

        private void COMF()
        {
            Console.WriteLine("COMF gefunden");
            uint f = getFAddr(command_element.data);
            uint d;
            uint result;
            d = command_element.data & 0b10000000;
            if (f <= 127)
            {
                result = ~regArrayHandler.getRegArray(f);
                result = result & 0xFF;
                if (result == 0)
                {
                    regArrayHandler.setZeroFlag(true);

                }
                else
                {
                    regArrayHandler.setZeroFlag(false);
                }

                if (d == 0)
                {
                    DataStorage.w_register = result;
                }
                else
                {
                    regArrayHandler.setRegArray(f,result);
                }
                DataStorage.addCycle(1);
            }
        }

        private void DECF()
        {
            Console.WriteLine("DECF gefunden");
            uint f = getFAddr(command_element.data);
            uint d;
            int result;
            int regValue;
            d = command_element.data & 0b10000000;
            if (f <= 127)
            {
                regValue = (int) regArrayHandler.getRegArray(f);
                result = regValue - 1;
                if (result == 0)
                {
                    regArrayHandler.setZeroFlag(true);

                }
                else
                {
                    regArrayHandler.setZeroFlag(false);
                }

                if (result == -1)
                {
                    result = 0xFF;
                }

                if (d == 0)
                {
                    DataStorage.w_register = (uint) result;
                }
                else
                {
                    regArrayHandler.setRegArray(f, (uint)result);
                }
                DataStorage.addCycle(1);
            }
        }

        private void INCF()
        {
            Console.WriteLine("INCF gefunden");
            uint f = getFAddr(command_element.data);
            uint d;
            uint result;
            d = command_element.data & 0b10000000;
            result = regArrayHandler.getRegArray(f) + 1;
            if (result == 256)
            {
                result = 0;
            }

            if (result == 0)
            {
                regArrayHandler.setZeroFlag(true);
            }
            else
            {
                regArrayHandler.setZeroFlag(false);
            }

            if (d == 0)
            {
                DataStorage.w_register = result;
            }
            else
            {
                regArrayHandler.setRegArray(f, result);
            }
            DataStorage.addCycle(1);
        }

        private void MOVF()
        {
            Console.WriteLine("MOVF gefunden");
            uint f = getFAddr(command_element.data);
            uint d;
            uint result;
            d = command_element.data & 0b10000000;
            result = regArrayHandler.getRegArray(f);
            if (result == 0)
            {
                regArrayHandler.setZeroFlag(true);
            }
            else
            {
                regArrayHandler.setZeroFlag(false);
            }

            if (d == 0)
            {
                DataStorage.w_register = result;
            }
            else
            {
                regArrayHandler.setRegArray(f,result);
            }
            DataStorage.addCycle(1);
        }

        private void IORWF()
        {
            Console.WriteLine("IORWF gefunden");
            uint f = getFAddr(command_element.data);
            uint d;
            uint result;
            d = command_element.data & 0b10000000;
            result = DataStorage.w_register | regArrayHandler.getRegArray(f);
            if (result == 0)
            {
                regArrayHandler.setZeroFlag(true);
            }
            else
            {
                regArrayHandler.setZeroFlag(false);
            }

            if (d == 0)
            {
                DataStorage.w_register = result;
            }
            else
            {
                regArrayHandler.setRegArray(f,result);
            }
            DataStorage.addCycle(1);
        }

        private void SUBWF()
        {
            Console.WriteLine("SUBWF gefunden");
            uint f = getFAddr(command_element.data);
            uint d;
            int result;
            int lowbitf;
            int lowbitw;
            d = command_element.data & 0b10000000;
            lowbitf = (int) (regArrayHandler.getRegArray(f) & 0b00001111);
            lowbitw = (int) ((~DataStorage.w_register + 1) & 0b00001111);
            bool dc = (lowbitf + lowbitw) > 15;

            regArrayHandler.setDigitCarryFlag(dc);

            result = ((int) regArrayHandler.getRegArray(f) - (int) DataStorage.w_register);

            if (result > 0)
            {
                regArrayHandler.setCarryFlag(true);

                regArrayHandler.setZeroFlag(false);

            }
            else if (result == 0)
            {
                regArrayHandler.setCarryFlag(true);

                regArrayHandler.setZeroFlag(true);

            }
            else
            {
                regArrayHandler.setZeroFlag(false);

                regArrayHandler.setCarryFlag(false);
                result = (int)(256 - Math.Abs(result));
            }


            if (d == 0)
            {
                DataStorage.w_register = (uint)result;
            }
            else
            {
                regArrayHandler.setRegArray(f, (uint)result);
            }
            DataStorage.addCycle(1);
        }

        private void SWAPF()
        {
            Console.WriteLine("SWAPF gefunden");
            uint f = getFAddr(command_element.data);
            uint d;
            uint result;
            d = command_element.data & 0b10000000;
            result = ((regArrayHandler.getRegArray(f) & 0x0F) << 4 | (regArrayHandler.getRegArray(f) & 0xF0) >> 4);
            if (d == 0)
            {
                DataStorage.w_register = result;
            }
            else
            {
                regArrayHandler.setRegArray(f,result);
            }
            DataStorage.addCycle(1);
        }

        private void XORWF()
        {
            Console.WriteLine("XORWF gefunden");
            uint f = getFAddr(command_element.data);
            uint d;
            uint result;
            d = command_element.data & 0b10000000;
            result = DataStorage.w_register ^ regArrayHandler.getRegArray(f);
            if (result == 0)
            {
                regArrayHandler.setZeroFlag(true);
            }
            else
            {
                regArrayHandler.setZeroFlag(false);
            }

            if (d == 0)
            {
                DataStorage.w_register = result;
            }
            else
            {
                regArrayHandler.setRegArray(f, result);
            }
            DataStorage.addCycle(1);
        }

        private void CLRW()
        {
            Console.WriteLine("CLRW gefunden");
            regArrayHandler.setZeroFlag(true);

            DataStorage.w_register = 0;
            DataStorage.addCycle(1);
        }

        private void RLF()
        {

            Console.WriteLine("RLF gefunden");
            uint f = getFAddr(command_element.data);
            byte regValue;
            uint d;
            byte statusbyte;
            byte checker;
            statusbyte = (byte) (regArrayHandler.getRegArray(0x03) & 0b000000001);
            d = command_element.data & 0b10000000;
            checker = (byte) (regArrayHandler.getRegArray(f) & 0b10000000);
            if (checker == 0)
            {
                regArrayHandler.setCarryFlag(false);
            }
            else
            {
                regArrayHandler.setCarryFlag(true);
            }

            regValue = (byte) (regArrayHandler.getRegArray(f));
            regValue = (byte) (regValue << 1);
            regValue = (byte) (regValue | statusbyte);

            if (d == 0)
            {
                DataStorage.w_register = regValue;
            }
            else
            {
                regArrayHandler.setRegArray(f,regValue);
            }
            DataStorage.addCycle(1);
        }

        private void RRF()
        {
            Console.WriteLine("RRF gefunden");
            uint f = getFAddr(command_element.data);
            byte regValue;
            uint d;
            byte statusbyte;
            byte checker;
            statusbyte = (byte) (regArrayHandler.getRegArray(0x03) & 0b000000001);
            if (statusbyte == 1)
            {
                statusbyte = 0b10000000;
            }
            d = command_element.data & 0b10000000;
            checker = (byte) (regArrayHandler.getRegArray(f) & 0b00000001);
            if (checker == 0)
            {
                regArrayHandler.setCarryFlag(false);
            }
            else
            {
                regArrayHandler.setCarryFlag(true);
            }

            regValue = (byte) (regArrayHandler.getRegArray(f));
            regValue = (byte) (regValue >> 1);
            regValue = (byte) (regValue | statusbyte);

            if (d == 0)
            {
                DataStorage.w_register = regValue;
            }
            else
            {
                regArrayHandler.setRegArray(f, regValue);
            }
            DataStorage.addCycle(1);
        }

        private void DECFSZ()
        {
            Console.WriteLine("DECFSZ gefunden");
            uint f = getFAddr(command_element.data);
            uint d;
            int result;
            d = command_element.data & 0b10000000;
            result = (int)regArrayHandler.getRegArray(f);

            if ((result - 1) == 0)
            {
                PCL.addtoPCL();
                NOP();
            }
 

            if ((result - 1) < 0)
            {
                result = 255;
            }
            else
            {
                result = result - 1;
            }

            if (d == 0)
            {
                DataStorage.w_register = (uint)result;
            }
            else
            {
                regArrayHandler.setRegArray(f, (uint)result);
            }
            DataStorage.addCycle(1);
        }

        private void INCFSZ()
        {
            Console.WriteLine("INCFSZ gefunden");
            uint f = getFAddr(command_element.data);
            uint d;
            uint result;
            d = command_element.data & 0b10000000;
            result = regArrayHandler.getRegArray(f);
            if ((result + 1) == 256)
            {
                result = 0;
            }
            else
            {
                result += 1;
            }

            if (result == 0)
            {
                PCL.addtoPCL();
                NOP();
            }

            if (d == 0)
            {
                DataStorage.w_register = result;
            }
            else
            {
                regArrayHandler.setRegArray(f, result);
            }
        }

        private void BSF()
        {
            Console.WriteLine("BSF gefunden");
            uint f = getFAddr(command_element.data);
            uint b;
            //uint result;
            b = command_element.data & 0b1110000000;
            b = b >> 7;

            regArrayHandler.setRegArray(f, regArrayHandler.getRegArray(f) | (uint)(Math.Pow((double)2, (double)b)));
            DataStorage.addCycle(1);
        }

        private void BCF()
        {
            Console.WriteLine("BCF gefunden");
            uint f = getFAddr(command_element.data);
            uint b;
            //uint result;
            b = command_element.data & 0b1110000000;
            b = b >> 7;

            regArrayHandler.setRegArray(f, regArrayHandler.getRegArray(f) ^ (uint)(Math.Pow((double)2, (double)b)));
            DataStorage.addCycle(1);
        }

        private void BTFSC()
        {
            Console.WriteLine("BTFSC gefunden");
            uint f = getFAddr(command_element.data);
            uint b;
            uint result;
            uint bCalc;
            b = command_element.data & 0b1110000000;
            b = b >> 7;
            result = regArrayHandler.getRegArray(f);
            bCalc = (uint) (Math.Pow((double) 2, (double) b));

            if ((result & bCalc) == bCalc)
            {
                // result[b] = 1
                // do nothing
            }
            else
            {
                // result[b] = 0
                PCL.addtoPCL();
                NOP();
            }
            DataStorage.addCycle(1);
        }

        private void BTFSS()
        {
            Console.WriteLine("BTFSS gefunden");
            uint f = getFAddr(command_element.data);
            uint b;
            uint result;
            uint bCalc;
            b = command_element.data & 0b1110000000;
            b = b >> 7;
            result = regArrayHandler.getRegArray(f);
            bCalc = (uint)(Math.Pow((double)2, (double)b));

            if ((result & bCalc) == 0)
            {
                // result[b] = 1
                // do nothing
                
            }
            else
            {
                // result[b] = 0
                PCL.addtoPCL();
                NOP();
            }
            DataStorage.addCycle(1);
        }

        private void SLEEP()
        {
            Console.WriteLine("SLEEP gefunden");
            
            // reset watchdog counter
            DataStorage.watchdogCounter = 0;

            // reset prescaler -> TODO

            // set TO-bit [4]
            //DataStorage.regArray[0x03] &= 0x10;
            regArrayHandler.setRegArray(0x03, regArrayHandler.getRegArray(0x03) & 0x10);

            // reset PD-bit [3]
            //DataStorage.regArray[0x03] ^= 0x08;
            regArrayHandler.setRegArray(0x03, regArrayHandler.getRegArray(0x03) ^ 0x08);

            // set i/o pins to active -> TODO

            // sleep ended?
            /* POWER ON RESET: pd = 1 && to = 1 */
            if (((regArrayHandler.getRegArray(0x03) & 0x08) == 0x08) && ((regArrayHandler.getRegArray(0x03) & 0x10) == 0x10))
            {
                // reset (status) register -> TODO 

                // stop SLEEP
                return;
            }

            /* MCLR (master reset): pd = 0 && to = 1 */
            else if (((regArrayHandler.getRegArray(0x03) & 0x08) == 0) && ((regArrayHandler.getRegArray(0x03) & 0x10) == 0x10))
            {
                // stop SLEEP
                return;
            }

            /* TIMEOUT OF WATCHDOG: pd = 0 && to = 0 */
            else if (((regArrayHandler.getRegArray(0x03) & 0x08) == 0) && ((regArrayHandler.getRegArray(0x03) & 0x10) == 0))
            {
                // stop SLEEP
                return;
            }

        }

        private void CLRWDT()
        {
            Console.WriteLine("CLRWDT gefunden");
            DataStorage.watchdogCounter = 0;
            if ((regArrayHandler.getRegArray(0x81) | 0x08) == 0x08)
            {
                // PSA is set for watchdog -> reset PSA bit
                //DataStorage.tim0.SelectClockSource(0);
            }
        }

        private void RETFIE()
        {
            var topofstack = DataStorage.stack1.GetValueFromStck();
            PCL.setPCL(topofstack);
            regArrayHandler.setRegArray(0x0B, (regArrayHandler.getRegArray(0x0B) | 0b10000000));
            DataStorage.addCycle(2);
        }

        private uint getFAddr(uint data)
        {
            uint f = data & 0b01111111;
            if (f == 0)
            {
                f = regArrayHandler.getRegArray(0x04);
            }

            uint status_bit = regArrayHandler.getRegArray(0x03) & 0b00100000;
            if (status_bit != 0)
            {
                f = f + 128;
            }

            return f;
        }

        private bool checkBreakpoint()
        {
            var linenumberdata = command_element.linenumber;
            bool filelistelementfound = false;
            foreach (var filelistelement in DataStorage.fileList)
            {
                if ((filelistelement.linenumber == linenumberdata) && filelistelement.breakpoint)
                {
                    filelistelementfound = true;
                }
            }

            if (filelistelementfound)
            {
                MainWindow.breakpointOccured(PCL.getPCL().ToString("X4"));
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}


