using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace microsim
{
    public static class DataStorage
    {
        public static List<Command> commandList = new List<Command>();
        public static List<FileList> fileList = new List<FileList>();
        public static uint[] regArray = new uint[256];
        public class Command
        {
            public string command { get; set; }
            public uint data { get; set; }
        }

        public class FileList : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            public string counter
            {
                get { return counter; }
                set
                {
                    counter = value;
                    OnPropertyChanged("counter");
                }
            }
            public string command { get; set; }
            public string program { get; set; }
            private void OnPropertyChanged(string propertyName)
            {
                var handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs(propertyName));
            }

        }

        // pcl
        public static uint programCounter = 0;
        public static uint maskProgramCounter = 0xFF; // first 8 bits of programCounter



        // register array

    }
}
