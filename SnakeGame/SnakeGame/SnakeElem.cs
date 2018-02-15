using System.Windows.Shapes;

namespace SnakeGame
{
    public class SnakeElem
    {
        private GamepageSnake.Directions direction = GamepageSnake.Directions.stay;
        private double x, y;
        private Rectangle rect;

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public Rectangle Rect { get => rect; set => rect = value; }
        public GamepageSnake.Directions Direction { get => direction; set => direction = value; }
    }
}
