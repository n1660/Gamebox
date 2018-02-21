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
                if (gamePage.Snakeplayers[gamePage.PlayerID - 1].Snake.Count == 0)
                    return;

                if (!GamepageSnake.started)
                {
                    GamepageSnake.started = true;
                }
                else
                {
                    if (e.Key == Key.Up && gamePage.Snakeplayers[gamePage.PlayerID - 1].Snake[0].Direction != GamepageSnake.Directions.down)
                        gamePage.Snakeplayers[gamePage.PlayerID - 1].Snake[0].Direction = GamepageSnake.Directions.up;
                    if (e.Key == Key.Down && gamePage.Snakeplayers[gamePage.PlayerID - 1].Snake[0].Direction != GamepageSnake.Directions.up)
                        gamePage.Snakeplayers[gamePage.PlayerID - 1].Snake[0].Direction = GamepageSnake.Directions.down;
                    if (e.Key == Key.Left && gamePage.Snakeplayers[gamePage.PlayerID - 1].Snake[0].Direction != GamepageSnake.Directions.right)
                        gamePage.Snakeplayers[gamePage.PlayerID - 1].Snake[0].Direction = GamepageSnake.Directions.left;
                    if (e.Key == Key.Right && gamePage.Snakeplayers[gamePage.PlayerID - 1].Snake[0].Direction != GamepageSnake.Directions.left)
                        gamePage.Snakeplayers[gamePage.PlayerID - 1].Snake[0].Direction = GamepageSnake.Directions.right;
                }

                //reload headpic for the new direction
                gamePage.Snakeplayers[gamePage.PlayerID - 1].Pictures[Pictures.Head.ToString()] = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri("../../Images/" + gamePage.Snakeplayers[gamePage.PlayerID - 1].Color + "/snakehead_" + gamePage.Snakeplayers[gamePage.PlayerID - 1].Snake[0].Direction.ToString() + "_" + gamePage.Snakeplayers[gamePage.PlayerID - 1].Color.ToString() + ".png", UriKind.RelativeOrAbsolute))
                };
                /*----------------------------------------------*/

                gamePage.Snakeplayers[gamePage.PlayerID - 1].Snake[0].Rect.Fill = gamePage.Snakeplayers[gamePage.PlayerID - 1].Pictures[Pictures.Head.ToString()];
            }
        }
    }
}