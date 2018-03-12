﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Sockets;

namespace Quadcade
{
    public class Member //: IEquatable<Member>
    {
        public int ID { get; set; } // Für Sockets als int -> ID wird vom Socket zugewiesen!
        public string Username { get; set; }
        public int Score { get; set; }
        public int Moves { get; set; }
        public bool Guessed_Correctly { get; set; }

        public Stream Stream { get; set; }
        public StreamReader Reader { get; set; }
        public StreamWriter Writer { get; set; }
        public IPEndPoint End { get => end; set => end = value; }

        private TcpClient client;
        private IPAddress adr;
        private IPEndPoint end;

        public Member (string Username)
        {           
            this.Username = Username;
            this.Score = 0;
            this.Moves = 0; // neccessary??? 
            this.Guessed_Correctly = false;

            this.end = new IPEndPoint(adr, 10000);

            this.client = new TcpClient();
            this.client.Connect(end);

            this.Stream = client.GetStream();
            this.Reader = new StreamReader(Stream, Encoding.ASCII);
            this.Writer = new StreamWriter(Stream, Encoding.ASCII)
            {
                AutoFlush = true
            };
        }

        public void Join_Game(IPEndPoint ip)
        {
            // show games, which are running -> select with Buttons (The Hosts Username)
            this.client.Connect(ip.Address, ip.Port);
            this.ID = Int32.Parse(Reader.ReadLine());
        }
    }
    // Bei Add -> Daten müssen auch an den andern geschickt werden
}