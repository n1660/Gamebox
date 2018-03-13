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
<<<<<<< HEAD
                this.Hostname = this.Get_Host_Username((new IPEndPoint(IPAddress.Loopback, 4444).ToString()));
                this.client.Connect(end); // Will sich nicht connecten/ Host darf nicht connecten 
=======
                this.client.Connect(end);
>>>>>>> 5ad7361bdeb468e081f1f9ae4255027309630a8d
                this.stream = client.GetStream();
                Console.WriteLine("got through");
                this.reader = new StreamReader(stream, Encoding.ASCII);
                this.writer = new StreamWriter(stream, Encoding.ASCII)
                {
                    AutoFlush = true
                };
<<<<<<< HEAD
            }
            else if(host)
            {
                ((Menu)App.Current.MainWindow.Content).New_Host();
                this.reader = new StreamReader(new TcpClient(this.Username, 4444).GetStream() , Encoding.ASCII);
                this.writer = new StreamWriter(new TcpClient(this.Username, 4444).GetStream(), Encoding.ASCII)
                {
                    AutoFlush = true
                };
            }
            else
                return;
            //new Socket(SocketType.Stream, ProtocolType.Tcp).Bind(new IPEndPoint(IPAddress.Loopback, 4444));
=======
<<<<<<< HEAD
=======

                if (!(Menu.MemberList.ContainsKey(Username)))
                {
                    Menu.MemberList.Add(Username, new List<Member>());
                    instance.get_player_data();
                }
            }
>>>>>>> a0cb6389c73084a88d0649301f30db3dce43fd32

                if (!(Menu.MemberList.ContainsKey(Username)))
                {
                    Menu.MemberList.Add(Username, new List<Member>());
                }
            }

            this.client = new TcpClient();
>>>>>>> 5ad7361bdeb468e081f1f9ae4255027309630a8d
        }

        public void Join_Game(IPEndPoint ip)
        {
           // show games, which are running -> select with Buttons (The Hosts Username)
<<<<<<< HEAD
            this.client.Connect(ip.Address, ip.Port);
            if (!(Menu.MemberList.ContainsKey(this.Username)))
            {
                Menu.MemberList.Add(this.Username, new List<Member>());
            }
            else
                return;
=======
           this.client.Connect(ip.Address, ip.Port);

>>>>>>> a0cb6389c73084a88d0649301f30db3dce43fd32

            this.ID = Int32.Parse(reader.ReadLine());
        }

        public String Get_Host_Username(String str_ip)
        {
            writer.WriteLine(this.ID.ToString() + ";GetHostName;" + str_ip);
            return reader.ReadLine().Split(';')[1];
        }
    }
    // Bei Add -> Daten müssen auch an den andern geschickt werden
}
