using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCP_IP_Connection
{
    class Server
    {
        public Server()
        {
            int received;
            IPEndPoint localhorst = new IPEndPoint(Dns.GetHostEntry(Dns.GetHostName()).AddressList[0], 11000);
            Socket server = new Socket(SocketType.Stream, ProtocolType.Tcp);
            server.Bind(localhorst);
            server.Listen(10);
            Socket client;
            String receive = "recieve";
            byte[] data = new byte[1024];
            while (true)
            {
                client = server.Accept();
                while (true)
                {
                    data = new byte[1024];
                    received = client.Receive(data);
                    receive += Encoding.ASCII.GetString(data, 0, received);
                    if (receive.IndexOf("<EOF>") > -1)
                    {
                        break;
                    }
                }
                receive = "receive";
            }
        }
    }
}
