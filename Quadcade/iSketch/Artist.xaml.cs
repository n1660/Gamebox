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
using System.Net;
using System.Net.Sockets;

namespace Quadcade
{

    // TODO: - Groß und kleinschreibung unbeachtet lassen!
    //       - Anzeige der Momentanen Runde neben Timer
    //       - Host darf bei erstellen eines Spiels Rundenanzahl bestimmen!
    //       - Wenn Spiel zu ende, was dann? -> Anzeigen der Gewinner in bestimmter Reihenfolge
    //                                       -> Fragen ob neues Spiel? -> Host ( Wenn ja, neu mit Rundenbestimmung, sonst Alle ins Menu)
    public partial class Artist : Page
    {
        private static int Timer_Seconds = 20;
        private static int Timer_Minutes = 0;
        public  int counter = Timer_Seconds + (Timer_Minutes*60);
        private System.Timers.Timer countdown2;

        public static bool registered = false;

        // Wordlist
        private int List_Length;
        private string random_word1;
        private string random_word2;
        private string random_word3;

        private int Stroke_Thickness = 4;
        private Point lastPoint;
        private SolidColorBrush colour = Brushes.Black;

        private int Max_Score = 300;
        private static int Max_Time;
        public static int Max_Rounds = 5;
        private static int Current_Round = 1;
        public static int Max_Players = 5;

        public string Current_Artist;
        public int Current_Artist_ID = 0;

        public static List<IPEndPoint> HostIPs = new List<IPEndPoint>();

        public Artist()
        {
            InitializeComponent();

            Max_Time = counter;

            this.List_Length = Get_List_Length();
            Chat_Window.KeyDown += new KeyEventHandler(Key_Events);
            this.Rounds.Text = "Round: " + Current_Round + "/" + Max_Rounds;
            CreateContdown();
            Set_ChooseWords();
            Show_Scores();
            
        }

        private void Key_Events(object sender, KeyEventArgs k)
        {
            if (k.Key == Key.Enter)
            {
                Check_Input_Word();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == this.BTN_Clear)
                this.MyCanvas.Children.Clear();

            else if (sender == this.BTN_Submit)
            {
                Check_Input_Word();
            }
            else if (sender == this.BTN_HOME)
            {
                Stop_All2(); // Close Timer Thread!
                App.Current.MainWindow.Content = new Menu();
                // Aus der Liste entfernen mit entsprechenden ID 
                // when host leaves
                Current_Round = 1;
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

            // Choose Word
            else if(sender == this.word1_B)
            {
                this.ChooseWordCanvas.Visibility = Visibility.Hidden;
                this.word1_B.IsEnabled = false;
                this.Your_Word.Text = this.word1.Text;
                this.MyCanvas.IsEnabled = true;
                Start_All2();
            }
            else if (sender == this.word2_B)
            {
                this.ChooseWordCanvas.Visibility = Visibility.Hidden;
                this.word2_B.IsEnabled = false;
                this.Your_Word.Text = this.word2.Text;
                this.MyCanvas.IsEnabled = true;
                Start_All2();
            }
            else if (sender == this.word3_B)
            {
                this.ChooseWordCanvas.Visibility = Visibility.Hidden;
                this.word3_B.IsEnabled = false;
                this.Your_Word.Text = this.word3.Text;
                this.MyCanvas.IsEnabled = true;
                Start_All2();
            }
        }

        // For Painting  
        private void MyCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {   
            // Sicherheitsabfrage ID -> Bist du derjenige der malen darf?
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

            // Verschicken des Punktes an die anderen
        }
       
        private void CreateContdown()
        {
            countdown2 = new System.Timers.Timer();
            countdown2.Interval = 1000; // 1 second timer
            countdown2.Elapsed += Countdown2_Elapsed;
        }

        private void Countdown2_Elapsed(object sender, ElapsedEventArgs e)
        {
            counter--;
            Dispatcher.BeginInvoke((Action) (() =>       /*  Lambda Schreibweie */
            {
                this.Countdown.Text = counter.ToString();

                if (counter == 0)
                {
                    Stop_All2();
                    this.Chat_Window.Clear();
                    this.MyCanvas.Children.Clear();
                    GoToNextPlayer("timer"); // Go to next player, when time is elapsed
                    //Set_ChooseWords();
                };
            }));        
        }

        private void Stop_All2()
        {
            countdown2.Stop();
            counter = Timer_Seconds + (Timer_Minutes * 60); // RESET countdown
            Popup_Word.IsOpen = false; // Hide Popup
        }

        private void Start_All2()
        {
            this.Countdown.Text = counter.ToString();
            countdown2.Start();
        }

        private void MyCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            // Sicherheitsabfrage ID -> Bist du derjenige der malen darf?
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

                // Sockets: senden des letzten Strichs an den anderen Rechner. toSend ... X1, X2, Y2, Y1
                // String muss in einen String umgewandelt werden -> Muss beim Empfänger wieder zu einen Strich umgewandelt werden 
            }
        }

        // Manage Words
        public int Get_List_Length()
        {
            int count = 0;
            string line;

            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(@"..\..\Wordlist\Montagsmaler_Liste.txt");
            while ((line = file.ReadLine()) != null)
            {
                count++;
            }
            file.Close();
            return count;
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
            return line;
        }

        public void Set_ChooseWords()
        { 
            word1.Text = Get_Random_Word();
            while (word1.Text == null)
            {
                word1.Text = Get_Random_Word();
            }

            word2.Text = Get_Random_Word();
            while (word2.Text == word1.Text || word2.Text == null) // no dubplicates allwoded
            {
                word2.Text = Get_Random_Word();
            }

            word3.Text = Get_Random_Word();
            while (word1.Text == word3.Text || word2.Text == word3.Text || word3.Text == null) // no dubplicates allwoded
            {
                word3.Text = Get_Random_Word();
            }

            this.word1_B.IsEnabled = true;
            this.word2_B.IsEnabled = true;
            this.word3_B.IsEnabled = true;

            this.ChooseWordCanvas.Visibility = Visibility.Visible;

            this.MyCanvas.IsEnabled = false;
            Show_Scores();

        }

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
            registered = true;
        }

        void Show_MessageBox()
        {
            Create_MessageBox();
            string MessageBoxText = "Chose your word!\n\nWord 1:  " + random_word1 + "\nWord 2:  " + random_word2 + "\nWord 3:  " + random_word3;

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

            Show_Scores();
        }

        void Check_Input_Word()
        {
            Popup_Word.IsOpen = false;

            if (this.Your_Word.Text == this.Chat_Window.Text)
            {
                // [0] Dies muss für mehrspieler angepasst werden. Receive der nachricht des anderen, zuweisung der richtigen ID
                Menu.MemberList[Menu.Host][0].Guessed_Correctly = true;
                Menu.MemberList[Menu.Host][0].Score += Calculate_Points();

                Set_Popup("correct");
                Popup_Word.IsOpen = true;

                GoToNextPlayer("");
            }
            else
            {
                Compare_Imput_Word();
                this.Chat_Window.Clear();
            }
        }

        void Compare_Imput_Word()
        {
            int faulty_letters = 0;
            Set_Popup("incorrect");
            if (Chat_Window.Text.Length == Your_Word.Text.Length)
            {
                for (int i = 0; i < Chat_Window.Text.Length; i++)
                {
                    if (Chat_Window.Text[i] != Your_Word.Text[i])
                        faulty_letters++;
                }
            }

            if (faulty_letters == 1) // Show Popup
                Popup_Word.IsOpen = true;  
        }
       
        void Set_Popup(string situation)
        {
            if(situation == "correct")
            {
                BrushConverter bc = new BrushConverter();
                Brush brush = (Brush)bc.ConvertFrom("#FFCEEE97");
                brush.Freeze();
                Popup_Text.Background = brush;
                Popup_Text.Text = "Your word is correct! :)";
            }
            else if (situation == "incorrect")
            {
                BrushConverter bc = new BrushConverter();
                Brush brush = (Brush)bc.ConvertFrom("#FFF1BEE6");
                brush.Freeze();
                Popup_Text.Background = brush;
                Popup_Text.Text = "Your word is almost correct!";
            }
        }

        public int Calculate_Points()
        {
            double  percent = 0;

            percent = (double) counter / (double) Max_Time;

            Console.WriteLine("Counter: "+ counter);
            Console.WriteLine("Percent: "+ percent);

            return  (int) (Max_Score * percent);
        }

        void Show_Scores()
        {
            string ScoreTxt ="Score:\n";
            foreach(Member member in Menu.MemberList[Menu.Host])
            {
                ScoreTxt += member.Username + ":  " + member.Score + "\n" ;
            }

            Scores.Text = ScoreTxt;

        }

        void GoToNextPlayer(string position)
        {
            bool next_player = true;
            for (int i = 0; i < Menu.MemberList[Menu.Host].Count; i++)
            {
                if (Menu.MemberList[Menu.Host][i].Guessed_Correctly == false) // Check: Has everybody guessed the word correctly?
                {
                    next_player = false;
                    break;
                }
            }

            if (next_player || position == "timer")
            {
                Stop_All2();

                this.Chat_Window.Clear();
                this.MyCanvas.Children.Clear();

                if (Current_Artist_ID + 1 >= Menu.MemberList[Menu.Host].Count) // if last player was painting, the next artist is the first player in the list
                {
                    Current_Artist_ID = 0;
                    Current_Round++;

                    if (Current_Round == Max_Rounds +1)
                    {
                        Current_Round = 1;

                        foreach(Member member in Menu.MemberList[Menu.Host]) // Scores zurücksetzen!
                        {
                            member.Score = 0;
                        }
                        // Spiel beenden bzw neue Runde anfragen
                    }
                    this.Rounds.Text = "Round: " + Current_Round + "/" + Max_Rounds;
                }
                else Current_Artist_ID ++;

                Set_ChooseWords();
            }
            // Send Artist_ID if it changed to the others
        }

        void ShowRounds()
        {
            this.Rounds.Text = "Round: " + Current_Round + "/" + Max_Rounds;
        }
    }
}
