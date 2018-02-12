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
using System.Timers;
using System.Windows.Threading;
using System.Threading;

namespace iSketch
{
    /// <summary>
    /// Interaktionslogik für Artist.xaml
    /// </summary>
    public partial class Artist : Page
    {
        private static int Timer_Seconds = 10;
        private static int Timer_Minutes = 0;
        private int counter = Timer_Seconds + (Timer_Minutes*60);
        private int Stroke_Thickness = 4;
        private int List_Length;
        private Point lastPoint;
        private SolidColorBrush colour = Brushes.Black;
        private static DispatcherTimer time;
        private static DispatcherTimer countdown;

        public Artist()
        {
            InitializeComponent();

            this.List_Length = Get_List_Length();
            this.Your_Word.Text = Get_Random_Word();
            CreateTimer();
            CreateContdown();
            //Thread counterthread = new Thread(CreateContdown); //////// !!!!!!
            Start_All();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (sender == this.BTN_Clear)
                this.MyCanvas.Children.Clear();

            else if (sender == this.BTN_Submit)
            {
                if (this.Your_Word.Text == this.Chat_Window.Text)
                {
                    Stop_All();
                    Console.Write("yay\n");
                    this.Chat_Window.Clear();
                    this.MyCanvas.Children.Clear();
                    this.Your_Word.Text = Get_Random_Word();
                    Start_All();
                }
                else
                {
                    Console.Write("No Way\n");
                    this.Chat_Window.Clear();
                }
            }
            else if (sender == this.BTN_HOME)
            {
                MainWindow.win.Content = new Menu();
            }

            // Colours
            else if (sender == this.Colour_1)
                colour = Brushes.Black;
            else if (sender == this.Colour_2)
                colour = (SolidColorBrush)this.Colour_2.Background;
            else if (sender == this.Colour_3)
                colour = Brushes.White;
            else if (sender == this.Colour_4)
                colour = (SolidColorBrush)this.Colour_4.Background;
            else if (sender == this.Colour_5)
                colour = (SolidColorBrush)this.Colour_5.Background;
            else if (sender == this.Colour_6)
                colour = (SolidColorBrush)this.Colour_6.Background;
            else if (sender == this.Colour_7)
                colour = (SolidColorBrush)this.Colour_7.Background;
            else if (sender == this.Colour_8)
                colour = (SolidColorBrush)this.Colour_8.Background;
            else if (sender == this.Colour_9)
                colour = (SolidColorBrush)this.Colour_9.Background;

            // Strocke Thickness
            else if (sender == this.Size_1)
                this.Stroke_Thickness = 8;

            else if (sender == this.Size_2)
                this.Stroke_Thickness = 4;

            else if (sender == this.Size_3)
                this.Stroke_Thickness = 2;
        }

        // For Painting  
        private void MyCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(this.MyCanvas); 

            this.lastPoint = p;

            Ellipse ell = new Ellipse();
            ell.Width = this.Stroke_Thickness *1.5;
            ell.Height = this.Stroke_Thickness *1.5;

            ell.Stroke = colour;
            ell.Fill = colour;
            ell.StrokeThickness = Stroke_Thickness;
            ell.SetValue(Canvas.LeftProperty, p.X - Stroke_Thickness);
            ell.SetValue(Canvas.TopProperty, p.Y - Stroke_Thickness);
            this.MyCanvas.Children.Add(ell);
        }

        // TIMER!!!!!!
        private void CreateTimer()
        {
            // Create a timer with a two second interval.
            time = new DispatcherTimer()
            {
                Interval = new TimeSpan(0,Timer_Minutes, Timer_Seconds) // H, M, S
            };
            // Hook up the Elapsed event for the timer. 
            time.Tick += TimerEvent;
        }


        private void CreateContdown()
        {
                // Create a timer with a two second interval.
                countdown = new DispatcherTimer()
                {
                    Interval = new TimeSpan(0, 0, 1) // H, M, S
                };
            // Hook up the Elapsed event for the timer.  
            countdown.Tick += CountdownEvent;

        }
        private void TimerEvent(object sender, EventArgs e)
        {
            Console.WriteLine("The Elapsed");
        }

        private void CountdownEvent(object sender, EventArgs e)
        {
            counter--;
            Console.Write(counter); // FOR DEBUG
            this.Countdown.Text = counter.ToString();

            if (counter == 0)
            {
                Stop_All();
                this.Chat_Window.Clear();
                this.MyCanvas.Children.Clear();
                this.Your_Word.Text = Get_Random_Word();
                Start_All();
            }
        }
  

        private void Stop_All()
        {
            time.Stop();
            countdown.Stop();
            counter = Timer_Seconds + (Timer_Minutes * 60); // RESET countdown
        }

        private void Start_All()
        {
            time.Start();
            this.Countdown.Text = counter.ToString();
            countdown.Start();
            
        }

        private void MyCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point p = e.GetPosition(this.MyCanvas);

                // Eine Linie malen
                Line line = new Line();
                line.StrokeStartLineCap = PenLineCap.Round; // Damit die Linie nicht abgehackt ist
                line.StrokeEndLineCap = PenLineCap.Round;
                line.X1 = this.lastPoint.X;
                line.Y1 = this.lastPoint.Y;
                line.X2 = p.X;
                line.Y2 = p.Y;
                line.Stroke = colour; // Farbe muss änderbar sein! -> In Array gespeichert
                line.StrokeThickness = this.Stroke_Thickness; // Muss änderbar sein!
                this.MyCanvas.Children.Add(line);

                this.lastPoint = p;

            }
        }

        // Manage Words
        public int Get_List_Length()
        {
            int counter = 0;
            string line;

            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(@"..\..\Wordlist\Montagsmaler_Liste.txt");
            while ((line = file.ReadLine()) != null)
            {
                //System.Console.WriteLine(line);
                counter++;
            }

            file.Close();
            //System.Console.WriteLine("There were {0} lines.", counter);
            return counter;
        }

        public string Get_Random_Word()
        {
            Random rnd = new Random();
            int line_numb = rnd.Next(0, Get_List_Length());
            int count = 0;
            string line;

            System.IO.StreamReader file = new System.IO.StreamReader(@"..\..\Wordlist\Montagsmaler_Liste.txt");
            while ((line = file.ReadLine()) != null)
            {
                if (count == line_numb)
                    break;
                count++;
            }

            file.Close();
            System.Console.WriteLine(line);
            return line;
        }
    }
}
