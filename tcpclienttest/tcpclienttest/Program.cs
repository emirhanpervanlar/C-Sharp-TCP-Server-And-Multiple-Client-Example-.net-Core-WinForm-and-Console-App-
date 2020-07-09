using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace tcpclienttest
{
    class Program
    {
        static void Main(string[] args)
        {
            connectTCP();
        }

        static void connectTCP()
        {
            // Client oluşturuyoruz
            using var client = new TcpClient();
            client.Connect("127.0.0.1", 9999);

            using NetworkStream networkStream = client.GetStream();

            // Gelen mesajı okuyoruz
            using var reader = new StreamReader(networkStream, Encoding.UTF8);
            Byte[] bytes = new Byte[256];
            String data = null;
            try
            {
                int i;
                while ((i = networkStream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                    // Gelen dataya göre karşılaştırıp geriye cevap basıyoruz
                    if(data == "10")
                    {
                        byte[] sendMsg = System.Text.Encoding.ASCII.GetBytes("false");
                        networkStream.Write(sendMsg);
                    }
                    Console.WriteLine(data);
                }
                Console.WriteLine(reader.ReadToEnd());
            }
            catch { }

            //byte[] bytes = Encoding.UTF8.GetBytes(message);
            //networkStream.Write(bytes, 0, bytes.Length);

        }
    }
}
