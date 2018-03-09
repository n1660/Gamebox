﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace Server0123
{
    class Server
    {
        public static int online = 0;
        public static bool request ;
        public static void Main()
        {
            IPAddress adr = IPAddress.Loopback;
            IPEndPoint end = new IPEndPoint(adr, 10000);

            TcpClient client;
            Connection conn;
            ThreadStart ts;
            Thread t;

            TcpListener server = new TcpListener(end);
            server.Start();

            while (true)
            {
                if (request)
                {
                    Console.WriteLine("ready to accept clients ...");
                    request = false;
                }
                try
                {
                    client = server.AcceptTcpClient();
                }
                catch
                {
                    continue;
                }
                Console.WriteLine(++online + " clients logged in");
                request = true;
                conn = new Connection(client, server);
                ts = new ThreadStart(conn.ServeSingleClient);
                t = new Thread(ts);
                t.Start();
            }
        }
    }
}