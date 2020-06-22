namespace microsim
{
    internal class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel()
        {
            FileRegisterColumns = new[] {"+0", "+1", "+2", "+3", "+4", "+5", "+6", "+7"};
            FileRegisterRows = new[]
            {
                "00", "08", "10", "18", "20", "28", "30", "38", "40", "48", "50", "58", "60", "68", "70", "78", "80",
                "88", "90", "98", "A0", "A8", "B0", "B8", "C0", "C8", "D0", "D8", "E0", "E8", "F0", "F8"
            };
            FileRegisterData = new string[32, 8];
            StatusRegisterData = new char[8];
            OptionRegisterData = new char[8];
            IntconRegisterData = new char[8];
            Pina = new bool[8];
            Pinb = new bool[8];
            Trisa = new bool[8];
            Trisb = new bool[8];
            Timespent = "0 µ";
            Timer = "00";
            Prescaler = "1:1";
            WReg = "00";
            PCL = "00";
            PCLATH = "00";
            Pcintern = "00";
            STATUS = "00";
            FSR = "00";
            OPTION = "00";
            Watchdog = "0 µ";
            StackUI = new string[8]
                {"00000000", "00000000", "00000000", "00000000", "00000000", "00000000", "00000000", "00000000"};
        }

        #region w-Register

        private string wReg;

        public string WReg
        {
            get => wReg;
            set { SetAndNotify(ref wReg, value, () => wReg); }
        }

        #endregion

        #region PCL

        private string pcl;

        public string PCL
        {
            get => pcl;
            set { SetAndNotify(ref pcl, value, () => pcl); }
        }

        #endregion

        #region PCintern

        private string pcintern;

        public string Pcintern
        {
            get => pcintern;
            set { SetAndNotify(ref pcintern, value, () => pcintern); }
        }

        #endregion

        #region STACK

        private string[] stackUI;

        public string[] StackUI
        {
            get => stackUI;
            set { SetAndNotify(ref stackUI, value, () => stackUI); }
        }

        #endregion

        #region FileRegister

        public string[] FileRegisterColumns { get; }
        public string[] FileRegisterRows { get; }
        private string[,] fileRegisterData;

        public string[,] FileRegisterData
        {
            get => fileRegisterData;
            set { SetAndNotify(ref fileRegisterData, value, () => FileRegisterData); }
        }

        #endregion

        #region SFR

        private char[] statusRegisterData;

        public char[] StatusRegisterData
        {
            get => statusRegisterData;
            set { SetAndNotify(ref statusRegisterData, value, () => statusRegisterData); }
        }

        private char[] optionRegisterData;

        public char[] OptionRegisterData
        {
            get => optionRegisterData;
            set { SetAndNotify(ref optionRegisterData, value, () => optionRegisterData); }
        }

        private char[] intconRegisterData;

        public char[] IntconRegisterData
        {
            get => intconRegisterData;
            set { SetAndNotify(ref intconRegisterData, value, () => intconRegisterData); }
        }

        #endregion

        #region Pins

        private bool[] pina;

        public bool[] Pina
        {
            get => pina;
            set { SetAndNotify(ref pina, value, () => pina); }
        }

        private bool[] pinb;

        public bool[] Pinb
        {
            get => pinb;
            set { SetAndNotify(ref pinb, value, () => pinb); }
        }

        private bool[] trisa;

        public bool[] Trisa
        {
            get => trisa;
            set { SetAndNotify(ref trisa, value, () => trisa); }
        }

        private bool[] trisb;

        public bool[] Trisb
        {
            get => trisb;
            set { SetAndNotify(ref trisb, value, () => trisb); }
        }

        #endregion

        #region time

        private string timespent;

        public string Timespent
        {
            get => timespent;
            set { SetAndNotify(ref timespent, value, () => timespent); }
        }

        #endregion

        #region PCLATH

        private string pclath;

        public string PCLATH
        {
            get => pclath;
            set { SetAndNotify(ref pclath, value, () => pclath); }
        }

        #endregion

        #region Status

        private string status;

        public string STATUS
        {
            get => status;
            set { SetAndNotify(ref status, value, () => status); }
        }

        #endregion

        #region FSR

        private string fsr;

        public string FSR
        {
            get => fsr;
            set { SetAndNotify(ref fsr, value, () => fsr); }
        }

        #endregion

        #region OPTION

        private string option;

        public string OPTION
        {
            get => option;
            set { SetAndNotify(ref option, value, () => option); }
        }

        #endregion

        #region Timer

        private string timer;

        public string Timer
        {
            get => timer;
            set { SetAndNotify(ref timer, value, () => timer); }
        }

        #endregion

        #region Prescaler

        private string prescaler;

        public string Prescaler
        {
            get => prescaler;
            set { SetAndNotify(ref prescaler, value, () => prescaler); }
        }

        #endregion

        #region Watchdog

        private string watchdog;

        public string Watchdog
        {
            get => watchdog;
            set { SetAndNotify(ref watchdog, value, () => watchdog); }
        }

        #endregion
    }
}