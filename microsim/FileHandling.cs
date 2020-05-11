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
            DataStorage.fileList = new List<DataStorage.FileList>();
            DataStorage.commandList = new List<DataStorage.Command>();
            foreach(string line in File.ReadAllLines(openFileDialog.FileName))
            {
                if (!string.IsNullOrEmpty(line))
                {
                    DataStorage.fileList.Add(new DataStorage.FileList
                    {
                        counter = line.Substring(0, 4),
                        command = line.Substring(5, 4),
                        program = line.Substring(21)

                    }
                        ) ;
                 
                    if(!string.IsNullOrWhiteSpace(line.Substring(5, 4)))
                    {
                        DataStorage.commandList.Add(new DataStorage.Command
                        {
                            command = line.Substring(5, 4),
                            data = 0
                        }) ;
                    }
                }
            }
        }
    }
}
