using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LiFiReceiver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SerialPort receiverIn = new SerialPort();
        private string filepath = "";
        private FileStream fs;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            DialogResult result = saveFileDialog1.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                filepath = saveFileDialog1.FileName;
            }

            filePathtoSave_textBox.Text = filepath;
        }

        private void baudRateTextBox_KeyPress(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!Char.IsDigit((char)KeyInterop.VirtualKeyFromKey(e.Key)) & e.Key != Key.Back | e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void dataTextBox_KeyPress(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!Char.IsDigit((char)KeyInterop.VirtualKeyFromKey(e.Key)) & e.Key != Key.Back | e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void stopBitsTextBox_KeyPress(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!Char.IsDigit((char)KeyInterop.VirtualKeyFromKey(e.Key)) & e.Key != Key.Back | e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void SerialPort_Button_Click(object sender, RoutedEventArgs e)
        {

            receiverIn.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            StopBits sb = new StopBits();


            if (receiverIn.IsOpen)
            {
                receiverIn.Close();
                fs.Close();
                ConnectionStatus_textBlock.Text = "UART Connection is CLOSED";
            }
            else
            {
                if (Int32.Parse(stopBitsTextBox.Text) == 2)
                {
                    sb = StopBits.Two;
                }
                else if (float.Parse(stopBitsTextBox.Text) == 1.5)
                {
                    sb = StopBits.OnePointFive;
                }
                else
                {
                    sb = StopBits.One;
                }
                //receiverIn = new SerialPort(serialTextBox.Text, Int32.Parse(baudRateTextBox.Text), Parity.None, Int32.Parse(dataTextBox.Text), sb)

                receiverIn.PortName = serialTextBox.Text;
                receiverIn.ReadBufferSize = 1048576;
                receiverIn.BaudRate = Int32.Parse(baudRateTextBox.Text);
                receiverIn.Parity = Parity.None;
                receiverIn.DataBits = Int32.Parse(dataTextBox.Text);
                receiverIn.StopBits = sb;

                if (File.Exists(@filepath))
                    File.Delete(@filepath);

                fs = new FileStream(filepath, FileMode.CreateNew, FileAccess.Write);

                receiverIn.Open();
                ConnectionStatus_textBlock.Text = "UART Connection is OPEN";
                fs.Close();
            }

        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            fs = new FileStream(filepath, FileMode.Append, FileAccess.Write);
            int availBuff = receiverIn.BytesToRead;
            byte t;
            for (int i = 0; i < availBuff; i++)
            {
                t = (byte)receiverIn.ReadByte();
                fs.WriteByte(t);
                this.Dispatcher.Invoke(() =>
                { 
                    incomingData_textBox.Text = incomingData_textBox.Text + (char)t + " ";
                });
            }
            fs.Close();
        }

        private void filePathtoSave_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            filepath = filePathtoSave_textBox.Text;
        }
    }
}
