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

        public static void BroadcastScore()
        {
            StringBuilder playerBuilder = new StringBuilder();
            playerBuilder.Append("SCORE;");
            foreach (iSketch.Member member in iSketch.Menu.MemberList[iSketch.Menu.Host])
            {
                playerBuilder.Append(member.Username).Append("=").Append(member.Score).Append(";");
            }
            Console.WriteLine("SCORES: " + playerBuilder.ToString());
            foreach (iSketch.Member member in iSketch.Menu.MemberList[iSketch.Menu.Host])
            {
                if (member.Writer == null) continue;
                member.Writer.WriteLine(playerBuilder.ToString());
            }
        }

        public static void StartServer()
        {

            IPAddress adr = IPAddress.Loopback;
            IPEndPoint end = new IPEndPoint(adr, 4444);

            TcpClient client;
            Connection conn = null;
            ThreadStart ts;
            Thread t;
            Thread serverThread = new Thread(() =>
            {
                TcpListener server = new TcpListener(end);
                try
                {
                    server.Start();
                }
                catch (Exception)
                {
                    Console.WriteLine("A");
                }
            
                while (true)
                {
                    Console.WriteLine("ready to accept clients ...");
                    try
                    {
                        client = conn.server.AcceptTcpClient();
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