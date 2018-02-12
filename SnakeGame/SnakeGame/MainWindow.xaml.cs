using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SnakeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int sizeElem = 14;
        public static double speed, xtmp, ytmp;
        private static DispatcherTimer timer;
        private List<SnakeElem> snakebody;
        private static Random rnd = new Random();
        private Apple apple;
        private static int stay = 0, left = 4, right = 6, up = 8, down = 2, score = 0, tileIndex = 1;

        public static int Stay { get => stay; set => stay = value; }
        public static int Left1 { get => left; set => left = value; }
        public static int Right { get => right; set => right = value; }
        public static int Up { get => up; set => up = value; }
        public static int Down { get => down; set => down = value; }
        public static int Score { get => score; set => score = value; }

        public void Render()
        {
            /*---------------add Apple to 'myCanvas.Children' if needed--------------*/
            if (apple == null)
                apple = new Apple(rnd.Next(0, 26), rnd.Next(0, 26));
            if (mycanvas.Children.Contains(apple.Shape))
                mycanvas.Children.Remove(apple.Shape);
            apple.Setfoodposition();
            if (mycanvas.Children.Contains(apple.Shape))
                mycanvas.Children.Remove(apple.Shape);
            mycanvas.Children.Add(apple.Shape);


            /*---------------remove snakeElements from and add them again to 'myCanvas.Children'--------------*/
            if (mycanvas.Children.Count != 0)
            {
                foreach (SnakeElem snkEl in snakebody)
                {
                    if (mycanvas.Children.Contains(snkEl.Rect))
                        mycanvas.Children.Remove(snkEl.Rect);
                }

                foreach (SnakeElem snk in snakebody)
                {
                    Canvas.SetLeft(snk.Rect, (int) snk.X);
                    Canvas.SetTop(snk.Rect, (int) snk.Y);
                    if (!(mycanvas.Children.Contains(snk.Rect)))
                        mycanvas.Children.Add(snk.Rect);
                }
            }
        }

        public MainWindow()
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
            
            for (int i = snakebody.Count - 1; i >= 0; i--)
            {
                if (i >= 1)
                {
                    /*if (snakebody[i].X == xtmp && snakebody[i].Y == ytmp)
                        snakebody[i].Direction = head.Direction;*/

                    snakebody[i].X = snakebody[i - 1].X;
                    snakebody[i].Y = snakebody[i - 1].Y;
                    Console.WriteLine(snakebody[i].X + ", " + snakebody[i].Y);
                }

                if (i == 0)
                {
                    SnakeElem head = snakebody[0];
                    /*---------------set Position of snakeElements --------------*/
                    if (head.Direction == stay)
                        return;

                    xtmp = head.X;
                    ytmp = head.Y;

                    head.X += ((head.Direction == left) ? -sizeElem :
                        (head.Direction == right) ? sizeElem : 0);
                    head.Y += ((head.Direction == up) ? -sizeElem :
                        (head.Direction == down) ? sizeElem : 0);

                    if (head.X < 0)
                        head.X = mycanvas.ActualWidth - head.Rect.Width;
                    if (head.X > (mycanvas.ActualWidth - head.Rect.Width))
                        head.X = 0;

                    if (head.Y < 0)
                        head.Y = mycanvas.ActualHeight - head.Rect.Height;
                    if (head.Y > (mycanvas.ActualHeight))
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
                mycanvas.Children.Remove(apple.Shape);
                apple.X = rnd.Next(0, (int)mycanvas.ActualWidth - (int)apple.Shape.Width);
                apple.Y = rnd.Next(0, (int)mycanvas.ActualHeight - (int)apple.Shape.Height);
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
                Console.WriteLine(snakebody.Count + " | " + snakeTmp.X);
                Render();
                score++;
                timer.Stop();
                if(timer.Interval.Milliseconds > 4)
                    timer.Interval -= new TimeSpan(0, 0, 0, 0, 5);
                timer.Start();
            }

            //TODO: Collisiondetection with own body

            Render();
        }
        
        public void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up && snakebody[0].Direction != down)
                snakebody[0].Direction = up;
            if (e.Key == Key.Down && snakebody[0].Direction != up)
                snakebody[0].Direction = down;
            if (e.Key == Key.Left && snakebody[0].Direction != right)
                snakebody[0].Direction = left;
            if (e.Key == Key.Right && snakebody[0].Direction != left)
                snakebody[0].Direction = right;
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Render();
            timer.Start();
        }
    }
}
