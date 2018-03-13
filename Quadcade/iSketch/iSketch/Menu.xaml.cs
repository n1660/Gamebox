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

namespace Quadcade
{
    public partial class Menu : Page
    {
        public static Dictionary<String, List<Member>> MemberList = new Dictionary<String, List<Member>>();

        public static string Host;
        private static Quadcade.Server server = null;

        public Menu()
        {
            InitializeComponent();

            PlayerUsername.KeyDown += new KeyEventHandler(Key_Events);
            Username_Canvas.Visibility = Visibility.Hidden;
            //Popup_Username.IsOpen = true;
        }

        private void Key_Events(object sender, KeyEventArgs k)
        {
            if (k.Key == Key.Enter)
            {
                New_Host();
            }
        }

        private void Button_Click_Menu(object sender, RoutedEventArgs e)
        {
            if (sender == this.New_Game_B)
            {
                New_Host();
            }
            else if(sender == this.Join_Game_B)
            {

            }
            else if (Username_Canvas.Visibility == Visibility.Hidden)
            {
                if (sender == this.iSketch)
                {
                    Username_Canvas.Visibility = Visibility.Visible;
                    App.Current.MainWindow.Content = new Artist();
                }
                else if (sender == this.Snake)
                {
                    App.Current.MainWindow.Content = new MenupageSnake();
                }
                else if (sender == this.Hangman)
                {
                    Console.Write("Start Hangman X");
                }
            }
        }

        void New_Host()
        {
            Host = PlayerUsername.Text;

            if(server == null)  // Ein Spieler kann nur ein Spiel hosten!
                server = new Quadcade.Server();

            if (!(MemberList.ContainsKey(PlayerUsername.Text)))
            {
                MemberList.Add(PlayerUsername.Text, new List<Member>());
                get_player_data();
            }
        }

        void get_player_data()
        {

          Popup_Username_Exists.IsOpen = false;

            if(PlayerUsername.Text != null)
            {
                bool Not_Only_Blanks = false;
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
                        if (MemberList[Host] != null && MemberList[Host].Count > 0)
                        {
                            if (MemberList[Host].Exists(x => x.Username == PlayerUsername.Text)) // No Dublicates / Not Correct
                                Popup_Username_Exists.IsOpen = true;
                            else // Insert a player to a game, which already exists
                            {
                                MemberList[PlayerUsername.Text].Add(new Member(PlayerUsername.Text)); // ID = ??
                                Username_Canvas.Visibility = Visibility.Hidden;
                                App.Current.MainWindow.Content = new Artist();
                            }
                        }
                        else // Create game & Insert Host as Client in List
                        {
                            Artist.HostIPs.Add(MemberList[Host][0].End);
                            MemberList[PlayerUsername.Text].Add(new Member(PlayerUsername.Text));
                            MemberList[Host][0].Join_Game(Artist.HostIPs[0]);

                            Username_Canvas.Visibility = Visibility.Hidden;
                            App.Current.MainWindow.Content = new Artist();
                        }
                    }
                }
            }
        }
    }
}
