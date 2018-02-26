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
        private static TextBlock txtp;
        
        //globals
        public static bool STARTED = false;
        public static DispatcherTimer TIMER;
        public static Random RANDOM = new Random();
        public static Apple APPLE;
        public static int TICKMOVE = SnakePlayer.SIZEELEM;

        //UIElements
        public Canvas GameCanvas = new Canvas();
        public StackPanel spScores = new StackPanel();
        public DockPanel dpButtons = new DockPanel();

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

        public Button BtnAddPlayer = new Button
        {
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
            Background = Brushes.DarkSeaGreen,
            Foreground = Brushes.White,
            Width = 40,
            Content = "+"
        };

        public Button BtnRemovePlayer = new Button
        {
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
            Background = Brushes.DarkSeaGreen,
            Foreground = Brushes.White,
            Width = 40,
            Content = "-"
        };

        public Button BtnExit = new Button
        {
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Right,
            Background = Brushes.Red,
            Foreground = Brushes.White,
            Width = 40,
            Content = "X"
        };

        public Button BtnPause = new Button
        {
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Right,
            Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x22, 0x22, 0x22)),
            Foreground = Brushes.White,
            Width = 40,
            Content = "ll"
        };

        public TextBlock txtPlayers = new TextBlock
        {
            Background = Brushes.Olive,
            Foreground = Brushes.Green,
            FontWeight = FontWeights.Bold,
            FontStyle = FontStyles.Italic,
            FontSize = 15,
            Width = 75,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
            TextAlignment = TextAlignment.Center,
        };

        public TextBlock txtWinner = new TextBlock
        {
            Foreground = Brushes.DarkGoldenrod,
            FontStyle = FontStyles.Italic,
            FontSize = 30,
            Height = 400,
            VerticalAlignment = VerticalAlignment.Top,
            TextAlignment = TextAlignment.Center,
        };

        //Inputprompt
        public StackPanel newPlayerPrompt = new StackPanel
        {
            Height = 350,
            Width = 100,
            Background = Brushes.SeaGreen,
            Visibility = Visibility.Hidden
        };

        public Label LblInputheader = new Label
        {
            Margin = new Thickness(0, 15, 0, 0),
            Background = Brushes.LightSeaGreen,
            Foreground = Brushes.Black,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            Content = "create new player:",
            Width = 100,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            IsEnabled = false
        };

        public Label LblName = new Label
        {
            Margin = new Thickness(0, 15, 0, 0),
            Background = Brushes.LightSeaGreen,
            Foreground = Brushes.Black,
            Content = "Name:",
            Width = 80,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            IsEnabled = false
        };

        public TextBox InputName = new TextBox
        {
            Background = Brushes.LightSeaGreen,
            Foreground = Brushes.Black,
            TextAlignment = TextAlignment.Center,
            Width = 80
        };

        public Label LblIP = new Label
        {
            Margin = new Thickness(0, 15, 0, 0),
            Background = Brushes.LightSeaGreen,
            Foreground = Brushes.Black,
            Content = "IP-Adress:",
            Width = 80,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            IsEnabled = false
        };

        public TextBox InputIP = new TextBox
        {
            Background = Brushes.LightSeaGreen,
            Foreground = Brushes.Black,
            TextAlignment = TextAlignment.Center,
            Width = 80
        };

        public Label LblPort = new Label
        {
            Margin = new Thickness(0, 15, 0, 0),
            Background = Brushes.LightSeaGreen,
            Foreground = Brushes.Black,
            Content = "Port-nr.:",
            Width = 80,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            IsEnabled = false
        };

        public Label LblError = new Label
        {
            Margin = new Thickness(0, 15, 0, 0),
            Background = Brushes.OrangeRed,
            Foreground = Brushes.DarkRed,
            Content = "Sorry,\nsomething\nwent\nwrong\nwith\nthese\ninputs",
            Width = 80,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            Visibility = Visibility.Hidden,
            IsEnabled = false
        };

        public TextBox InputPort = new TextBox
        {
            Background = Brushes.LightSeaGreen,
            Foreground = Brushes.Black,
            TextAlignment = TextAlignment.Center,
            Width = 80
        };

        public Button BtnSubmitNewPlayer = new Button
        {
            Margin = new Thickness(0, 5, 0, 0),
            VerticalAlignment = VerticalAlignment.Bottom,
            HorizontalAlignment = HorizontalAlignment.Right,
            Background = Brushes.LightSeaGreen,
            Foreground = Brushes.Black,
            Width = 50,
            Content = "Submit"
        };

        public Button BtnCancelNewPlayer = new Button
        {
            Margin = new Thickness(0, 25, 0, 0),
            VerticalAlignment = VerticalAlignment.Bottom,
            HorizontalAlignment = HorizontalAlignment.Right,
            Background = Brushes.LightSeaGreen,
            Foreground = Brushes.Black,
            Width = 50,
            Content = "Cancel"
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

        //c'tor
        public GamepageSnake()
        {
            MainWindow.GamePage = this;
            STARTED = false;

            bgpic.Opacity = 0.8;
            InitializeComponent();

            GameCanvas.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            GameCanvas.Arrange(new Rect(0, 0, Application.Current.MainWindow.DesiredSize.Width, Application.Current.MainWindow.DesiredSize.Height));

            InitializePrompt();
            newPlayerPrompt.Children.Add(LblInputheader);
            newPlayerPrompt.Children.Add(LblName);
            newPlayerPrompt.Children.Add(InputName);
            newPlayerPrompt.Children.Add(LblIP);
            newPlayerPrompt.Children.Add(InputIP);
            newPlayerPrompt.Children.Add(LblPort);
            newPlayerPrompt.Children.Add(InputPort);
            BtnSubmitNewPlayer.Click += BtnSubmitNewPlayer_Click;
            BtnCancelNewPlayer.Click += BtnCancelNewPlayer_Click;
            newPlayerPrompt.Children.Add(BtnCancelNewPlayer);
            newPlayerPrompt.Children.Add(BtnSubmitNewPlayer);
            newPlayerPrompt.Children.Add(LblError);
            Canvas.SetZIndex(BtnNew, 2);
            Canvas.SetZIndex(newPlayerPrompt, 2);
            Canvas.SetLeft(newPlayerPrompt, GameCanvas.ActualWidth - newPlayerPrompt.Width / 2);
            Canvas.SetTop(newPlayerPrompt, GameCanvas.ActualHeight - newPlayerPrompt.Height / 2);
            GameCanvas.Children.Add(newPlayerPrompt);

            txtp = txtPlayers;

            Grid.SetColumn(GameCanvas, 0);
            Grid.SetRowSpan(GameCanvas, 2);
            GridGamepage.Children.Add(GameCanvas);

            Grid.SetColumn(spScores, 1);
            Grid.SetRow(spScores, 0);
            GridGamepage.Children.Add(spScores);

            Grid.SetColumn(dpButtons, 1);
            Grid.SetRow(dpButtons, 1);
            GridGamepage.Children.Add(dpButtons);

            GameCanvas.Background = bgpic;
            dpButtons.Background = spScores.Background = Brushes.LightGray;

            DockPanel.SetDock(BtnExit, Dock.Right);
            BtnExit.Click += BtnExit_Click;
            dpButtons.Children.Add(BtnExit);

            DockPanel.SetDock(BtnPause, Dock.Right);
            BtnPause.Click += BtnPause_Click;
            dpButtons.Children.Add(BtnPause);

            BtnAddPlayer.Click += BtnAddPlayer_Click;
            dpButtons.Children.Add(BtnAddPlayer);

            foreach (UIElement el in dpButtons.Children)
            {
                el.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                el.Arrange(new Rect(0, 0, Application.Current.MainWindow.DesiredSize.Width, Application.Current.MainWindow.DesiredSize.Height));
            }

            txtPlayers.Text = "players: " + SnakePlayer.CURPARTICIPANTS.ToString();
            txtPlayers.Height = BtnAddPlayer.ActualHeight;
            dpButtons.Children.Add(txtPlayers);

            BtnRemovePlayer.Click += BtnRemovePlayer_Click;
            dpButtons.Children.Add(BtnRemovePlayer);
            
            Snakeplayers.Clear();
            SnakePlayer.CURPARTICIPANTS = 0;
            TICKMOVE = SnakePlayer.SIZEELEM;

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

        //Eventlistening
        private void BtnCancelNewPlayer_Click(object sender, RoutedEventArgs e)
        {
            newPlayerPrompt.Visibility = Visibility.Hidden;
            LblError.Visibility = Visibility.Hidden;
            BtnPause.Visibility = Visibility.Visible;
            BtnPause.IsEnabled = true;
        }

        private void BtnSubmitNewPlayer_Click(object sender, RoutedEventArgs e)
        {
            SnakePlayer playerTmp = null;
            
            try
            {
                if (InputName.Text == "" || InputIP.Text == "" || InputPort.Text == "")
                    throw new Exception();

                playerTmp = AddPlayerToGame(InputName.Text, new IPEndPoint(IPAddress.Parse(InputIP.Text), Int32.Parse(InputPort.Text)), GameCanvas);
                snakeplayers.Add(playerTmp);
                newPlayerPrompt.Visibility = Visibility.Hidden;
                BtnPause.IsEnabled = true;
                BtnPause.Visibility = Visibility.Visible;
                LblError.Visibility = Visibility.Hidden;
            }
            catch (Exception)
            {
                LblError.Visibility = Visibility.Visible;
            }
        }

        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            UIElementCollection gameElems = new UIElementCollection(GameCanvas, this);
            if (!STARTED)
            {
                STARTED = true;
                GameCanvas.Background = bgpic;
                foreach (UIElement elem in gameElems)
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

        private void BtnAddPlayer_Click(object sender, RoutedEventArgs e)
        {
            InitializePrompt();
            newPlayerPrompt.Visibility = Visibility.Visible;
            if (SnakePlayer.CURPARTICIPANTS > 3)
                BtnAddPlayer.IsEnabled = false;
            if (SnakePlayer.CURPARTICIPANTS > 1)
                BtnRemovePlayer.IsEnabled = true;
            BtnPause.Visibility = Visibility.Hidden;
            BtnPause.IsEnabled = false;
        }

        private void BtnRemovePlayer_Click(object sender, RoutedEventArgs e)
        {
            foreach (SnakeElem snk in snakeplayers[snakeplayers.Count - 1].Snake)
            {
                GameCanvas.Children.Remove(snk.Rect);
            }
            snakeplayers[snakeplayers.Count - 1].Dead = true;
            snakeplayers.Remove(snakeplayers[snakeplayers.Count - 1]);
            SnakePlayer.CURPARTICIPANTS--;
            if (SnakePlayer.CURPARTICIPANTS < 1)
                BtnRemovePlayer.IsEnabled = false;
            BtnAddPlayer.IsEnabled = true;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        //timertickroutine
        public void Time_Tick(object sender, EventArgs e)
        {
            if (STARTED)
                BtnAddPlayer.IsEnabled = BtnRemovePlayer.IsEnabled = false;
            else
                BtnAddPlayer.IsEnabled = BtnRemovePlayer.IsEnabled = true;

            foreach (SnakePlayer p in snakeplayers)
            {
                if (p.Dead)
                    SnakePlayer.TODIE.Add(p);
            }
            Render();
        }

        //methods
        public void SpawnFood()
        {
            if (APPLE == null)
            {
                speedUp++;
                if (speedUp % 3 == 0)
                    TICKMOVE++;
                APPLE = new Apple(RANDOM.Next(0, (int)GameCanvas.ActualWidth - SnakePlayer.SIZEELEM - 30), RANDOM.Next(0, (int)GameCanvas.ActualHeight - SnakePlayer.SIZEELEM - 10));
                Canvas.SetLeft(APPLE.Shape, APPLE.X);
                Canvas.SetTop(APPLE.Shape, APPLE.Y);
                Canvas.SetZIndex(APPLE.Shape, 1);
                if (STARTED)
                    GameCanvas.Children.Add(APPLE.Shape);
            }
        }

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

                txtWinner.Width = GameCanvas.ActualWidth;
                txtWinner.Text = winner.Name.ToUpper() + "\nwon this round!\nsurvivng with a score of\n".ToUpper() + winner.score.ToString();
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
            if((SnakePlayer.SURVIVORS == 1 || SnakePlayer.SURVIVORS == 0) && STARTED)
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
                    return null;
                }
            }
            SnakePlayer player = new SnakePlayer(name, ip_port, gamecanv);
            return player;
        }

        public static TextBlock GetDPButtons()
        {
            return txtp;
        }

        public void InitializePrompt()
        {
            InputName.Text = "player " + (snakeplayers.Count + 1).ToString();
            InputIP.Text = IPAddress.Loopback.ToString();
            InputPort.Text = "1337";
        }
    }
}
