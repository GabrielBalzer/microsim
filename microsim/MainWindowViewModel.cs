using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microsim
{
    class MainWindowViewModel : ObservableObject
    {
        #region w-Register
        private string wReg;
        public string WReg
        {
            get { return this.wReg; }
            set { this.SetAndNotify(ref this.wReg, value, () => this.wReg); }
        }
        #endregion

        #region PCL
        private string pcl;

        public string PCL
        {
            get { return this.pcl; }
            set { this.SetAndNotify(ref this.pcl, value, () => this.pcl);}
        }
        #endregion

        #region PCintern

        private string pcintern;

        public string Pcintern
        {
            get { return this.pcintern; }
            set { this.SetAndNotify(ref this.pcintern, value, () => this.pcintern); }
        }

        #endregion

        #region STACK
        private string[] stackUI;
        public string[] StackUI
        {
            get { return this.stackUI; }
            set { this.SetAndNotify(ref this.stackUI, value, () => this.stackUI); }
        }
        #endregion

        #region FileRegister
        public string[] FileRegisterColumns { get; }
        public string[] FileRegisterRows { get; }
        private string[,] fileRegisterData;
        public string[,] FileRegisterData
        {
            get { return this.fileRegisterData; }
            set { this.SetAndNotify(ref this.fileRegisterData, value, () => this.FileRegisterData); }
        }
        #endregion

        #region SFR

        private char[] statusRegisterData;

        public char[] StatusRegisterData
        {
            get { return this.statusRegisterData; }
            set { this.SetAndNotify(ref this.statusRegisterData, value, () => this.statusRegisterData); }
        }

        private char[] optionRegisterData;

        public char[] OptionRegisterData
        {
            get { return this.optionRegisterData; }
            set { this.SetAndNotify(ref this.optionRegisterData, value, () => this.optionRegisterData); }
        }

        private char[] intconRegisterData;

        public char[] IntconRegisterData
        {
            get { return this.intconRegisterData; }
            set { this.SetAndNotify(ref this.intconRegisterData, value, () => this.intconRegisterData); }
        }

        #endregion

        #region Pins

        private bool[] pina;

        public bool[] Pina
        {
            get { return this.pina; }
            set { this.SetAndNotify(ref this.pina, value, () => this.pina); }
        }

        private bool[] pinb;

        public bool[] Pinb
        {
            get { return this.pinb; }
            set { this.SetAndNotify(ref this.pinb, value, () => this.pinb); }
        }

        private bool[] trisa;

        public bool[] Trisa
        {
            get { return this.trisa; }
            set { this.SetAndNotify(ref this.trisa, value, () => this.trisa); }
        }

        private bool[] trisb;

        public bool[] Trisb
        {
            get { return this.trisb; }
            set { this.SetAndNotify(ref this.trisb, value, () => this.trisb); }
        }

        #endregion

        #region time

        private string timespent;

        public string Timespent
        {
            get { return this.timespent; }
            set { this.SetAndNotify(ref this.timespent, value, () => this.timespent); }
        }

        #endregion

        #region PCLATH

        private string pclath;

        public string PCLATH
        {
            get { return this.pclath; }
            set { this.SetAndNotify(ref this.pclath, value, () => this.pclath); }
        }

        #endregion

        #region Status

        private string status;

        public string STATUS
        {
            get { return this.status; }
            set { this.SetAndNotify(ref this.status, value, () => this.status); }
        }
        #endregion

        #region FSR

        private string fsr;

        public string FSR
        {
            get { return this.fsr; }
            set { this.SetAndNotify(ref this.fsr, value, () => this.fsr); }
        }

        #endregion

        #region OPTION

        private string option;

        public string OPTION
        {
            get { return this.option; }
            set { this.SetAndNotify(ref this.option, value, () => this.option); }
        }

        #endregion

        #region Timer

        private string timer;
        public string Timer
        {
            get { return this.timer; }
            set { this.SetAndNotify(ref this.timer, value, () => this.timer); }
        }

        #endregion

        #region Prescaler

        private string prescaler;

        public string Prescaler
        {
            get { return this.prescaler; }
            set { this.SetAndNotify(ref this.prescaler, value, () => this.prescaler); }
        }

        #endregion

        #region Watchdog

        private string watchdog;

        public string Watchdog
        {
            get { return this.watchdog; }
            set { this.SetAndNotify(ref this.watchdog, value, () => this.watchdog); }
        }

        #endregion
        public MainWindowViewModel()
        {
            this.FileRegisterColumns = new string[] { "+0", "+1", "+2", "+3", "+4", "+5", "+6", "+7" };
            this.FileRegisterRows = new string[] { "00", "08", "10", "18", "20", "28", "30", "38", "40", "48", "50", "58", "60", "68", "70", "78", "80", "88", "90", "98", "A0", "A8", "B0", "B8", "C0", "C8", "D0", "D8", "E0", "E8", "F0", "F8" };
            this.FileRegisterData = new string[32, 8];
            this.StatusRegisterData = new char[8];
            this.OptionRegisterData = new char[8];
            this.IntconRegisterData = new char[8];
            this.Pina = new bool[8];
            this.Pinb = new bool[8];
            this.Trisa = new bool[8];
            this.Trisb = new bool[8];
            this.Timespent = "0 µ";
            this.Timer = "00";
            this.Prescaler = "1:1";
            this.WReg = "00";
            this.PCL = "00";
            this.PCLATH = "00";
            this.Pcintern = "00";
            this.STATUS = "00";
            this.FSR = "00";
            this.OPTION = "00";
            this.Watchdog = "00";
            this.StackUI = new string[8]
                {"00000000", "00000000", "00000000", "00000000", "00000000", "00000000", "00000000", "00000000"};
        }
    }
}
