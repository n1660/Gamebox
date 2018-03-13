using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Server
{
    public class Connection
    {
        private TcpClient client;
        public TcpListener server;
        public static Canvas PAINTINGCANV;

        public Connection(TcpClient client, TcpListener server)
        {
            this.client = client;
            this.server = server;
        }

        public void ServeSingleClient()
        {
            try
            {
                NetworkStream stream = client.GetStream();

                StreamReader reader = new StreamReader(stream, Encoding.ASCII);
                StreamWriter writer = new StreamWriter(stream, Encoding.ASCII)
                {
                    AutoFlush = true
                };

                writer.WriteLine(iSketch.Menu.MemberList.Count);
                String[] received = { "void" };

                while (true)
                {
                    if (received == null)
                    {
                        Server.online--;
                        Console.WriteLine((Server.online != 0) ? ("A Client just logged off.\nStill online: " + Server.online.ToString()) : "... this loneliness ... is killing me ... :'-(");
                        break;
                    }
                    received = reader.ReadLine().Split(';');
                    foreach (String str in received)
                    {
                        Console.WriteLine("SERVER received " + str);
                    }
                    /*foreach (KeyValuePair<String, List<iSketch.Member>> kvp in iSketch.Menu.MemberList)
                    {
                        foreach (iSketch.Member m in kvp.Value)
                        {
                            if (m.Adr.ToString() == reader.ReadLine().Split(';')[2])
                                writer.WriteLine(reader.ReadLine().Split(';')[0] + ';' + m.Username);
                        }
                    }*/
                    if (received.Length == 1 && received[0].Trim().ToLower().Equals("bye"))
                    {
                        break;
                    }
                    else if (received[0] == "SCORE")
                    {
                        String[] score = new String[2];
                        foreach(String str in received)
                        {
                            if (str == received[0])
                                continue;

                            score = str.Split('=');
                            iSketch.Menu.MemberList[iSketch.Menu.Host][(iSketch.Member.GetMmbByName(iSketch.Menu.Host, score[0])).ID].Score = Int32.Parse(score[1]);
                        }
                        writer.Write('\n');
                    }
                    else if (received[1] == "Line" && received[0] == iSketch.Member.GetMmbByName(iSketch.Menu.Host, iSketch.Artist.curArtist).ID.ToString())
                    {
                        Point start = new Point
                        {
                            X = double.Parse(received[2]),
                            Y = double.Parse(received[3])
                        };
                        Point end = new Point {
                            X = double.Parse(received[4]),
                            Y = double.Parse(received[5])
                        };

                        Line line = new Line
                        {
                            StrokeStartLineCap = PenLineCap.Round,
                            StrokeEndLineCap = PenLineCap.Round,
                            X1 = start.X,
                            Y1 = start.Y,
                            X2 = end.X,
                            Y2 = end.Y,
                            Stroke = (SolidColorBrush)(new BrushConverter()).ConvertFromString(received[6]),
                            StrokeThickness = double.Parse(received[7]),
                        };
                        PAINTINGCANV.Children.Add(line);
                    }
                    else if (received[1] == "LoginPacket")
                    {
                        Console.WriteLine("User logged in: " + received[2]);
                        iSketch.Member newMember = new iSketch.Member(received[2], true)
                        {
                            Writer = writer
                        }; // Set host=true because it should not connect
                        iSketch.Menu.MemberList[iSketch.Menu.Host].Add(newMember);
                            
                        writer.WriteLine(received[0] + ';' + iSketch.Menu.Host);
                        Server.BroadcastScore();
                    }
                }

                server.Stop();
                reader.Close();
                writer.Close();
                stream.Close();
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Connection closed!");
                Console.WriteLine(e.Message);
            }
        }
    }
}

