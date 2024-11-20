using chat_app;
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
using System.Windows.Shapes;

namespace test_wpf
{
    /// <summary>
    /// Interaction logic for Server_window.xaml
    /// </summary>
    public partial class Server_window : Window
    {
        public Server_window()
        {
            InitializeComponent();
            
        }

        private void Connect_button_Click(object sender, RoutedEventArgs e)
        {
            Server_log.Text += "\"Waiting for a test..." + "\n";
            Server_service server = new Server_service(this);

            Task.Run(() => { server.start_server(); });
            //server.start_server1();
        }

        public void UpdateServerLog(string message)
        { // Ensure the update occurs on the UI thread
            Dispatcher.Invoke(() =>
            {
                Server_log.Text += message + "\n";
            });
        }
    }
}

