using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tcpservertest
{
    public partial class Form1 : Form
    {
        // Bağlantıları kontrol etmek için listeye aldık
        static List<NetworkStream> clients = new List<NetworkStream>();

        public Form1()
        {
            InitializeComponent();
            startServer();
        }

        public void startServer()
        {
            TcpListener server = new TcpListener(IPAddress.Any, 9999);
            TcpClient client;
            server.Start();


            while (true)  
            {
                try
                {
                    // Client oturumu bekliyoruz
                    client = server.AcceptTcpClient();

                    // Birden fazla oturum dinleyebilmek için thread ile kullanıyoruz
                    ThreadPool.QueueUserWorkItem(ThreadProc, client);
                }
                catch { }
            }
        }

        private void ThreadProc(object obj)
        {
            try
            {
                // Gelen mesajları işlemek için varsayılan değerleri oluşturuyoruz
                Byte[] bytes = new Byte[256];
                String data = null;
                // Oturumu çekiyoruz
                var client = (TcpClient)obj;

                // Oturumdan gelen mesajı okuyoruz
                NetworkStream ns = client.GetStream();
                byte[] msg2 = System.Text.Encoding.ASCII.GetBytes("10");

                // Clientımızı havumuza ekliyoruz
                clients.Add(ns);

                // Client'a mesaj gönderiyoruz
                ns.Write(msg2, 0, msg2.Length);

                // Verileri okuyoruz
                int i;
                while ((i = ns.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    MessageBox.Show(data);
                }
            }
            catch { }
        }

    }
}
