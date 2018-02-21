using SnakeGame;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace SnakeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static GamepageSnake gamePage;

        public static ImageBrush startpic = new ImageBrush
        {
            ImageSource = new BitmapImage(new Uri("../../Images/snakestart.png", UriKind.RelativeOrAbsolute))
        };

        public static GamepageSnake GamePage { get => gamePage; set => gamePage = value; }

        public MainWindow()
        {
            InitializeComponent();
            BtnStartSnake.Background = startpic;
        }

        private void BtnStartSnake_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new GamepageSnake();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.Content.GetType().Name == (typeof(GamepageSnake).Name))
            {
                if (GamepageSnake.Snakeplayers[gamePage.PlayerID - 1].Snake.Count == 0)
                    return;

                if (e.Key == Key.Space && !GamepageSnake.STARTED)
                {
                    GamepageSnake.STARTED = true;
                }
                else if(GamepageSnake.STARTED)
                {
                    if (e.Key == Key.W && GamepageSnake.Snakeplayers[0].Snake[0].Direction != GamepageSnake.Directions.down)
                        GamepageSnake.Snakeplayers[0].Snake[0].Direction = GamepageSnake.Directions.up;
                    if (e.Key == Key.S && GamepageSnake.Snakeplayers[0].Snake[0].Direction != GamepageSnake.Directions.up)
                        GamepageSnake.Snakeplayers[0].Snake[0].Direction = GamepageSnake.Directions.down;
                    if (e.Key == Key.A && GamepageSnake.Snakeplayers[0].Snake[0].Direction != GamepageSnake.Directions.right)
                        GamepageSnake.Snakeplayers[0].Snake[0].Direction = GamepageSnake.Directions.left;
                    if (e.Key == Key.D && GamepageSnake.Snakeplayers[0].Snake[0].Direction != GamepageSnake.Directions.left)
                        GamepageSnake.Snakeplayers[0].Snake[0].Direction = GamepageSnake.Directions.right;

                    if (e.Key == Key.Up && GamepageSnake.Snakeplayers[1].Snake[0].Direction != GamepageSnake.Directions.down)
                        GamepageSnake.Snakeplayers[1].Snake[0].Direction = GamepageSnake.Directions.up;
                    if (e.Key == Key.Down && GamepageSnake.Snakeplayers[1].Snake[0].Direction != GamepageSnake.Directions.up)
                        GamepageSnake.Snakeplayers[1].Snake[0].Direction = GamepageSnake.Directions.down;
                    if (e.Key == Key.Left && GamepageSnake.Snakeplayers[1].Snake[0].Direction != GamepageSnake.Directions.right)
                        GamepageSnake.Snakeplayers[1].Snake[0].Direction = GamepageSnake.Directions.left;
                    if (e.Key == Key.Right && GamepageSnake.Snakeplayers[1].Snake[0].Direction != GamepageSnake.Directions.left)
                        GamepageSnake.Snakeplayers[1].Snake[0].Direction = GamepageSnake.Directions.right;
                }

                //reload headpic for the new direction
                GamepageSnake.Snakeplayers[gamePage.PlayerID - 1].Pictures["Head"] = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri("../../Images/" + GamepageSnake.Snakeplayers[gamePage.PlayerID - 1].Color + "/snakehead_" + GamepageSnake.Snakeplayers[gamePage.PlayerID - 1].Snake[0].Direction.ToString() + "_" + GamepageSnake.Snakeplayers[gamePage.PlayerID - 1].Color.ToString() + ".png", UriKind.RelativeOrAbsolute))
                };
                /*----------------------------------------------*/

                GamepageSnake.Snakeplayers[gamePage.PlayerID - 1].Snake[0].Rect.Fill = GamepageSnake.Snakeplayers[gamePage.PlayerID - 1].Pictures[Pictures.Head.ToString()];
            }
        }
    }
}