using System;
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
                int port = 13000;
                IPAddress localIp = IPAddress.Parse("192.168.1.172");

                server = new TcpListener(localIp, port);
                server.Start();

                TcpClient client = server.AcceptTcpClient();

                NetworkStream stream = client.GetStream();

                byte[] bytes = System.Text.Encoding.ASCII.GetBytes("start cmd.exe");
                stream.Write(bytes, 0, bytes.Length);
                Console.WriteLine("Sent !");
                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
