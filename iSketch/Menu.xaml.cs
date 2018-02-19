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
            username.KeyDown += new KeyEventHandler(Key_Events);
            Popup_Username.IsOpen = true;
            
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
            if (sender == this.iSketch)
            {
                MainWindow.win.Content = new Artist();
            }
            else if(sender == this.Snake)
            {
                Console.Write("Start Snake ~");
            }
            else if(sender == this.Hangman)
            {
                Console.Write("Start Hangman X");
            }
            else if(sender==this.Submit)
            {
                get_player_data();
            }
        }

        void get_player_data()
        {
            if(username.Text != null)
            {
                bool Not_Only_Blanks = false;
                for ( int i = 0; i < username.Text.Length; i++)
                {
                    if (username.Text[i] != ' ')
                    {
                        Not_Only_Blanks = true;
                        break;
                    }
                }

                if (Not_Only_Blanks)
                {
                    MemberList.Add(new Member() { ID = null, Username = username.Text, Score = 0, Moves = 0 }); // ID = IP
                    Popup_Username.IsOpen = false;
                }
            }
        }




    }
}
