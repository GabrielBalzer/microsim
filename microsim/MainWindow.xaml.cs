using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using MahApps.Metro.Controls;

namespace microsim
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        FileHandling FileHandlingLocal = new FileHandling();
        CommandDecoder CommandDecoder = new CommandDecoder();
        CommandHandler CommandHandler = new CommandHandler();
        Initializer Initializer = new Initializer();
        PCL PCL = new PCL();
        RegArrayHandler regArrayHandler = new RegArrayHandler();
        MainWindowViewModel View = new MainWindowViewModel();
        public static CancellationTokenSource _canceller;
        public int pclold = 0;

        public MainWindow()
        {
            DataContext = View;
            InitializeComponent();
            Initializer.fullReset();
            UpdateFileRegisterUI();
            UpdateStackUI();
            UpdateSFR();
            UpdatePin();
            updateTime();
        }

        private void File_Open_Click(object sender, RoutedEventArgs e)
        {

            FileHandlingLocal.Filehandlingfunc();
            CommandDecoder.decodeCommands();
            programdata.ItemsSource = DataStorage.fileList;
            pclold = 0;
            Initializer.fullReset();
            completeUpdate();
        }

        private void step_button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Ein Schritt weiter!");
            CommandHandler.nextCommand();
            completeUpdate();
        }

        private async void start_stop_button_Checked(object sender, RoutedEventArgs e)
        {
            var test = 0;
            if (start_stop_button.Content.ToString() == "START")
            {
                start_stop_button.Content = "STOP";
            }
            _canceller = new CancellationTokenSource();
            await Task.Run(() =>
            {
                do
                {
                    Console.WriteLine("Ein Schritt weiter!");
                    CommandHandler.nextCommand();

                    if ((test % 4) == 0)
                    {
                        UpdatewithDispatcher();
                    }
                    test++;

                    Thread.Sleep(5);

                if (_canceller.Token.IsCancellationRequested)
                    {
                        
                        break;
                    }
                        

                } while (true);
            });
            updateActiveRow();
            completeUpdatewithoutRow();
        }


        private void UpdatewithDispatcher()
        {
            Dispatcher.Invoke(() =>
            {
                try
                {
                    completeUpdatewithoutRow();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }

            });
        }



        private void start_stop_button_Unchecked(object sender, RoutedEventArgs e)
        {
            if (start_stop_button.Content.ToString() == "STOP")
            {
                start_stop_button.Content = "START";
            }
            _canceller.Cancel();
        }

        private void UpdateFileRegisterUI()
        {
            string[,] data = new string[32, 8];

            int index = 0;
            for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    data[i, j] = DataStorage.regArray[index++].ToString("X2");
                }
            }
            View.FileRegisterData = data;
        }

        

        private void UpdateStackUI()
        {
            string[] data = new string[8];
            for (int i = 0; i < 8; i++)
            {
                data[i] = DataStorage.stack1.StackElementatIndex(i);
            }

            View.StackUI = data;
        }

        private void UpdateSFR()
        {
            string wreg;
            wreg = DataStorage.w_register.ToString("X2");
            View.WReg = wreg;

            string pcl;
            pcl = PCL.getPCL().ToString("X2");
            View.PCL = pcl;

            string pclath;
            pclath = PCL.getPCLLath().ToString("X2");
            View.PCLATH = pclath;

            string status;
            status = regArrayHandler.getRegArray(0x03).ToString("X2");
            View.STATUS = status;

            string fsr;
            fsr = regArrayHandler.getRegArray(0x04).ToString("X2");
            View.FSR = fsr;

            string option;
            option = regArrayHandler.getRegArray(0x81).ToString("X2");
            View.OPTION = option;

            char[] statusarray;
            statusarray = Convert.ToString(regArrayHandler.getRegArray(0x03), 2).PadLeft(8, '0').ToCharArray();
            View.StatusRegisterData = statusarray;

            char[] optionarray;
            optionarray = Convert.ToString(regArrayHandler.getRegArray(0x81), 2).PadLeft(8, '0').ToCharArray();
            View.OptionRegisterData = optionarray;

            char[] intconarray;
            intconarray = Convert.ToString(regArrayHandler.getRegArray(0x0B), 2).PadLeft(8, '0').ToCharArray();
            View.IntconRegisterData = intconarray;

            string timer;
            timer = regArrayHandler.getRegArray(0x01).ToString("X2");
            View.Timer = timer;

            string prescaler;
            prescaler = ("1:" + DataStorage.prescalerValue.ToString());
            View.Prescaler = prescaler;

            string pcintern;
            pcintern = DataStorage.programCounter.ToString("X4");
            View.Pcintern = pcintern;

            double time;
            time = (double) DataStorage.watchdogValue * 1000000;
            if (time <= 1000)
            {
                View.Watchdog = time.ToString("n0") + " µs";
            }
            else
            {
                time = time / 1000.0;
                View.Watchdog = time.ToString("n3") + " ms";
            }
        }

        private void UpdatePin()
        {
            var pinavalue = (int)regArrayHandler.getRegArray(0x05);
            var pinaarray = Int32Extensions.ToBooleanArray(pinavalue);
            View.Pina = pinaarray;

            var pinbvalue = (int)regArrayHandler.getRegArray(0x06);
            var pinbarray = Int32Extensions.ToBooleanArray(pinbvalue);
            View.Pinb = pinbarray;

            var trisavalue = (int)regArrayHandler.getRegArray(0x85);
            var trisaarray = Int32Extensions.ToBooleanArray(trisavalue);
            View.Trisa = trisaarray;

            var trisbvalue = (int)regArrayHandler.getRegArray(0x86);
            var trisbarray = Int32Extensions.ToBooleanArray(trisbvalue);
            View.Trisb = trisbarray;
        }

        private void FileRegister_CellEditEnding(object sender, System.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {
            var editingTextBox = e.EditingElement as TextBox;
            string newValue = editingTextBox.Text;

            if (e.EditAction == DataGridEditAction.Commit)
            {
                if (newValue.Length <= 2)
                {
                    try
                    {
                        byte b = (byte)int.Parse(newValue, System.Globalization.NumberStyles.HexNumber);
                        editingTextBox.Text = b.ToString("X2");
                        //DataStorage.regArray[e.Row.GetIndex() * 8 + e.Column.DisplayIndex] = b;
                        regArrayHandler.setRegArray((uint)(e.Row.GetIndex() * 8 + e.Column.DisplayIndex), b);
                        UpdateSFR();
                        UpdateFileRegisterUI();
                        UpdatePin();
                    }
                    catch
                    {
                        e.Cancel = true;
                        (sender as DataGrid).CancelEdit(DataGridEditingUnit.Cell);
                        MessageBox.Show("Invalid hexadecimal value", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    e.Cancel = true;
                    (sender as DataGrid).CancelEdit(DataGridEditingUnit.Cell);
                    MessageBox.Show("Only one hexadecimal byte allowed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void reset_button_clicked(object sender, RoutedEventArgs e)
        {
            DataStorage.fileList[DataStorage.commandLines[pclold + 1]].isActive = false;
            Initializer.fullReset();
            completeUpdate();
        
        }

        private void completeUpdate()
        {
            UpdateFileRegisterUI();
            UpdateStackUI();
            UpdateSFR();
            UpdatePin();
            updateTime();
            updateActiveRow();
        }

        private void completeUpdatewithoutRow()
        {
            UpdateFileRegisterUI();
            UpdateStackUI();
            UpdateSFR();
            UpdatePin();
            updateTime();
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var checkboxname = (sender as CheckBox).Name;
            var isChecked = (bool)((sender as CheckBox).IsChecked);


            switch (checkboxname)
            {
                case "Pina4":
                    if (isChecked)
                    {
                        regArrayHandler.setRegArray(0x05, regArrayHandler.getRegArray(0x05) | 0b00010000);
                    }
                    else
                    {
                        regArrayHandler.setRegArray(0x05, regArrayHandler.getRegArray(0x05) & 0b11101111);
                    }
                    break;
                case "Pina3":
                    if (isChecked)
                    {
                        regArrayHandler.setRegArray(0x05, regArrayHandler.getRegArray(0x05) | 0b00001000);
                    }
                    else
                    {
                        regArrayHandler.setRegArray(0x05, regArrayHandler.getRegArray(0x05) & 0b11110111);
                    }
                    break;
                case "Pina2":
                    if (isChecked)
                    {
                        regArrayHandler.setRegArray(0x05, regArrayHandler.getRegArray(0x05) | 0b00000100);
                    }
                    else
                    {
                        regArrayHandler.setRegArray(0x05, regArrayHandler.getRegArray(0x05) & 0b11111011);
                    }
                    break;
                case "Pina1":
                    if (isChecked)
                    {
                        regArrayHandler.setRegArray(0x05, regArrayHandler.getRegArray(0x05) | 0b00000010);
                    }
                    else
                    {
                        regArrayHandler.setRegArray(0x05, regArrayHandler.getRegArray(0x05) & 0b11111101);
                    }
                    break;
                case "Pina0":
                    if (isChecked)
                    {
                        regArrayHandler.setRegArray(0x05, regArrayHandler.getRegArray(0x05) | 0b00000001);
                    }
                    else
                    {
                        regArrayHandler.setRegArray(0x05, regArrayHandler.getRegArray(0x05) & 0b11111110);
                    }
                    break;

                case "Pinb7":
                    if (isChecked)
                    {
                        regArrayHandler.setRegArray(0x06, regArrayHandler.getRegArray(0x06) | 0b10000000);
                    }
                    else
                    {
                        regArrayHandler.setRegArray(0x06, regArrayHandler.getRegArray(0x06) & 0b01111111);
                    }
                    break;
                case "Pinb6":
                    if (isChecked)
                    {
                        regArrayHandler.setRegArray(0x06, regArrayHandler.getRegArray(0x06) | 0b01000000);
                    }
                    else
                    {
                        regArrayHandler.setRegArray(0x06, regArrayHandler.getRegArray(0x06) & 0b10111111);
                    }
                    break;
                case "Pinb5":
                    if (isChecked)
                    {
                        regArrayHandler.setRegArray(0x06, regArrayHandler.getRegArray(0x06) | 0b00100000);
                    }
                    else
                    {
                        regArrayHandler.setRegArray(0x06, regArrayHandler.getRegArray(0x06) & 0b11011111);
                    }
                    break;
                case "Pinb4":
                    if (isChecked)
                    {
                        regArrayHandler.setRegArray(0x06, regArrayHandler.getRegArray(0x06) | 0b00010000);
                    }
                    else
                    {
                        regArrayHandler.setRegArray(0x06, regArrayHandler.getRegArray(0x06) & 0b11101111);
                    }
                    break;
                case "Pinb3":
                    if (isChecked)
                    {
                        regArrayHandler.setRegArray(0x06, regArrayHandler.getRegArray(0x06) | 0b00001000);
                    }
                    else
                    {
                        regArrayHandler.setRegArray(0x06, regArrayHandler.getRegArray(0x06) & 0b11110111);
                    }
                    break;
                case "Pinb2":
                    if (isChecked)
                    {
                        regArrayHandler.setRegArray(0x06, regArrayHandler.getRegArray(0x06) | 0b00000100);
                    }
                    else
                    {
                        regArrayHandler.setRegArray(0x06, regArrayHandler.getRegArray(0x06) & 0b11111011);
                    }
                    break;
                case "Pinb1":
                    if (isChecked)
                    {
                        regArrayHandler.setRegArray(0x06, regArrayHandler.getRegArray(0x06) | 0b00000010);
                    }
                    else
                    {
                        regArrayHandler.setRegArray(0x06, regArrayHandler.getRegArray(0x06) & 0b11111101);
                    }
                    break;
                case "Pinb0":
                    if (isChecked)
                    {
                        regArrayHandler.setRegArray(0x06, regArrayHandler.getRegArray(0x06) | 0b00000001);
                    }
                    else
                    {
                        regArrayHandler.setRegArray(0x06, regArrayHandler.getRegArray(0x06) & 0b11111110);
                    }
                    break;


            }
            UpdateFileRegisterUI();
        }

        private void updateTime()
        {
            double timespent;
            timespent = (4.0 / ((double) DataStorage.quarzfreq)) * ((double) DataStorage.cycleCount) * 1000000;
            DataStorage.timeSpent = (4.0 / ((double)DataStorage.quarzfreq)) * ((double)DataStorage.cycleCount);
            if (timespent <= 1000)
            {
                View.Timespent = timespent.ToString("0.###") + " µs";
            }
            else
            {
                timespent = timespent / 1000.0;
                View.Timespent = timespent.ToString("0.###") + " ms";
            }

            
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var boxItem = (QuarzCombo.SelectedItem as ComboBoxItem).Name;
            
            switch (boxItem)
            {
                case "Item32":
                    DataStorage.quarzfreq = 32000;
                    break;
                case "Item500":
                    DataStorage.quarzfreq = 500000;
                    break;
                case "Item1":
                    DataStorage.quarzfreq = 1000000;
                    break;
                case "Item2":
                    DataStorage.quarzfreq = 2000000;
                    break;
                case "Item4":
                    DataStorage.quarzfreq = 4000000;
                    break;
                case "Item8":
                    DataStorage.quarzfreq = 8000000;
                    break;
                case "Item16":
                    DataStorage.quarzfreq = 16000000;
                    break;
            }
            updateTime();
            Console.WriteLine("Quarz set to: {0}", DataStorage.quarzfreq);
        }


        public static void breakpointOccured(string line)
        {

            if (_canceller != null)
            {
                _canceller.Cancel();
            }
            
            MessageBox.Show("Breakpoint ausgelöst bei PCL: " + line, "Error", MessageBoxButton.OK, MessageBoxImage.Error);


        }

        public static void WatchdogReset()
        {

            if (_canceller != null)
            {
                _canceller.Cancel();
            }
            MessageBox.Show("Watchdog Timer hat Reset ausgelöst!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);


        }

        private void updateActiveRow()
        {

            var pclnew = (int)PCL.getPCL();
            var currentlinenumber = DataStorage.commandLines[pclnew];
            
            //Markieren der nächsten Code-Zeile
            if (DataStorage.startCounter != 0)
            {
                //DataStorage.fileList[DataStorage.commandLines[ + 1]].isActive = false;
                DataStorage.fileList[DataStorage.commandLines[pclold]].isActive = false;
                DataStorage.fileList[DataStorage.commandLines[pclold + 1]].isActive = false;
                DataStorage.fileList[DataStorage.commandLines[pclnew + 1]].isActive = true;
            }
            else
            {
                DataStorage.fileList[DataStorage.commandLines[pclold]].isActive = false;
                DataStorage.fileList[DataStorage.commandLines[pclnew]].isActive = true;
            }
            

            pclold = pclnew;
            CollectionViewSource.GetDefaultView(DataStorage.fileList).Refresh();
            //Springen zur nächsten Code-Zeile
            if ((currentlinenumber + 7) < (programdata.Items.Count - 1))
            {
                programdata.ScrollIntoView(programdata.Items.GetItemAt(currentlinenumber + 7));
            }

        }

        private void watchdogOnClick(object sender, RoutedEventArgs e)
        {
            var isChecked = (bool)(sender as CheckBox).IsChecked;
            if (isChecked)
            {
                DataStorage.watchdogEnabled = true;
            }
            else
            {
                DataStorage.watchdogEnabled = false;
            }
        }
    }
}
