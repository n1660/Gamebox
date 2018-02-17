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
            stay,
            left,
            up,
            right,
            down
        };

        public static bool started, dead = started = false;
        public static int sizeElem = 14, score = 0;
        public static double xtmp, ytmp;
        public static DispatcherTimer timer;
        public static List<SnakeElem> snakebody;
        private static Random rnd = new Random();
        public Apple apple;

        public static int Score { get => score; set => score = value; }

        public void Render()
        {
            if (snakebody == null ||snakebody.Count == 0)
                return;

            SnakeElem head = snakebody[0];
            foreach (SnakeElem snk in snakebody)
            {
                if (snk == head)
                    continue;

                Console.WriteLine();
                Console.WriteLine(head.X >= snk.X
                        && head.X <= (snk.X + snk.Rect.ActualWidth)
                        && head.Y >= snk.Y
                        && head.Y <= (snk.Y + snk.Rect.ActualHeight));
                Console.WriteLine();

                if (head.X >= snk.X
                    && head.X <= (snk.X + snk.Rect.ActualWidth)
                    && head.Y >= snk.Y
                    && head.Y <= (snk.Y + snk.Rect.ActualHeight))
                {
                    if (GameCanvas.IsInitialized)
                    {
                        Console.WriteLine("jo, i bims");
                        GameOver();
                    }
                }
            }

            if (!dead && started && GameCanvas.IsInitialized)
            {
                //remove snakeElements from and add them again to 'myCanvas.Children'
                if (GameCanvas.IsInitialized)
                {
                    foreach (SnakeElem snkEl in snakebody)
                    {
                        if (GameCanvas.Children.Contains(snkEl.Rect))
                            GameCanvas.Children.Remove(snkEl.Rect);
                    }

                    //wrap-around
                    if (head.Direction != (int)Directions.stay)
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

                    //set Position of snakeElements
                    foreach (SnakeElem snk in snakebody)
                    {
                        Canvas.SetLeft(snk.Rect, (int)snk.X);
                        Canvas.SetTop(snk.Rect, (int)snk.Y);
                        if (!(GameCanvas.Children.Contains(snk.Rect)))
                            GameCanvas.Children.Add(snk.Rect);
                    }
                }
            }
        }
        

        public GamepageSnake(bool needComponents)
        {
            if (needComponents)
            {
                InitializeComponent();
                started = true;
            }

            GameCanvas.Background = Brushes.Green; //y is the GameCanvas = null here?
            SpawnFood();

            timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 200), //speed
            };

            snakebody = null;
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
            Canvas.SetLeft(snakebody[0].Rect, (int)snakebody[0].X);
            Canvas.SetTop(snakebody[0].Rect, (int)snakebody[0].Y);
            if (!(GameCanvas.Children.Contains(snakebody[0].Rect)))
                GameCanvas.Children.Add(snakebody[0].Rect);

            timer.Tick += Time_Tick;
            timer.Start();

            if (GameCanvas.IsInitialized && apple != null)
                Render();
        }

        private void MoveSnake()
        {
            if (snakebody.Count > 1)
                return;

            SnakeElem head = snakebody[0];
            if (GameCanvas.IsInitialized)
            {
                for (int i = snakebody.Count - 1; i >= 0; i--)
                {
                    //move head
                    head.X += ((head.Direction == Directions.left) ? -sizeElem :
                        (head.Direction == Directions.right) ? sizeElem: 0);
                    head.Y += ((head.Direction == Directions.up) ? -sizeElem :
                        (head.Direction == Directions.down) ? sizeElem : 0);

                    //make body follow the head
                    if (i > 0)
                    {
                        snakebody[i].X = snakebody[i - 1].X;
                        snakebody[i].Y = snakebody[i - 1].Y;
                    }

                    if (i == 0)
                    {
                        xtmp = head.X;
                        ytmp = head.Y;
                    }

                    //draw
                    Render();
                }
            }
        }

        public void Time_Tick(object sender, EventArgs e)
        {
            if (!started)
            {
                timer.Stop();
            }
            Console.WriteLine(dead);
            if(txtbScore != null)
                txtbScore.Text = score.ToString();

            MoveSnake();
            SnakeEat();

            Render();
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
                    SpawnFood();
                    SnakeElem lastElem = snakebody[snakebody.Count - 1];
                    SnakeElem snakeTmp = new SnakeElem
                    {
                        X = snakebody[0].X +
                            ((lastElem.Direction == Directions.left) ? -sizeElem :
                            ((lastElem.Direction == Directions.right)? sizeElem : 0)),
                        Y = snakebody[0].Y +
                            ((lastElem.Direction == Directions.down) ? -sizeElem :
                            ((lastElem.Direction == Directions.up) ? sizeElem : 0)),
                        Rect = new Rectangle
                        {
                            Fill = Brushes.Yellow,
                            Width = sizeElem,
                            Height = sizeElem
                        }
                    };
                    snakebody.Add(snakeTmp);
                    Render();
                    score++;
                    timer.Stop();
                    if (timer.Interval.Milliseconds > 150)
                        timer.Interval -= new TimeSpan(0, 0, 0, 0, 1);
                    timer.Start();
                }
            }
        }

        public void SpawnFood()
        {
            if(apple == null)
                apple = new Apple(-100, -100);
            apple = new Apple(rnd.Next(0, (int)GameCanvas.ActualWidth - (int)apple.Shape.ActualWidth), rnd.Next(0, (int)GameCanvas.ActualHeight - (int)apple.Shape.ActualHeight));
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
