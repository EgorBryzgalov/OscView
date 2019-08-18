using System;
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
using COMTRADEParser;
using System.IO;
using Microsoft.Win32;

namespace OscView
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ConfigReader configReader { get; set; }
        private DataReader dataReader { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Файл конфигурации (*.cfg)|*.cfg|All files (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.ShowDialog();
            configReader = ConfigReader.GetInstance(dialog.FileName);
            dataReader = new DataReader(configReader);
            dataReader.GetData();
                      
        }
    }
}
