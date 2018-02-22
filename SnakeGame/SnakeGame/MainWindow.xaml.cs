using SnakeGame;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace SnakeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //membervariables
        private static GamepageSnake gamePage;
        private bool keysInitialized = false;

        //globals
        public static Dictionary<String, Dictionary<String, Key>> PLAYERKEYS = new Dictionary<string, Dictionary<String, Key>>();

        //images
        public static ImageBrush startpic = new ImageBrush
        {
            ImageSource = new BitmapImage(new Uri("../../Images/snakestart.png", UriKind.RelativeOrAbsolute))
        };

        //properties
        public static GamepageSnake GamePage { get => gamePage; set => gamePage = value; }

        //c'tor
        public MainWindow()
        {
            InitializeComponent();

            BtnStartSnake.Background = startpic;
        }

        //methods
        private void BtnStartSnake_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new GamepageSnake();
        }

        private void InitializePlayerKeys()
        {
            Dictionary<String, Key> pg = new Dictionary<string, Key>
            {
                {"upG",  Key.Up},
                {"downG", Key.Down},
                {"leftG", Key.Left},
                {"rightG", Key.Right}
            };
            PLAYERKEYS.Add("playerGreen", pg);

            Dictionary<String, Key> pb = new Dictionary<string, Key>
            {
                {"upB",  Key.W},
                {"downB", Key.S},
                {"leftB", Key.A},
                {"rightB", Key.D}
            };
            PLAYERKEYS.Add("playerBlue", pb);

            Dictionary<String, Key> pr = new Dictionary<string, Key>
            {
                {"upR",  Key.Up },
                {"downR", Key.Down},
                {"leftR", Key.Left},
                {"rightR", Key.Right}
            };
            PLAYERKEYS.Add("playerRed", pr);

            Dictionary<String, Key> pp = new Dictionary<string, Key>
            {
                {"upP",  Key.Up },
                {"downP", Key.Down},
                {"leftP", Key.Left},
                {"rightP", Key.Right}
            };
            PLAYERKEYS.Add("playerPurple", pp);
            keysInitialized = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.Content.GetType().Name == (typeof(GamepageSnake).Name))
            {
                if (GamepageSnake.Snakeplayers.Count == 0)
                    return;

                if (!keysInitialized)
                    InitializePlayerKeys();

                if (e.Key == Key.Space && !GamepageSnake.STARTED)
                {
                    GamepageSnake.STARTED = true;
                }
                else if(GamepageSnake.STARTED)
                {
                    foreach (SnakePlayer p in GamepageSnake.Snakeplayers)
                    {
                        foreach (GamepageSnake.Directions dir in Enum.GetValues(typeof(GamepageSnake.Directions)))
                        {
                            if (e.Key == PLAYERKEYS["player" + (p.Color.ToString())][dir.ToString() + p.Color.ToString()[0]] && p.Snake[0].Direction != dir + (((int) dir > 1) ? -2 : 2))
                                p.Snake[0].Direction = dir;
                        }

                        //if (e.Key == PLAYERKEYS["player" + (p.Color.ToString())]["Up" + p.Color.ToString()[0]] && p.Snake[0].Direction != GamepageSnake.Directions.down)
                        //    p.Snake[0].Direction = GamepageSnake.Directions.up;
                        //if (e.Key == PLAYERKEYS["player" + (p.Color.ToString())]["Down" + p.Color.ToString()[0]] && p.Snake[0].Direction != GamepageSnake.Directions.up)
                        //    p.Snake[0].Direction = GamepageSnake.Directions.down;
                        //if (e.Key == PLAYERKEYS["player" + (p.Color.ToString())]["Left" + p.Color.ToString()[0]] && p.Snake[0].Direction != GamepageSnake.Directions.right)
                        //    p.Snake[0].Direction = GamepageSnake.Directions.left;
                        //if (e.Key == PLAYERKEYS["player" + (p.Color.ToString())]["Right" + p.Color.ToString()[0]] && p.Snake[0].Direction != GamepageSnake.Directions.left)
                        //    p.Snake[0].Direction = GamepageSnake.Directions.right;
                    }
                }
                GamepageSnake.Snakeplayers[gamePage.PlayerID - 1].Snake[0].Rect.Fill = GamepageSnake.Snakeplayers[gamePage.PlayerID - 1].Pictures[Pictures.Head.ToString()];
            }
        }
    }
}