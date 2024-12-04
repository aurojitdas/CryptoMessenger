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
        AES_Service aes;
        byte[] IV;
        bool IV_Generated = false;
        bool sharedKeyGenerated = false;
        byte[] sharedSecretKey;
        String data = null;

        public Server_window server_window;
        public Server_service(Server_window window)
        {
            server_window = window;
            keys = new KeyExchange_Service();
        }
        

        public void start_server()
        {
            try
            {
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localAddr, port);
                server_window.UpdateServerLog("Waiting for a connection...");               
                server.Start();            
                
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    server_window.UpdateServerLog("Connected");
                    
                    // Get a stream object for reading and writing NetworkStream
                    stream = client.GetStream();
                    int i;
                    if (!isKeyExchanged)
                    {
                        //initiating key exchange using DH
                        server_window.UpdateServerLog("Initiating Key exchange...");
                        string publicKey = Convert.ToBase64String(keys.generatekeyPublickey());                       
                        publicKey = "Key_Start" + publicKey;
                        byte[] msg = Encoding.ASCII.GetBytes(publicKey);
                        stream.Write(msg, 0, msg.Length);
                        server_window.UpdateServerLog("Public key Sent...");

                        //Generating and sending IV for use with AES
                        server_window.UpdateServerLog("Initiating IV exchange...");
                        aes = new AES_Service();
                        IV = aes.generateIV();
                        string iv = Convert.ToBase64String(IV);
                        iv =  "IV_Start" + iv;
                        msg = Encoding.ASCII.GetBytes(iv);
                        stream.Write(msg, 0, msg.Length);
                        server_window.UpdateServerLog("IV Sent...");
                        IV_Generated = true;

                    }
                    //Recive subroutine
                    readNetworkStream_subroutine();

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (Exception e)
            {
                server_window.UpdateServerLog("Exception: " + e.Message );
            }
            finally
            { 
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
                    server_window.UpdateServerLog("Public key Recieved...");
                    String publicKeyClient = data.Replace("CKey_Start", string.Empty);
                    server_window.UpdateServerLog("Generating Secret key for secure Communication...");
                    sharedSecretKey = keys.generateSharedSecret(Encoding.ASCII.GetBytes(publicKeyClient));
                    server_window.UpdateServerLog("Secret key Generated for secure Communication...");
                    sharedKeyGenerated = true;
                    server_window.UpdateServerLog(">>>>>>>>>>Secure Channel Established<<<<<<<<<<");
                }
                else
                {
                    string base64Message = Convert.ToBase64String(bytes, 0, i);
                    server_window.UpdateServerLog("Received Encrypted Message: " + base64Message);
                    byte[] recievedBytes = Convert.FromBase64String(base64Message);

                    if (sharedKeyGenerated&&IV_Generated)
                    {
                        String decryptedMessage =aes.decrypt(recievedBytes, sharedSecretKey,IV);
                        server_window.UpdateServerLog("Decrypted Message: " + decryptedMessage);
                        /*stream.Write(recievedBytes, 0, recievedBytes.Length);
                        mWindow.UpdateServerLog("Sent: " + base64Message);*/
                    }
                   
                   
                   
                }

            }
        }

        public void send_message()
        {
            try
            {
                if (IV_Generated && sharedKeyGenerated)
                {
                    if (aes == null)
                    {
                        aes = new AES_Service();

                    }

                    data = server_window.getMessage();
                    server_window.UpdateServerLog("Plain Text: " + data);

                    byte[] enc_msg = aes.encrypt(data, sharedSecretKey, IV);
                    server_window.UpdateServerLog("Encrypted Text: " + Convert.ToBase64String(enc_msg));
                    // Send the message to the connected TcpServer
                    stream.Write(enc_msg, 0, enc_msg.Length);
                }
                else
                {
                    data = "Channel is not secure please wait...";
                    byte[] msg = Encoding.ASCII.GetBytes(data);
                    // Send the message to the connected TcpServer
                    stream.Write(msg, 0, msg.Length);
                }


            }
            catch (Exception ex)
            {

            }
        }

    }
}
