using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientWPF
{
    /// <summary>
    /// Interaktionslogik für ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        private IPAddress adr;
        private IPEndPoint end;

        private TcpClient client;

        private NetworkStream stream;
        private StreamReader reader;
        private StreamWriter writer;

        // c'tor
        public ClientWindow()
        {
            this.adr = IPAddress.Loopback;
            this.end = new IPEndPoint(adr, 4444);

            this.client = new TcpClient();
            this.client.Connect(end);

            this.stream = client.GetStream();
            this.reader = new StreamReader(stream, Encoding.ASCII);
            this.writer = new StreamWriter(stream, Encoding.ASCII)
            {
                AutoFlush = true
            };

            this.Closed += ClientWindow_Closed;
        }

        private void ClientWindow_Closed(object sender, EventArgs e)
        {
            reader.Close();
            writer.Close();
            stream.Close();
            client.Close();
        }

        public void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            if (TxtSend.Text == "" || TxtSend.Text == null)
                return;

            String line = TxtSend.Text;
            TxtBlReceive.Text += "\nClient: " + line.ToLower() + "?";
            String response = TxtBlReceive.Text += " --> Server: " + line.ToUpper() + "!";
            TxtSend.Text = "";
        }
    }
}
