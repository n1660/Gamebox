using System.Windows.Shapes;

namespace SnakeGame
{
    public class SnakeElem
    {
        private GamepageSnake.Directions direction;
        private int x, y;
        private Rectangle rect;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public Rectangle Rect { get => rect; set => rect = value; }
        public GamepageSnake.Directions Direction { get => direction; set => direction = value; }
    }
}
