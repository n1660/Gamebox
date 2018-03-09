using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Sockets;

namespace iSketch
{
    public class Member //: IEquatable<Member>
    {
        public int ID { get; set; } // Für Sockets als int -> ID wird vom Socket zugewiesen!
        public string Username { get; set; }
        public int Score { get; set; }
        public int Moves { get; set; }
        public bool Guessed_Correctly { get; set; }
        public Stream stream { get; set; }
        public StreamReader reader { get; set; }
        public StreamWriter writer { get; set; }
        private TcpClient client;
        private IPAddress adr;
        private IPEndPoint end;


        public Member (int ID, string Username)
        {
            this.ID = ID;
            this.Username = Username;
            this.Score = 0;
            this.Moves = 0; // neccessary??? 
            this.Guessed_Correctly = false;

            this.end = new IPEndPoint(adr, 10000);

            this.client = new TcpClient();
            this.client.Connect(end);

            this.stream = client.GetStream();
            this.reader = new StreamReader(stream, Encoding.ASCII);
            this.writer = new StreamWriter(stream, Encoding.ASCII)
            {
                AutoFlush = true
            };
        }
    }
    // Bei Add -> Daten müssen auch an den andern geschickt werden
}
