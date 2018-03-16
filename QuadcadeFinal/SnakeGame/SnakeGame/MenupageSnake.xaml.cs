using SnakeGame;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace SnakeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MenupageSnake : Page
    {
        //membervariables
        private static GamepageSnake gamePage;

        //globals

        //UIElements
        public Canvas BtnCanvStartSnake = new Canvas
        {
            Height = 180,
            Width = 150
        };

        public TextBlock BtnTBStartSnakeSP = new TextBlock
        {
            Background = Brushes.Transparent,
            Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x77, 0xAA, 0x77)),
            FontSize = 30,
            FontWeight = FontWeights.Bold,
            Text = "Single player"
        };

        public TextBlock BtnTBStartSnakeMP = new TextBlock
        {
            Background = Brushes.Transparent,
            Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x77, 0xAA, 0x77)),
            FontSize = 30,
            FontWeight = FontWeights.Bold,
            Text = "Multiplayer"
        };

        //images
        public static ImageBrush startpic = new ImageBrush
        {
            ImageSource = new BitmapImage(new Uri("../../Images/snakestart.png", UriKind.RelativeOrAbsolute))
        };

        //properties
        public static GamepageSnake GamePage { get => gamePage; set => gamePage = value; }

        //c'tor
        public MenupageSnake()
        {
            App.Current.MainWindow.Width = 300;
            App.Current.MainWindow.Height = 300;
            App.Current.MainWindow.ResizeMode = ResizeMode.NoResize;
            InitializeComponent();
            BtnTBStartSnakeSP.MouseDown += BtnStartSnake_Click;
            BtnTBStartSnakeMP.MouseDown += BtnStartSnake_Click;
            BtnTBStartSnakeSP.Typography.Capitals = BtnTBStartSnakeMP.Typography.Capitals = FontCapitals.AllSmallCaps;
            Canvas.SetBottom(BtnTBStartSnakeMP, -40);
            Canvas.SetLeft(BtnTBStartSnakeMP, 40);
            BtnCanvStartSnake.Background = startpic;

            BtnCanvStartSnake.Children.Add(BtnTBStartSnakeMP);
            BtnCanvStartSnake.Children.Add(BtnTBStartSnakeSP);
            GridMenu.Children.Add(BtnCanvStartSnake);
        }

        //methods
        private void BtnStartSnake_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.Content = new GamepageSnake(((sender == BtnTBStartSnakeSP) ? false : true));
        }
    }
}