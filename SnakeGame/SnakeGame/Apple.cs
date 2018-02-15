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
    class Apple
    {
        public double x,y;
        public Rectangle poli = new Rectangle();
        public Apple(double x,double y)
        {
            this.x = x;
            this.y = y;
        }
        public void setfoodposition()
        {
            poli.Width = poli.Height = 15;
            poli.Fill = Brushes.Red;
            Canvas.SetLeft(poli, x);
            Canvas.SetTop(poli,y);
        }
    }
}
