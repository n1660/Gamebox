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

            App.Current.MainWindow.Height = 300;
            App.Current.MainWindow.Width = 300;

            InitializeComponent();
            PlayerUsername.KeyDown += new KeyEventHandler(KeyEvents);
            CanvUsername.Visibility = Visibility.Hidden;
            Host = PlayerUsername.Text;           
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
            if (sender == this.BtnHost)
            {
                Host = PlayerUsername.Text;
                NewHost();
            }
            else if(sender == this.BtnJoin)
            {
                Menu.member = new Member(PlayerUsername.Text, false);

                List<Member> members = new List<Member>
                {
                    member
                };
                MemberList.Add(Menu.Host, members);

                Console.WriteLine("XXX");

                MainWindow.win.Content = new Artist();
            }
            else if (CanvUsername.Visibility == Visibility.Hidden)
            {
                if (sender == this.iSketch)
                {
                    CanvUsername.Visibility = Visibility.Visible;
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
            if (server == null)
            {  // Ein Spieler kann nur ein Spiel hosten!
                server = new Server.Server();
                Server.Server.StartServer();
            }

            if (!(MemberList.ContainsKey(PlayerUsername.Text)))
            {
                MemberList.Add(Host, new List<Member>());
                Menu.member = new Member(PlayerUsername.Text, true); // Creating the host
                MemberList[Host].Add(member);

                CanvUsername.Visibility = Visibility.Hidden;
                MainWindow.win.Content = new Artist();          
            }
        }
    }
}
