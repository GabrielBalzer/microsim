using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        private CancellationTokenSource _canceller;

        public MainWindow()
        {
            DataContext = View;
            InitializeComponent();
            Initializer.fullReset();
            completeUpdate();
        }

        private void File_Open_Click(object sender, RoutedEventArgs e)
        {

            FileHandlingLocal.Filehandlingfunc();
            CommandDecoder.decodeCommands();
            foreach(uint item in DataStorage.regArray)
            {
                Console.WriteLine(item);
            }
            programdata.ItemsSource = DataStorage.fileList;
        }

        private void step_button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Ein Schritt weiter!");
            CommandHandler.nextCommand();
            UpdateSFR();
            UpdateFileRegisterUI();
            UpdateStackUI();
            UpdatePin();
        }

        private async void start_stop_button_Checked(object sender, RoutedEventArgs e)
        {
            if(start_stop_button.Content.ToString() == "START")
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
                    UpdateSFR();
                    UpdateFileRegisterUI();
                    UpdateStackUI();

                    Thread.Sleep(1000);

                    if (_canceller.Token.IsCancellationRequested)
                        break;

                } while (true);
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
                        DataStorage.regArray[e.Row.GetIndex() * 8 + e.Column.DisplayIndex] = b;
                        completeUpdate();
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
            Initializer.fullReset();
            UpdateFileRegisterUI();
            UpdateSFR();
            UpdateStackUI();
        }

        private void completeUpdate()
        {
            UpdateFileRegisterUI();
            UpdateStackUI();
            UpdateSFR();
            UpdatePin();
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
            completeUpdate();
        }
    }
}
