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
    public partial class Artist : Page
    {
        private static int Timer_Seconds = 10;
        private static int Timer_Minutes = 0;
        private static int counter = Timer_Seconds + (Timer_Minutes*60);
        private int Stroke_Thickness = 4;
        private int List_Length;

        private string random_word1;
        private string random_word2;
        private string random_word3;

        private Point lastPoint;
        private SolidColorBrush colour = Brushes.Black;
        private System.Timers.Timer countdown2;

        public Artist()
        {
            InitializeComponent();

            this.List_Length = Get_List_Length();
            //this.Your_Word.Text = Get_Random_Word();  
            CreateContdown();

            Set_MessageBox();
            Show_MessageBox();
            //Start_All2();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (sender == this.BTN_Clear)
                this.MyCanvas.Children.Clear();

            else if (sender == this.BTN_Submit)
            {
                if (this.Your_Word.Text == this.Chat_Window.Text)
                {
                    Stop_All2(); 
                    Console.Write("yay\n");
                    this.Chat_Window.Clear();
                    this.MyCanvas.Children.Clear();
                    this.Your_Word.Text = Get_Random_Word();
                    Start_All2(); 
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
       
        private void CreateContdown()
        {
            countdown2 = new System.Timers.Timer();
            countdown2.Interval = 1000;
            countdown2.Elapsed += Countdown2_Elapsed;
        }

        private void Countdown2_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Elapsed ...");

            counter--;

            Dispatcher.BeginInvoke((Action) (() =>       /*  Lambda Schreibweie */
            {
                this.Countdown.Text = counter.ToString();

                if (counter == 0)
                {
                    Stop_All2();
                    this.Chat_Window.Clear();
                    this.MyCanvas.Children.Clear();
                    Show_MessageBox();
                    //this.Your_Word.Text = Get_Random_Word();
                };
            }));        
        }

        private void TimerEvent(object sender, EventArgs e)
        {
            Console.WriteLine("The Elapsed");
        }

        private void Stop_All2()
        {
            countdown2.Stop();
            counter = Timer_Seconds + (Timer_Minutes * 60); // RESET countdown
        }

        private void Start_All2()
        {
            this.Countdown.Text = counter.ToString();
            countdown2.Start();
        }

        private void MyCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point p = e.GetPosition(this.MyCanvas);

                Line line = new Line();
                line.StrokeStartLineCap = PenLineCap.Round; 
                line.StrokeEndLineCap = PenLineCap.Round;
                line.X1 = this.lastPoint.X;
                line.Y1 = this.lastPoint.Y;
                line.X2 = p.X;
                line.Y2 = p.Y;
                line.Stroke = colour; 
                line.StrokeThickness = this.Stroke_Thickness; 
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

        [STAThread]

        public void Create_MessageBox()
        {
            random_word1 = Get_Random_Word();
            while (random_word1 == null)
            {
                random_word1 = Get_Random_Word();
            }

            random_word2 = Get_Random_Word();
            while (random_word2 == random_word1 || random_word2 == null) // no dubplicates allwoded
            {
                random_word2 = Get_Random_Word();
            }

            random_word3 = Get_Random_Word();
            while (random_word1 == random_word3 || random_word2 == random_word3 || random_word3 == null) // no dubplicates allwoded
            {
                random_word3 = Get_Random_Word();
            }
        }

        public void Set_MessageBox()
        {
            System.Windows.Forms.MessageBoxManager.Yes = "Word 1" ;
            System.Windows.Forms.MessageBoxManager.No = "Word 2";
            System.Windows.Forms.MessageBoxManager.Cancel = "Word 3";
            System.Windows.Forms.MessageBoxManager.Register();
        }

        void Show_MessageBox()
        {
            Create_MessageBox();
            string MessageBoxText = "Chose your word!\nWord 1: ";
            MessageBoxText += random_word1;
            MessageBoxText += "\nWord 2: ";
            MessageBoxText += random_word2;
            MessageBoxText += "\nWord 3: ";
            MessageBoxText += random_word3;

            MessageBoxResult result = MessageBox.Show(MessageBoxText, "Your Word", MessageBoxButton.YesNoCancel);
            switch (result)
            {
                case MessageBoxResult.Yes: // if word 1 is chosen
                    this.Your_Word.Text = random_word1;
                    Start_All2();
                    break;
                case MessageBoxResult.No: // if word 2 is chosen
                    this.Your_Word.Text = random_word2;
                    Start_All2();
                    break;
                case MessageBoxResult.Cancel: // if word 3 is chosen
                    this.Your_Word.Text = random_word3;
                    Start_All2();
                    break;
            }
        }
    }
}
