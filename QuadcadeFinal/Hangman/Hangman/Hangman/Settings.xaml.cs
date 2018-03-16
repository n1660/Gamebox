﻿using System;
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
using System.Timers;

namespace Hangman
{
    public partial class Settings : Page
    {
        public static int difficultylvl = 2;
        public static int language = 2;
        public static int countRounds = 5;
        public static int points = 100;
        public Settings()
        {
            InitializeComponent();
            FiveRounds.Visibility = Visibility.Hidden;
            TenRounds.Visibility = Visibility.Hidden;
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
                    Rounds.Visibility = Visibility.Hidden;
                    English.Visibility = Visibility.Hidden;
                    German.Visibility = Visibility.Hidden;
                    Easy.Visibility = Visibility.Visible;
                    Medium.Visibility = Visibility.Visible;
                    //Extrem.Visibility = Visibility.Visible;
                    FiveRounds.Visibility = Visibility.Hidden;
                    TenRounds.Visibility = Visibility.Hidden;
                    break;
                case "Language":
                    Difficulty.Visibility = Visibility.Hidden;
                    Language.Visibility = Visibility.Hidden;
                    Rounds.Visibility = Visibility.Hidden;
                    Easy.Visibility = Visibility.Hidden;
                    Medium.Visibility = Visibility.Hidden;
                    //Extrem.Visibility = Visibility.Hidden;
                    English.Visibility = Visibility.Visible;
                    German.Visibility = Visibility.Visible;
                    FiveRounds.Visibility = Visibility.Hidden;
                    TenRounds.Visibility = Visibility.Hidden;
                    break;
                case "Rounds":
                    Difficulty.Visibility = Visibility.Hidden;
                    Language.Visibility = Visibility.Hidden;
                    Rounds.Visibility = Visibility.Hidden;
                    Easy.Visibility = Visibility.Hidden;
                    Medium.Visibility = Visibility.Hidden;
                    //Extrem.Visibility = Visibility.Hidden;
                    English.Visibility = Visibility.Hidden;
                    German.Visibility = Visibility.Hidden;
                    FiveRounds.Visibility = Visibility.Visible;
                    TenRounds.Visibility = Visibility.Visible;
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
                case "5 Rounds":
                    countRounds = 5;
                    points = 100;
                    mw.Content = new Settings();
                    break;
                case "10 Rounds":
                    countRounds = 10;
                    points = 200;
                    mw.Content = new Settings();
                    break;
                case "Back":
                    mw.Content = new Hangman_menu();
                    break;
                default:
                    break;
            }
        }
    }
}
