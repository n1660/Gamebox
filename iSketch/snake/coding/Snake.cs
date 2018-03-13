using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace test
{
    class Snake
    {
        public double x;
        public double y;
        public Rectangle rect = new Rectangle
        {
            Fill = Brushes.Coral,
            Width = 15,
            Height = 15
        };
        public Snake(double x = 100, double y = 100)
        {
            this.x = x;
            this.y = y;
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
        }
        public void Setsnakeposition()
        {
            rect.Width = rect.Height = 15;
            rect.Fill = Brushes.White;
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
        }
    }
}
