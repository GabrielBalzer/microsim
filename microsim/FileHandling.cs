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
        public string test;
        

        public void Filehandlingfunc()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "LST Dateien (*.lst) |*.LST";
            openFileDialog.ShowDialog();
            test = File.ReadAllText(openFileDialog.FileName);
            List<string> file_list = new List<string>();
            foreach(string line in File.ReadAllLines(openFileDialog.FileName))
            {
                if (!string.IsNullOrEmpty(line))
                {
                    file_list.Add(line);
                }
            }
            foreach(string item in file_list)
            {
                Console.WriteLine(item);
            }
        }
    }
}
