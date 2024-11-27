using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows.Threading;
using test_wpf;

namespace chat_app
{
    internal class Server_service
    {
        bool isKeyExchanged = false;
        TcpListener server;
        KeyExchange_Service keys;

        public Server_window mWindow;
        public Server_service(Server_window window)
        {
            mWindow = window;
            keys = new KeyExchange_Service();
        }
        

        public void start_server()
        {
            try
            {
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localAddr, port);
              
                mWindow.UpdateServerLog("Waiting for a connection...");
               
                server.Start();
                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String? data;
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    mWindow.UpdateServerLog("Connected");
                    data = null;
                    // Get a stream object for reading and writing NetworkStream
                    NetworkStream stream = client.GetStream();
                    int i;
                    if (!isKeyExchanged)
                    {
                       
                        string publicKey = Convert.ToBase64String(keys.generatekeyPublickey());
                        publicKey = "Key_Start" + publicKey;
                        byte[] msg = Encoding.ASCII.GetBytes(publicKey);
                        stream.Write(msg, 0, msg.Length);
                        
                        mWindow.UpdateServerLog("keyStart: \n"+ publicKey + "\nKey end \n");

                    }

                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = Encoding.ASCII.GetString(bytes, 0, i);

                        if (data.StartsWith("CKey_Start", StringComparison.OrdinalIgnoreCase))
                        {

                            mWindow.UpdateServerLog("Received: " + data);
                            

                        }
                        else
                        {
                            mWindow.UpdateServerLog("Received: " + data);
                            // Console.WriteLine("Received: {0}", data);
                            data = data.ToUpper();
                            byte[] msg = Encoding.ASCII.GetBytes(data);
                            stream.Write(msg, 0, msg.Length);
                            mWindow.UpdateServerLog("Sent: " + data);
                            //Console.WriteLine("Sent: {0}", data);
                        }

                    }
                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (Exception e)
            {
                mWindow.UpdateServerLog("Exception: " + e.Message );
            }
            finally
            { // Stop listening for new clients
                server.Stop();
            }
            //Console.WriteLine("\nHit enter to continue...");
            Console.Read();

        }

    }
}
