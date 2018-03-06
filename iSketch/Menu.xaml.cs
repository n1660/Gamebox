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

namespace iSketch
{
    /// TODO: Sperre! Wenn kein Username eingegeben-> Spielauswahl verweigern
    /// ( Wenn man zurück ins Menu geht: Erneute Username Eingabe -> Notwendig? (Falls jemand es ändern will, ja))
    public partial class Menu : Page
    {
        public static List<Member> MemberList = new List<Member>();

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
                get_player_data();
            }
        }

        private void Button_Click_Menu(object sender, RoutedEventArgs e)
        {
            if (sender == this.Submit_Username)
            {
                get_player_data();
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
                        if (MemberList.Exists(x => x.Username == PlayerUsername.Text) || MemberList.Exists(x => x.ID == IPAddress.Loopback)) // No Dublicates / Not Correct
                            Popup_Username_Exists.IsOpen = true;
                        else
                        {

                            MemberList.Add(new Member() { ID = null, Username = PlayerUsername.Text, Score = 0, Moves = 0 }); // ID = IPv4

                            Username_Canvas.Visibility = Visibility.Hidden;

                            for (int i = 0; i < MemberList.Count; i++)
                            {
                                if (MemberList[i].Username == PlayerUsername.Text)
                                {
                                    MemberList[i].ID = IPAddress.Loopback; // Ist nicht das was wir brauchen. Wir wollen IPv4!
                                    Console.WriteLine(MemberList[i].ID); // DEBUG 
                                }
                            }
                            MainWindow.win.Content = new Artist();
                        }
                    }

                }

            }
        }




    }
}
