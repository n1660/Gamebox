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

            String line = "void";

            while (true)
            {
                line = reader.ReadLine();

                if (line == null)
                {
                    Server.online--;
                    Console.WriteLine((Server.online != 0) ? ("A Client just logged off.\nStill online: " + Server.online.ToString()) : "... this loneliness ... is killing me ... :'-(");
                    Server.request = true;
                    break;
                } else if (line.Trim().ToLower().Equals("bye"))
                {
                    break;
                }

                line = String.Format("Echo from Server: {0}", line);

                Console.WriteLine(line);
                writer.WriteLine(line);
            }

            server.Stop();
            reader.Close();
            writer.Close();
            stream.Close();
            client.Close();
        }
    }
}

