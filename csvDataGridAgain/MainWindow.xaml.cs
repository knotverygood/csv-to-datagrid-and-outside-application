using System;
using System.Runtime.InteropServices;
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
using System.ComponentModel;
using System.Data;
using System.IO;
using LumenWorks.Framework.IO.Csv;
using LumenWorks.Framework.Tests.Unit.IO.Csv;
using Microsoft.Win32;



//using Microsoft.VisualBasic.FileIO;

namespace csvGridDataParse
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<System.Windows.Controls.CheckBox> checkBoxList = new List<System.Windows.Controls.CheckBox>();
        public System.Windows.Controls.CheckBox chk;
        Boolean DataLoaded = false;

        //Import the FindWindow API to find our window
        [DllImportAttribute("User32.dll")]
        private static extern IntPtr FindWindow(String ClassName, String WindowName);
        [DllImportAttribute("User32.dll")]
        public static extern IntPtr GetDlgItem(IntPtr hDlg, int nIDDlgItem);
        [DllImportAttribute("User32.dll")]
        public static extern IntPtr SendMessage(HandleRef hWnd, uint Msg, IntPtr wParam, string lParam);
        public const uint WM_SETTEXT = 0x000C;
        //public const int textbox_controlid = 0x11B6E;
        public const int textbox_controlid = 0x0004;
        public const int textbox_controlid2 = 0x0005;


        public MainWindow()
        {
            InitializeComponent();

        }

       private void selectFileButton_Click(object send, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = @"D:";
            openFileDialog1.Filter = "CSV files (*.csv)|*.CSV";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.ShowDialog();

            textBox1.Text = openFileDialog1.FileName;
        }



        private void loadFileButton_Click(object sender, EventArgs e)
        {
            if (DataLoaded == false)
            {
                //string filePath = @"D:";
                //textBox1.Text = filePath;
                CheckBoxes1.Children.Clear();
                DataTable csvTable = new DataTable();
                using (CsvReader csv = new CsvReader(new StreamReader(textBox1.Text), true))
                {
                    csvTable.Load(csv);

                    for (int i = 0; i <= csvTable.Columns.Count - 1; i++)
                    {
                        csvTable.Columns[i].ColumnName = csvTable.Columns[i].ColumnName.ToString().Replace("_", " ");
                    }

                    csvGrid.ItemsSource = csvTable.DefaultView;
                    Console.WriteLine(csv);
                }

                //int y = 443;
                string fileRow;
                string[] fileDataField;
                int count = 0;

                if (System.IO.File.Exists(textBox1.Text))
                {
                    System.IO.StreamReader fileReader = new StreamReader(textBox1.Text);

                    if (fileReader.Peek() != -1)
                    {
                        //Reading Header information
                        //int w = 100;

                        fileRow = fileReader.ReadLine();
                        fileRow = System.Text.RegularExpressions.Regex.Replace(fileRow, "_", " ");
                        fileDataField = fileRow.Split(',');
                        //Console.WriteLine(fileRow);
                        count = fileDataField.Count();
                        count = count - 1;

                        // checkBoxList.Clear();
                        for (int i = 0; i <= count; i++)
                        {
                            CheckBox chk = new CheckBox();
                            //chk.Name = fileDataField[i];
                            chk.Tag = fileDataField[i];
                            chk.Content = fileDataField[i];
                            CheckBoxes1.Children.Add(chk);
                            //Console.WriteLine(fileDataField[1]);
                            //Console.WriteLine(fileDataField[1]);
                        }
                    }
                }
            }

        }

        private void sendDataButton_Click(object sender, EventArgs e)
        {
            //Find the window, using the CORRECT Window Title
            //IntPtr hWnd = FindWindow(null, "AppB");
            //var textBox1 = "texb1";
            var textBox1 = new[] { "hello", "number1", "world" }; // int[] 
            IntPtr hWnd = FindWindow("AutoIt v3 GUI", "HPOV");
            if (hWnd != IntPtr.Zero) //If found
            {
                Console.WriteLine("found AppB");
                //IntPtr htextbox = GetDlgItem(hWnd, 110); //110 is control id of textbox in AppB gotten using Spy++
                InteropSetText(hWnd, textbox_controlid, textBox1[2].ToString());
                InteropSetText(hWnd, textbox_controlid2, "textbox2");
            }
            else //Not Found
            {
                MessageBox.Show("Error: The sample naming window is NOT open.");
            }
        }

        private void InteropSetText(IntPtr iptrHWndDialog, int iControlID, string strTextToSet)
        {
            IntPtr iptrHWndControl = GetDlgItem(iptrHWndDialog, iControlID);
            HandleRef hrefHWndTarget = new HandleRef(null, iptrHWndControl);
            SendMessage(hrefHWndTarget, WM_SETTEXT, IntPtr.Zero, strTextToSet);
        }

    }
}

