using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class Connection
    {
        private TcpClient client;
        private TcpListener server;

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
                String[] line = { "void" };

                while (true)
                {
                    line = reader.ReadLine().Split(';');

                    if (line == null)
                    {
                        Server.online--;
                        Console.WriteLine((Server.online != 0) ? ("A Client just logged off.\nStill online: " + Server.online.ToString()) : "... this loneliness ... is killing me ... :'-(");
                        break;
                    }
                    foreach (String str in line)
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
                    if (line[1] == "LoginPacket")
                    {
                        Console.WriteLine("User logged in: " + line[2]);
                        iSketch.Member newMember = new iSketch.Member(line[2], true); // Set host=true because it should not connect
                        Console.WriteLine("B");
                        newMember.writer = writer;
                        Console.WriteLine("C");
                        iSketch.Menu.MemberList[iSketch.Menu.Host].Add(newMember);
                        Console.WriteLine("D");
                            
                        writer.WriteLine(line[0] + ';' + iSketch.Menu.Host);
                        Console.WriteLine("E");
                        Server.BroadcastScore();
                    }
                    if (line.Length == 1 && line[0].Trim().ToLower().Equals("bye"))
                    {
                        break;
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
                Console.WriteLine(e);
            }
        }
    }
}

