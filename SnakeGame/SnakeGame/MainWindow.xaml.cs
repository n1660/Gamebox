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
        public static double speed, xtmp, ytmp;
        private static DispatcherTimer timer;
        private List<SnakeElem> snakebody;
        private static Random rnd = new Random();
        private Apple apple;
        private int direction = 0, left = 4, right = 6, up = 8, down = 2, score = 0, tileIndex = 1;

        public void Render()
        {
            /*---------------add Apple to 'myCanvas.Children' if needed--------------*/
            if (apple == null)
                apple = new Apple(rnd.Next(0, 26), rnd.Next(0, 26));
            if (mycanvas.Children.Contains(apple.Shape))
                mycanvas.Children.Remove(apple.Shape);
            apple.Setfoodposition();
            mycanvas.Children.Add(apple.Shape);
            /*---------------set Position of snakeElements --------------*/
            xtmp = snakebody[0].X;
            ytmp = snakebody[0].Y;
            switch (snakebody[0].Direction) {
                case 0:
                    {
                        break;
                    }
                default:
                    {
                        for (int i = snakebody.Count - 1; i > 0; i--)
                        {
                            snakebody[i].Direction = snakebody[i - 1].Direction;
                            if (xtmp == snakebody[i].X && ytmp == snakebody[i].Y)
                            {
                                snakebody[i].X = snakebody[i - 1].X + ((snakebody[i - 1].Direction == left) ? snakebody[i - 1].Rect.Width : ((snakebody[i - 1].Direction == right) ? -snakebody[i - 1].Rect.Width : 0));
                                snakebody[i].Y = snakebody[i - 1].Y + ((snakebody[i - 1].Direction == up) ? snakebody[i - 1].Rect.Height : ((snakebody[i - 1].Direction == down) ? -snakebody[i - 1].Rect.Height : 0));
                            }
                        }
                        break;
                    }
            }
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
                    foreach (SnakeElem s in snakebody)
                    {
                        Canvas.SetLeft(s.Rect, s.X);
                        Canvas.SetTop(s.Rect, s.Y);
                    }
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
                        Width = 16,
                        Height = 16
                    }
                }
            };
            Render();
            Console.WriteLine(snakebody.Count);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10); //speed
            timer.Tick += Time_Tick;
        }

        private void MoveHead()
        {
            snakebody[0].X -= (snakebody[0].Direction == left) ? speed : 0;
            snakebody[0].X += (snakebody[0].Direction == right) ? speed : 0;
            snakebody[0].Y -= (snakebody[0].Direction == up) ? speed : 0;
            snakebody[0].Y += (snakebody[0].Direction == down) ? speed : 0;
        }

        public void Time_Tick(object sender, EventArgs e)
        {
            txtbScore.Text = score.ToString();
            MoveHead();
            

            if ((snakebody[0].X <= apple.X + apple.Shape.Width && snakebody[0].X + snakebody[0].Rect.Width >= apple.X)
                && (snakebody[0].Y <= apple.Y + apple.Shape.Height && snakebody[0].Y + snakebody[0].Rect.Height >= apple.Y))
            {
                mycanvas.Children.Remove(apple.Shape);
                apple.X = rnd.Next(0, (int)mycanvas.ActualWidth - (int)apple.Shape.Width);
                apple.Y = rnd.Next(0, (int)mycanvas.ActualHeight - (int)apple.Shape.Height);
                Render();
                SnakeElem snakeTmp = new SnakeElem
                {
                    X = (direction == up || direction == down) ? snakebody[snakebody.Count - 1].X : snakebody[snakebody.Count - 1].X + snakebody[snakebody.Count - 1].Rect.Width,
                    Y = (direction == left || direction == right) ? snakebody[snakebody.Count - 1].Y : snakebody[snakebody.Count - 1].Y + snakebody[snakebody.Count - 1].Rect.Height,
                    Rect = new Rectangle
                    {
                        Fill = Brushes.Yellow,
                        Width = 14,
                        Height = 14
                    }
                };
                snakeTmp.X += (snakebody[0].Direction == up || snakebody[0].Direction == down) ? 1 : 0;
                snakeTmp.Y += (snakebody[0].Direction == up || snakebody[0].Direction == down) ? 0 : 1;
                snakebody.Add(snakeTmp);
                Render();
                score++;
                speed += 0.15;
            }

            if (snakebody[0].X < 0)
                snakebody[0].X = mycanvas.ActualWidth - snakebody[0].Rect.Width;
            if (snakebody[0].X > (mycanvas.ActualWidth - snakebody[0].Rect.Width))
                snakebody[0].X = 0;

            if (snakebody[0].Y < 0)
                snakebody[0].Y = mycanvas.ActualHeight - snakebody[0].Rect.Height;
            if (snakebody[0].Y > (mycanvas.ActualHeight))
                snakebody[0].Y = 0;

            //TODO: Collisiondetection with own body

            Render();
            tileIndex++;
            if (tileIndex >= snakebody.Count)
                tileIndex = 1;
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
