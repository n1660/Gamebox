using SnakeGame;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
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
        public static Dictionary<String, Dictionary<Key, GamepageSnake.Directions>> PLAYERKEYS = new Dictionary<String, Dictionary<Key, GamepageSnake.Directions>>();

        //UIElements
        public Canvas BtnCanvStartSnake = new Canvas
        {
            Height = 180,
            Width = 150
        };

        public TextBlock BtnTBStartSnake = new TextBlock
        {
            Background = Brushes.Transparent,
            Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x77, 0xAA, 0x77)),
            FontSize = 30,
            FontWeight = FontWeights.Bold,
            Text = "start"
        };

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
            BtnCanvStartSnake.MouseDown += BtnStartSnake_Click;
            BtnTBStartSnake.Typography.Capitals = FontCapitals.AllSmallCaps;
            Canvas.SetBottom(BtnTBStartSnake, -40);
            Canvas.SetLeft(BtnTBStartSnake, 40);
            BtnCanvStartSnake.Background = startpic;

            BtnCanvStartSnake.Children.Add(BtnTBStartSnake);
            GridMenu.Children.Add(BtnCanvStartSnake);
        }

        //methods
        private void BtnStartSnake_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new GamepageSnake();
        }

        private void InitializePlayerKeys()
        {
            Dictionary<Key, GamepageSnake.Directions> pg = new Dictionary<Key, GamepageSnake.Directions>
            {
                {Key.Right, GamepageSnake.Directions.right},
                {Key.Down, GamepageSnake.Directions.down},
                {Key.Left, GamepageSnake.Directions.left},
                {Key.Up, GamepageSnake.Directions.up
}
            };
            PLAYERKEYS.Add("playerGreen", pg);

            Dictionary<Key, GamepageSnake.Directions> pb = new Dictionary<Key, GamepageSnake.Directions>
            {
                {Key.D, GamepageSnake.Directions.right},
                {Key.S, GamepageSnake.Directions.down},
                {Key.A, GamepageSnake.Directions.left},
                {Key.W, GamepageSnake.Directions.up}
            };
            PLAYERKEYS.Add("playerBlue", pb);

            Dictionary<Key, GamepageSnake.Directions> pr = new Dictionary<Key, GamepageSnake.Directions>
            {
                {Key.NumPad6, GamepageSnake.Directions.right},
                {Key.NumPad5, GamepageSnake.Directions.down},
                {Key.NumPad4, GamepageSnake.Directions.left},
                {Key.NumPad8, GamepageSnake.Directions.up
}
            };
            PLAYERKEYS.Add("playerRed", pr);

            Dictionary<Key, GamepageSnake.Directions> pp = new Dictionary<Key, GamepageSnake.Directions>
            {
                {Key.L, GamepageSnake.Directions.right},
                {Key.K, GamepageSnake.Directions.down},
                {Key.J, GamepageSnake.Directions.left},
                {Key.I, GamepageSnake.Directions.up}
            };
            PLAYERKEYS.Add("playerPurple", pp);
            keysInitialized = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.Content.GetType().Name == (typeof(GamepageSnake).Name))
            {
                if (!keysInitialized)
                    InitializePlayerKeys();

                if (e.Key == Key.P && !GamepageSnake.STARTED)
                {
                    GamepageSnake.STARTED = true;
                }
                else
                {
                    //keyrequests for changing direction
                    if (e.Key != Key.P && GamepageSnake.STARTED)
                    {
                        foreach (SnakePlayer p in GamepageSnake.Snakeplayers)
                        {
                            foreach (Colors c in Enum.GetValues(typeof(Colors)))
                            {
                                if (p.Color.ToString() == c.ToString() && PLAYERKEYS["player" + c.ToString()].ContainsKey(e.Key) && PLAYERKEYS["player" + c.ToString()][e.Key] != p.DisabledDirection)
                                    p.Snake[0].Direction = PLAYERKEYS["player" + c.ToString()][e.Key];
                            }
                        }
                    }
                }
            }
        }
    }
}