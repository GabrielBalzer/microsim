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
            int linecounter = 0;
            string[] filearray = new string[150];
            foreach(string line in File.ReadAllLines(openFileDialog.FileName))
            {
                linecounter++;
                filearray[linecounter] = line;
            }
            foreach(string item in filearray)
            {
                Console.WriteLine(item);
            }
        }
    }
}
