using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace test_wpf
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
       
        private void Server_Button_Click(object sender, RoutedEventArgs e)
        {
            Server_window server_Window = new Server_window();
            server_Window.Show();
            //this.Hide();
        }

        private void Client_Button_Click(object sender, RoutedEventArgs e)
        {
           Client_window client_Window = new Client_window();
            client_Window.Show();
            //this.Hide();
        }
    }
}