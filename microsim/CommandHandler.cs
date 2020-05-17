using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                Console.WriteLine(DataStorage.commandList.ElementAt((int)DataStorage.programCounter).command);
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
            command_element = DataStorage.commandList.ElementAt((int)DataStorage.programCounter);
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
            Console.WriteLine("SUBLW gefunden");
            int result;
            if (command_element.data <= 255)
            { 
                //DC-Flag missing
                result = (int)command_element.data - (int)DataStorage.w_register;
                if (result > 0)
                {
                    DataStorage.w_register = (uint)result;
                    // C-Flag = 1
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000001;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000001;

                    // Z-Flag = 0
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] & 0b11111011;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] & 0b11111011;

                }
                else if (result == 0)
                {
                    DataStorage.w_register = (uint)result;

                    // C-Flag = 1
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000001;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000001;

                    // Z-Flag = 1
                    DataStorage.regArray[0x03] = DataStorage.regArray[0x03] | 0b00000100;
                    DataStorage.regArray[0x83] = DataStorage.regArray[0x83] | 0b00000100;

                }
                else
                {
                    DataStorage.w_register = (uint)(256 - Math.Abs(result));
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
    }
}
