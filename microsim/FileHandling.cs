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
        OpenFileDialog openFileDialog = new OpenFileDialog();
        char[] cBuffer = new char[4];

        public void filehandlingfunc()
        {
            openFileDialog.Filter = "LST Dateien (*.lst) |*.LST";
            openFileDialog.ShowDialog();
            test = File.ReadAllText(openFileDialog.FileName);
            StringReader strReader = new StringReader(test);
            using (StringReader reader = new StringReader(test))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Do something with the line
                    string test2 = line.Substring(0,4);
                    MessageBox.Show(test2);
                }
            }
        }
    }
}
