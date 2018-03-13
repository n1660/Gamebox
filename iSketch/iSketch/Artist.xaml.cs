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

namespace iSketch
{
    // TODO: - Groß und kleinschreibung unbeachtet lassen!
    //       - Anzeige der Momentanen Runde neben Timer
    //       - Host darf bei erstellen eines Spiels Rundenanzahl bestimmen!
    //       - Wenn Spiel zu ende, was dann? -> Anzeigen der Gewinner in bestimmter Reihenfolge
    //                                       -> Fragen ob neues Spiel? -> Host ( Wenn ja, neu mit Rundenbestimmung, sonst Alle ins Menu)
    public partial class Artist : Page
    {
        private static int timerSeconds = 20;
        private static int timerMinutes = 0;
        public  int counter = timerSeconds + (timerMinutes*60);
        private System.Timers.Timer countdown2;
        private int popupCounter = 3; // 3 sec

        public static bool registered = false;

        // Wordlist
        private int listLength;
        private string randomWord1;
        private string randomWord2;
        private string randomWord3;

        private int strokeThickness = 4;
        private Point lastPoint;
        private SolidColorBrush color = Brushes.Black;

        private int maxScore = 300;
        private static int maxTime;
        public static int MAXROUNDS = 5;
        private static int CurRound = 1;
        public static int maxPlayers = 5;

        public static String curArtist;
        public int curArtistID = 0;

        public Artist(String host = null)
        {
            if (host != null)
            {
                Menu.Host = host;
                Menu.MemberList.Add(Menu.Host, new List<Member>());
                Menu.MemberList[Menu.Host].Add(Menu.member);
            }

            Console.WriteLine("Artist Constructor");
            InitializeComponent();
            Server.Connection.PAINTINGCANV = MyCanvas;

            maxTime = counter;

            this.listLength = GetListLength();
            TxtSolve.KeyDown += new KeyEventHandler(KeyEvents);
            this.Rounds.Text = "Round: " + CurRound + "/" + MAXROUNDS;
            CreateContdown();
            SetWordsToChoose();
            ShowScores();
            Console.WriteLine("Artist Constructor END");
        }

        private void KeyEvents(object sender, KeyEventArgs k)
        {
            if (k.Key == Key.Enter)
            {
                CheckInputWord();
            }
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender == this.BtnClear)
                this.MyCanvas.Children.Clear();

            else if (sender == this.BtnSubmit)
            {
                CheckInputWord();
            }
            else if (sender == this.BtnHome)
            {
                StopAll2(); // Close Timer Thread!
                MainWindow.win.Content = new Menu();
                Member.instance = (Menu)MainWindow.win.Content;
                // Aus der Liste entfernen mit entsprechenden ID 
                // when host leaves
                CurRound = 1;
            }

            // Colours
            else if (sender == this.Colour1)
                color = Brushes.Black;
            else if (sender == this.Colour2)
                color = (SolidColorBrush)this.Colour2.Background;
            else if (sender == this.Colour3)
                color = Brushes.White;
            else if (sender == this.Colour4)
                color = (SolidColorBrush)this.Colour4.Background;
            else if (sender == this.Colour5)
                color = (SolidColorBrush)this.Colour5.Background;
            else if (sender == this.Colour6)
                color = (SolidColorBrush)this.Colour6.Background;
            else if (sender == this.Colour7)
                color = (SolidColorBrush)this.Colour7.Background;
            else if (sender == this.Colour8)
                color = (SolidColorBrush)this.Colour8.Background;
            else if (sender == this.Colour9)
                color = (SolidColorBrush)this.Colour9.Background;

            // Strocke Thickness
            else if (sender == this.Size1)
                this.strokeThickness = 8;

            else if (sender == this.Size2)
                this.strokeThickness = 4;

            else if (sender == this.Size3)
                this.strokeThickness = 2;

            // Choose Word
            else if(sender == this.BtnWord1)
            {
                this.ChooseWordCanvas.Visibility = Visibility.Hidden;
                this.BtnWord1.IsEnabled = false;
                this.YourWord.Text = this.word1.Text;
                this.MyCanvas.IsEnabled = true;
                StartAll2();
            }
            else if (sender == this.BtnWord2)
            {
                this.ChooseWordCanvas.Visibility = Visibility.Hidden;
                this.BtnWord2.IsEnabled = false;
                this.YourWord.Text = this.word2.Text;
                this.MyCanvas.IsEnabled = true;
                StartAll2();
            }
            else if (sender == this.BtnWord3)
            {
                this.ChooseWordCanvas.Visibility = Visibility.Hidden;
                this.BtnWord3.IsEnabled = false;
                this.YourWord.Text = this.word3.Text;
                this.MyCanvas.IsEnabled = true;
                StartAll2();
            }
        }

        // For Painting  
        private void MyCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {   
            // Sicherheitsabfrage ID -> Bist du derjenige der malen darf?
            Point p = e.GetPosition(this.MyCanvas); 

            this.lastPoint = p;

            Ellipse ell = new Ellipse();
            ell.Width = this.strokeThickness *1.5;
            ell.Height = this.strokeThickness *1.5;

            ell.Stroke = color;
            ell.Fill = color;
            ell.StrokeThickness = strokeThickness;
            ell.SetValue(Canvas.LeftProperty, p.X - strokeThickness);
            ell.SetValue(Canvas.TopProperty, p.Y - strokeThickness);
            this.MyCanvas.Children.Add(ell);

            // Verschicken des Punktes an die anderen
        }
       
        private void CreateContdown()
        {
            countdown2 = new System.Timers.Timer();
            countdown2.Interval = 1000; // 1 second timer
            countdown2.Elapsed += Countdown2Elapse;
        }

        private void Countdown2Elapse(object sender, ElapsedEventArgs e)
        {
            counter--;
            Dispatcher.BeginInvoke((Action) (() =>       /*  Lambda Schreibweie */
            {
                this.Countdown.Text = counter.ToString();

                if (counter == 0)
                {
                    StopAll2();
                    this.TxtSolve.Clear();
                    this.MyCanvas.Children.Clear();
                    GoToNextPlayer("timer"); // Go to next player, when time is elapsed
                    //Set_ChooseWords();
                };

                if (PopupWord.IsOpen == true)
                {
                    popupCounter--;
                    if (popupCounter == 0)
                    {
                        PopupWord.IsOpen = false;
                        popupCounter = 3;
                    }
                }
            }));        
        }

        private void StopAll2()
        {
            countdown2.Stop();
            counter = timerSeconds + (timerMinutes * 60); // RESET countdown
            PopupWord.IsOpen = false; // Hide Popup
        }

        private void StartAll2()
        {
            this.Countdown.Text = counter.ToString();
            countdown2.Start();
        }

        private void MyCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if(Menu.member.ID != curArtistID)
                return;

            // Sicherheitsabfrage ID -> Bist du derjenige der malen darf?
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DrawLine(e.GetPosition(this.MyCanvas));
                // Sockets: senden des letzten Strichs an den anderen Rechner. toSend ... X1, X2, Y2, Y1
                // String muss in einen String umgewandelt werden -> Muss beim Empfänger wieder zu einen Strich umgewandelt werden 
            }
        }

        public void DrawLine(Point curpos)
        {
            curpos = Mouse.GetPosition(this.MyCanvas);
            Line line = new Line
            {
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round,
                X1 = this.lastPoint.X,
                Y1 = this.lastPoint.Y,
                X2 = curpos.X,
                Y2 = curpos.Y,
                Stroke = color,
                StrokeThickness = this.strokeThickness
            };
            this.MyCanvas.Children.Add(line);

            this.lastPoint = curpos;
            Console.WriteLine(Member.GetMmbByName(Menu.Host, curArtist).Writer.ToString());
            Member.GetMmbByName(Menu.Host, curArtist).Writer.WriteLine(Menu.member.ID.ToString() + ';' + line.X1 + ';' + line.Y1 + ';' + line.X2 + ';' + line.Y2 + ';' + color + ';' + strokeThickness);
        }

        // Manage Words
        public int GetListLength()
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

        public string GetRndWord()
        {
            Random rnd = new Random();
            int line_numb = rnd.Next(0, GetListLength());
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

        public void SetWordsToChoose()
        { 
            word1.Text = GetRndWord();
            while (word1.Text == null)
            {
                word1.Text = GetRndWord();
            }

            word2.Text = GetRndWord();
            while (word2.Text == word1.Text || word2.Text == null) // no dubplicates allwoded
            {
                word2.Text = GetRndWord();
            }

            word3.Text = GetRndWord();
            while (word1.Text == word3.Text || word2.Text == word3.Text || word3.Text == null) // no dubplicates allwoded
            {
                word3.Text = GetRndWord();
            }

            this.BtnWord1.IsEnabled = true;
            this.BtnWord2.IsEnabled = true;
            this.BtnWord3.IsEnabled = true;

            this.ChooseWordCanvas.Visibility = Visibility.Visible;

            this.MyCanvas.IsEnabled = false;
            ShowScores();

        }

        public void CreateMsgBox()
        {
            randomWord1 = GetRndWord();
            while (randomWord1 == null)
            {
                randomWord1 = GetRndWord();
            }

            randomWord2 = GetRndWord();
            while (randomWord2 == randomWord1 || randomWord2 == null) // no dubplicates allwoded
            {
                randomWord2 = GetRndWord();
            }

            randomWord3 = GetRndWord();
            while (randomWord1 == randomWord3 || randomWord2 == randomWord3 || randomWord3 == null) // no dubplicates allwoded
            {
                randomWord3 = GetRndWord();
            }
        }

        void CheckInputWord()
        {
            PopupWord.IsOpen = false;

            if (this.YourWord.Text == this.TxtSolve.Text)
            {
                // [0] Dies muss für mehrspieler angepasst werden. Receive der nachricht des anderen, zuweisung der richtigen ID
                Menu.MemberList[Menu.Host][0].GuessedCorrectly = true;
                Menu.MemberList[Menu.Host][0].Score += CalculateScore();

                Set_Popup("correct");
                PopupWord.IsOpen = true;

                GoToNextPlayer("");
            }
            else
            {
                CompareInputWord();
                this.TxtSolve.Clear();
            }
        }

        void CompareInputWord()
        {
            int faulty_letters = 0;
            Set_Popup("incorrect");
            if (TxtSolve.Text.Length == YourWord.Text.Length)
            {
                for (int i = 0; i < TxtSolve.Text.Length; i++)
                {
                    if (TxtSolve.Text[i] != YourWord.Text[i])
                        faulty_letters++;
                }
            }

            if (faulty_letters == 1) // Show Popup
                PopupWord.IsOpen = true;  
        }
       
        void Set_Popup(string situation)
        {
            if(situation == "correct")
            {
                BrushConverter bc = new BrushConverter();
                Brush brush = (Brush)bc.ConvertFrom("#FFCEEE97");
                brush.Freeze();
                PopupText.Background = brush;
                PopupText.Text = "Your word is correct! :)";
            }
            else if (situation == "incorrect")
            {
                BrushConverter bc = new BrushConverter();
                Brush brush = (Brush)bc.ConvertFrom("#FFF1BEE6");
                brush.Freeze();
                PopupText.Background = brush;
                PopupText.Text = "Your word is almost correct!";
            }
        }

        public int CalculateScore()
        {
            double  percent = 0;

            percent = (double) counter / (double) maxTime;

            Console.WriteLine("Counter: "+ counter);
            Console.WriteLine("Percent: "+ percent);

            return  (int) (maxScore * percent);
        }

        void ShowScores()
        {
            string ScoreTxt ="Score:\n";
            foreach(Member member in Menu.MemberList[Menu.Host])
            {
                ScoreTxt += member.Username + ": " + member.Score + "\n" ;
            }

            Scores.Text = ScoreTxt;

        }

        void GoToNextPlayer(string position)
        {
            bool nextPlayer = true;
            for (int i = 0; i < Menu.MemberList[Menu.Host].Count; i++)
            {
                if (Menu.MemberList[Menu.Host][i].GuessedCorrectly == false) // Check: Has everybody guessed the word correctly?
                {
                    nextPlayer = false;
                    break;
                }
            }

            if (nextPlayer || position == "timer")
            {
                StopAll2();

                this.TxtSolve.Clear();
                this.MyCanvas.Children.Clear();

                if (curArtistID + 1 >= Menu.MemberList[Menu.Host].Count) // if last player was painting, the next artist is the first player in the list
                {
                    curArtistID = 0;
                    CurRound++;

                    if (CurRound == MAXROUNDS +1)
                    {
                        CurRound = 1;

                        foreach(Member member in Menu.MemberList[Menu.Host]) // Scores zurücksetzen!
                        {
                            member.Score = 0;
                        }
                        Server.Server.BroadcastScore();
                        // Spiel beenden bzw neue Runde anfragen
                    }
                    this.Rounds.Text = "Round: " + CurRound + "/" + MAXROUNDS;
                }
                else curArtistID ++;

                SetWordsToChoose();
            }
            // Send Artist_ID if it changed to the others
        }

        void ShowRounds()
        {
            this.Rounds.Text = "Round: " + CurRound + "/" + MAXROUNDS;
        }
    }
}
