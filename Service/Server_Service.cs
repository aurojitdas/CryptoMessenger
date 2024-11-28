using System;
using System.Collections.Generic;
using System.IO;
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
        NetworkStream stream;
        
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
                
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    mWindow.UpdateServerLog("Connected");
                    
                    // Get a stream object for reading and writing NetworkStream
                    stream = client.GetStream();
                    int i;
                    if (!isKeyExchanged)
                    {
                       
                        string publicKey = Convert.ToBase64String(keys.generatekeyPublickey());
                        mWindow.UpdateServerLog("Initiating Key exchange...");
                        publicKey = "Key_Start" + publicKey;
                        byte[] msg = Encoding.ASCII.GetBytes(publicKey);
                        stream.Write(msg, 0, msg.Length);
                        mWindow.UpdateServerLog("Public key Sent...");

                    }
                    readNetworkStream_subroutine();

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
          

        }

        public void readNetworkStream_subroutine()
        {
            String? data;
            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            int i;

            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                data = Encoding.ASCII.GetString(bytes, 0, i);

                if (data.StartsWith("CKey_Start", StringComparison.OrdinalIgnoreCase))
                {

                    //mWindow.UpdateServerLog("Received: " + data);
                    mWindow.UpdateServerLog("Public key Recieved...");
                    String publicKeyClient = data.Replace("CKey_Start", string.Empty);
                    mWindow.UpdateServerLog("Generating Secret key for secure Communication...");
                    byte [] sharedSecretKey = keys.generateSharedSecret(Encoding.ASCII.GetBytes(publicKeyClient));
                    mWindow.UpdateServerLog("Secret key Generated for secure Communication...");
                    mWindow.UpdateServerLog(Convert.ToBase64String(sharedSecretKey));
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
        }

    }
}
