﻿using System;
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
        Initializer Initializer = new Initializer();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void File_Open_Click(object sender, RoutedEventArgs e)
        {
            FileHandlingLocal.Filehandlingfunc();
            CommandDecoder.decodeCommands();
            Initializer.initRegArray();
            foreach(uint item in DataStorage.regArray)
            {
                Console.WriteLine(item);
            }

            // to edit stack
            //Stack stack1 = new Stack();
            //stack1.SetValueToStck(2);
            //stack1.SetValueToStck(3);
            //stack1.SetValueToStck(4);
            //Console.WriteLine(stack1.GetValueFromStck());
        }
    }
}
