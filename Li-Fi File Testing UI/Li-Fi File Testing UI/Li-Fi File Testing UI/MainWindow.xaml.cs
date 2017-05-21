using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using System.IO;

namespace Li_Fi_File_Testing_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private StopBits stopBits = StopBits.One;
        private SerialPort transmitOut;
        private string filepath = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                filepath = openFileDialog1.FileName;
            }
            filePathTextBox.Text = filepath;
        }

        private void StopBitsBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as System.Windows.Controls.ComboBox;

            string value = comboBox.SelectedItem as string;

            if (value.Equals("1"))
                stopBits = StopBits.One;
            else if (value.Equals("1.5"))
                stopBits = StopBits.OnePointFive;
            else
                stopBits = StopBits.Two;
        }

        private void StopBitsBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> data = new List<string>();
            data.Add("1");
            data.Add("1.5");
            data.Add("2");

            var comboBox = sender as System.Windows.Controls.ComboBox;

            comboBox.ItemsSource = data;
            comboBox.SelectedIndex = 0;
        }

        private void transmitButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void baudRateTextBox_KeyPress(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key < Key.D0 || e.Key > Key.D9)
                if (e.Key < Key.NumPad0 || e.Key > Key.NumPad9)
                    if (e.Key != Key.Back)
                        e.Handled = true;                     
        }

        private void dataTextBox_KeyPress(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key < Key.D0 || e.Key > Key.D9)
                if (e.Key < Key.NumPad0 || e.Key > Key.NumPad9)
                    if (e.Key != Key.Back)
                        e.Handled = true;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void filePathTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            filepath = filePathTextBox.Text;
        }
    }
}
