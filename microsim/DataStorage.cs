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
        public static uint w_register;
        public static List<Command> commandList = new List<Command>();
        public static ObservableCollection<FileList> fileList = new ObservableCollection<FileList>();
        public static uint[] regArray = new uint[256];
        public class Command
        {
            public string command { get; set; }
            public uint data { get; set; }
            public int linenumber { get; set; }
        }

        public class FileList
        {
            public string counter { get; set; }
            public string command { get; set; }
            public string program { get; set; }
            public int linenumber { get; set; }

        }

        // pcl
        public static uint programCounter = 0;
        public static uint maskProgramCounter = 0xFF; // first 8 bits of programCounter

        // stack
        public static Stack stack1 = new Stack();

    }
}
