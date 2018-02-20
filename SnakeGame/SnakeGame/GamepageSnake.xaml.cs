using System;
using System.Collections.Generic;
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

        public static bool started = true, dead = false;
        public static int score, sizeElem = 14;
        public static DispatcherTimer timer;
        public static List<SnakeElem> snakebody = new List<SnakeElem>();
        public static Random rnd = new Random();
        public Apple apple;
        private int startlength = 3;

        public static ImageBrush snakeElemPic = new ImageBrush
        {
            ImageSource = new BitmapImage(new Uri("../../Images/snakeElem.jpg", UriKind.RelativeOrAbsolute))
        };

        public static ImageBrush headpic = new ImageBrush
        {
            ImageSource = new BitmapImage(new Uri("../../Images/snakehead/snakehead_right.png", UriKind.RelativeOrAbsolute))
        };

        public static ImageBrush tailpic = new ImageBrush
        {
            ImageSource = new BitmapImage(new Uri("../../Images/snaketail/snaketail_right.png", UriKind.RelativeOrAbsolute))
        };

        public static ImageBrush bgpic = new ImageBrush
        {
            ImageSource = new BitmapImage(new Uri("../../Images/snake.jpg", UriKind.RelativeOrAbsolute))
        };

        public static ImageBrush snakePausePic = new ImageBrush
        {
            ImageSource = new BitmapImage(new Uri("../../Images/snakepause.jpeg", UriKind.RelativeOrAbsolute))
        };

        public void Render()
        {
            if (!dead)
                GameCanvas.Background = bgpic;

            if (snakebody == null || snakebody.Count == 0)
                return;

            if (apple == null)
                SpawnFood();
            
            Canvas.SetLeft(apple.Shape, apple.X);
            Canvas.SetTop(apple.Shape, apple.Y);
            Canvas.SetZIndex(apple.Shape, 1);

            SnakeElem head = snakebody[0];
            SnakeElem tail = snakebody[snakebody.Count - 1];

            if (!dead && started)
            {
                //wrap-around
                if (head.X < 0)
                    head.X = GameCanvas.ActualWidth - head.Rect.ActualWidth;
                if (head.X > (GameCanvas.ActualWidth - head.Rect.ActualWidth))
                    head.X = 0;

                if (head.Y < 0)
                    head.Y = GameCanvas.ActualHeight - head.Rect.ActualHeight;
                if (head.Y > (GameCanvas.ActualHeight))
                    head.Y = 0;

                //remove snakeElements from and add them again to 'myCanvas.Children'
                GameCanvas.Children.Clear();
                GameCanvas.Children.Add(apple.Shape);

                //set Position of snakeElements
                foreach (SnakeElem snk in snakebody)
                {
                    if (snk == tail)
                    {
                        tailpic = new ImageBrush
                        {
                            ImageSource = new BitmapImage(new Uri("../../Images/snaketail/snaketail_" + GamepageSnake.snakebody[GamepageSnake.snakebody.Count - 1].Direction.ToString() + ".png", UriKind.RelativeOrAbsolute))
                        };
                        snk.Rect.Fill = tailpic;
                    }

                    Canvas.SetZIndex(snk.Rect, 0);
                    Canvas.SetLeft(snk.Rect, (int)snk.X);
                    Canvas.SetTop(snk.Rect, (int)snk.Y);
                    GameCanvas.Children.Add(snk.Rect);
                }
            }
        }

        public GamepageSnake()
        {
            bgpic.Opacity = 0.8;

            score = 0;
            dead = false;
            InitializeComponent();

            if(snakebody.Count != 0)
                snakebody.Clear();

            GameCanvas.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            GameCanvas.Arrange(new Rect(0, 0, Application.Current.MainWindow.DesiredSize.Width, Application.Current.MainWindow.DesiredSize.Height));

            timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 200), //speed
            };

            snakebody.Add(new SnakeElem
            {
                X = 100,
                Y = 100,
                Direction = Directions.right,
                    Rect = new Rectangle
                    {
                        Fill = headpic,
                        Width = sizeElem,
                        Height = sizeElem
                    }
                });
            for (int i = 0; i < startlength; i++)
            {
                snakebody.Add(new SnakeElem
                {
                    X = 100 - (i + 1) * sizeElem,
                    Y = 100,
                    Rect = new Rectangle
                    {
                        Fill = snakeElemPic,
                        Width = sizeElem,
                        Height = sizeElem
                    }
                });
            }

            Canvas.SetLeft(snakebody[0].Rect, (int)snakebody[0].X);
            Canvas.SetTop(snakebody[0].Rect, (int)snakebody[0].Y);

            timer.Tick += Time_Tick;
            timer.Start();
            
            Render();
        }

        private void MoveSnake()
        {
            SnakeElem head = snakebody[0];

            if (started)
            {
                //move head
                head.X += ((head.Direction == Directions.left) ? -sizeElem :
                    (head.Direction == Directions.right) ? sizeElem : 0);
                head.Y += ((head.Direction == Directions.up) ? -sizeElem :
                    (head.Direction == Directions.down) ? sizeElem : 0);
            }

            //detect collision with own body
            foreach(SnakeElem snk in snakebody)
            {
                if (snk == head)
                    continue;

                if ((head.X < snk.X + snk.Rect.ActualWidth)
                    && (head.X + head.Rect.ActualWidth > snk.X)
                    && (head.Y < snk.Y + snk.Rect.ActualHeight)
                    && (head.Y + head.Rect.ActualHeight > snk.Y))
                {
                    GameOver();
                }
            }

            if (GameCanvas.IsInitialized)
            {
                for (int i = snakebody.Count - 1; i > 0; i--)
                {
                    //make body follow the head
                    if (i > 1)
                    {
                        snakebody[i].X = snakebody[i - 1].X;
                        snakebody[i].Y = snakebody[i - 1].Y;
                    }

                    if (i == 1)
                    {
                        snakebody[i].X = head.X + ((head.Direction == Directions.right) ? -sizeElem :
                            (head.Direction == Directions.left) ? sizeElem : 0);
                        snakebody[i].Y = head.Y + ((head.Direction == Directions.down) ? -sizeElem :
                            (head.Direction == Directions.up) ? sizeElem : 0);
                    }
                    snakebody[i].Direction = snakebody[i - 1].Direction;

                }
            }
            snakebody[0] = head;
        }

        public void Time_Tick(object sender, EventArgs e)
        {
            BtnPause.IsEnabled = true;
            if (dead)
            {
                timer.Stop();
                BtnPause.IsEnabled = false;
            }
            else
            {
                txtbScore.Text = score.ToString();
                txtbScore.Visibility = Visibility.Visible;
            }

            MoveSnake();
            SnakeEat();
            Render();
        }

        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            UIElementCollection gameElems = new UIElementCollection(GameCanvas, this);
            if (!started)
            {
                GameCanvas.Background = bgpic;
                foreach(UIElement elem in gameElems)
                {
                    GameCanvas.Children.Add(elem);
                }
                timer.Start();
            }
            else
            {
                gameElems = GameCanvas.Children;
                GameCanvas.Children.Clear();
                GameCanvas.Background = snakePausePic;
                timer.Stop();
            }
            started = !started;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            GameOver();
        }

        public void SnakeEat()
        {
            if (snakebody != null && apple != null)
            {
                SnakeElem head = snakebody[0];

                if ((head.X <= apple.X + apple.Shape.ActualWidth)
                    && (head.X + head.Rect.ActualWidth >= apple.X)
                    && (head.Y <= apple.Y + apple.Shape.ActualHeight && snakebody[0].Y + snakebody[0].Rect.ActualHeight >= apple.Y))
                {
                    apple = null;
                    SpawnFood();
                    GameCanvas.Children.Remove(apple.Shape);
                    SnakeElem lastElem = snakebody[snakebody.Count - 1];
                    SnakeElem snakeTmp = new SnakeElem
                    {
                        X = lastElem.X + 1,
                        Y = lastElem.Y + 1,
                        Rect = new Rectangle
                        {
                            Fill = tailpic,
                            Width = sizeElem,
                            Height = sizeElem
                        }
                    };
                    snakebody.Add(snakeTmp);
                    snakebody[snakebody.Count - 2].Rect.Fill = snakeElemPic;
                    Render();
                    score++;
                    timer.Stop();
                    if (timer.Interval.Milliseconds > 50)
                        timer.Interval = new TimeSpan(0, 0, 0, 0, timer.Interval.Milliseconds - 5);
                    timer.Start();
                }
            }
        }

        public void SpawnFood()
        {
            if(apple == null)
                apple = new Apple(rnd.Next(0, (int)GameCanvas.ActualWidth - sizeElem - 30), rnd.Next(0, (int)GameCanvas.ActualHeight - sizeElem - 10));
        }

        public void GameOver()
        {
            timer.Stop();
            txtbScore.Visibility = Visibility.Hidden;
            txtsc.Text = "X.X";
            dead = true;
            GameCanvas.Children.Clear();
            ImageBrush gopic = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("../../Images/snake_gameover.png", UriKind.RelativeOrAbsolute))
            };
            GameCanvas.Background = gopic;
        }
    }
}
