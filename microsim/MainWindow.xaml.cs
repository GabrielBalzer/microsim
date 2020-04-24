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

namespace microsim
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FileHandling FileHandlingLocal = new FileHandling();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void File_Open_Click(object sender, RoutedEventArgs e)
        {
            FileHandlingLocal.Filehandlingfunc();
        }
    }
}
