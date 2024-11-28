using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;

namespace TCPListener
{
    internal class Program
    {
        // Initialisations of color for all the programm
        const ConsoleColor errorColor = ConsoleColor.Red;
        const ConsoleColor succesColor = ConsoleColor.Green;
        const ConsoleColor infoColor = ConsoleColor.Blue;

        static void Main(string[] args)
        {
            // Initialisation
            int numberIpBytes = 0;

            // Get the server
            TcpListener server = createListener();

            if (server != null)
            {
                setColor(succesColor);
                Console.WriteLine("Server opened ! \n");

                // Do it for each client that connected
                while (true)
                {
                    handleCommunication(server, numberIpBytes);
                }
            }
            else
            {
                setColor(errorColor);
                Console.WriteLine("The server wasn't able to open...");
            }
        }

        /// <summary>
        /// Create a listener (It's the server which will listen for messages from clients)
        /// </summary>
        /// <returns>The TcpListener which is the server tgat started</returns>
        static TcpListener createListener()
        {
            // Initialisation
            const int port = 13000;
            const string serverIp = "127.0.0.1";

            IPAddress localIp = IPAddress.Parse(serverIp);
            // Build the server
            TcpListener server = new TcpListener(localIp, port);
            server.Start();

            return server;
        }

        /// <summary>
        /// Principal function which send an command to do to the client and collect back the client public IP
        /// </summary>
        /// <param name="server">The server</param>
        /// <param name="numberIpBytes">The </param>
        /// <param name="clientIpBytes"></param>
        /// <param name="clientIp"></param>
        private static void handleCommunication(TcpListener server, int numberIpBytes)
        {
            try
            {
                byte[] clientIpBytes = new byte[256];
                string clientIp = string.Empty;

                // Connexion part
                setColor(infoColor);
                Console.WriteLine("Waiting for a client... \n");

                TcpClient client = server.AcceptTcpClient();
                NetworkStream stream = client.GetStream();

                Console.WriteLine("New client found ! \n");

                // Transform command into bytes and puts it in the stream
                string command = "start cmd.exe";
                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(command);
                stream.Write(bytes, 0, bytes.Length);

                setColor(succesColor);
                Console.WriteLine("Message sent: " + command);

                // Get IP and create file but only if an IP has been returned
                numberIpBytes = stream.Read(clientIpBytes, 0, clientIpBytes.Length);
                clientIp = System.Text.Encoding.ASCII.GetString(clientIpBytes);

                // "Nothing" is sended if the client hasn't found any IP, so it's only letters and no numbers
                if (clientIp.All(char.IsLetter))
                {
                    createFile(clientIp);
                }
                else
                {
                    setColor(errorColor);
                    Console.WriteLine("\nNo IP was found...\n\n");
                }
            }
            catch (Exception ex)
            {
                setColor(errorColor);
                Console.WriteLine("\nError: " + ex.Message + "\n\n");
            }
        }

        private static void setColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }

        private static void createFile(string clientIp)
        {
            //Initialisation
            string fileName = "ip.txt";

            Console.WriteLine("Client IP: " + clientIp);

            if (!File.Exists(fileName))
            {
                File.Create(fileName).Close();
            }
            File.AppendAllText(fileName, clientIp + "\n");
        }
    }
}
