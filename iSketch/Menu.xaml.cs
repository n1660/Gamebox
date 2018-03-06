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
                        if (MemberList.Exists(x => x.Username == PlayerUsername.Text)) // No Dublicates / Not Correct
                            Popup_Username_Exists.IsOpen = true;
                        else
                        {
                            MemberList.Add(new Member() {Username = PlayerUsername.Text, Score = 0, Moves = 0 }); // ID = IPv4
                            Username_Canvas.Visibility = Visibility.Hidden;
                            MainWindow.win.Content = new Artist();
                        }
                    }

                }

            }
        }




    }
}
