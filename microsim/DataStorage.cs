using System;
using System.CodeDom;
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
        public static uint quarzfreq;
        public static List<Command> commandList = new List<Command>();
        public static ObservableCollection<FileList> fileList = new ObservableCollection<FileList>();
        public static uint[] regArray = new uint[256];
        public static uint cycleCount;
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

        public static int startCounter = 0;


        // stack
        public static Stack stack1 = new Stack();

        public static void addCycle(int cycle)
        {
            DataStorage.cycleCount = DataStorage.cycleCount + (uint)cycle;
        }

        // timer0
        public static Timer0 tim0 = new Timer0();

        public static uint timer0 = 0;
        public static List<Prescaler> PrescTimer0 = new List<Prescaler>(8);

        public static int ichzaehlehoch = 0;
        
        public static uint Timer0_Limit;
        public static uint Timer0_Predef_Value;

        public class Prescaler
        {
            public uint Timer0Value;
            public uint prescalerValue;
        }
        public static uint prescalerValue;
        //uint static prescaler;

        // watchdog
        //public static Watchdog watchdog1 = new Watchdog();

        public static uint watchdogCounter = 0;
        public static List<Prescaler> PrescWatchdog = new List<Prescaler>();


    }
}
