using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SnakeGame
{
    /// <summary>
    /// Interaktionslogik für GamepageSnake.xaml
    /// </summary>
    public partial class GamepageSnake : Page
    {
        public enum Directions
        {
            right,
            down,
            left,
            up
        };

        //membervariables
        private static int speedUp = -1;
        private static List<SnakePlayer> snakeplayers = new List<SnakePlayer>();

        //globals
        public static bool STARTED = false;
        public static DispatcherTimer TIMER;
        public static Random RANDOM = new Random();
        public static Apple APPLE;
        public static int TICKMOVE = SnakePlayer.SIZEELEM;

        //buttons
        public Button BtnNew = new Button
        {
            Background = Brushes.DarkGoldenrod,
            Foreground = Brushes.Black,
            FontWeight = FontWeights.Bold,
            FontStyle = FontStyles.Italic,
            FontSize = 30,
            Height = 150,
            Width = 150,
            Content = "TRY\nAGAIN",
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Bottom,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center
        };

        public Button BtnExit = new Button
        {
            VerticalAlignment = VerticalAlignment.Bottom,
            HorizontalAlignment = HorizontalAlignment.Right,
            Background = Brushes.Red,
            Foreground = Brushes.White,
            Width = 40,
            Content = "X"
        };

        public Button BtnPause = new Button
        {
            VerticalAlignment = VerticalAlignment.Bottom,
            HorizontalAlignment = HorizontalAlignment.Right,
            Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x22, 0x22, 0x22)),
            Foreground = Brushes.White,
            Width = 40,
            Content = "ll"
        };

        //images
        public static ImageBrush bgpic = new ImageBrush
        {
            ImageSource = new BitmapImage(new Uri("../../Images/snake.jpg", UriKind.RelativeOrAbsolute))
        };
        public static ImageBrush gopic = new ImageBrush
        {
            ImageSource = new BitmapImage(new Uri("../../Images/snake_gameover.jpg", UriKind.RelativeOrAbsolute))
        };
        public static ImageBrush snakePausePic = new ImageBrush
        {
            ImageSource = new BitmapImage(new Uri("../../Images/snakepause.jpeg", UriKind.RelativeOrAbsolute))
        };

        //properties
        public static List<SnakePlayer> Snakeplayers { get => snakeplayers; set => snakeplayers = value; }

        //methods
        public void Render()
        {
            if (App.Current.MainWindow.Content.GetType().Name != (typeof(GamepageSnake).Name))
                return;

            int helpVar = -1, scorePrev, tmpCount = -1;
            TextBlock[] tbPrev;
            SnakePlayer.UpdateRanking();
            tmpCount = -1;
            restart:
            if (helpVar > -1 && helpVar < snakeplayers.Count)
                helpVar = -1;
            
            foreach (SnakePlayer p in snakeplayers)
            {
                if (!p.Dead)
                {
                    scorePrev = p.score;
                    tbPrev = p.Scoretext;
                    p.Render();
                    if (p.Dead)
                    {
                        helpVar = p.Id;
                        break;
                    }
                    if (scorePrev != p.score || tbPrev != p.Scoretext)
                        break;
                }
            }

            if(tmpCount == -1)
                tmpCount = SnakePlayer.TODIE.Count;

            foreach (SnakePlayer p in SnakePlayer.TODIE)
            {
                p.Dead = true;

                if (SnakePlayer.TODIE.Contains(p))
                {
                    SnakePlayer.TODIE.Remove(p);

                    foreach (SnakeElem snk in p.Snake)
                    {
                        p.GameCanvas.Children.Remove(snk.Rect);
                    }
                    p.Scoretext[0].Foreground = Brushes.OrangeRed;
                    p.Scoretext[1].Foreground = Brushes.OrangeRed;
                    goto restart;
                }
                
                if (SnakePlayer.TODIE.Count != tmpCount)
                    goto restart;
            }

            if (SnakePlayer.SURVIVORS == 1 && STARTED)
            {
                BtnPause.IsEnabled = false;
                SnakePlayer winner = snakeplayers[0];
                foreach (SnakePlayer p in snakeplayers)
                {
                    if (!p.Dead)
                    {
                        winner = p;
                        break;
                    }
                }
                TIMER.Stop();
                STARTED = false;
                GameCanvas.Children.Clear();
                GameCanvas.Background = Brushes.Gold;
                TextBlock txtWinner = new TextBlock
                {
                    Foreground = Brushes.DarkGoldenrod,
                    FontStyle = FontStyles.Italic,
                    FontSize = 30,
                    Width = GameCanvas.ActualWidth,
                    Height = 400,
                    VerticalAlignment = VerticalAlignment.Top,
                    Text = winner.Name.ToUpper() + "\nwon this round\n with a score of\n".ToUpper() + winner.score.ToString(),
                    TextAlignment = TextAlignment.Center,
                };
                txtWinner.Typography.Capitals = FontCapitals.AllSmallCaps;
                GameCanvas.Children.Add(txtWinner);
                Canvas.SetTop(BtnNew, (GameCanvas.ActualHeight / 2));
                Canvas.SetLeft(BtnNew, ((GameCanvas.ActualWidth - BtnNew.Width) / 2));
                BtnNew.Click += (object sender, RoutedEventArgs e) =>
                {
                    BtnPause.IsEnabled = true;
                    App.Current.MainWindow.Content = new GamepageSnake();
                };
            }
            if(SnakePlayer.SURVIVORS == 1 || SnakePlayer.SURVIVORS == 0)
                GameCanvas.Children.Add(BtnNew);

            if (SnakePlayer.SURVIVORS == 0 && STARTED)
            {
                BtnPause.IsEnabled = false;
                TIMER.Stop();
                GameCanvas.Children.Clear();
                GameCanvas.Background = gopic;
            }

            spScores.Children.Clear();
            foreach(SnakePlayer p in snakeplayers)
            {
                spScores.Children.Add(p.Scoretext[0]);
                spScores.Children.Add(p.Scoretext[1]);
            }

            if(STARTED)
                SpawnFood();
        }

        public GamepageSnake()
        {
            MainWindow.GamePage = this;
            STARTED = false;
            
            DockPanel.SetDock(BtnExit, Dock.Right);
            DockPanel.SetDock(BtnPause, Dock.Right);
            
            bgpic.Opacity = 0.8;
            InitializeComponent();
            GameCanvas.Background = bgpic;

            BtnExit.Click += BtnExit_Click;
            dp_buttons.Children.Add(BtnExit);

            BtnPause.Click += BtnPause_Click;
            dp_buttons.Children.Add(BtnPause);

            GameCanvas.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            GameCanvas.Arrange(new Rect(0, 0, Application.Current.MainWindow.DesiredSize.Width, Application.Current.MainWindow.DesiredSize.Height));
            Snakeplayers.Clear();
            SnakePlayer.CURPARTICIPANTS = 0;
            TICKMOVE = SnakePlayer.SIZEELEM;

            for (int i = 0; i < SnakePlayer.AMOUNT_PLAYERS; i++)
            {
                SnakePlayer playerTmp = AddPlayerToGame(("player " + (i + 1).ToString()), new IPEndPoint(IPAddress.Loopback, 1337 + i), GameCanvas);
                if(playerTmp != null)
                    snakeplayers.Add(playerTmp);
            }

            if (TIMER != null)
                TIMER = null;

            TIMER = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 150), //speed
            };

            SnakePlayer.SURVIVORS = snakeplayers.Count;

            TIMER.Tick += Time_Tick;
            TIMER.Start();
        }

        public void Time_Tick(object sender, EventArgs e)
        {
            foreach(SnakePlayer p in snakeplayers)
            {
                if (p.Dead)
                    SnakePlayer.TODIE.Add(p);
            }
            Render();
        }

        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            UIElementCollection gameElems = new UIElementCollection(GameCanvas, this);
            if (!STARTED)
            {
                STARTED = true;
                GameCanvas.Background = bgpic;
                foreach(UIElement elem in gameElems)
                {
                    GameCanvas.Children.Add(elem);
                }
                TIMER.Start();
            }
            else
            {
                gameElems = GameCanvas.Children;
                GameCanvas.Children.Clear();
                GameCanvas.Background = snakePausePic;
                STARTED = false;
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        public void SpawnFood()
        {
            if (APPLE == null)
            {
                speedUp++;
                if(speedUp % 3 == 0)
                    TICKMOVE++;
                APPLE = new Apple(RANDOM.Next(0, (int)GameCanvas.ActualWidth - SnakePlayer.SIZEELEM - 30), RANDOM.Next(0, (int)GameCanvas.ActualHeight - SnakePlayer.SIZEELEM - 10));
                Canvas.SetLeft(APPLE.Shape, APPLE.X);
                Canvas.SetTop(APPLE.Shape, APPLE.Y);
                Canvas.SetZIndex(APPLE.Shape, 1);
                if(STARTED)
                    GameCanvas.Children.Add(APPLE.Shape);
            }
        }

        public Canvas GetGameCanvas()
        {
            return GameCanvas;
        }

        public StackPanel GetScoreSP()
        {
            return spScores;
        }

        public SnakePlayer AddPlayerToGame(String name, IPEndPoint ip_port, Canvas gamecanv)
        {
            foreach (SnakePlayer p in snakeplayers)
            {
                if(p.Name == name)
                {
                    Console.WriteLine("there's already a player named like that involved in this competition!");
                    return null;
                }
            }

            SnakePlayer player = new SnakePlayer(name, ip_port, gamecanv);
            return player;
        }
    }
}
