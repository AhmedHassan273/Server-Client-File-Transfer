using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

namespace networkingTask1
{
    class Server
    {
        static void Main(string[] args)
        {
            try
            {
                int port = 8000;
                IPAddress host = IPAddress.Parse("127.0.0.1");
                IPEndPoint ep = new IPEndPoint(host, port);
                Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Bind(ep);
                while (true)
                {
                    server.Listen(100);
                    Socket client = server.Accept();
                    FileStream file = File.Open(args[0], FileMode.Open, FileAccess.Read, FileShare.None);
                    Console.WriteLine("{0}\n{1}\n{2}\n", args[0], args[1], args[2]);
                    byte[] fileData = new byte[file.Length];
                    string message = "";
                    file.Read(fileData, 0, fileData.Length);
                    message = args[0].Length.ToString() + args[0];
                    byte[] messageHeader = Encoding.ASCII.GetBytes(message);
                    byte[] finalMessage = new byte[4 + args[0].Length + file.Length];
                    int currentIndex = 0;

                    for (int i = 0; i < messageHeader.Length; i++)
                    {
                        finalMessage[currentIndex++] = messageHeader[i];
                    }

                    for (int i = 0; i < fileData.Length; i++)
                    {
                        finalMessage[currentIndex++] = fileData[i];
                    }
                    client.Send(finalMessage);
                    Console.WriteLine(">>> File is sent successfully.");
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}