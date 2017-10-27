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
                server.Listen(100);
                Console.WriteLine(">>> Listening at IP:{0} , Port:{1}", host, port);
                Socket client = server.Accept();
                Console.WriteLine(">>> Client Accepted");
                FileStream file = File.Open(args[0], FileMode.Open, FileAccess.Read, FileShare.None);
                Console.WriteLine("{0}\n{1}\n{2}\n", args[0], args[1], args[2]);

                byte[] fileData = new byte[file.Length];
                file.Read(fileData, 0, fileData.Length);

                Int32 len = args[0].Length;
                byte[] fileNameLen = new byte[4];
                fileNameLen = BitConverter.GetBytes(len);

                byte[] fileNameInBytes = Encoding.ASCII.GetBytes(args[0]);

                byte[] finalMessage = new byte[4 + len + file.Length + 1];

                fileNameLen.CopyTo(finalMessage, 0);
                fileNameInBytes.CopyTo(finalMessage, 4);
                fileData.CopyTo(finalMessage, 4 + len - 1);

                Console.WriteLine(">>> Sending File: {0}", args[0]);

                client.Send(finalMessage);

                Console.WriteLine(">>> File Sent");
                    
                client.Shutdown(SocketShutdown.Both);
                client.Close();
                file.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}