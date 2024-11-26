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
                byte[] clientIp = new byte[256];
                int numberIpBytes = 0;
                int port = 13000;
                string ip = string.Empty;
                IPAddress localIp = IPAddress.Parse("192.168.1.172");

                server = new TcpListener(localIp, port);
                server.Start();

                TcpClient client = null;
                NetworkStream stream = null;


                while (true)
                {
                    client = server.AcceptTcpClient();
                    stream = client.GetStream();

                    byte[] bytes = System.Text.Encoding.ASCII.GetBytes("start cmd.exe");
                    stream.Write(bytes, 0, bytes.Length);
                    Console.WriteLine("Message sent !");

                    numberIpBytes = stream.Read(clientIp, 0, clientIp.Length);

                    if (numberIpBytes != 0)
                    {
                        ip = System.Text.Encoding.ASCII.GetString(clientIp);

                        Console.WriteLine("CLient IP: " + ip);

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
