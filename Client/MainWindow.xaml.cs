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

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            txtUsername.Text = Controller.getController().getIPAddress();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = this.txtUsername.Text;
            string password = this.pwdPassword.Password;
            Controller.getController().user = Controller.getController().service.Login(Controller.getController().getIPAddress(),username, password);
            if(Controller.getController().user !=null)
            {
                winFiles wf = new winFiles();
                wf.Show();
                this.Close();
            } 
            else
            {
                MessageBox.Show("Error Loggin in");
            }
            
        }


    }
}
