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
        DispatcherTimer time;
        List<Snake> snakebody;
        List<Apple> food;
        Random rd = new Random();
        double x = 100;
        double y = 100;
        int direction = 0;
        int left = 4;
        int right = 6;
        int up = 8;
        int down = 2;
        int score = 0;
        int count = 0;
        public MainWindow()
        {
            InitializeComponent();
            time = new DispatcherTimer();
            snakebody = new List<Snake>();
            food = new List<Apple>();
            snakebody.Add(new Snake(x, y));
            food.Add(new Apple(rd.Next(0, 37) * 10, rd.Next(0, 35) * 10));
            time.Interval = new TimeSpan(0, 0, 0, 0, 100); // spped
            time.Tick += time_Tick;
        }

        void addfoodincanvas()
        {
            food[0].setfoodposition();
            mycanvas.Children.Insert(0, food[0].poli);
        }


        void addsnakeincanvas()
        {
            foreach (Snake snake in snakebody)
            {
                snake.setsnakeposition();
                mycanvas.Children.Add(snake.polygon);
            }
        }


        void time_Tick(object sender, EventArgs e)
        {
            if (direction != 0)
            {
                for (int i = snakebody.Count - 1; i > 0; i--)
                {
                    snakebody[i] = snakebody[i - 1];
                }
            }


            if (direction == up)
                y -= 10;
            if (direction == down)
                y += 10;
            if (direction == left)
                x -= 10;
            if (direction == right)
                x += 10;


       
            if ((snakebody[0].x <= food[0].x + food[0].poli.Width && snakebody[0].x + snakebody[0].polygon.Width >= food[0].x)
                && (snakebody[0].y <= food[0].y + food[0].poli.Height && snakebody[0].y + snakebody[0].polygon.Height >= food[0].y))
            {
                Snake snakeTmp = new Snake();
                snakeTmp.x = (direction == up || direction == down) ? snakebody[snakebody.Count - 1].x : snakebody[snakebody.Count - 1].x + snakebody[snakebody.Count - 1].polygon.Width;
                snakeTmp.y = (direction == left || direction == right) ? snakebody[snakebody.Count - 1].y : snakebody[snakebody.Count - 1].y + snakebody[snakebody.Count - 1].polygon.Height;
                snakebody.Add(snakeTmp);

                food[0] = new Apple(rd.Next(0, 37) * 10, rd.Next(0, 35) * 10);

                addfoodincanvas();
                score++;
                txtbScore.Text = score.ToString();
            }

            snakebody[0] = new Snake(x, y);

         

            if (snakebody[0].x < 0)
                snakebody[0].x += mycanvas.ActualWidth;
            if (snakebody[0].x > mycanvas.ActualWidth)
                snakebody[0].x -= mycanvas.ActualWidth;

            if (snakebody[0].y < 0)
                snakebody[0].y += mycanvas.ActualHeight;
            if (snakebody[0].y > mycanvas.ActualHeight)
                snakebody[0].y -= mycanvas.ActualHeight;


            for (int i = 1; i < snakebody.Count; i++)
            {
                if (snakebody[0].x == snakebody[i].x && snakebody[0].y == snakebody[i].y)
                    this.Close();
            }


            for (int i = 0; i < mycanvas.Children.Count; i++)
            {
                if (mycanvas.Children[i] is Rectangle)
                    count++;
            }
            mycanvas.Children.RemoveRange(1, count);
            count = 0;
            addsnakeincanvas();
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up && direction != down)
                direction = up;
            if (e.Key == Key.Down && direction != up)
                direction = down;
            if (e.Key == Key.Left && direction != right)
                direction = left;
            if (e.Key == Key.Right && direction != left)
                direction = right;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            addsnakeincanvas();
            addfoodincanvas();
            time.Start();
        }
    }
}
