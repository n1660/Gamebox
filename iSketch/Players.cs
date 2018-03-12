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
        public static Menu instance;

        public int ID { get; set; } // Für Sockets als int -> ID wird vom Socket zugewiesen!
        public string Username { get; set; }
        public int Score { get; set; }
        public int Moves { get; set; }
        public bool Guessed_Correctly { get; set; }

        public Stream stream { get; set; }
        public StreamReader reader { get; set; }
        public StreamWriter writer { get; set; }
        public IPEndPoint End { get => end; set => end = value; }

        private TcpClient client = new TcpClient();
        private IPAddress adr = IPAddress.Loopback;
        private IPEndPoint end;

        public Member (string Username, bool host)
        {           
            this.Username = Username;
            this.Score = 0;
            this.Moves = 0; // neccessary??? 
            this.Guessed_Correctly = false;

            this.end = new IPEndPoint(adr, 4444);

            if (!host && Username != "")
            {
                this.client.Connect(end); // Will sich nicht connecten/ Host darf nicht connecten 
                this.stream = client.GetStream();
                this.reader = new StreamReader(stream, Encoding.ASCII);
                this.writer = new StreamWriter(stream, Encoding.ASCII)
                {
                    AutoFlush = true
                };

                if (!(Menu.MemberList.ContainsKey(Username)))
                {
                    Menu.MemberList.Add(Username, new List<Member>());
                    instance.get_player_data();
                }
            }


            this.client = new TcpClient();
        }

        public void Join_Game(IPEndPoint ip)
        {
           // show games, which are running -> select with Buttons (The Hosts Username)
           this.client.Connect(ip.Address, ip.Port);


            this.ID = Int32.Parse(reader.ReadLine());
        }
    }
    // Bei Add -> Daten müssen auch an den andern geschickt werden
}
