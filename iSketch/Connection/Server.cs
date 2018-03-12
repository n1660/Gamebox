using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace Server
{
    public class Server
    {
        public static int online = 0;
        public static void M_Server()
        {
            IPAddress adr = IPAddress.Loopback;
            IPEndPoint end = new IPEndPoint(adr, 4444);

            TcpClient client;
            Connection conn;
            ThreadStart ts;
            Thread t;
            Thread serverThread = new Thread(() =>
            {

                TcpListener server = new TcpListener(end);
                server.Start();

                while (true)
                {
                    Console.WriteLine("ready to accept clients ...");
                    try
                    {
                        client = server.AcceptTcpClient();
                    }
                    catch
                    {
                        continue;
                    }
                    Console.WriteLine(++online + " clients logged in");
                    conn = new Connection(client, server);
                    ts = new ThreadStart(conn.ServeSingleClient);
                    t = new Thread(ts);
                    t.Start();
                }
            });

            serverThread.Start();
        }
    }
}