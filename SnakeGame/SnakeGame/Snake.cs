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
    class Snake
    {
        public double x;
        public double y;
        public Rectangle polygon = new Rectangle();
        public Snake(double x = 100,double y = 100)
        {
            this.x = x;
            this.y = y;
        }
        public void setsnakeposition()
        {
            polygon.Width = polygon.Height = 15;
            polygon.Fill = Brushes.White;
            Canvas.SetLeft(polygon, x);
            Canvas.SetTop(polygon, y);
        }
    }
}
