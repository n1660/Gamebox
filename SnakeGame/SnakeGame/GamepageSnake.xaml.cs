﻿using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
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

        public static int sizeElem = 14, score = 0;
        public static double speed, xtmp, ytmp;
        public static DispatcherTimer timer;
        public static List<SnakeElem> snakebody;
        private static Random rnd = new Random();
        private Apple apple;

        public static int Score { get => score; set => score = value; }

        public void Render()
        {
            GameCanvas.Children.Clear();
            /*---------------add Apple to 'myCanvas.Children' if needed--------------*/
            if (apple == null)
                apple = new Apple(rnd.Next(0, 26), rnd.Next(0, 26));
            apple.Setfoodposition();
            if (GameCanvas.Children.Contains(apple.Shape))
                GameCanvas.Children.Remove(apple.Shape);
            GameCanvas.Children.Add(apple.Shape);


            /*---------------remove snakeElements from and add them again to 'myCanvas.Children'--------------*/
            if (GameCanvas.Children.Count > 2)
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
            timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 250), //speed
            };
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
            timer.Tick += Time_Tick;
            timer.Start();
            Render();
        }

        private void MoveSnake()
        {
            SnakeElem head = snakebody[0];
            for (int i = snakebody.Count - 1; i >= 0; i--)
            {
                /*---------------collisiondetection with own body --------------*/
                double nextX = head.X + ((head.Direction == Directions.left) ? -sizeElem :
                    (head.Direction == Directions.right) ? sizeElem : 0);
                double nextY = head.Y + ((head.Direction == Directions.up) ? -sizeElem :
                    (head.Direction == Directions.down) ? sizeElem : 0);
                foreach (SnakeElem snk in snakebody)
                {
                    if (!(nextX > snk.X
                        && nextX < (snk.X + snk.Rect.Width)
                        && nextY > snk.Y
                        && nextY < (snk.Y + snk.Rect.Height)))
                    {
                        GameCanvas.Children.Clear();
                        Console.WriteLine("bla");
                        //this.Content = new GameOverPage();
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
                    /*---------------set Position of snakeElements --------------*/
                    if (head.Direction == (int)Directions.stay)
                        return;

                    xtmp = head.X;
                    ytmp = head.Y;

                    head.X += ((head.Direction == Directions.left) ? -sizeElem :
                        (head.Direction == Directions.right) ? sizeElem : 0);
                    head.Y += ((head.Direction == Directions.up) ? -sizeElem :
                        (head.Direction == Directions.down) ? sizeElem : 0);

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
            Console.WriteLine(GamepageSnake.snakebody[0].Direction);

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
                Directions dirTmp = snakebody[snakebody.Count - 1].Direction;
                snakeTmp.X = snakebody[0].X +
                    ((dirTmp == Directions.left) ? (-sizeElem * (snakebody.Count - 2)) - sizeElem :
                    ((dirTmp == Directions.right) ? (snakebody[snakebody.Count - 1].Rect.Width * (snakebody.Count - 1)) + sizeElem : 0));
                snakebody.Add(snakeTmp);
                Render();
                score++;
                timer.Stop();
                if (timer.Interval.Milliseconds > 4)
                    timer.Interval -= new TimeSpan(0, 0, 0, 0, 5);
                timer.Start();
            }

            Render();
        }
    }
}
