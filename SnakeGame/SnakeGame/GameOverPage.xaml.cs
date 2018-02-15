using System;
using System.Windows.Controls;

namespace SnakeGame
{
    /// <summary>
    /// Interaktionslogik für GameOverPage.xaml
    /// </summary>
    public partial class GameOverPage : Page
    {
        public GameOverPage()
        {
            InitializeComponent();
            GamepageSnake.snakebody[0].Direction = GamepageSnake.Directions.stay;
        }


        private void GameOver_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //if (e.Key == Key.Enter)
            //    MainWindow.GetWindow(Canvas_GO).Content = new GamepageSnake();
            Console.WriteLine("blubb");
        }
    }
}
