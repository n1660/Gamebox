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
        private int playerID = 1; // TODO: NO HARDCODE
        private List<SnakePlayer> snakeplayers = new List<SnakePlayer>();

        //globals
        public static bool STARTED = false;
        public static DispatcherTimer TIMER;
        public static Random RANDOM = new Random();
        public static Apple APPLE;

        //images
        public static ImageBrush bgpic = new ImageBrush
        {
            ImageSource = new BitmapImage(new Uri("../../Images/snake.jpg", UriKind.RelativeOrAbsolute))
        };
        public static ImageBrush snakePausePic = new ImageBrush
        {
            ImageSource = new BitmapImage(new Uri("../../Images/snakepause.jpeg", UriKind.RelativeOrAbsolute))
        };

        //properties
        public int PlayerID { get => playerID; set => playerID = value; }
        public List<SnakePlayer> Snakeplayers { get => snakeplayers; set => snakeplayers = value; }

        //methods
        public void Render()
        {
            foreach(SnakePlayer p in snakeplayers)
            {
                p.Render();
            }
            if (snakeplayers[0].Snake.Count != 0) {
                int survivecount = snakeplayers.Count;
                foreach (SnakePlayer p in snakeplayers)
                {
                    if (p.Dead)
                        survivecount--;
                }
                //if (survivecount == 0)
                //TODO: GAMEOVER!
            }
            SpawnFood();
        }

        public GamepageSnake()
        {
            MainWindow.GamePage = this;

            bgpic.Opacity = 0.8;
            InitializeComponent();
            GameCanvas.Background = bgpic;
            snakeplayers.Add(AddPlayerToGame("player1", new IPEndPoint(IPAddress.Loopback, 1337), GameCanvas)
            );
            snakeplayers.Add(AddPlayerToGame("player2", new IPEndPoint(IPAddress.Loopback, 1338), GameCanvas)
            );

            GameCanvas.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            GameCanvas.Arrange(new Rect(0, 0, Application.Current.MainWindow.DesiredSize.Width, Application.Current.MainWindow.DesiredSize.Height));

            TIMER = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 200), //speed
            };

            TIMER.Tick += Time_Tick;
            TIMER.Start();
            Render();
        }

        public void Time_Tick(object sender, EventArgs e)
        {
            Render();
        }

        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            UIElementCollection gameElems = new UIElementCollection(GameCanvas, this);
            if (!STARTED)
            {
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
                TIMER.Stop();
            }
            STARTED = !STARTED;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {

            ImageBrush gopic = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("../../Images/snake_gameover.png", UriKind.RelativeOrAbsolute))
            };
            GameCanvas.Background = gopic;
        }

        public void SpawnFood()
        {
            if (APPLE == null)
            {
                APPLE = new Apple(RANDOM.Next(0, (int)GameCanvas.ActualWidth - SnakePlayer.SIZEELEM - 30), RANDOM.Next(0, (int)GameCanvas.ActualHeight - SnakePlayer.SIZEELEM - 10));
                Canvas.SetLeft(APPLE.Shape, APPLE.X);
                Canvas.SetTop(APPLE.Shape, APPLE.Y);
                Canvas.SetZIndex(APPLE.Shape, 1);
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
            SnakePlayer.AMOUNT_PLAYERS++;
            SnakePlayer player = new SnakePlayer(name, ip_port, gamecanv);

            spScores.Children.Add(new TextBlock
            {
                Name = "LblPlayer" + player.Id,
                FontSize = 22,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
                Text = player.Name + ": ",
                Foreground = (player.Dead) ? Brushes.Gray : Brushes.Black
            });
            return player;
        }
    }
}
