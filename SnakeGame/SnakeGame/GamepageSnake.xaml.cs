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
        public static Random rnd = new Random();
        public Apple apple;

        public void Render()
        {
            if (snakebody == null || snakebody.Count == 0)
                return;

            if (apple != null && !GameCanvas.Children.Contains(apple.Shape))
            {
                Canvas.SetLeft(apple.Shape, apple.X);
                Canvas.SetTop(apple.Shape, apple.Y);
                GameCanvas.Children.Add(apple.Shape);
            }

            SnakeElem head = snakebody[0];
            foreach (SnakeElem snk in snakebody)
            {
                if (snk == head)
                    continue;

                if (head.X >= snk.X
                    && head.X <= (snk.X + snk.Rect.ActualWidth)
                    && head.Y >= snk.Y
                    && head.Y <= (snk.Y + snk.Rect.ActualHeight))
                {
                    GameOver();
                }
            }

            if (!dead && started)
            {
                if (GameCanvas.IsInitialized)
                {
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

            GameCanvas.Background = Brushes.Green;
            SpawnFood();

            timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 200), //speed
            };

            if (!started)
            {
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
                GameCanvas.Children.Add(snakebody[0].Rect);
                started = true;
            }
            Canvas.SetLeft(snakebody[0].Rect, (int)snakebody[0].X);
            Canvas.SetTop(snakebody[0].Rect, (int)snakebody[0].Y);

            timer.Tick += Time_Tick;
            timer.Start();

            if (apple != null)
                Render();
        }

        private void MoveSnake()
        {

            SnakeElem head = snakebody[0];
            if (GameCanvas.IsInitialized)
            {
                for (int i = snakebody.Count - 1; i >= 0; i--)
                {
                    //move head
                    head.X += ((head.Direction == Directions.left) ? -sizeElem :
                        (head.Direction == Directions.right) ? sizeElem : 0);
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
                }
            }snakebody[0] = head;
        }

        public void Time_Tick(object sender, EventArgs e)
        {
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
                    Console.WriteLine("in eatfunc");
                    GameCanvas.Children.Remove(apple.Shape);
                    SpawnFood();
                    SnakeElem lastElem = snakebody[snakebody.Count - 1];
                    SnakeElem snakeTmp = new SnakeElem
                    {
                        X = lastElem.X +
                            ((lastElem.Direction == Directions.left) ? -sizeElem :
                            ((lastElem.Direction == Directions.right)? sizeElem : 0)),
                        Y = lastElem.Y +
                            ((lastElem.Direction == Directions.down) ? -sizeElem :
                            ((lastElem.Direction == Directions.up) ? sizeElem : 0)),
                        Rect = new Rectangle
                        {
                            Fill = Brushes.Yellow,
                            Width = sizeElem,
                            Height = sizeElem
                        }
                    };
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
                apple = new Apple(rnd.Next(0, (int)GameCanvas.ActualWidth - sizeElem), rnd.Next(0, (int)GameCanvas.ActualHeight - sizeElem));
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
