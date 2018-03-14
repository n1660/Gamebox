using iSketch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace Server
{
    public class Server
    {
        private static Connection conn = null;

        public static int online = 0;

        public static Connection Conn { get => conn; set => conn = value; }

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

            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {           
                Artist artist = (Artist)App.Current.MainWindow.Content;
                iSketch.Connection.PacketUtil.HandlePacket(artist, playerBuilder.ToString(), Menu.member.Writer);
            }));
        }

        public static void BroadcastLine(String packet)
        {
            foreach (iSketch.Member member in iSketch.Menu.MemberList[iSketch.Menu.Host])
            {
                if (member.Writer == null) continue;
                member.Writer.WriteLine(packet);
            }
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Artist artist = (Artist)App.Current.MainWindow.Content;
                Console.WriteLine("artist: " + artist.ToString() + "\npacket: " + packet + "\nwriter: " + Menu.member.Writer);
                iSketch.Connection.PacketUtil.HandlePacket(artist, packet, Menu.member.Writer);
            }));
        }

        public static void BroadcastStart(String packet)
        {
            foreach (iSketch.Member member in iSketch.Menu.MemberList[iSketch.Menu.Host])
            {
                if (member.Writer == null) continue;
                member.Writer.WriteLine(packet);
            }
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Artist artist = (Artist)App.Current.MainWindow.Content;
                iSketch.Connection.PacketUtil.HandlePacket(artist, packet, Menu.member.Writer);
            }));
        }

        public static void BroadcastClear()
        {
            foreach (iSketch.Member member in iSketch.Menu.MemberList[iSketch.Menu.Host])
            {
                if (member.Writer == null) continue;
                member.Writer.WriteLine("CLEAR");
            }
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Artist artist = (Artist)App.Current.MainWindow.Content;
                iSketch.Connection.PacketUtil.HandlePacket(artist, "CLEAR", Menu.member.Writer);
            }));
        }

        public static void SendPacket(String packet)
        {
            if (Menu.member.IsHost)
            {
                foreach (iSketch.Member member in iSketch.Menu.MemberList[iSketch.Menu.Host])
                {
                    if (member.Writer == null) continue;
                    member.Writer.WriteLine(packet);
                }
            } else
            {
                Menu.member.Writer.WriteLine(packet);
            }
        }

        public static void SendPacket(String packet, String username)
        {
            if (Menu.member.IsHost)
            {
                foreach (iSketch.Member member in iSketch.Menu.MemberList[iSketch.Menu.Host])
                {
                    if (member.Writer == null) continue;
                    Console.WriteLine("###### Sending packet: " + member.Username + " == " + username + " ?");
                    if (member.Username == username)
                    {
                        member.Writer.WriteLine(packet);
                    }
                }
            }
        }

        public static void StartServer()
        {
            TcpClient client;
            ThreadStart ts;
            Thread t;
            Thread serverThread = new Thread(() =>
            {
                IPAddress adr = IPAddress.Parse("192.169.43.253");
                IPEndPoint end = new IPEndPoint(IPAddress.Any, 4444);

                TcpListener server = new TcpListener(end);
                try
                {
                    server.Start();
                   
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                }
            
                while (true)
                {
                    try
                    {
                        Console.WriteLine("Ready to accept client...");
                        client = server.AcceptTcpClient();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: " + e.Message);
                        Console.WriteLine(e.StackTrace);
                        continue;
                    }
                    Console.WriteLine(++online + " clients logged in");
                    Server.conn = new Connection(client, server);
                    
                    ts = new ThreadStart(Server.conn.ServeSingleClient);
                    t = new Thread(ts);
                    t.Start();
                }
            });

            serverThread.Start();
        }
    }
}