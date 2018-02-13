using System.Windows;
using System.Windows.Input;


namespace SnakeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum Directions
        {
            stay,
            left,
            up,
            right,
            down
        };
    }
}