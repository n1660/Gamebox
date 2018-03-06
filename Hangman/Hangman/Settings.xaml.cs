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

namespace Hangman
{
    public partial class Settings : Page
    {
        public static int difficultylvl = 2;
        public static int language = 1;
        public Settings()
        {
            InitializeComponent();
            Easy.Visibility = Visibility.Hidden;
            Medium.Visibility = Visibility.Hidden;
            Extrem.Visibility = Visibility.Hidden;
            English.Visibility = Visibility.Hidden;
            German.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string content = btn.Content.ToString();
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            switch (content)
            {
                case "Difficulty":
                    Difficulty.Visibility = Visibility.Hidden;
                    Language.Visibility = Visibility.Hidden;
                    English.Visibility = Visibility.Hidden;
                    German.Visibility = Visibility.Hidden;
                    Easy.Visibility = Visibility.Visible;
                    Medium.Visibility = Visibility.Visible;
                    //Extrem.Visibility = Visibility.Visible;
                    break;
                case "Language":
                    Difficulty.Visibility = Visibility.Hidden;
                    Language.Visibility = Visibility.Hidden;
                    Easy.Visibility = Visibility.Hidden;
                    Medium.Visibility = Visibility.Hidden;
                    //Extrem.Visibility = Visibility.Hidden;
                    English.Visibility = Visibility.Visible;
                    German.Visibility = Visibility.Visible;               
                    break;
                case "German words":
                    language = 1;
                    mw.Content = new Settings();
                    break;
                case "English words":
                    language = 2;
                    mw.Content = new Settings();
                    break;
                case "3-5 letters":
                    difficultylvl = 1;
                    mw.Content = new Settings();
                    break;
                case "6-8 letters":
                    difficultylvl = 2;
                    mw.Content = new Settings();
                    break;
                case "Extrem":
                    difficultylvl = 3;
                    mw.Content = new Settings();
                    break;
                case "Back":
                    mw.Content = new Hangman_menu();
                    break;
                default:
                    break;
            }

            //switch (content)
            //{
            //    case "3-5 Buchstaben":
            //        difficultylvl = 1;
            //        mw.Content = new Hangman_menu();
            //        break;
            //    case "6-8 Buchstaben":
            //        difficultylvl = 2;
            //        mw.Content = new Hangman_menu();
            //        break;
            //    case "Extrem":
            //        difficultylvl = 3;
            //        mw.Content = new Hangman_menu();
            //        break;
            //    case "Back":
            //        mw.Content = new Hangman_menu();
            //        break;
            //    default:
            //        break;
            //}
        }
    }
}
