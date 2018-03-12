using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Quadcade
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //membervariables
        private bool keysInitialized;

        //globals
        public Dictionary<String, Dictionary<Key, Directions>> PLAYERKEYS = new Dictionary<string, Dictionary<Key, Directions>>();

        //FrameworkElements

        //UIElements

        //images

        //properties

        //c'tor
        public MainWindow()
        {
            InitializeComponent();
            this.Content = new MenupageSnake();
        }

        //methods
        private void InitializePlayerKeys()
        {
            Dictionary<Key, Directions> pg = new Dictionary<Key, Directions>
            {
                {Key.Right, Directions.right},
                {Key.Down, Directions.down},
                {Key.Left, Directions.left},
                {Key.Up, Directions.up
}
            };
            PLAYERKEYS.Add("playerGreen", pg);
            Dictionary<Key, Directions> pb = new Dictionary<Key, Directions>
            {
                {Key.D, Directions.right},
                {Key.S, Directions.down},
                {Key.A, Directions.left},
                {Key.W, Directions.up}
            };
            PLAYERKEYS.Add("playerBlue", pb);
            Dictionary<Key, Directions> pr = new Dictionary<Key, Directions>
            {
                {Key.NumPad6, Directions.right},
                {Key.NumPad5, Directions.down},
                {Key.NumPad4, Directions.left},
                {Key.NumPad8, Directions.up
}
            };
            PLAYERKEYS.Add("playerRed", pr);
            Dictionary<Key, Directions> pp = new Dictionary<Key, Directions>
            {
                {Key.L, Directions.right},
                {Key.K, Directions.down},
                {Key.J, Directions.left},
                {Key.I, Directions.up}
            };
            PLAYERKEYS.Add("playerPurple", pp);
            keysInitialized = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (App.Current.MainWindow.Content.GetType().Name == (typeof(GamepageSnake).Name))
            {
                if (!keysInitialized)
                    InitializePlayerKeys();

                if (e.Key == Key.P && !GamepageSnake.STARTED)
                {
                    GamepageSnake.STARTED = true;
                    GamepageSnake.TIMER.Start();
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
                                    p.Snake[0].Direction = p.Direction = PLAYERKEYS["player" + c.ToString()][e.Key];

                                if (GamepageSnake.STARTED)
                                    p.DisabledDirection = ((int)p.Direction < 2) ? (Directions)((int)p.Snake[0].Direction + 2) : (Directions)((int)p.Snake[0].Direction - 2);
                            }
                        }
                    }
                }
            }
        }
    }
}