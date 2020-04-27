using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microsim
{
    static class DataStorage
    {
        public static List<Command> commandList = new List<Command>();
        public static List<string> fileList = new List<string>();
        public class Command
        {
            public string command { get; set; }
            public uint data { get; set; }
        }
    }
}
