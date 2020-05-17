using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microsim
{
    class MainWindowViewModel : ObservableObject
    {
        private string wReg;
        public string WReg
        {
            get { return this.wReg; }
            set { this.SetAndNotify(ref this.wReg, value, () => this.wReg); }
        }
        public string[] FileRegisterColumns { get; }
        public string[] FileRegisterRows { get; }
        private string[,] fileRegisterData;
        public string[,] FileRegisterData
        {
            get { return this.fileRegisterData; }
            set { this.SetAndNotify(ref this.fileRegisterData, value, () => this.FileRegisterData); }
        }
        public MainWindowViewModel()
        {
            this.FileRegisterColumns = new string[] { "+0", "+1", "+2", "+3", "+4", "+5", "+6", "+7" };
            this.FileRegisterRows = new string[] { "00", "08", "10", "18", "20", "28", "30", "38", "40", "48", "50", "58", "60", "68", "70", "78", "80", "88", "90", "98", "A0", "A8", "B0", "B8", "C0", "C8", "D0", "D8", "E0", "E8", "F0", "F8" };
            this.FileRegisterData = new string[32, 8];
            this.WReg = "00";
        }
    }
}
