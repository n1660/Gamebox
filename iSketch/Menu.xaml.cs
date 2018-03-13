using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;

namespace iSketch
{
    public partial class Menu : Page
    {
        public static Dictionary<String, List<Member>> MemberList = new Dictionary<String, List<Member>>();
        public static List<IPEndPoint> HostIPs = new List<IPEndPoint>();

        public static string Host;
        public static Member member;
        private static Server.Server server = null;
        
        public Menu()
        {
            InitializeComponent();
            PlayerUsername.KeyDown += new KeyEventHandler(KeyEvents);
            Username_Canvas.Visibility = Visibility.Hidden;
            //Popup_Username.IsOpen = true;
            Server.Server.StartServer();
            Host = PlayerUsername.Text;
            NewHost();
        }

        private void KeyEvents(object sender, KeyEventArgs k)
        {
            if (k.Key == Key.Enter)
            {
                NewHost();
            }
        }

        private void ButtonClickMenu(object sender, RoutedEventArgs e)
        {
            if (sender == this.New_Game_B)
            {
                Server.Server.StartServer();
                Host = PlayerUsername.Text;
                NewHost();
            }
            else if(sender == this.Join_Game_B)
            {
                Menu.member = new Member(PlayerUsername.Text, false);
                //member.Join_Game(new IPEndPoint(IPAddress.Loopback, 4444));
                //MemberList[PlayerUsername.Text].Add(member);

                Console.WriteLine("XXX");

                MainWindow.win.Content = new Artist();
            }
            else if (Username_Canvas.Visibility == Visibility.Hidden)
            {
                if (sender == this.iSketch)
                {
                    Username_Canvas.Visibility = Visibility.Visible;
                    //MainWindow.win.Content = new Artist();
                }
                else if (sender == this.Snake)
                {
                    Console.Write("Start Snake ~");
                }
                else if (sender == this.Hangman)
                {
                    Console.Write("Start Hangman X");
                }
            }
        }

        public void NewHost()
        {
            if(server == null)  // Ein Spieler kann nur ein Spiel hosten!
                server = new Server.Server();

            if (!(MemberList.ContainsKey(PlayerUsername.Text)))
            {
                MemberList.Add(Host, new List<Member>());
                Menu.member = new Member(PlayerUsername.Text, true); // Creating the host
                MemberList[Host].Add(member);

                GetPlayerData();
                Username_Canvas.Visibility = Visibility.Hidden;
                MainWindow.win.Content = new Artist();          
            }
        }

        public void GetPlayerData()
        {
        Popup_Username_Exists.IsOpen = false;

            /*if(PlayerUsername.Text != null && (!MemberList.ContainsKey(PlayerUsername.Text) || MemberList[PlayerUsername.Text].Count == 0))
            {
                for ( int i = 0; i < PlayerUsername.Text.Length; i++)
                {
                    if (PlayerUsername.Text[i] != ' ')
                    {
                        Not_Only_Blanks = true;
                        break;
                    }
                }

                if (Not_Only_Blanks)
                {
                    if (MemberList.Count < Artist.Max_Players)
                    {
                        if (MemberList[member.Hostname] != null && MemberList[Host].Count > 0)
                        {
                            if (MemberList[Host].Exists(x => x.Username == PlayerUsername.Text)) // No Dubbblicates / Not Correct
                                Popup_Username_Exists.IsOpen = true;
                            else // Insert a player to a game, which already exists
                            {
                                MemberList[PlayerUsername.Text].Add(new Member(PlayerUsername.Text, false)); // ID = ??
                                Username_Canvas.Visibility = Visibility.Hidden;
                                MainWindow.win.Content = new Artist();
                            }
                        }
                        /*else // Create game & Insert Host as Client in List
                        {
                            new Member(PlayerUsername.Text, true);
                            MemberList[PlayerUsername.Text].Add(new Member(PlayerUsername.Text, true));
                            HostIPs.Add(MemberList[Host][0].End);
                            //MemberList[Host][0].Join_Game(HostIPs[0]);

                            Username_Canvas.Visibility = Visibility.Hidden;
                            MainWindow.win.Content = new Artist();
                        }* /
                        
                    }

                }

            }*/
        }
    }
}
