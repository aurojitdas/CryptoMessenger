using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace test_wpf
{
    internal class client_Service
    {
        Client_window client_Window;
        TcpClient client;
        NetworkStream stream;
        //buffer
        Byte[] bytes = new Byte[256];
        String data = null;
        public client_Service(Client_window client_Window)
        {
            this.client_Window = client_Window;
        }
        public void client_start()
        {
            try
            {
                Int32 port = 13000;
                client = new TcpClient(client_Window.getIpAddress(), port);
                stream = client.GetStream();
                client_Window.UpdateClientLog("Connected to : " + client_Window.getIpAddress());
                //stream.Close();
                //client.Close();
            }
            catch (Exception ex)
            {
                client_Window.UpdateClientLog("Exception: " + ex.Message);

            }
        }

        public void send_message()
        {
            try
            {
                data = client_Window.getMessage();
                byte[] msg = Encoding.ASCII.GetBytes(data);
                // Send the message to the connected TcpServer
                stream.Write(msg, 0, msg.Length);

            }
            catch (Exception ex)
            {

            }
        }
        public void recieve_subroutine()
        {
            while (true)
            {
                if (stream!=null)
                {
                    Int32 bytesRead = stream.Read(bytes, 0, bytes.Length);
                    data = Encoding.ASCII.GetString(bytes, 0, bytesRead);
                    client_Window.UpdateClientLog("Received: " + data);
                    //Console.WriteLine("Received: {0}", data);
                    if (data.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }
                }

                

            }
        }
    }
}
