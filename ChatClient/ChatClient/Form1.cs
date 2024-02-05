using ChatLib.Models;
using System.Drawing.Text;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class Form1 : Form
    {
        private TcpClient _client;
        private NetworkStream _stream;

        public Form1()
        {
            InitializeComponent();
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
             _client = new TcpClient();
            await _client.ConnectAsync(IPAddress.Parse("127.0.0.1"), 8888);
            _ = HandleClient(_client);
        }

        private async Task HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int read;

            while ((read = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, read);

                listBox1.Items.Add(message);

            }

        }

        private async void btnSend_Click_1(object sender, EventArgs e)
        {
            NetworkStream stream = _client.GetStream();

            string text = textBox1.Text;
            ChatHub hub = new ChatHub
            {
                UserId = 1,
                RoomId = 2,
                UserName = "���¿�",
                Message = text
            };
            var messageBuffer = Encoding.UTF8.GetBytes(hub.ToJsonString());

            var messageLangthBuffer = BitConverter.GetBytes(messageBuffer.Length);

            stream.Write(messageLangthBuffer);
            stream.Write(messageBuffer);

        }
    }
}



