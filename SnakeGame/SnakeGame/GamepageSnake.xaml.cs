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
            up,
            stay
        };

        public static bool started, dead = started = false;
        public static int sizeElem = 14, score = 0;
        public static double xtmp, ytmp;
        public static DispatcherTimer timer;
        public static List<SnakeElem> snakebody;
        public static Random rnd = new Random();
        public Apple apple;
        private int startlength = 3;

        private ImageBrush snakeElemPic = new ImageBrush
        {
            ImageSource = new BitmapImage(new Uri("../../Images/snakeElem.jpg", UriKind.RelativeOrAbsolute))
        };

        private ImageBrush tailpic = new ImageBrush
        {
            ImageSource = new BitmapImage(new Uri("../../Images/snaketail.jpg", UriKind.RelativeOrAbsolute))
        };

        private ImageBrush bgpic = new ImageBrush
        {
            ImageSource = new BitmapImage(new Uri("../../Images/snake.jpg", UriKind.RelativeOrAbsolute))
        };

        public void Render()
        {
            if (snakebody == null || snakebody.Count == 0)
                return;

            if (apple == null)
                SpawnFood();
            else
            {
                Canvas.SetLeft(apple.Shape, apple.X);
                Canvas.SetTop(apple.Shape, apple.Y);
            }

            SnakeElem head = snakebody[0];
            foreach (SnakeElem snk in snakebody)
            {
                if (snk == head)
                    continue;

                if (head.X > snk.X
                    && head.X < (snk.X + snk.Rect.ActualWidth)
                    && head.Y > snk.Y
                    && head.Y < (snk.Y + snk.Rect.ActualHeight))
                {
                    GameOver();
                }
            }

            if (!dead && started)
            {
                if (GameCanvas.IsInitialized)
                {
                    //wrap-around
                    if (head.Direction != Directions.stay)
                    {
                        if (head.X < 0)
                            head.X = GameCanvas.ActualWidth - head.Rect.ActualWidth;
                        if (head.X > (GameCanvas.ActualWidth - head.Rect.ActualWidth))
                            head.X = 0;

                        if (head.Y < 0)
                            head.Y = GameCanvas.ActualHeight - head.Rect.ActualHeight;
                        if (head.Y > (GameCanvas.ActualHeight))
                            head.Y = 0;
                    }

                    //remove snakeElements from and add them again to 'myCanvas.Children'
                    GameCanvas.Children.Clear();
                    GameCanvas.Children.Add(apple.Shape);

                    //set Position of snakeElements
                    foreach (SnakeElem snk in snakebody)
                    {
                        Canvas.SetLeft(snk.Rect, (int)snk.X);
                        Canvas.SetTop(snk.Rect, (int)snk.Y);
                        GameCanvas.Children.Add(snk.Rect);
                    }
                }
            }
        }

        public GamepageSnake()
        {
            InitializeComponent();
                
            GameCanvas.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            GameCanvas.Arrange(new Rect(0, 0, Application.Current.MainWindow.DesiredSize.Width, Application.Current.MainWindow.DesiredSize.Height));

            timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 200), //speed
            };

            if (!started)
            {
                snakebody = null;
                ImageBrush headpic = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri("../../Images/snakehead.jpg", UriKind.RelativeOrAbsolute))
                };
                snakebody = new List<SnakeElem>
                {
                new SnakeElem
                    {
                        X = 100,
                        Y = 100,
                        Direction = Directions.stay,
                        Rect = new Rectangle
                        {
                            Fill = headpic,
                            Width = sizeElem,
                            Height = sizeElem
                        }
                    }
                };
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
                GameCanvas.Children.Add(snakebody[0].Rect);
                started = true;
            }
            Canvas.SetLeft(snakebody[0].Rect, (int)snakebody[0].X);
            Canvas.SetTop(snakebody[0].Rect, (int)snakebody[0].Y);

            timer.Tick += Time_Tick;
            timer.Start();
            
            Render();
        }

        private void MoveSnake()
        {
            if (snakebody[0].Direction == Directions.stay)
                return;
            SnakeElem head = snakebody[0];

            //move head
            head.X += ((head.Direction == Directions.left) ? -sizeElem :
                (head.Direction == Directions.right) ? sizeElem : 0);
            head.Y += ((head.Direction == Directions.up) ? -sizeElem :
                (head.Direction == Directions.down) ? sizeElem : 0);

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

                    //rotate texture
                    if (i == 1)
                    {
                        snakebody[i].X = head.X + ((head.Direction == Directions.right) ? -sizeElem :
                            (head.Direction == Directions.left) ? sizeElem : 0);
                        snakebody[i].Y = head.Y + ((head.Direction == Directions.down) ? -sizeElem :
                            (head.Direction == Directions.up) ? sizeElem : 0);
                    }
                }
            }
            snakebody[0] = head;
        }

        public void Time_Tick(object sender, EventArgs e)
        {
            txtbScore.Text = score.ToString();

            snakebody[snakebody.Count - 1].Rect.Fill = tailpic;
            snakebody[snakebody.Count - 2].Rect.Fill = snakeElemPic;

            MoveSnake();
            SnakeEat();
            Render();
        }

        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
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
                if ((snakebody[0].X <= apple.X + apple.Shape.ActualWidth && snakebody[0].X + snakebody[0].Rect.ActualWidth >= apple.X)
                    && (snakebody[0].Y <= apple.Y + apple.Shape.ActualHeight && snakebody[0].Y + snakebody[0].Rect.ActualHeight >= apple.Y)
                    && snakebody[0].Direction != Directions.stay)
                {
                    GameCanvas.Children.Remove(apple.Shape);
                    apple = null;
                    SpawnFood();
                    SnakeElem lastElem = snakebody[snakebody.Count - 1];
                    SnakeElem snakeTmp = new SnakeElem
                    {
                        X = lastElem.X + 1,
                        Y = lastElem.Y + 1,
                        Rect = new Rectangle
                        {
                            Fill = snakeElemPic,
                            Width = sizeElem,
                            Height = sizeElem
                        }
                    };
                    Render();
                    snakebody.Add(snakeTmp);
                    score++;
                    timer.Stop();
                    if (timer.Interval.Milliseconds > 50)
                        timer.Interval = new TimeSpan(0, 0, 0, 0, timer.Interval.Milliseconds - 5);
                    Console.WriteLine(timer.Interval.Milliseconds);
                    timer.Start();
                }
            }
        }

        public void SpawnFood()
        {
            if(apple == null)
                apple = new Apple(rnd.Next(0, (int)GameCanvas.ActualWidth - sizeElem), rnd.Next(0, (int)GameCanvas.ActualHeight - sizeElem));

            Console.WriteLine(apple.X + " | " + apple.Y);
        }

        public void GameOver()
        {
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
