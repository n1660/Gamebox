using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace test
{
    /// <summary>
    /// Interaktionslogik für GamepageSnake.xaml
    /// </summary>
    public partial class GamepageSnake : Page
    {
        //membervariables
        private static List<SnakePlayer> snakeplayers = new List<SnakePlayer>();
        private static List<SnakePlayer> snakeplayersTmp = new List<SnakePlayer>();

        //globals
        public static bool STARTED = false, MULTIPLAYER = false;
        public static DispatcherTimer TIMER;
        public static Random RANDOM = new Random();
        public static Apple APPLE;
        private bool tmp = false;

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
        public Button BtnExit = new Button
        {
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Right,
            Background = Brushes.Red,
            Foreground = Brushes.White,
            Width = 40,
            Content = "X"
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
            Text = IPAddress.Loopback.ToString()
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

            bgpic.Opacity = 0.6;
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
            BtnCancelA.Click += BtnCancel_Click;
            BtnCancelR.Click += BtnCancel_Click;
            removePlayerPrompt.Children.Add(LblInputheaderR);
            removePlayerPrompt.Children.Add(playerbox);
            BtnSubmitRemovePlayer.Click += BtnSubmitRemovePlayer_Click;
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
                InputIPA.Visibility = Visibility.Visible;
                InputPortA.Visibility = Visibility.Visible;
                LblIPA.Visibility = Visibility.Visible;
                LblPortA.Visibility = Visibility.Visible;
                BtnRemovePlayer.Click += BtnRemovePlayer_Click;
                dpButtons.Children.Add(BtnRemovePlayer);

                txtPlayers.Text = "players: " + SnakePlayer.CURPARTICIPANTS.ToString();
                txtPlayers.Height = BtnRemovePlayer.ActualHeight;
                txtPlayers.Foreground = Brushes.IndianRed;
                dpButtons.Children.Add(txtPlayers);

                BtnAddPlayer.Click += BtnAddPlayer_Click;
                dpButtons.Children.Add(BtnAddPlayer);
            }
            else
            {
                InputIPA.Visibility = Visibility.Hidden;
                InputPortA.Visibility = Visibility.Hidden;
                LblIPA.Visibility = Visibility.Hidden;
                LblPortA.Visibility = Visibility.Hidden;
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
                if ((MULTIPLAYER && (InputIPA.Text == "" || InputPortA.Text == "" || InputIPA.Text.Split('.').Length != 4)) || InputNameA.Text == "")
                    throw new Exception();

                playerTmp = AddPlayerToGame(InputNameA.Text, (MULTIPLAYER) ? new IPEndPoint(IPAddress.Parse(InputIPA.Text), Int32.Parse(InputPortA.Text)) : new IPEndPoint(IPAddress.Loopback, 666), GameCanvas);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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

            InputNameA.Text = InputPortA.Text = "";
            InputIPA.Text = IPAddress.Loopback.ToString();

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
        }
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            GamepageSnake.STARTED = false;
            GamepageSnake.TIMER.Stop();
            SnakePlayer.CURPARTICIPANTS = 0;
            snakeplayers.Clear();
            App.Current.MainWindow.Content = new MenupageSnake(); ;
        }

    //methods
    private void BtnStartSnake_Click(object sender, RoutedEventArgs e)
    {
        App.Current.MainWindow.Content = new GamepageSnake(((GamepageSnake.MULTIPLAYER) ? true : false));
    }
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
            List<SnakePlayer> movementlist = new List<SnakePlayer>(snakeplayers.Count);
            foreach (SnakePlayer toMove in snakeplayers)
            {
                movementlist.Add(toMove);
                toMove.Render();
            }

            snakeplayers.Clear();

            foreach (SnakePlayer moved in movementlist)
            {
                snakeplayers.Add(moved);
            }

            foreach (SnakePlayer player in snakeplayers)
            {
                player.Scoretext[0].Foreground = (SolidColorBrush)(new BrushConverter().ConvertFromString(player.Color.ToString()));
                player.Scoretext[1].Foreground = (SolidColorBrush)(new BrushConverter().ConvertFromString(player.Color.ToString()));
            }

            if (App.Current.MainWindow.Content.GetType().Name != (typeof(GamepageSnake).Name))
                return;

            if (GamepageSnake.STARTED)
                CollisionDetection();

            if (snakeplayers.Count == 1 && STARTED && MULTIPLAYER)
            {
                BtnPause.IsEnabled = false;
                BtnNew.Visibility = Visibility.Visible;
                SnakePlayer winner = snakeplayers[0];
                foreach (SnakePlayer p in snakeplayers)
                {
                    winner = p;
                    break;
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
                    SnakePlayer.CURPARTICIPANTS = 0;
                    BtnPause.IsEnabled = true;
                    App.Current.MainWindow.Content = new GamepageSnake(MULTIPLAYER);
                };
            }

            snakeplayers.Clear();

            foreach (SnakePlayer p in movementlist)
            {
                p.Scoretext[1].Text = p.score.ToString();
                snakeplayers.Add(p);
            }
            if (snakeplayers.Count == 0 && STARTED && MULTIPLAYER)
            {
                BtnPause.IsEnabled = false;
                BtnNew.Visibility = Visibility.Visible;
                TIMER.Stop();
                GameCanvas.Children.Clear();
                GameCanvas.Background = gopic;
            }

            movementlist.Clear();

            if (STARTED)
            {
                SpawnFood();
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
            foreach (SnakePlayer origin in snakeplayers)
            {
                snakeplayersTmp.Add(origin);
            }
            //detect collision with any snakebody
            foreach (SnakePlayer p in GamepageSnake.snakeplayersTmp)
            {
                foreach (SnakePlayer pl in GamepageSnake.SnakeplayersTmp)
                {
                    if (p == pl)
                    {
                        //detect collision with own snakebody
                        foreach (SnakeElem snk in p.Snake)
                        {
                            if (snk == p.Snake[0])
                            {
                                if (!p.GameCanvas.Children.Contains(snk.Rect))
                                    p.GameCanvas.Children.Add(snk.Rect);

                                continue;
                            }

                            if ((p.Snake[0].X < snk.X + snk.Rect.ActualWidth)
                                && (p.Snake[0].X + p.Snake[0].Rect.Width > snk.X)
                                && (p.Snake[0].Y < snk.Y + snk.Rect.ActualHeight)
                                && (p.Snake[0].Y + p.Snake[0].Rect.Height > snk.Y))
                            {
                                if (snakeplayers.Contains(p))
                                    snakeplayers.Remove(p);
                            }
                        }
                    }
                    if (snakeplayers.Count < 2 || !STARTED)
                        return;

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
                snakeplayersTmp.Clear();
            }
        }
    }
}
