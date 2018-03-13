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
        public IPAddress Adr { get => adr; set => adr = value; }

        public String Hostname { get; set; }
        public TcpClient Client { get => client; set => client = value; }

        private TcpClient client = new TcpClient();
        private IPAddress adr = IPAddress.Loopback;
        private IPEndPoint end;

        public Member (string Username, bool host)
        {           
            this.Username = Username;
            this.Score = 0;
            this.Moves = 0; // neccessary??? 
            this.Guessed_Correctly = false;

            if (!host && Username != "")
            {
                Console.Write("Connecting...");
                this.end = new IPEndPoint(adr, 4444);
                this.client.Connect(end); // Will sich nicht connecten/ Host darf nicht connecten
                Console.WriteLine("Connected.");
                this.stream = client.GetStream();
                this.reader = new StreamReader(stream, Encoding.ASCII);
                this.writer = new StreamWriter(stream, Encoding.ASCII)
                {
                    AutoFlush = true
                };
                Console.WriteLine("Reading");
                this.ID = Int32.Parse(reader.ReadLine());
                Console.WriteLine("Got id " + ID);
                this.Hostname = this.SendLoginPacket();
                Console.WriteLine("got through, HOST: " + this.Hostname);
            }
            Console.WriteLine("done " + host + ", " + Username);
            //new Socket(SocketType.Stream, ProtocolType.Tcp).Bind(new IPEndPoint(IPAddress.Loopback, 4444));

            /*if (!(Menu.MemberList.ContainsKey(Username)))
            {
                Menu.MemberList.Add(Username, new List<Member>());
            }*/
        }

        public void Join_Game(IPEndPoint ip)
        {
           // show games, which are running -> select with Buttons (The Hosts Username)
            this.client.Connect(ip.Address, ip.Port);
            if (!(Menu.MemberList.ContainsKey(this.Username)))
            {
                Menu.MemberList.Add(this.Username, new List<Member>());
            }
            else
                return;
        }

        public String SendLoginPacket()
        {
            writer.WriteLine(this.ID.ToString() + ";LoginPacket;" + this.Username);
            String received = reader.ReadLine();
            Console.WriteLine("[CLIENT] LOGGED IN SUCCESSFULLY: '" + received + "'");
            return received.Split(';')[1];
        }
    }
    // Bei Add -> Daten müssen auch an den andern geschickt werden
}
