using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace microsim
{
    public static class DataStorage
    {
        public static uint w_register;
        public static uint quarzfreq;
        public static List<Command> commandList = new List<Command>();
        public static ObservableCollection<FileList> fileList = new ObservableCollection<FileList>();
        public static uint[] regArray = new uint[256];
        public static uint cycleCount;
        public static List<int> commandLines = new List<int>();

        public static int startCounter = 0;


        // pcl
        public static uint programCounter = 0;


        // stack
        public static Stack stack1 = new Stack();


        //prescaler
        public static uint prescalerValue;
        public static int prescalerCount = 0;

        public static bool lowHighFlankRA4;
        public static bool highLowFlankRA4;

        public static bool watchdogEnabled;

        public static double timeSpent;

        public static double watchdogValue;


        public static void addCycle(int cycle)
        {
            cycleCount = cycleCount + (uint) cycle;
        }

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
            public bool breakpoint { get; set; }
            public bool isActive { get; set; }
        }
    }
}