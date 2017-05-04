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
using System.IO.Ports;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LiFiTransmitApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private String us_const = "We the People of the United States, in Order to form a more perfect Union," +
            " establish Justice, insure domestic Tranquility, provide for the common defense," +
            " promote the general Welfare, and secure the Blessings of Liberty to ourselves and" +
            " our Posterity, do ordain and establish this Constitution for the United States of America." +
            "All legislative Powers herein granted shall be vested in a Congress of the United States," +
            " which shall consist of a Senate and House of Representatives." +
            "The House of Representatives shall be composed of Members chosen" +
            " every second Year by the People of the several States, and the Electors" +
            " in each State shall have the Qualifications requisite for Electors of the" +
            " most numerous Branch of the State Legislature." + "No Person shall be a Representative" +
            " who shall not have attained to the Age of twenty five Years, and been seven Years a Citizen" +
            " of the United States, and who shall not, when elected, be an Inhabitant of that State in which" +
            " he shall be chosen. Representatives and direct Taxes shall be apportioned among the several States" +
            " which may be included within this Union, according to their respective Numbers, which shall be determined" +
            " by adding to the whole Number of free Persons, including those bound to Service for a Term of Years, and" +
            " excluding Indians not taxed, three fifths of all other Persons. The actual Enumeration shall be made" +
            " within three Years after the first Meeting of the Congress of the United States, and within every"
            + " subsequent Term of ten Years, in such Manner as they shall by Law direct. The Number of" +
            " Representatives shall not exceed one for every thirty Thousand, but each State shall have at" +
            " Least one Representative; and until such enumeration shall be made, the State of New Hampshire" +
            " shall be entitled to choose three, Massachusetts eight, Rhode-Island and Providence Plantations" +
            " one, Connecticut five, New-York six, New Jersey four, Pennsylvania eight, Delaware one, Maryland" +
            " six, Virginia ten, North Carolina five, South Carolina five, and Georgia three.";
        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            string filepath = "";
            DialogResult result = openFileDialog1.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                filepath = openFileDialog1.FileName;
            }
            filePathTextBox.Text = filepath;
        }

        private void baudRateTextBox_KeyPress(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(!Char.IsDigit((char)KeyInterop.VirtualKeyFromKey(e.Key)) & e.Key != Key.Back | e.Key == Key.Space){
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

        private void transmitButton_Click(object sender, RoutedEventArgs e)
        {

            FileStream selFile = new FileStream(filePathTextBox.Text, FileMode.Open, FileAccess.Read);
            StopBits sb = new StopBits();

            if(Int32.Parse(stopBitsTextBox.Text) == 2)
            {
                sb = StopBits.Two;
            }
            else if(float.Parse(stopBitsTextBox.Text) == 1.5)
            {
                sb = StopBits.OnePointFive;
            }
            else
            {
                sb = StopBits.One;
            }

            SerialPort transmitOut = new SerialPort(serialTextBox.Text, Int32.Parse(baudRateTextBox.Text), Parity.None, Int32.Parse(dataTextBox.Text), sb);
            transmitOut.Open();

            while(selFile.Position != selFile.Length)
            {
                byte[] buff = new byte[1];
                buff[0] = (byte)selFile.ReadByte();
                transmitOut.Write(buff, 0, buff.Length);
            }

            transmitOut.Close();
            selFile.Close();

            /*
            string strCmdText;
            strCmdText = String.Format("/K plink.exe -serial {0} -sercfg {1},{2},{3},{4},{5} < {6}", serialTextBox.Text,
                baudRateTextBox.Text, dataTextBox.Text, 'n', stopBitsTextBox.Text, 'X', filePathTextBox.Text);
            System.Diagnostics.Process.Start("CMD.exe", strCmdText);
            Console.Write(strCmdText);
            */
        }

        private void testTransmit_Button_Click(object sender, RoutedEventArgs e)
        {
            SerialPort transmitOut = null;
            StopBits sb = new StopBits();

            if (Int32.Parse(stopBitsTextBox.Text) == 2)
            {
                sb = StopBits.Two;
            }
            else if (float.Parse(stopBitsTextBox.Text) == 1.5)
            {
                sb = StopBits.OnePointFive;
            }
            else
            {;
                sb = StopBits.One;
            }


            transmitOut = new SerialPort(serialTextBox.Text, Int32.Parse(baudRateTextBox.Text), Parity.None, Int32.Parse(dataTextBox.Text), sb);
            transmitOut.Open();
            transmitOut.Write(us_const);
            transmitOut.Close();
        }
    }
}
