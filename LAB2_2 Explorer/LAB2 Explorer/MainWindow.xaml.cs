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
using System.IO;
using System.Windows.Input;

namespace LAB2_Explorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DriveInfo[] Drives = DriveInfo.GetDrives();
       
        DriveInfo CurrentDrive;
        DirectoryInfo CurrentDirectory;
        FileInfo CurrentFile;

        public MainWindow()
        {
            InitializeComponent();

            ListBoxDirectory.Items.Add(Drives);

            TextBoxAddressBar.Text = "This PC";
        }

        private void TextBoxAddressBar_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    CurrentDirectory = new DirectoryInfo(TextBoxAddressBar.Text);
                    ListBoxDirectory.Items.Clear();
                    ListBoxDirectory.Items.Add(CurrentDirectory.GetDirectories());
                    ListBoxDirectory.Items.Add(CurrentDirectory.GetFiles());

                    TextBoxAddressBar.Text = CurrentDirectory.FullName.ToString();
                }
            }
            catch
            {
                MessageBox.Show("Invalid Directory!");
            }
        }

        private void LeftUpButton_Click(object sender, EventArgs e)
        {
            try //Fuck C# for making me do this.
            {
                CurrentDirectory = CurrentDirectory.Parent;
                TextBoxAddressBar.Text = CurrentDirectory.FullName.ToString();

                ListBoxDirectory.Items.Clear();
                ListBoxDirectory.Items.Add(CurrentDirectory.GetDirectories());
                ListBoxDirectory.Items.Add(CurrentDirectory.GetFiles());
            }
            catch
            {
                TextBoxAddressBar.Text = "This PC";
                ListBoxDirectory.Items.Clear();
                ListBoxDirectory.Items.Add(Drives);
            }
        }

        private void LeftListBox_DoubleClick(object sender, EventArgs e)
        {
            if (ListBoxDirectory.SelectedItem.GetType().ToString() == "System.IO.DriveInfo")
            {
                CurrentDrive = (DriveInfo)ListBoxDirectory.SelectedItem;

                if (CurrentDrive.IsReady)
                {
                    TextBoxAddressBar.Text = CurrentDrive.RootDirectory.ToString();

                    ListBoxDirectory.Items.Clear();
                    ListBoxDirectory.Items.Add(CurrentDrive.RootDirectory.GetDirectories());
                    ListBoxDirectory.Items.Add(CurrentDrive.RootDirectory.GetFiles());
                }
            }
            else if (ListBoxDirectory.SelectedItem.GetType().ToString() == "System.IO.DirectoryInfo")
            {
                CurrentDirectory = (DirectoryInfo)ListBoxDirectory.SelectedItem;
                TextBoxAddressBar.Text = CurrentDirectory.FullName.ToString();

                ListBoxDirectory.Items.Clear();
                ListBoxDirectory.Items.Add(CurrentDirectory.GetDirectories());
                ListBoxDirectory.Items.Add(CurrentDirectory.GetFiles());
            }
            else
            {
                MessageBox.Show("Selection is a file!");
            }
        }

        
    }
}
