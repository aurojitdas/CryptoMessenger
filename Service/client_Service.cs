using chat_app;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace test_wpf
{
    internal class client_Service
    {
        Client_window client_Window;
        TcpClient client;
        NetworkStream stream;
        KeyExchange_Service keys;
        //buffer
        Byte[] bytes = new Byte[256];
        String data = null;
        public client_Service(Client_window client_Window)
        {
            this.client_Window = client_Window;
            keys = new KeyExchange_Service();
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
                if (stream != null)
                {
                    Int32 bytesRead = stream.Read(bytes, 0, bytes.Length);
                    data = Encoding.ASCII.GetString(bytes, 0, bytesRead);

                    if (data.StartsWith("Key_Start", StringComparison.OrdinalIgnoreCase))
                    {

                        String publicKeyServer = data.Replace("Key_Start", string.Empty);

                        client_Window.UpdateClientLog("Initiating Key exchange...");
                        client_Window.UpdateClientLog("Received: Public key of server..." );
                        string publicKey = Convert.ToBase64String(keys.generatekeyPublickey());
                        publicKey = "CKey_Start" + publicKey;
                        byte[] publickeyMsg = Encoding.ASCII.GetBytes(publicKey);
                        stream.Write(publickeyMsg, 0, publickeyMsg.Length);
                        client_Window.UpdateClientLog("Public key Sent...");
                        client_Window.UpdateClientLog("Generating Secret key for secure Communication...");
                        byte[] sharedScretKey =keys.generateSharedSecret(Encoding.ASCII.GetBytes(publicKeyServer));
                        client_Window.UpdateClientLog("Secret key Generated for secure Communication...");
                        client_Window.UpdateClientLog(Convert.ToBase64String(sharedScretKey));
                    }
                    else
                    {
                        //client_Window.UpdateClientLog("Received: " + data);
                    }

                    //Console.WriteLine("Received: {0}", data);
                    if (data.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }
                }



            }
        }

		public void sendFile()
		{

			try
			{
				byte[] startMarker = Encoding.ASCII.GetBytes("START_OF_FILE\n");
				stream.Write(startMarker, 0, startMarker.Length);
				FileStream fs = new FileStream("", FileMode.Open, FileAccess.Read);
				byte[] buffer = new byte[1024]; int bytesRead;
				while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
				{
					stream.Write(buffer, 0, bytesRead);
				}
				byte[] endMarker = Encoding.ASCII.GetBytes("END_OF_FILE\n");
				stream.Write(endMarker, 0, endMarker.Length);
				client_Window.UpdateClientLog("File sent!");
			}
			catch (FileNotFoundException ex)
			{
				client_Window.UpdateClientLog("Error: File not found. " + ex.Message);
			}
			catch (IOException ex)
			{
				client_Window.UpdateClientLog("Error: I/O error occurred. " + ex.Message);
			}
			catch (SocketException ex)
			{
				client_Window.UpdateClientLog("Error: Network error occurred. " + ex.Message);
			}
			catch (Exception ex)
			{
				client_Window.UpdateClientLog("An unexpected error occurred. " + ex.Message);

			}
		}

       
	}
}