using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace microsim
{
    class FileHandling
    {
        public class Command
        {
            public string command { get; set; }
            public uint data { get; set; }
        }

        public void Filehandlingfunc()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "LST Dateien (*.LST) |*.LST";
            openFileDialog.ShowDialog();
            List<string> file_list = new List<string>();
            var commandList = new List<Command>();
            foreach(string line in File.ReadAllLines(openFileDialog.FileName))
            {
                if (!string.IsNullOrEmpty(line))
                {
                    file_list.Add(line);
                 
                    if(!string.IsNullOrWhiteSpace(line.Substring(5, 4)))
                    {
                        commandList.Add(new Command
                        {
                            command = line.Substring(5, 4),
                            data = 0
                        }) ;
                    }
                }
            }
            CommandDecoder(commandList);
        }

        public void CommandDecoder(List<Command> commandList)
        {
            uint hex;
            foreach(var item in commandList)
            {

                hex = Convert.ToUInt32(item.command, 16);

                switch(hex)
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


                Console.WriteLine(item.command);
                Console.WriteLine(item.data);
            }


        }
    }
}
