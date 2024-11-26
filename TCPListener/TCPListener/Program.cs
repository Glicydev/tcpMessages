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
            byte[] clientIp = new byte[256];
            int numberIpBytes = 0;
            int port = 13000;
            string ip = string.Empty;
            IPAddress localIp = IPAddress.Parse("192.168.1.172");

            // Build the server
            server = new TcpListener(localIp, port);
            server.Start();

            TcpClient client = null;
            NetworkStream stream = null;

            // Do it for each client that connected
            while (true)
            {
                getClientIp(ref client, ref stream, ref server, ref numberIpBytes, ref clientIp, ref ip);
            }
        }

        static void getClientIp(ref TcpClient client, ref NetworkStream stream, ref TcpListener server, ref int numberIpBytes, ref byte[] clientIp, ref string ip)
        {
            try
            {
                client = server.AcceptTcpClient();
                stream = client.GetStream();
                Console.WriteLine("New client found ! \n");

                // Transform command into bytes and puts it in the stream
                string command = "start cmd.exe";
                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(command);


                stream.Write(bytes, 0, bytes.Length);
                Console.WriteLine("Message sent !");

                // Get IP and create file but only if an IP has been returned
                numberIpBytes = stream.Read(clientIp, 0, clientIp.Length);

                if (numberIpBytes != 0)
                {
                    ip = System.Text.Encoding.ASCII.GetString(clientIp);

                    Console.WriteLine("Client IP: " + ip);

                    string fileName = "ip.txt";

                    File.Delete(fileName);
                    File.Create(fileName).Close();
                    File.AppendAllText(fileName, ip + "\n");
                }
                else
                {
                    Console.WriteLine("No IP was found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
