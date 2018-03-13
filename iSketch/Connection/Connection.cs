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
            NetworkStream stream = client.GetStream();

            StreamReader reader = new StreamReader(stream, Encoding.ASCII);
            StreamWriter writer = new StreamWriter(stream, Encoding.ASCII)
            {
                AutoFlush = true
            };

            writer.WriteLine(iSketch.Menu.MemberList.Count);
            String[] line = { "void" };

            writer.WriteLine(iSketch.Artist.CURPLAYERS + 1);

            while (true)
            {
                line = reader.ReadLine().Split(';');

                if (line == null)
                {
                    Server.online--;
                    Console.WriteLine((Server.online != 0) ? ("A Client just logged off.\nStill online: " + Server.online.ToString()) : "... this loneliness ... is killing me ... :'-(");
                    break;
                }
                foreach (KeyValuePair<String, List<iSketch.Member>> kvp in iSketch.Menu.MemberList)
                {
                    foreach (iSketch.Member m in kvp.Value)
                    {
                        if (m.Adr.ToString() == reader.ReadLine().Split(';')[2])
                            writer.WriteLine(reader.ReadLine().Split(';')[0] + ';' + m.Username);
                    }
                }
                writer.WriteLine(line[0] + ';' + iSketch.Menu.MemberList[iSketch.Menu.Host][0].Get_Host_Username(line[2]));
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
    }
}

