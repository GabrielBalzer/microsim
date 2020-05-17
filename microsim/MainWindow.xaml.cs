using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        MainWindowViewModel View = new MainWindowViewModel();

        public MainWindow()
        {


            DataContext = View;
            InitializeComponent();
            Initializer.fullReset();
            UpdateFileRegisterUI();


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
            // to edit stack
            //Stack stack1 = new Stack();
            //stack1.SetValueToStck(2);
            //stack1.SetValueToStck(3);
            //stack1.SetValueToStck(4);
            //stack1.SetValueToStck(5);
            //stack1.SetValueToStck(6);
            //stack1.SetValueToStck(7);
            //stack1.SetValueToStck(8);
            //stack1.SetValueToStck(9);
            //stack1.SetValueToStck(11);
            //stack1.SetValueToStck(22);
            //Console.WriteLine("Ret val: " + stack1.GetValueFromStck());
            //Console.WriteLine("Ret val: " + stack1.GetValueFromStck());
            //Console.WriteLine("Ret val: " + stack1.GetValueFromStck());
            //Console.WriteLine("Ret val: " + stack1.GetValueFromStck());




        }

        private void step_button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Ein Schritt weiter!");
            CommandHandler.nextCommand();
            UpdateWregUI();
            UpdateFileRegisterUI();
            
        }

        private void start_stop_button_Checked(object sender, RoutedEventArgs e)
        {
            if(start_stop_button.Content.ToString() == "START")
            {
                start_stop_button.Content = "STOP";
            }
        }

        private void start_stop_button_Unchecked(object sender, RoutedEventArgs e)
        {
            if (start_stop_button.Content.ToString() == "STOP")
            {
                start_stop_button.Content = "START";
            }
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

        private void UpdateWregUI()
        {
            string wreg;
            wreg = DataStorage.w_register.ToString("X2");
            View.WReg = wreg;
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
                        UpdateFileRegisterUI();
                        MessageBox.Show(DataStorage.regArray[0].ToString());
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
            UpdateWregUI();
        }
    }
}
