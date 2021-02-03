using encryption;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace server
{
    public class Program
    {
        public static Encryption enc = new Encryption();
        public static List<TcpClient> clients = new List<TcpClient>();
        static void Main(string[] args)
        {

            int port = 13356;
            IPAddress ip = IPAddress.Any;
            IPEndPoint localEndpoint = new IPEndPoint(ip, port);

            TcpListener listener = new TcpListener(localEndpoint);
            listener.Start();

            Console.WriteLine("venter på klient\'");
            AcceptClients(listener);
            byte key = 255;
            bool isRunning = true;
            while (isRunning)
            {
                Console.WriteLine("skiv en besked til klient");
                string message = Console.ReadLine();
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                
                foreach (TcpClient client in clients)
                {
                    client.GetStream().Write(enc.EncryptByte(buffer, key), 0, buffer.Length);
                }

                if (message.ToLower() == "c")
                {
                    Console.WriteLine("Conn closed");
                    break;
                }
            }
        }

        public static async void AcceptClients(TcpListener listener)
        {
            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                clients.Add(client);
                NetworkStream stream = client.GetStream();
                RecieveMessages(stream);
            }
        }

        public static async void RecieveMessages(NetworkStream stream)
        {
            byte[] buffer = new byte[256];
            byte key = 255;

            while (true)
            {
                int read = await stream.ReadAsync(buffer, 0, buffer.Length);



                string text = Encoding.UTF8.GetString(enc.DecryptByte(buffer, key), 0, read);
                Console.WriteLine($"Client writes: {text}");
                if (text.ToLower() == "c")
                {
                    break;
                }
            }
        }
    }
}
