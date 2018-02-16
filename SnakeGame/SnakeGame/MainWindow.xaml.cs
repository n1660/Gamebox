using SnakeGame;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace SnakeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void BtnStartSnake_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new GamepageSnake(true);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.Content.GetType() == (new GamepageSnake(false)).GetType())
            {
                if (e.Key == Key.Up && GamepageSnake.snakebody[0].Direction != GamepageSnake.Directions.down)
                    GamepageSnake.snakebody[0].Direction = GamepageSnake.Directions.up;
                if (e.Key == Key.Down && GamepageSnake.snakebody[0].Direction != GamepageSnake.Directions.up)
                    GamepageSnake.snakebody[0].Direction = GamepageSnake.Directions.down;
                if (e.Key == Key.Left && GamepageSnake.snakebody[0].Direction != GamepageSnake.Directions.right)
                    GamepageSnake.snakebody[0].Direction = GamepageSnake.Directions.left;
                if (e.Key == Key.Right && GamepageSnake.snakebody[0].Direction != GamepageSnake.Directions.left)
                    GamepageSnake.snakebody[0].Direction = GamepageSnake.Directions.right;

                if(GamepageSnake.dead)
                {
                    if(e.Key == Key.Space)
                    {
                        GamepageSnake.dead = false;
                    }
                }
            }
        }
    }
}