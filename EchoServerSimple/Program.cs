using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace EchoServerSimple
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This is the server");

            TcpListener listener = new TcpListener(System.Net.IPAddress.Loopback, 7); //specifying IP Address and port number. 
            listener.Start(); // initializing TcpListerner

            Console.WriteLine("Server ready");
            while (true)
            {
                TcpClient socket = listener.AcceptTcpClient();
                Console.WriteLine("Incoming client");
                Task.Run(() => { HandleClient(socket);});
            }
        }

        public static void HandleClient(TcpClient socket)
        {
            NetworkStream ns = socket.GetStream(); //processes the data received and sent. Must be separated into reader & writer
            StreamReader reader = new StreamReader(ns);
            StreamWriter writer = new StreamWriter(ns);

            string message = reader.ReadLine(); //reads request from client. Excecution will wait here until receiving a line.
            Console.WriteLine("Message received:" + message);

            message = AlteredMessage(message);
            Console.WriteLine("Altered Message:" + message);

            writer.WriteLine(message);
            writer.Flush(); //Ensure message is sent to the client and clears buffer.
            socket.Close(); //Closes the socket. Disposes of reader and writer.

        }
        private static string AlteredMessage(string message)
        {
            if(message.ToLower().StartsWith("upper:"))
            {
                string sub = message.Substring(message.IndexOf(":") + 1, message.Length - (message.IndexOf(":")+1));
                return sub.ToUpper();

            }

            if(message.ToUpper().StartsWith("lower:"))
            {
                string sub = message.Substring(message.IndexOf(":") + 1, message.Length - (message.IndexOf(":")+1));
                return sub.ToLower();
            }
            return message;

        }
    }
}
