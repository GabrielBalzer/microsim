using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace microsim
{
    class FileHandling
    {
        public void Filehandlingfunc()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "LST Dateien (*.LST) |*.LST";
            openFileDialog.ShowDialog();
            int linenumber = 0;
            DataStorage.fileList = new ObservableCollection<DataStorage.FileList>();
            DataStorage.commandList = new List<DataStorage.Command>();

            foreach(string line in File.ReadAllLines(openFileDialog.FileName, Encoding.Default))
            {
                if (!string.IsNullOrEmpty(line))
                {

                    DataStorage.fileList.Add(new DataStorage.FileList
                    {
                        counter = line.Substring(0, 4),
                        command = line.Substring(5, 4),
                        program = line.Substring(21),
                        linenumber = linenumber,
                        breakpoint = false,
                        isActive = false

                    }
                        ) ;
                 
                    if(!string.IsNullOrWhiteSpace(line.Substring(5, 4)))
                    {
                        DataStorage.commandList.Add(new DataStorage.Command
                        {
                            command = line.Substring(5, 4),
                            data = 0,
                            linenumber = linenumber
                        }) ;
                    }
                }
                linenumber++;
            }


            //tim0.SelectClockSource(0); // set psa for timer0
            Console.WriteLine("OPTION_REG: " + DataStorage.regArray[0x81]);
            Console.WriteLine("INTERRUPTS 1: " + DataStorage.regArray[0x0B]);
            Console.WriteLine("INTERRUPTS 2: " + DataStorage.regArray[0x8B]);
        }
    }
}
