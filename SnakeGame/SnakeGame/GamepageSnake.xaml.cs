using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
=======
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
>>>>>>> fea_hangman
using System.Windows.Threading;

namespace SnakeGame
{
<<<<<<< HEAD
=======
    public static enum Directions
    {
        stay = 0,
        left = 1,
        up = 2,
        right = 3,
        down = 4
    };
>>>>>>> fea_hangman
    /// <summary>
    /// Interaktionslogik für GamepageSnake.xaml
    /// </summary>
    public partial class GamepageSnake : Page
    {
<<<<<<< HEAD
        //membervariables
        private static List<SnakePlayer> snakeplayers = new List<SnakePlayer>();
        private static List<SnakePlayer> snakeplayersTmp = new List<SnakePlayer>();

        //globals
        public static bool STARTED = false, MULTIPLAYER = false;
        public static DispatcherTimer TIMER;
        public static Random RANDOM = new Random();
        public static Apple APPLE;

        //FrameworkElements
        public Canvas GameCanvas = new Canvas();
        public StackPanel spScores = new StackPanel();
        public DockPanel dpButtons = new DockPanel();

        //UIElements
        public Button BtnNew = new Button
        {
            Background = Brushes.DarkGoldenrod,
            Foreground = Brushes.Black,
            FontWeight = FontWeights.Bold,
            FontStyle = FontStyles.Italic,
            FontSize = 30,
            Height = 150,
            Width = 150,
            Visibility = Visibility.Hidden,
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
            Background = Brushes.DarkGreen,
            Foreground = Brushes.White,
            Width = 40,
            Content = "+"
        };
        public Button BtnRemovePlayer = new Button
        {
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
            Background = Brushes.DarkGreen,
            Foreground = Brushes.White,
            IsEnabled = false,
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
            Foreground = Brushes.DarkGreen,
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

        //InputpromptRemove
        public StackPanel removePlayerPrompt = new StackPanel
        {
            Height = 400,
            Width = 100,
            Background = Brushes.SeaGreen,
            Visibility = Visibility.Hidden
        };
        public Label LblInputheaderR = new Label
        {
            Margin = new Thickness(0, 5, 0, 0),
            Background = Brushes.LightSeaGreen,
            Foreground = Brushes.Black,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            Content = "remove player:",
            Width = 90,
            FontSize = 12,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            IsEnabled = false
        };
        public ComboBox playerbox = new ComboBox
        {
            Margin = new Thickness(0, 5, 0, 0),
            Background = Brushes.LightSeaGreen,
            Foreground = Brushes.Black,
            HorizontalContentAlignment = HorizontalAlignment.Center
        };
        public Button BtnSubmitRemovePlayer = new Button
        {
            Margin = new Thickness(0, 5, 0, 0),
            VerticalAlignment = VerticalAlignment.Bottom,
            HorizontalAlignment = HorizontalAlignment.Right,
            Background = Brushes.LightSeaGreen,
            Foreground = Brushes.Black,
            Width = 50,
            Content = "Remove"
        };
        public Button BtnCancelR = new Button
        {
            Margin = new Thickness(0, 25, 0, 0),
            VerticalAlignment = VerticalAlignment.Bottom,
            HorizontalAlignment = HorizontalAlignment.Right,
            Background = Brushes.LightSeaGreen,
            Foreground = Brushes.Black,
            Width = 50,
            Content = "Cancel"
        };
        public Label LblErrorR = new Label
        {
            Margin = new Thickness(0, 5, 0, 0),
            Background = Brushes.OrangeRed,
            Foreground = Brushes.DarkRed,
            Content = "Sorry,\nsomething\nwent\nwrong with this selection!",
            Width = 80,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            Visibility = Visibility.Hidden,
            IsEnabled = false
        };

        //InputpromptAdd
        public StackPanel newPlayerPrompt = new StackPanel
        {
            Height = 400,
            Width = 100,
            Background = Brushes.SeaGreen,
            Visibility = Visibility.Hidden,
        };
        public Label LblInputheaderA = new Label
        {
            Margin = new Thickness(0, 5, 0, 0),
            Background = Brushes.LightSeaGreen,
            Foreground = Brushes.Black,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            Content = "create player:",
            Width = 90,
            FontSize = 12,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            IsEnabled = false
        };
        public Label LblNameA = new Label
        {
            Margin = new Thickness(0, 5, 0, 0),
            Background = Brushes.LightSeaGreen,
            Foreground = Brushes.Black,
            Content = "Name:",
            Width = 80,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            IsEnabled = false
        };
        public TextBox InputNameA = new TextBox
        {
            Background = Brushes.LightSeaGreen,
            Foreground = Brushes.Black,
            TextAlignment = TextAlignment.Center,
            Width = 80
        };
        public Label LblIPA = new Label
        {
            Margin = new Thickness(0, 5, 0, 0),
            Background = Brushes.LightSeaGreen,
            Foreground = Brushes.Black,
            Content = "IP-Adress:",
            Width = 80,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            IsEnabled = false
        };
        public TextBox InputIPA = new TextBox
        {
            Background = Brushes.LightSeaGreen,
            Foreground = Brushes.Black,
            TextAlignment = TextAlignment.Center,
            Width = 80,
        };
        public Label LblPortA = new Label
        {
            Margin = new Thickness(0, 5, 0, 0),
            Background = Brushes.LightSeaGreen,
            Foreground = Brushes.Black,
            Content = "Port-nr.:",
            Width = 80,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            IsEnabled = false
        };
        public TextBox InputPortA = new TextBox
        {
            Background = Brushes.LightSeaGreen,
            Foreground = Brushes.Black,
            TextAlignment = TextAlignment.Center,
            Width = 80,
            Text = ""
        };
        public Label LblErrorA = new Label
        {
            Margin = new Thickness(0, 5, 0, 0),
            Background = Brushes.OrangeRed,
            Foreground = Brushes.DarkRed,
            Content = "Sorry,\nsomething\nwent\nwrong with these inputs!",
            Width = 80,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            Visibility = Visibility.Hidden,
            IsEnabled = false
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
        public Button BtnCancelA = new Button
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
        public static List<SnakePlayer> SnakeplayersTmp { get => snakeplayersTmp; set => snakeplayersTmp = value; }

        //timertickroutine
        public void Time_Tick(object sender, EventArgs e)
        {
            foreach (SnakePlayer p in snakeplayers)
            {
                if (p.Dead)
                    SnakePlayer.TODIE.Add(p);
            }
            Render();
        }

        //c'tor
        public GamepageSnake(bool multiplayer)
        {
            MULTIPLAYER = multiplayer;

            App.Current.MainWindow.MinWidth = 1200;
            App.Current.MainWindow.MinHeight = 560;

            MenupageSnake.GamePage = this;
            STARTED = false;

            bgpic.Opacity = 0.8;
            InitializeComponent();

            BtnPause.Visibility = Visibility.Hidden;
            BtnPause.IsEnabled = false;

            GameCanvas.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            GameCanvas.Arrange(new Rect(0, 0, Application.Current.MainWindow.DesiredSize.Width, Application.Current.MainWindow.DesiredSize.Height));

            newPlayerPrompt.Children.Add(LblInputheaderA);
            newPlayerPrompt.Children.Add(LblNameA);
            newPlayerPrompt.Children.Add(InputNameA);
            newPlayerPrompt.Children.Add(LblIPA);
            newPlayerPrompt.Children.Add(InputIPA);
            newPlayerPrompt.Children.Add(LblPortA);
            newPlayerPrompt.Children.Add(InputPortA);
            BtnSubmitNewPlayer.Click += BtnSubmitNewPlayer_Click;
            BtnCancelA.Click += BtnCancel_Click;
            newPlayerPrompt.Children.Add(BtnCancelA);
            newPlayerPrompt.Children.Add(BtnSubmitNewPlayer);
            newPlayerPrompt.Children.Add(LblErrorA);
            Canvas.SetZIndex(BtnNew, 2);
            Canvas.SetZIndex(newPlayerPrompt, 2);
            Canvas.SetLeft(newPlayerPrompt, GameCanvas.ActualWidth - newPlayerPrompt.Width / 2);
            Canvas.SetTop(newPlayerPrompt, GameCanvas.ActualHeight - newPlayerPrompt.Height / 2);
            newPlayerPrompt.Visibility = Visibility.Visible;
            Canvas.SetLeft(newPlayerPrompt, ((GameCanvas.ActualWidth / 2) - (newPlayerPrompt.Width / 2)));
            Canvas.SetTop(newPlayerPrompt, ((GameCanvas.ActualHeight / 2) - (newPlayerPrompt.Height / 2)));
            GameCanvas.Children.Add(newPlayerPrompt);

            BtnSubmitNewPlayer.Click += BtnSubmitNewPlayer_Click;
            BtnCancelR.Click += BtnCancel_Click;
            removePlayerPrompt.Children.Add(LblInputheaderR);
            removePlayerPrompt.Children.Add(playerbox);
            BtnSubmitRemovePlayer.Click += BtnSubmitRemovePlayer_Click;
            BtnCancelR.Click += BtnCancel_Click;
            removePlayerPrompt.Children.Add(BtnCancelR);
            removePlayerPrompt.Children.Add(BtnSubmitRemovePlayer);
            removePlayerPrompt.Children.Add(LblErrorR);
            Canvas.SetZIndex(BtnNew, 2);
            Canvas.SetZIndex(removePlayerPrompt, 2);
            Canvas.SetLeft(removePlayerPrompt, ((GameCanvas.ActualWidth / 2) - (removePlayerPrompt.Width / 2)));
            Canvas.SetTop(removePlayerPrompt, ((GameCanvas.ActualHeight / 2) - (removePlayerPrompt.Height / 2)));
            GameCanvas.Children.Add(removePlayerPrompt);

            BtnRemovePlayer.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            BtnRemovePlayer.Arrange(new Rect(0, 0, Application.Current.MainWindow.DesiredSize.Width, Application.Current.MainWindow.DesiredSize.Height));

            if (snakeplayers.Count < 1)
                BtnCancelA.IsEnabled = false;

            if (MULTIPLAYER)
            {
                BtnRemovePlayer.Click += BtnRemovePlayer_Click;
                dpButtons.Children.Add(BtnRemovePlayer);

                txtPlayers.Text = "players: " + SnakePlayer.CURPARTICIPANTS.ToString();
                txtPlayers.Height = BtnRemovePlayer.ActualHeight;
                txtPlayers.Foreground = Brushes.IndianRed;
                dpButtons.Children.Add(txtPlayers);

                BtnAddPlayer.Click += BtnAddPlayer_Click;
                dpButtons.Children.Add(BtnAddPlayer);
            }

            GameCanvas.Children.Add(BtnNew);

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

            foreach (UIElement el in dpButtons.Children)
            {
                el.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                el.Arrange(new Rect(0, 0, Application.Current.MainWindow.DesiredSize.Width, Application.Current.MainWindow.DesiredSize.Height));
            }

            Snakeplayers.Clear();
            SnakePlayer.CURPARTICIPANTS = 0;

            if (TIMER != null)
                TIMER = null;

            TIMER = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 155), //speed
            };

            TIMER.Tick += Time_Tick;
            TIMER.Start();
        }

        //Eventlistening
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            removePlayerPrompt.Visibility = newPlayerPrompt.Visibility = Visibility.Hidden;
            LblErrorA.Visibility = LblErrorR.Visibility = Visibility.Hidden;
            BtnPause.Visibility = Visibility.Visible;
            BtnPause.IsEnabled = true;

            if (SnakePlayer.CURPARTICIPANTS > 3)
            {
                BtnAddPlayer.IsEnabled = false;
                txtPlayers.Foreground = Brushes.IndianRed;
            }
            else if (SnakePlayer.CURPARTICIPANTS < 2)
            {
                BtnRemovePlayer.IsEnabled = false;
                txtPlayers.Foreground = Brushes.IndianRed;
            }
            else
            {
                txtPlayers.Foreground = Brushes.DarkGreen;
            }
        }
        private void BtnSubmitRemovePlayer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (playerbox.SelectedValue == null)
                    return;

                RemovePlayer(playerbox.SelectedValue.ToString());

                removePlayerPrompt.Visibility = Visibility.Hidden;
                BtnPause.IsEnabled = true;
                BtnPause.Visibility = Visibility.Visible;
                LblErrorR.Visibility = Visibility.Hidden;
                BtnRemovePlayer.IsEnabled = true;
                if (SnakePlayer.CURPARTICIPANTS < 2)
                {
                    BtnRemovePlayer.IsEnabled = false;
                    txtPlayers.Foreground = Brushes.IndianRed;
                }
                else
                {
                    txtPlayers.Foreground = Brushes.DarkGreen;
                }
                txtPlayers.Text = "players: " + SnakePlayer.CURPARTICIPANTS.ToString();
            }
            catch (Exception)
            {
                LblErrorR.Visibility = Visibility.Visible;
            }
        }
        private void BtnSubmitNewPlayer_Click(object sender, RoutedEventArgs e)
        {
            SnakePlayer playerTmp = null;
            try
            {
                if (InputNameA.Text == "" || InputIPA.Text == "" || InputPortA.Text == "" || InputIPA.Text.Split('.').Length != 4)
                    throw new Exception();

                playerTmp = AddPlayerToGame(InputNameA.Text, new IPEndPoint(IPAddress.Parse(InputIPA.Text), Int32.Parse(InputPortA.Text)), GameCanvas);
                if (playerTmp == null)
                    throw new Exception();

                snakeplayers.Add(playerTmp);
                newPlayerPrompt.Visibility = Visibility.Hidden;
                BtnPause.IsEnabled = true;
                BtnPause.Visibility = Visibility.Visible;
                LblErrorA.Visibility = Visibility.Hidden;
                BtnRemovePlayer.IsEnabled = true;
                if (SnakePlayer.CURPARTICIPANTS > 3)
                {
                    BtnAddPlayer.IsEnabled = false;
                    txtPlayers.Foreground = Brushes.Lavender;
                }
                else if (SnakePlayer.CURPARTICIPANTS < 2)
                {
                    BtnRemovePlayer.IsEnabled = false;
                    txtPlayers.Foreground = Brushes.IndianRed;
                }
                else
                {
                    txtPlayers.Foreground = Brushes.DarkGreen;
                }
                txtPlayers.Text = "players: " + SnakePlayer.CURPARTICIPANTS.ToString();
                BtnCancelA.IsEnabled = true;
            }
            catch (Exception)
            {
                LblErrorA.Visibility = Visibility.Visible;
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
            newPlayerPrompt.Visibility = Visibility.Visible;
            LblErrorA.Visibility = Visibility.Hidden;

            InputNameA.Text = InputIPA.Text = InputPortA.Text = "";

            newPlayerPrompt.Visibility = Visibility.Visible;
            BtnPause.Visibility = Visibility.Hidden;
            BtnPause.IsEnabled = false;

            Canvas.SetZIndex(newPlayerPrompt, 2);
            Canvas.SetLeft(newPlayerPrompt, GameCanvas.ActualWidth / 2 - newPlayerPrompt.Width / 2);
            Canvas.SetTop(newPlayerPrompt, GameCanvas.ActualHeight / 2 - newPlayerPrompt.Height / 2);
        }
        private void BtnRemovePlayer_Click(object sender, RoutedEventArgs e)
        {
            removePlayerPrompt.Visibility = Visibility.Visible;
            LblErrorR.Visibility = Visibility.Hidden;

            List<string> vals = new List<string>();
            foreach (SnakePlayer p in snakeplayers)
            {
                vals.Add(p.Name);
            }

            //Assign playernames to Comboboxitems
            playerbox.ItemsSource = vals;

            //select the last player by default
            playerbox.SelectedIndex = snakeplayers.Count - 1;
            removePlayerPrompt.Visibility = Visibility.Visible;
            BtnAddPlayer.IsEnabled = true;

            Canvas.SetZIndex(removePlayerPrompt, 2);
            Canvas.SetLeft(removePlayerPrompt, ((GameCanvas.ActualWidth / 2) - (removePlayerPrompt.Width / 2)));
            Canvas.SetTop(removePlayerPrompt, ((GameCanvas.ActualHeight / 2) - (removePlayerPrompt.Height / 2)));
        }
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            SnakePlayer.CURPARTICIPANTS = 0;
            snakeplayers.Clear();
            App.Current.MainWindow.Content = new MenupageSnake(); ;
        }

        //methods
        public void SpawnFood()
        {
            if (APPLE == null)
            {
                APPLE = new Apple(RANDOM.Next(0, (int)GameCanvas.ActualWidth - SnakePlayer.SIZEELEM - 30), RANDOM.Next(0, (int)GameCanvas.ActualHeight - SnakePlayer.SIZEELEM - 10));
                Canvas.SetLeft(APPLE.Shape, APPLE.X);
                Canvas.SetTop(APPLE.Shape, APPLE.Y);
                Canvas.SetZIndex(APPLE.Shape, 1);
                if (STARTED)
                {
                    GameCanvas.Children.Add(APPLE.Shape);
                    if (TIMER.Interval.Milliseconds > 49)
                    {
                        TIMER.Stop();
                        TIMER.Interval = new TimeSpan(0, 0, 0, 0, TIMER.Interval.Milliseconds - 5);
                        TIMER.Start();
                    }
                }
            }
        }
        public void Render()
        {
            if (App.Current.MainWindow.Content.GetType().Name != (typeof(GamepageSnake).Name))
                return;

            int helpVar = -1, scorePrev, tmpCount = -1;
            TextBlock[] tbPrev;
            SnakePlayer.UpdateRanking();
            if (helpVar > -1 && helpVar < snakeplayers.Count)
                helpVar = -1;

            foreach (SnakePlayer p in snakeplayers)
            {
                scorePrev = p.score;
                tbPrev = p.Scoretext;
                p.Render();
                CollisionDetection();
            }

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
                }
            }

            if (snakeplayers.Count == 1 && STARTED && MULTIPLAYER)
            {
                BtnPause.IsEnabled = false;
                BtnNew.Visibility = Visibility.Visible;
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
                    BtnNew.Visibility = Visibility.Hidden;
                    snakeplayers.Clear();
                    SnakePlayer.CURPARTICIPANTS = 0;
                    BtnPause.IsEnabled = true;
                    App.Current.MainWindow.Content = new GamepageSnake(MULTIPLAYER);
                };
            }

            if (snakeplayers.Count == 0 && STARTED && MULTIPLAYER)
            {
                BtnPause.IsEnabled = false;
                BtnNew.Visibility = Visibility.Visible;
                TIMER.Stop();
                GameCanvas.Children.Clear();
                GameCanvas.Background = gopic;
            }

            foreach (SnakePlayer p in snakeplayers) {
                p.Scoretext[1].Text = p.score.ToString();
            }

            if (STARTED)
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
        public void RemovePlayer(String name)
        {
            foreach (SnakePlayer p in Snakeplayers)
            {
                if (p.Name == name)
                {
                    Snakeplayers.Remove(p);
                    foreach (SnakeElem snk in p.Snake)
                    {
                        GameCanvas.Children.Remove(snk.Rect);
                    }
                    foreach (TextBlock tb in p.Scoretext)
                    {
                        spScores.Children.Remove(tb);
                    }
                    SnakePlayer.CURPARTICIPANTS--;
                    return;
                }
            }
        }
        public SnakePlayer AddPlayerToGame(String name, IPEndPoint ip_port, Canvas gamecanv)
        {
            foreach (SnakePlayer p in snakeplayers)
            {
                if (name == p.Name || ip_port == p.Address)
                {
                    return null;
                }
            }
            SnakePlayer player = new SnakePlayer(name, ip_port, gamecanv);
            foreach (TextBlock tb in player.Scoretext)
            {
                spScores.Children.Add(tb);
            }

            return player;
        }
        public static void CollisionDetection()
        {
            //detect collision with any snakebody
            foreach (SnakePlayer p in GamepageSnake.Snakeplayers)
            {
                if (p.Dead)
                    continue;

                foreach (SnakePlayer pl in GamepageSnake.Snakeplayers)
                {
                    if (pl.Dead)
                        continue;

                    SnakeplayersTmp = snakeplayers;

                    if (p == pl)
                    {
                        //detect collision with own snakebody
                        if (!p.Dead)
                        {
                            foreach (SnakeElem snk in p.Snake)
                            {
                                if (snk == p.Snake[0])
                                    continue;

                                if ((p.Snake[0].X < snk.X + snk.Rect.ActualWidth)
                                    && (p.Snake[0].X + p.Snake[0].Rect.Width > snk.X)
                                    && (p.Snake[0].Y < snk.Y + snk.Rect.ActualHeight)
                                    && (p.Snake[0].Y + p.Snake[0].Rect.Height > snk.Y))
                                {
                                    if (snakeplayersTmp.Contains(p))
                                        snakeplayersTmp.Remove(p);
                                }
                            }
                        }
                    }
                    else if (MULTIPLAYER)
                    {
                        foreach (SnakeElem snk in pl.Snake)
                        {
                            //detect collision with another snakebody
                            if ((p.Snake[0].X < snk.X + snk.Rect.ActualWidth)
                                && (p.Snake[0].X + p.Snake[0].Rect.Width > snk.X)
                                && (p.Snake[0].Y < snk.Y + snk.Rect.ActualHeight)
                                && (p.Snake[0].Y + p.Snake[0].Rect.Height > snk.Y))
                            {
                                if (snk == pl.Snake[0])
                                {
                                    if (snakeplayersTmp.Contains(pl))
                                        snakeplayersTmp.Remove(pl);
                                }

                                if (snakeplayersTmp.Contains(p))
                                    snakeplayersTmp.Remove(p);
                            }
                        }
                    }
                }
            }

            foreach (SnakePlayer dead in snakeplayers)
            {
                dead.Dead = true;
            }

            foreach (SnakePlayer alive in snakeplayersTmp)
            {
                alive.Dead = false;
            }

            foreach (SnakePlayer player in snakeplayers)
            {
                player.Scoretext[0].Foreground = (player.Dead) ? Brushes.Gray : (SolidColorBrush)(new BrushConverter().ConvertFromString(player.Color.ToString()));
                player.Scoretext[1].Foreground = (player.Dead) ? Brushes.Gray : (SolidColorBrush)(new BrushConverter().ConvertFromString(player.Color.ToString()));
            }
=======
        public static int sizeElem = 14;
        public static double speed;
        private static DispatcherTimer timer;
        public static List<SnakeElem> snakebody;
        private static Random rnd = new Random();
        private Apple apple;

        private void Game_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Up && GamepageSnake.snakebody[0].Direction != GamepageSnake.down)
                GamepageSnake.snakebody[0].Direction = GamepageSnake.up;
            if (e.Key == Key.Down && GamepageSnake.snakebody[0].Direction != GamepageSnake.up)
                GamepageSnake.snakebody[0].Direction = GamepageSnake.down;
            if (e.Key == Key.Left && GamepageSnake.snakebody[0].Direction != GamepageSnake.right)
                GamepageSnake.snakebody[0].Direction = GamepageSnake.left;
            if (e.Key == Key.Right && GamepageSnake.snakebody[0].Direction != GamepageSnake.left)
                GamepageSnake.snakebody[0].Direction = GamepageSnake.right;
        }

        public static int Score { get => score; set => score = value; }

        public void Render()
        {
            /*---------------add Apple to 'myCanvas.Children' if needed--------------*/
            if (apple == null)
                apple = new Apple(rnd.Next(0, 26), rnd.Next(0, 26));
            if (GameCanvas.Children.Contains(apple.Shape))
                GameCanvas.Children.Remove(apple.Shape);
            apple.Setfoodposition();
            if (GameCanvas.Children.Contains(apple.Shape))
                GameCanvas.Children.Remove(apple.Shape);
            GameCanvas.Children.Add(apple.Shape);


            /*---------------remove snakeElements from and add them again to 'myCanvas.Children'--------------*/
            if (GameCanvas.Children.Count != 0)
            {
                foreach (SnakeElem snkEl in snakebody)
                {
                    if (GameCanvas.Children.Contains(snkEl.Rect))
                        GameCanvas.Children.Remove(snkEl.Rect);
                }

                foreach (SnakeElem snk in snakebody)
                {
                    Canvas.SetLeft(snk.Rect, (int)snk.X);
                    Canvas.SetTop(snk.Rect, (int)snk.Y);
                    if (!(GameCanvas.Children.Contains(snk.Rect)))
                        GameCanvas.Children.Add(snk.Rect);
                }
            }
        }

        public GamepageSnake()
        {
            InitializeComponent();
            speed = 1.0;
            timer = new DispatcherTimer();
            snakebody = new List<SnakeElem>
            {
                new SnakeElem
                {
                    X = 100,
                    Y = 100,
                    Rect = new Rectangle
                    {
                        Fill = Brushes.GreenYellow,
                        Width = sizeElem,
                        Height = sizeElem
                    }
                }
            };
            Render();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 250); //speed
            timer.Tick += Time_Tick;
        }

        private void MoveSnake()
        {
            SnakeElem head = snakebody[0];
            for (int i = snakebody.Count - 1; i >= 0; i--)
            {
                /*---------------collisiondetection with own body --------------*/
                double nextX = head.X + ((head.Direction == GamepageSnake.left) ? -sizeElem :
                    (head.Direction == GamepageSnake.right) ? sizeElem : 0);
                double nextY = head.Y + ((head.Direction == GamepageSnake.up) ? -sizeElem :
                    (head.Direction == GamepageSnake.down) ? sizeElem : 0);
                foreach (SnakeElem snk in snakebody)
                {
                    if (!(nextX > snk.X
                        && nextX < (snk.X + snk.Rect.Width)
                        && nextY > snk.Y
                        && nextY < (snk.Y + snk.Rect.Height)))
                    {
                        GameCanvas.Children.Clear();
                        this.Content = new GameOverPage();
                    }
                }
                /*---------------make body follow the head --------------*/
                if (i >= 1)
                {
                    snakebody[i].X = snakebody[i - 1].X;
                    snakebody[i].Y = snakebody[i - 1].Y;
                }

                if (i == 0)
                {
                    /*---------------update Position of snakeHead --------------*/
                    if (head.Direction == (int) stay)
                        return;

                    head.X += ((head.Direction == GamepageSnake.left) ? -sizeElem :
                        (head.Direction == GamepageSnake.right) ? sizeElem : 0);
                    head.Y += ((head.Direction == GamepageSnake.up) ? -sizeElem :
                        (head.Direction == GamepageSnake.down) ? sizeElem : 0);

                    if (head.X < 0)
                        head.X = GameCanvas.ActualWidth - head.Rect.Width;
                    if (head.X > (GameCanvas.ActualWidth - head.Rect.Width))
                        head.X = 0;

                    if (head.Y < 0)
                        head.Y = GameCanvas.ActualHeight - head.Rect.Height;
                    if (head.Y > (GameCanvas.ActualHeight))
                        head.Y = 0;
                }
            }

            Render();
        }

        public void Time_Tick(object sender, EventArgs e)
        {
            txtbScore.Text = score.ToString();

            MoveSnake();

            if ((snakebody[0].X <= apple.X + apple.Shape.Width && snakebody[0].X + snakebody[0].Rect.Width >= apple.X)
                && (snakebody[0].Y <= apple.Y + apple.Shape.Height && snakebody[0].Y + snakebody[0].Rect.Height >= apple.Y))
            {
                GameCanvas.Children.Remove(apple.Shape);
                apple.X = rnd.Next(0, (int)GameCanvas.ActualWidth - (int)apple.Shape.Width);
                apple.Y = rnd.Next(0, (int)GameCanvas.ActualHeight - (int)apple.Shape.Height);
                SnakeElem snakeTmp = new SnakeElem
                {
                    X = -100,
                    Y = -100,
                    Rect = new Rectangle
                    {
                        Fill = Brushes.Yellow,
                        Width = sizeElem,
                        Height = sizeElem
                    }
                };
                int dirTmp = snakebody[snakebody.Count - 1].Direction;
                snakeTmp.X = snakebody[0].X +
                    ((dirTmp == left) ? (-sizeElem * (snakebody.Count - 2)) - sizeElem :
                    ((dirTmp == right) ? (snakebody[snakebody.Count - 1].Rect.Width * (snakebody.Count - 1)) + sizeElem : 0));
                snakebody.Add(snakeTmp);
                Render();
                score++;
                timer.Stop();
                if (timer.Interval.Milliseconds > 4)
                    timer.Interval -= new TimeSpan(0, 0, 0, 0, 5);
                timer.Start();
            }

            Render();
>>>>>>> fea_hangman
        }
    }
}
