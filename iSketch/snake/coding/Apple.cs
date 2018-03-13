using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace test
{
    public class Apple
    {
        private double x = -1.0, y = -1.0;
        private Ellipse shape;
        public static ImageBrush applepic = new ImageBrush
        {
            ImageSource = new BitmapImage(new Uri("../../Images/apple.png", UriKind.RelativeOrAbsolute))
        };

        public Apple(double left,double top)
        {
            shape = new Ellipse
            {
                Fill = applepic,
                Width = SnakePlayer.SIZEELEM * 2,
                Height = SnakePlayer.SIZEELEM * 2,
                Stretch = Stretch.Fill
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
