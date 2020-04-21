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
        public void filehandlingfunc()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "LST Dateien (*.lst) |*.LST";
            openFileDialog.ShowDialog();
            string test = File.ReadAllText(openFileDialog.FileName);
            MessageBox.Show(test);
        }
    }
}
