using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace iSketch
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow win;
        public ContentControl Page;

        public MainWindow()
        {
            InitializeComponent();
            win = this;

            this.Loaded += MainWindow_Loaded;

            // this.Content = new Menu();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Content = new Menu();
        }
    }
}
