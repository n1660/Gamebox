using Btn_iSketch;
using iSketch;
using iSketch.Connection;
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
                    String receivedLine = reader.ReadLine();
                    if (receivedLine == null)
                    {
                        Server.online--;
                        Console.WriteLine((Server.online != 0) ? ("A Client just logged off.\nStill online: " + Server.online.ToString()) : "... this loneliness ... is killing me ... :'-(");
                        break;
                    }
                    received = receivedLine.Split(';');
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
                    else if (received[0] == "CORRECTWORD")
                    {
                        Console.WriteLine("Correctword: " + received[1]);
                        String correctWord = received[1];
                        iSketch.App.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            Artist artist = (Artist)iSketch.App.Current.MainWindow.Content;
                            artist.correctWord = correctWord;
                        }));
                    }
                    else if (received.Length > 1) {
                        if (received[1] == "LINE")
                        {
                            Console.WriteLine("RECEIVED LINE " + receivedLine);
                            Server.BroadcastLine(receivedLine);
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
                        else if (received[1] == "START")
                        {
                            Console.WriteLine("Start: " + received[2]);
                            Server.BroadcastStart(receivedLine);                        
                        } else if (received[1] == "CHECKWORD")
                        {
                            Console.WriteLine("Checkword: " + received[2]);
                            iSketch.App.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                Artist artist = (Artist)iSketch.App.Current.MainWindow.Content;
                                artist.CheckInputWord(received[0], received[2]);
                            }));
                        }                        
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
                Console.WriteLine("Connection closed! ---> " + e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}

