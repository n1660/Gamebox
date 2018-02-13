using System.Windows.Shapes;

namespace SnakeGame
{
    public class SnakeElem
    {
        private int direction = (int)MainWindow.Directions.stay;
        private double x, y;
        private Rectangle rect;

        public int Direction { get => direction; set => direction = value; }
        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public Rectangle Rect { get => rect; set => rect = value; }
    }
}
