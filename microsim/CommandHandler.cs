using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
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

        public void nextCommand()
        {
            if (DataStorage.programCounter < DataStorage.commandList.Count)
            {
                Console.WriteLine(DataStorage.commandList.ElementAt((int) DataStorage.programCounter).command);
                handleCommand();
            }
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
            }
        }

        private void ANDLW()
        {
            Console.WriteLine("ANDLW gefunden");
            if (command_element.data <= 255)
            {
                DataStorage.w_register = DataStorage.w_register & command_element.data;
                if (DataStorage.w_register == 0)
                {
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000100;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000100;

                }
                else
                {
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] & 0b11111011;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] & 0b11111011;
                }

                Console.WriteLine("w-register: " + DataStorage.w_register);
            }
        }

        private void IORLW()
        {
            Console.WriteLine("IORLW gefunden");
            if (command_element.data <= 255)
            {
                DataStorage.w_register = DataStorage.w_register | command_element.data;
                if (DataStorage.w_register == 0)
                {
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000100;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000100;

                }
                else
                {
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] & 0b11111011;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] & 0b11111011;
                }

                Console.WriteLine("w-register: " + DataStorage.w_register);
            }

        }

        private void SUBLW()
        {
            int lowbitf;
            int lowbitw;
            Console.WriteLine("SUBLW gefunden");
            int result;
            lowbitf = (int) (command_element.data & 0b00001111);
            lowbitw = (int) (DataStorage.w_register & 0b00001111);
            if (command_element.data <= 255)
            {
                if ((lowbitw - lowbitf) < 0)
                {
                    //DC-Flag = 1
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000010;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000010;
                }
                else
                {
                    //DC-Flag = 0
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] & 0b11111101;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] & 0b11111101;
                }

                //DC-Flag missing
                result = (int) command_element.data - (int) DataStorage.w_register;
                if (result > 0)
                {
                    DataStorage.w_register = (uint) result;
                    // C-Flag = 1
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000001;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000001;

                    // Z-Flag = 0
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] & 0b11111011;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] & 0b11111011;

                }
                else if (result == 0)
                {
                    DataStorage.w_register = (uint) result;

                    // C-Flag = 1
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000001;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000001;

                    // Z-Flag = 1
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000100;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000100;

                }
                else
                {
                    DataStorage.w_register = (uint) (256 - Math.Abs(result));
                    // Z-Flag = 0
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] & 0b11111011;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] & 0b11111011;

                    // C-Flag = 0
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] & 0b11111110;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] & 0b11111110;
                }
            }
        }

        private void XORLW()
        {
            Console.WriteLine("XORLW gefunden");
            if (command_element.data <= 255)
            {
                DataStorage.w_register = DataStorage.w_register ^ command_element.data;
                if (DataStorage.w_register == 0)
                {
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000100;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000100;

                }
                else
                {
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] & 0b11111011;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] & 0b11111011;
                }

                Console.WriteLine("w-register: " + DataStorage.w_register);
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
                    //DC-Flag = 1
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000010;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000010;
                }
                else
                {
                    //DC-Flag = 0
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] & 0b11111101;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] & 0b11111101;
                }

                if (result <= 255)
                {
                    DataStorage.w_register = result;

                    // C-Flag = 0
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] & 0b11111110;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] & 0b11111110;
                }
                else
                {
                    // C-Flag = 1
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000001;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000001;

                    DataStorage.w_register = result - 256;
                }

                if (DataStorage.w_register == 0)
                {
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000100;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000100;

                }
                else
                {
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] & 0b11111011;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] & 0b11111011;
                }
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
                pclath3 = DataStorage.regArray[0x0A] & 0b00000100;
                pclath4 = DataStorage.regArray[0x0A] & 0b00001000;

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

                pclath3 = DataStorage.regArray[0x0A] & 0b00000100;
                pclath4 = DataStorage.regArray[0x0A] & 0b00001000;

                pclath = pclath4;
                pclath = pclath << 1;
                pclath = pclath | pclath3;
                pclath = pclath << 11;
                pclath = pclath | command_element.data;
                Console.WriteLine("PCLTEST CALL : " + pclath);
                PCL.setPCL(pclath - 1);
                Console.WriteLine("W-reg : " + DataStorage.w_register);
            }
        }

        private void NOP()
        {
            Console.WriteLine("NOP gefunden");
        }

        private void RETURN()
        {
            Console.WriteLine("RETURN gefunden");
            PCL.setPCL(DataStorage.stack1.GetValueFromStck() - 1);
            Console.WriteLine("Stack WERT : " + (DataStorage.programCounter));
            Console.WriteLine("W-reg : " + DataStorage.w_register);
        }

        private void RETLW()
        {
            Console.WriteLine("RETLW gefunden");
            if (command_element.data <= 255)
            {
                DataStorage.w_register = command_element.data;
                PCL.setPCL(DataStorage.stack1.GetValueFromStck() - 1);
                Console.WriteLine("W-reg : " + DataStorage.w_register);
            }
        }

        private void MOVWF()
        {
            Console.WriteLine("MOVWF gefunden");

            /*for (int counter = 0; counter < DataStorage.VarCounter; counter++)
            {
                DataStorage.Variable ausgabe = DataStorage.variableList.ElementAt(counter);

                if (command_element.data <= 127)
                {
                    if(ausgabe.variableName.Equals(DataStorage.variableList[counter].variableName))
                    {
                        // variable pre-declared & write into it
                        Console.WriteLine("GEFUNDEN: " + ausgabe.variableName);
                        DataStorage.variableList[counter].variableValue = DataStorage.w_register;
                        break;
                    }
                    else
                    {
                        // no variable pre-declared
                        DataStorage.regArray[command_element.data] = DataStorage.w_register;
                        Console.WriteLine("NICHT GEFUNDEN " + ausgabe.variableName);
                    }
                }
            }*/

            //if (command_element.data <= 127)
            //{
              //  DataStorage.regArray[command_element.data] = DataStorage.w_register;
            //}
            uint f = getFAddr(command_element.data);
            DataStorage.regArray[f] = DataStorage.w_register;
        }

        private void ADDWF()
        {
            uint f;
            uint d;
            uint result;
            Console.WriteLine("ADDWF gefunden");
            f = command_element.data & 0b01111111;
            d = command_element.data & 0b10000000;
            Console.WriteLine("F Wert: " + f);
            Console.WriteLine("D Wert: " + d);
            if (f <= 127)
            {
                result = DataStorage.w_register + DataStorage.regArray[f];

                //DC-Flag
                if (((DataStorage.w_register & 0b00001111) + (DataStorage.regArray[f] & 0b00001111) > 15))
                {
                    //DC-Flag = 1
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000010;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000010;
                }
                else
                {
                    //DC-Flag = 0
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] & 0b11111101;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] & 0b11111101;
                }

                if (result <= 255)
                {

                    // C-Flag = 0
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] & 0b11111110;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] & 0b11111110;
                }
                else
                {
                    // C-Flag = 1
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000001;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000001;

                    result = result - 256;
                }

                if (result == 0)
                {
                    // Z-Flag = 1
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000100;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000100;

                }
                else
                {
                    // Z-Flag = 0
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] & 0b11111011;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] & 0b11111011;
                }

                if (d == 0)
                {
                    DataStorage.w_register = result;
                }
                else
                {
                    DataStorage.regArray[f] = result;
                }
            }
        }

        private void ANDWF()
        {
            Console.WriteLine("ANDWF gefunden");
            uint f;
            uint d;
            uint result;
            f = command_element.data & 0b01111111;
            d = command_element.data & 0b10000000;

            if (f <= 127)
            {
                result = DataStorage.w_register & DataStorage.regArray[f];
                if (d == 0)
                {
                    DataStorage.w_register = result;
                }
                else
                {
                    DataStorage.regArray[f] = result;
                }

                if (result == 0)
                {
                    // Z-Flag = 1
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000100;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000100;

                }
                else
                {
                    // Z-Flag = 0
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] & 0b11111011;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] & 0b11111011;
                }
            }

        }

        private void CLRF()
        {
            Console.WriteLine("CLRF gefunden");
            if (command_element.data <= 127)
            {
                DataStorage.regArray[command_element.data] = 0;
                // Z-Flag = 1
                DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000100;
                DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000100;
            }
        }

        private void COMF()
        {
            Console.WriteLine("COMF gefunden");
            uint f;
            uint d;
            uint result;
            f = command_element.data & 0b01111111;
            d = command_element.data & 0b10000000;
            if (f <= 127)
            {
                result = ~DataStorage.regArray[f];
                result = result & 0xFF;
                if (result == 0)
                {
                    // Z-Flag = 1
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000100;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000100;

                }
                else
                {
                    // Z-Flag = 0
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] & 0b11111011;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] & 0b11111011;
                }

                if (d == 0)
                {
                    DataStorage.w_register = result;
                }
                else
                {
                    DataStorage.regArray[f] = result;
                }
            }
        }

        private void DECF()
        {
            Console.WriteLine("DECF gefunden");
            uint f;
            uint d;
            int result;
            int regValue;
            f = command_element.data & 0b01111111;
            d = command_element.data & 0b10000000;
            if (f <= 127)
            {
                regValue = (int) DataStorage.regArray[f];
                result = regValue - 1;
                if (result == 0)
                {
                    // Z-Flag = 1
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000100;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000100;

                }
                else
                {
                    // Z-Flag = 0
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] & 0b11111011;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] & 0b11111011;
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
                    DataStorage.regArray[f] = (uint) result;
                }
            }
        }

        private void INCF()
        {
            Console.WriteLine("INCF gefunden");
            uint f;
            uint d;
            uint result;
            f = command_element.data & 0b01111111;
            d = command_element.data & 0b10000000;
            result = DataStorage.regArray[f] + 1;
            if (result == 256)
            {
                result = 0;
            }

            if (result == 0)
            {
                // Z-Flag = 1
                DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000100;
                DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000100;

            }
            else
            {
                // Z-Flag = 0
                DataStorage.regArray[0x03] = DataStorage.regArray[0x03] & 0b11111011;
                DataStorage.regArray[0x83] = DataStorage.regArray[0x83] & 0b11111011;
            }

            if (d == 0)
            {
                DataStorage.w_register = result;
            }
            else
            {
                DataStorage.regArray[f] = result;
            }
        }

        private void MOVF()
        {
            Console.WriteLine("MOVF gefunden");
            uint f;
            uint d;
            uint result;
            f = command_element.data & 0b01111111;
            d = command_element.data & 0b10000000;
            result = DataStorage.regArray[f];
            if (result == 0)
            {
                // Z-Flag = 1
                DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000100;
                DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000100;

            }
            else
            {
                // Z-Flag = 0
                DataStorage.regArray[0x03] = DataStorage.regArray[0x03] & 0b11111011;
                DataStorage.regArray[0x83] = DataStorage.regArray[0x83] & 0b11111011;
            }

            if (d == 0)
            {
                DataStorage.w_register = result;
            }
            else
            {
                DataStorage.regArray[f] = result;
            }
        }

        private void IORWF()
        {
            Console.WriteLine("IORWF gefunden");
            uint f;
            uint d;
            uint result;
            f = command_element.data & 0b01111111;
            d = command_element.data & 0b10000000;
            result = DataStorage.w_register | DataStorage.regArray[f];
            if (result == 0)
            {
                // Z-Flag = 1
                DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000100;
                DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000100;

            }
            else
            {
                // Z-Flag = 0
                DataStorage.regArray[0x03] = DataStorage.regArray[0x03] & 0b11111011;
                DataStorage.regArray[0x83] = DataStorage.regArray[0x83] & 0b11111011;
            }

            if (d == 0)
            {
                DataStorage.w_register = result;
            }
            else
            {
                DataStorage.regArray[f] = result;
            }
        }

        private void SUBWF()
        {
            Console.WriteLine("SUBWF gefunden");
            uint f;
            uint d;
            int result;
            int lowbitf;
            int lowbitw;
            f = command_element.data & 0b01111111;
            d = command_element.data & 0b10000000;
            lowbitf = (int) (DataStorage.regArray[f] & 0b00001111);
            lowbitw = (int) (DataStorage.w_register & 0b00001111);
            if ((lowbitf - lowbitw) < 0)
            {
                //DC-Flag = 1
                DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000010;
                DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000010;
            }
            else
            {
                //DC-Flag = 0
                DataStorage.regArray[0x03] = DataStorage.regArray[0x03] & 0b11111101;
                DataStorage.regArray[0x83] = DataStorage.regArray[0x83] & 0b11111101;
            }

            //DC-Flag missing
            result = ((int) DataStorage.regArray[f] - (int) DataStorage.w_register);
            if (result > 0)
            {
                DataStorage.w_register = (uint) result;
                // C-Flag = 1
                DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000001;
                DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000001;

                // Z-Flag = 0
                DataStorage.regArray[0x03] = DataStorage.regArray[0x03] & 0b11111011;
                DataStorage.regArray[0x83] = DataStorage.regArray[0x83] & 0b11111011;

            }
            else if (result == 0)
            {
                DataStorage.w_register = (uint) result;

                // C-Flag = 1
                DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000001;
                DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000001;

                // Z-Flag = 1
                DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000100;
                DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000100;

            }
            else
            {
                DataStorage.w_register = (uint) (256 - Math.Abs(result));
                // Z-Flag = 0
                DataStorage.regArray[0x03] = DataStorage.regArray[0x03] & 0b11111011;
                DataStorage.regArray[0x83] = DataStorage.regArray[0x83] & 0b11111011;

                // C-Flag = 0
                DataStorage.regArray[0x03] = DataStorage.regArray[0x03] & 0b11111110;
                DataStorage.regArray[0x83] = DataStorage.regArray[0x83] & 0b11111110;
            }
        }

        private void SWAPF()
        {
            Console.WriteLine("SWAPF gefunden");
            uint f;
            uint d;
            uint result;
            f = command_element.data & 0b01111111;
            d = command_element.data & 0b10000000;
            result = ((DataStorage.regArray[f] & 0x0F) << 4 | (DataStorage.regArray[f] & 0xF0) >> 4);
            if (d == 0)
            {
                DataStorage.w_register = result;
            }
            else
            {
                DataStorage.regArray[f] = result;
            }
        }

        private void XORWF()
        {
            Console.WriteLine("XORWF gefunden");
            uint f;
            uint d;
            uint result;
            f = command_element.data & 0b01111111;
            d = command_element.data & 0b10000000;
            result = DataStorage.w_register ^ DataStorage.regArray[f];
            if (result == 0)
            {
                // Z-Flag = 1
                DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000100;
                DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000100;
            }
            else
            {
                // Z-Flag = 0
                DataStorage.regArray[0x03] = DataStorage.regArray[0x03] & 0b11111011;
                DataStorage.regArray[0x83] = DataStorage.regArray[0x83] & 0b11111011;
            }

            if (d == 0)
            {
                DataStorage.w_register = result;
            }
            else
            {
                DataStorage.regArray[f] = result;
            }
        }

        private void CLRW()
        {
            Console.WriteLine("CLRW gefunden");
            // Z-Flag = 1
            DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000100;
            DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000100;

            DataStorage.w_register = 0;
        }

        private void RLF()
        {

            Console.WriteLine("RLF gefunden");
            uint f;
            byte regValue;
            uint d;
            byte statusbyte;
            byte checker;
            statusbyte = (byte) (DataStorage.regArray[0x03] & 0b000000001);
            f = command_element.data & 0b01111111;
            d = command_element.data & 0b10000000;
            checker = (byte) (DataStorage.regArray[f] & 0b10000000);
            if (checker == 0)
            {
                // C-Flag = 0
                DataStorage.regArray[0x03] = DataStorage.regArray[0x03] & 0b11111110;
                DataStorage.regArray[0x83] = DataStorage.regArray[0x83] & 0b11111110;
            }
            else
            {
                // C-Flag = 1
                DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000001;
                DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000001;
            }

            regValue = (byte) (DataStorage.regArray[f]);
            regValue = (byte) (regValue << 1);
            regValue = (byte) (regValue | statusbyte);

            if (d == 0)
            {
                DataStorage.w_register = regValue;
            }
            else
            {
                DataStorage.regArray[f] = regValue;
            }

        }

        private void RRF()
        {
            Console.WriteLine("RRF gefunden");
            uint f;
            byte regValue;
            uint d;
            byte statusbyte;
            byte checker;
            statusbyte = (byte) (DataStorage.regArray[0x03] & 0b000000001);
            if (statusbyte == 1)
            {
                statusbyte = 0b10000000;
            }
            f = command_element.data & 0b01111111;
            d = command_element.data & 0b10000000;
            checker = (byte) (DataStorage.regArray[f] & 0b00000001);
            if (checker == 0)
            {
                // C-Flag = 0
                DataStorage.regArray[0x03] = DataStorage.regArray[0x03] & 0b11111110;
                DataStorage.regArray[0x83] = DataStorage.regArray[0x83] & 0b11111110;
            }
            else
            {
                // C-Flag = 1
                DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000001;
                DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000001;
            }

            regValue = (byte) (DataStorage.regArray[f]);
            regValue = (byte) (regValue >> 1);
            regValue = (byte) (regValue | statusbyte);

            if (d == 0)
            {
                DataStorage.w_register = regValue;
            }
            else
            {
                DataStorage.regArray[f] = regValue;
            }
        }

        private void DECFSZ()
        {
            Console.WriteLine("DECFSZ gefunden");
            uint f;
            uint d;
            int result;
            f = command_element.data & 0b01111111;
            d = command_element.data & 0b10000000;
            result = (int)DataStorage.regArray[f];

            if ((result - 1) == 0)
            {
                PCL.addtoPCL();
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
                DataStorage.regArray[f] = (uint)result;
            }
        }

        private void INCFSZ()
        {
            Console.WriteLine("INCFSZ gefunden");
            uint f;
            uint d;
            uint result;
            f = command_element.data & 0b01111111;
            d = command_element.data & 0b10000000;
            result = DataStorage.regArray[f];
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
            }

            if (d == 0)
            {
                DataStorage.w_register = result;
            }
            else
            {
                DataStorage.regArray[f] = result;
            }
        }

        private void BSF()
        {
            Console.WriteLine("BSF gefunden");
            uint f;
            uint b;
            //uint result;
            f = command_element.data & 0b01111111;
            b = command_element.data & 0b1110000000;
            b = b >> 7;

            DataStorage.regArray[f] = DataStorage.regArray[f] | (uint)(Math.Pow((double)2, (double)b));
        }

        private void BCF()
        {
            Console.WriteLine("BCF gefunden");
            uint f;
            uint b;
            //uint result;
            f = command_element.data & 0b01111111;
            b = command_element.data & 0b1110000000;
            b = b >> 7;

            DataStorage.regArray[f] = DataStorage.regArray[f] ^ (uint)(Math.Pow((double)2, (double)b));
        }

        private void BTFSC()
        {
            Console.WriteLine("BTFSC gefunden");
            uint f;
            uint b;
            uint result;
            uint bCalc;
            f = command_element.data & 0b01111111;
            b = command_element.data & 0b1110000000;
            b = b >> 7;
            result = DataStorage.regArray[f];
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
            }
        }

        private void BTFSS()
        {
            Console.WriteLine("BTFSS gefunden");
            uint f;
            uint b;
            uint result;
            uint bCalc;
            f = command_element.data & 0b01111111;
            b = command_element.data & 0b1110000000;
            b = b >> 7;
            result = DataStorage.regArray[f];
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
            }
        }

        private uint getFAddr(uint data)
        {
            uint f = data & 0b01111111;
            if (f == 0)
            {
                f = DataStorage.regArray[0x04];
            }

            return f;
        }
    }
}


