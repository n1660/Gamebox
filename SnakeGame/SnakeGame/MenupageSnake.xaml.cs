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
        public StackPanel spMode = new StackPanel();

        public Canvas CanvStartSnake = new Canvas
        {
            Height = 180,
            Width = 150,
            Margin = new Thickness(0, 0, 0, 0)
        };

        public Button BtnTBStartSnakeSP = new Button
        {
            Background = Brushes.Transparent,
            Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x77, 0xAA, 0x77)),
            FontSize = 30,
            FontWeight = FontWeights.Bold,
            Content = "Single player"
        };

        public Button BtnTBStartSnakeMP = new Button
        {
            Background = Brushes.Transparent,
            Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x77, 0xAA, 0x77)),
            FontSize = 30,
            FontWeight = FontWeights.Bold,
            Content = "Multiplayer"
        };

        //images
        public static ImageBrush startpic = new ImageBrush
        {
            ImageSource = new BitmapImage(new Uri("../../Images/snakestart.png", UriKind.RelativeOrAbsolute)),
            Stretch = Stretch.Fill
        };

        //properties
        public static GamepageSnake GamePage { get => gamePage; set => gamePage = value; }

        //c'tor
        public MenupageSnake()
        {
            App.Current.MainWindow.MinWidth = 325;
            App.Current.MainWindow.MinHeight = 425;
            InitializeComponent();
            BtnTBStartSnakeSP.Click += BtnStartSnake_Click;
            BtnTBStartSnakeMP.Click += BtnStartSnake_Click;
            Canvas.SetBottom(spMode, -100);
            Canvas.SetLeft(spMode, -20);
            CanvStartSnake.Background = startpic;

            CanvStartSnake.Children.Add(spMode);
            spMode.Children.Add(BtnTBStartSnakeSP);
            spMode.Children.Add(BtnTBStartSnakeMP);
            GridMenu.Children.Add(CanvStartSnake);
        }

        //methods
        private void BtnStartSnake_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.Content = new GamepageSnake(((sender == BtnTBStartSnakeSP) ? false : true));
        }
    }
}