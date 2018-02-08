using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCP_IP_Connection
{
    class Client
    {
        public void StartClient()
        {
            IPEndPoint localhorst = new IPEndPoint(Dns.GetHostEntry(Dns.GetHostName()).AddressList[0], 11000);
            Socket client = new Socket(SocketType.Stream, ProtocolType.Tcp);
            client.Connect(localhorst);
        }
    }
}
