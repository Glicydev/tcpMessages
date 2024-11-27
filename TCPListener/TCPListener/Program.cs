using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;

namespace TCPListener
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = null;


            // Initialisation
            byte[] clientIpBytes = new byte[256];
            int numberIpBytes = 0;
            int port = 13000;
            string clientIp = string.Empty;
            const string serverIp = "10.5.48.44";

            IPAddress localIp = IPAddress.Parse(serverIp);

            // Build the server
            server = new TcpListener(localIp, port);
            server.Start();

            TcpClient client = null;
            NetworkStream stream = null;
            Console.WriteLine("Server opened ! \n");

            // Do it for each client that connected
            while (true)
            {
                getClientIp(ref client, ref stream, ref server, ref numberIpBytes, ref clientIpBytes, ref clientIp);
            }
        }

        static void getClientIp(ref TcpClient client, ref NetworkStream stream, ref TcpListener server, ref int numberIpBytes, ref byte[] clientIpBytes, ref string clientIp)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Waiting for a client... \n");
                client = server.AcceptTcpClient();
                stream = client.GetStream();
                Console.WriteLine("New client found ! \n");

                // Transform command into bytes and puts it in the stream
                string command = "start cmd.exe";
                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(command);


                stream.Write(bytes, 0, bytes.Length);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Message sent: " + command);

                // Get IP and create file but only if an IP has been returned
                numberIpBytes = stream.Read(clientIpBytes, 0, clientIpBytes.Length);

                if (numberIpBytes != 0)
                {
                    clientIp = System.Text.Encoding.ASCII.GetString(clientIpBytes);

                    Console.WriteLine("Client IP: " + clientIp);

                    string fileName = "ip.txt";

                    if (!File.Exists(fileName))
                    {
                        File.Create(fileName).Close();
                    }
                    File.AppendAllText(fileName, clientIp + "\n");
                }
                else
                {
                Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No IP was found");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nError: " + ex.Message + "\n");
            }
        }
    }
}
