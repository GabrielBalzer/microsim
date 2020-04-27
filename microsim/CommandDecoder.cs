using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static microsim.FileHandling;

namespace microsim
{
    class CommandDecoder
    {
        public void decodeCommands()
        {
            uint hex;
            uint schema1 = 0xFF00;
            uint bitSchema1 = 0x00FF;
            uint schema2 = 0xFC00;
            uint bitSchema2 = 0x03FF;
            uint schema3 = 0xFF80;
            uint bitSchema3 = 0x007F;
            uint schema4 = 0xFE00;
            uint bitSchema4 = 0x01FF;
            uint schema5 = 0xF800;
            uint bitSchema5 = 0x07FF;
            foreach (var item in DataStorage.commandList)
            {

                hex = Convert.ToUInt32(item.command, 16);

                switch (hex)
                {
                    case 0b0000000001100100:
                        item.command = "CLRWDT";
                        break;
                    case 0b0000000000001001:
                        item.command = "RETFIE";
                        break;
                    case 0b0000000000001000:
                        item.command = "RETURN";
                        break;
                    case 0b0000000001100011:
                        item.command = "SLEEP";
                        break;
                    case 0b0000000000000000:
                        item.command = "NOP";
                        break;
                    case 0b0000000001000000:
                        item.command = "NOP";
                        break;
                    case 0b0000000000100000:
                        item.command = "NOP";
                        break;
                    case 0b0000000001100000:
                        item.command = "NOP";
                        break;
                    default:
                        break;
                }




                //Schema 1
                switch (hex & schema1)
                {
                    case 0b0000011100000000:
                        item.command = "ADDWF";
                        item.data = hex & bitSchema1;
                        break;
                    case 0b0000010100000000:
                        item.command = "ANDWF";
                        item.data = hex & bitSchema1;
                        break;
                    case 0b0000100100000000:
                        item.command = "COMF";
                        item.data = hex & bitSchema1;
                        break;
                    case 0b0000001100000000:
                        item.command = "DECF";
                        item.data = hex & bitSchema1;
                        break;
                    case 0b0000101100000000:
                        item.command = "DECFSZ";
                        item.data = hex & bitSchema1;
                        break;
                    case 0b0000101000000000:
                        item.command = "INCF";
                        item.data = hex & bitSchema1;
                        break;
                    case 0b0000111100000000:
                        item.command = "INCFSZ";
                        item.data = hex & bitSchema1;
                        break;
                    case 0b0000010000000000:
                        item.command = "IORWF";
                        item.data = hex & bitSchema1;
                        break;
                    case 0b0000100000000000:
                        item.command = "MOVF";
                        item.data = hex & bitSchema1;
                        break;
                    case 0b0000110100000000:
                        item.command = "RLF";
                        item.data = hex & bitSchema1;
                        break;
                    case 0b0000110000000000:
                        item.command = "RRF";
                        item.data = hex & bitSchema1;
                        break;
                    case 0b0000001000000000:
                        item.command = "SUBWF";
                        item.data = hex & bitSchema1;
                        break;
                    case 0b0000111000000000:
                        item.command = "SWAPF";
                        item.data = hex & bitSchema1;
                        break;
                    case 0b0000011000000000:
                        item.command = "XORWF";
                        item.data = hex & bitSchema1;
                        break;
                    case 0b0011100100000000:
                        item.command = "ANDLW";
                        item.data = hex & bitSchema1;
                        break;
                    case 0b0011100000000000:
                        item.command = "IORLW";
                        item.data = hex & bitSchema1;
                        break;
                    case 0b0011101000000000:
                        item.command = "XORLW";
                        item.data = hex & bitSchema1;
                        break;
                    default:
                        break;
                }

                //Schema 2
                switch (hex & schema2)
                {
                    case 0b0001000000000000:
                        item.command = "BCF";
                        item.data = hex & bitSchema2;
                        break;
                    case 0b0001010000000000:
                        item.command = "BSF";
                        item.data = hex & bitSchema2;
                        break;
                    case 0b0001100000000000:
                        item.command = "BTFSC";
                        item.data = hex & bitSchema2;
                        break;
                    case 0b0001110000000000:
                        item.command = "BTFSS";
                        item.data = hex & bitSchema2;
                        break;
                    case 0b0011000000000000:
                        item.command = "MOVLW";
                        item.data = hex & bitSchema2;
                        break;
                    case 0b0011010000000000:
                        item.command = "RETLW";
                        item.data = hex & bitSchema2;
                        break;
                    default:
                        break;
                }

                //Schema3
                switch (hex & schema3)
                {
                    case 0b0000000110000000:
                        item.command = "CLRF";
                        item.data = hex & bitSchema3;
                        break;
                    case 0b0000000100000000:
                        item.command = "CLRW";
                        item.data = hex & bitSchema3;
                        break;
                    case 0b0000000010000000:
                        item.command = "MOVWF";
                        item.data = hex & bitSchema3;
                        break;
                    default:
                        break;
                }

                //Schema4
                switch (hex & schema4)
                {
                    case 0b0011111000000000:
                        item.command = "ADDLW";
                        item.data = hex & bitSchema4;
                        break;
                    case 0b0011110000000000:
                        item.command = "SUBLW";
                        item.data = hex & bitSchema4;
                        break;
                    default:
                        break;
                }

                //Schema5
                switch (hex & schema5)
                {
                    case 0b0010000000000000:
                        item.command = "CALL";
                        item.data = hex & bitSchema5;
                        break;
                    case 0b0010100000000000:
                        item.command = "GOTO";
                        item.data = hex & bitSchema5;
                        break;
                    default:
                        break;
                }
                Console.WriteLine(item.command + " Daten: " + item.data);

            }


        }
    }
}
