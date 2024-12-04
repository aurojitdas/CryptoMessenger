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
        Server_service? server = null;
        public Server_window()
        {
            InitializeComponent();
            
        }

        private void Connect_button_Click(object sender, RoutedEventArgs e)
        {
          
            server = new Server_service(this);

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

        private void Send_button_Click(object sender, RoutedEventArgs e)
        {
            if (server == null)
            {
                UpdateServerLog("Connect to server first");
            }
            else
            {
                server.send_message();
            }
        }

        public string getMessage()
        {
            string? message = null;
            Dispatcher.Invoke(() =>
            {
                message = ServermessageTextBox.Text;
            });
            return message;
        }

        private void Server_log_TextChanged(object sender, TextChangedEventArgs e)
        {
            Server_log.ScrollToEnd();
        }
    }
}

