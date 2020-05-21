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
            //List<DataStorage.Variable> variableList = new List<DataStorage.Variable>();

            foreach(string line in File.ReadAllLines(openFileDialog.FileName))
            {
                if (!string.IsNullOrEmpty(line))
                {
                    string variableEqu = "equ";
                    if (line.Contains(variableEqu))
                    {
                        var item = line.Substring(36, 16);
                        var indexVar = item.IndexOf(variableEqu);
                        var itemVarName = line.Substring(36, indexVar);
                        var itemVarValue = item.Substring(item.IndexOf(variableEqu) + variableEqu.Length);

                        // enter values to list of variables
                        //DataStorage.variableList.Add(new DataStorage.Variable()
                        //{
                        //    variableName = itemVarName,
                        //    variableValue = int.Parse(itemVarValue)
                        //});

                        string itemVarValueStringHex = itemVarValue.Substring(1, itemVarValue.Length - 2);
                        var itemVarValueInt = Convert.ToInt32(itemVarValueStringHex, 16);

                        DataStorage.variableList.Add(new DataStorage.Variable(){variableName = itemVarName, variableValue = itemVarValueInt});

                        foreach (var i in DataStorage.variableList)
                        {
                            Console.WriteLine("Var Name: " + i.variableName);
                            Console.WriteLine("Var Value: " + i.variableValue);
                        }
                    }

                    DataStorage.fileList.Add(new DataStorage.FileList
                    {
                        counter = line.Substring(0, 4),
                        command = line.Substring(5, 4),
                        program = line.Substring(21),
                        linenumber = linenumber

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
        }
    }
}
