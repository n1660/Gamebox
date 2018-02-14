using MySnake;
using System.Windows;


namespace MySnake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnStartSnake_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new GamepageSnake();
        }
    }
}