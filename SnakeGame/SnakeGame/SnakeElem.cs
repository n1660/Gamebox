using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGame
{
    public class SnakeElem
    {
        private int direction = MainWindow.Stay;
        private double x, y;
        private Rectangle rect;

        public int Direction { get => direction; set => direction = value; }
        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public Rectangle Rect { get => rect; set => rect = value; }
    }
}
