using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using ShareLibrary;

namespace Client
{
    /// <summary>
    /// Interaction logic for winFiles.xaml
    /// </summary>
    public partial class winFiles : Window
    {
        public winFiles()
        {
            InitializeComponent();

        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                string content = File.ReadAllText(filename);
                ShareFile share = new ShareFile(this.ToByte(content),filename,int.Parse(txtSharesTotal.Text),int.Parse(txtRequired.Text),int.Parse(txtReturn.Text));
                Controller.getController().service.UploadFile(share, Controller.getController().user, Controller.getController().getIPAddress());
            }

        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void refreshList()
        {
            //List<ShareFile> files = Controller.getController().service

        }
        private byte[] ToByte(string content)
        {
            byte[] byteArray = new byte[content.Length * sizeof(char)];
            System.Buffer.BlockCopy(content.ToCharArray(), 0, byteArray, 0, byteArray.Length);
            return byteArray;
        }

        private string toString(byte[] byteArray)
        {
            char[] chars = new char[byteArray.Length / sizeof(char)];
            System.Buffer.BlockCopy(byteArray, 0, chars, 0, byteArray.Length);
            return new string(chars);
        }
    }
}
