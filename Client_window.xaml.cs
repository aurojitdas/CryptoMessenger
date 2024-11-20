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
    /// Interaction logic for Client_window.xaml
    /// </summary>
    public partial class Client_window : Window
    {
        client_Service client =null;
        public Client_window()
        {
            InitializeComponent();

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Client_Log.Text = "Trying to connect to server at "+ IP_addressBox.Text+"...";
            client = new client_Service(this);
            Task.Run(() => { 
                client.client_start();
                client.recieve_subroutine();
            });
            Task.Run(() => {               
                client.recieve_subroutine();
            });

        }

        public void UpdateClientLog(string message)
        {
            Dispatcher.Invoke(() =>
            {
                Client_Log.Text += message + "\n";
            });
        }

        public string getMessage()
        {
            string message=null;
            Dispatcher.Invoke(() =>
            {
                message = MessageBox.Text;
            });
            return message;
        }

        public string getIpAddress()
        {
            String ipaddr ="127.0.0.1";
            Dispatcher.Invoke(() =>
            {
                ipaddr= IP_addressBox.Text;
            });
            return ipaddr;
        }

        private void Send_button_Click(object sender, RoutedEventArgs e)
        {

            if (client == null)
            {
                UpdateClientLog("Connect to server first");
            }
            else {
                client.send_message();
            }
        }
    }
}
