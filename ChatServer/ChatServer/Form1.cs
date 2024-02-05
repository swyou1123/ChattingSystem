using ChatLib.Models;
using System.DirectoryServices.ActiveDirectory;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ChatServer
{


    public partial class Form1 : Form
    {

        private TcpListener _listener;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8888);
            _listener.Start();

            while (true)
            {
                TcpClient client = await _listener.AcceptTcpClientAsync();

                _ = HandleClient(client);
            }

           
        }

        private async Task HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] sizeBuffer = new byte[4];
            int read;

            while (true)
            {
                read = await stream.ReadAsync(sizeBuffer, 0, sizeBuffer.Length);
                if (read == 0)
                    break;

                int size = BitConverter.ToInt32(sizeBuffer, 0);
                byte[] buffer = new byte[size];

                read = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (read == 0)
                    break;

                string message = Encoding.UTF8.GetString(buffer, 0, read);

                var hub = ChatHub.Parse(message);

                listBox1.Items.Add($"UserId : {hub.UserId}, RoomId : {hub.RoomId}, "+
                                   $"UserName : {hub.UserName}, Message : {hub.Message}");

                var messageBuffer = Encoding.UTF8.GetBytes($"Server : {message}");
                stream.Write(messageBuffer);
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
 