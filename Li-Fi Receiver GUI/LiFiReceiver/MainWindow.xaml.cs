using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Timers;
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
        private string bufferedOutput = "";
        private FileStream fs;
        private System.Timers.Timer receiveTimer;
        private Boolean Status = false;
        //private byte formatType = 0;

        public MainWindow()
        {
            InitializeComponent();
            receiveTimer = new System.Timers.Timer(333);
            receiveTimer.AutoReset = true;
            receiveTimer.Enabled = true;
            receiveTimer.Elapsed += OnTimedEvent;

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
                receiverIn.ReadBufferSize = 1048576; //Buffer Size
                receiverIn.BaudRate = Int32.Parse(baudRateTextBox.Text);
                receiverIn.Parity = Parity.None;
                receiverIn.DataBits = Int32.Parse(dataTextBox.Text);
                receiverIn.StopBits = sb;

                if (File.Exists(@filepath))
                    File.Delete(@filepath);

                fs = new FileStream(filepath, FileMode.CreateNew, FileAccess.Write);

                receiverIn.Open();
                ConnectionStatus_textBlock.Text = "UART Connection is OPEN";
                //fs.Close();
            }
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            //fs = new FileStream(filepath, FileMode.Append, FileAccess.Write); //Creates file stream
            int availBuff = receiverIn.BytesToRead; //check available buffer
            byte t;
            for (int i = 0; i < availBuff; i++)
            {
                t = (byte)receiverIn.ReadByte();
                fs.WriteByte(t); //Writes char to a file
                if(bufferedOutput.Length < 500)
                    bufferedOutput += (char)t;
                /*
                switch (formatType)
                {
                    case 0:
                        bufferedOutput += (char)t;
                        break;
                    case 1:
                        bufferedOutput += t;
                        break;
                    case 2:
                        bufferedOutput += "0x" + t.ToString("X2") + " ";
                        break;
                    default:
                        bufferedOutput += (char)t;
                        break;
                }
                */
                if (!Status)
                {
                    Status = true;
                }
            }
            fs.Flush(true);
            //fs.Close(); //Closes file stream
        }

        private void filePathtoSave_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            filepath = filePathtoSave_textBox.Text;
        }

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (Status)
            {
                this.Dispatcher.Invoke(() =>
                {
                    statusIndicator.Fill = new SolidColorBrush(System.Windows.Media.Colors.Lime);
                    incomingData_textBox.Text = bufferedOutput;
                });
                bufferedOutput = "";
                Status = false;
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    statusIndicator.Fill = new SolidColorBrush(System.Windows.Media.Colors.Gray);
                });
            }
        }

        private void clearConsole_Button_Click(object sender, RoutedEventArgs e)
        {
            incomingData_textBox.Text = "";
        }

        /*
        private void toggleOutputDataFormat_Button_Click(object sender, RoutedEventArgs e)
        {
            switch (formatType)
            {
                case 0:
                    formatType = 1;
                    outputFormat_textBlock.Text = "Output data will be displayed in DECIMAL";
                    break;
                case 1:
                    formatType = 2;
                    outputFormat_textBlock.Text = "Output data will be displayed in HEX";
                    break;
                case 2:
                    formatType = 0;
                    outputFormat_textBlock.Text = "Output data will be displayed in ASCII";
                    break;
                default:
                    formatType = 0;
                    outputFormat_textBlock.Text = "Output data will be displayed in ASCII";
                    break;
            }
        }
        */
    }
}
