using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;

namespace SnakeGame
{
    public class Apple
    {
        private double x,y;
        private Ellipse shape;
        public Apple(double left,double top)
        {
            shape = new Ellipse
            {
                Fill = Brushes.Red,
                Width = GamepageSnake.sizeElem,
                Height = GamepageSnake.sizeElem
            };
            x = left;
            y = top;
        }

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public Ellipse Shape { get => shape; set => shape = value; }

        public void Setfoodposition()
        {
            Canvas.SetLeft(shape, x);
            Canvas.SetTop(shape, y);
        }
    }
}
