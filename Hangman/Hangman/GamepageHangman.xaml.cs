using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Diagnostics;
using System.Timers;

namespace Hangman
{
    public partial class GamepageHangman : Page
    {
        string randomWord = "";
        public static int points = 100;
        string checkrand = "";
        string solution;
        string alreadyUsed = "";
        static int maxfehler = 9;
        int anzfehler = 0;
        bool BonusUsed = false;
        Label[] lbls = new Label[15];
        Label[] lblPoints = new Label[1];
        Label[] lblRounds = new Label[1];
        Button[] btnPoints = new Button[1];
        public static int countRounds = 5;
        System.Timers.Timer aTimer = new System.Timers.Timer();

        public GamepageHangman()
        {
            InitializeComponent();
            Start();
        }
        public string NewWord()
        {
            string path = "Montagsmaler_Liste";
            
                switch(Settings.difficultylvl)
                { 
                    case 1:
                        switch (Settings.language)
                        {
                            case 1:
                                path = @"3-5Buchstaben.txt";
                                break;
                            case 2:
                                path = @"3-5Letters.txt";
                                break;
                            default:
                                break;
                        }
                        break;
                    case 2:
                        switch (Settings.language)
                        {
                            case 1:
                                path = @"6-8Buchstaben.txt";
                                break;
                            case 2:
                                path = @"6-8Letters.txt";
                                break;
                            default:
                                break;
                        }                  
                        break;
                    case 3:
                        switch (Settings.language)
                        {
                            case 1:
                                path = @"Extrem.txt";
                                break;
                            case 2:
                                break;
                            default:
                                break;
                        }                  
                        break;
                    default:
                        break;
                }
            string readText = File.ReadAllText(path);
            string[] wort = readText.Split('\n');

            Random rand = new Random();
            int zufallszahl = rand.Next(0, 184);
            string randomWord = wort[zufallszahl];

            randomWord = randomWord.Remove(randomWord.Length - 1);

            return randomWord;
        }
        public void Start()
        {
            popup.IsOpen = false;
            randomWord = NewWord();
            anzfehler = 0;
            alreadyUsed = "";
            checkrand = "";
            solution = "";
            BonusUsed = false;
            Show_Lines();
            Show_Buttons();
            Show_Image();
            Show_Points();
        }
        public void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            solution = textbox.Text;
        }
        public void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string letter = btn.Content.ToString();
            if(letter != "Hint" && letter != "Bonus")
            {
                btn.Background = Brushes.Red;
                bool containsLetter = randomWord.Contains(letter);
                bool letterUsed = alreadyUsed.Contains(letter);
                if (containsLetter && !letterUsed)
                {
                    ShowNewWord(letter);
                    alreadyUsed += letter;
                }
                else
                {
                    if(letterUsed)
                    {                      
                        ShowPopUp("Letter already chosen!", "Red");
                    }
                    else
                    {
                        ShowPopUp("Letter not included!", "Red");
                        MistakeWasMade();
                    }
                    alreadyUsed += letter;
                }
            }
            else if(letter == "Hint") //Hint wurde gedrückt
            {               
                Random rand = new Random();
                int randInt = rand.Next(0, randomWord.Length);
                string randString = randInt.ToString();
                bool posUsed = checkrand.Contains(randString);
                while(posUsed)      //solange die Position schon ausgewählt wurde
                {
                    rand = new Random();
                    randInt = rand.Next(0, randomWord.Length);
                    randString = randInt.ToString();
                    posUsed = checkrand.Contains(randString);
                }

                ShowNewWord(randomWord[randInt].ToString());    //Buchstabe auslesen und anzeigen
                CheckIfWordShown("You needed help!");

                checkrand += randString;
                points -= 10;
                MistakeWasMade();
            }
            else if(letter == "Bonus")
            {
                anzfehler = 0;
                Show_Image();
                BonusUsed = true;
                canvas.Children.Remove(btnPoints[0]);
            }
        }
        public void ShowNewWord(string letter)
        {
            int pos = randomWord.IndexOf(letter);
            lbls[pos].Content = letter;
            points += 10;
            checkrand += pos;
            int pos2 = randomWord.IndexOf(letter, pos + 1);
            if (pos2 != -1)  // weiterer Buchstabe gefunden (6 mal überprüfen)
            {
                lbls[pos2].Content = letter;
                points += 10;
                checkrand += pos2;
                int pos3 = randomWord.IndexOf(letter, pos2 + 1);
                if (pos3 != -1)
                {
                    lbls[pos3].Content = letter;
                    points += 10;
                    checkrand += pos3;
                    int pos4 = randomWord.IndexOf(letter, pos3 + 1);
                    if (pos4 != -1)
                    { 
                        lbls[pos4].Content = letter;
                        points += 10;
                        checkrand += pos4;
                        int pos5 = randomWord.IndexOf(letter, pos4 + 1);
                        if(pos5 != -1)
                        {
                            lbls[pos5].Content = letter;
                            points += 10;
                            checkrand += pos5;
                            int pos6 = randomWord.IndexOf(letter, 5 + 1);
                            if(pos6 != -1)
                            {
                                lbls[pos6].Content = letter;
                                points += 10;
                                checkrand += pos6;
                            }
                        }
                    }
                }
            }
            Show_Points();
            CheckIfWordShown("You won!");
        }
        public void CheckIfWordShown(string message)
        {
            string lbl = "";
            for (int i = 0; i < randomWord.Length; i++)
            {
                lbl += lbls[i];
            }
            int won = lbl.IndexOf("_");
            if (won == -1)
            {
                if (message == "You won!")
                    points += 50;
                MessageBox.Show(message);
                QuitGame();
            }
        }
        public void MistakeWasMade()
        {
            anzfehler++;
            Show_Image();
            points = points - 10;
            Show_Points();
            if (anzfehler == maxfehler)
            {
                for(int i = 0; i < randomWord.Length; i++)
                {
                    lbls[i].Content = randomWord[i];
                }
                MessageBox.Show("You lost!");
                QuitGame();
            }
        }
        public void Button_Click_Submit(object sender, RoutedEventArgs e)
        {
            if (solution == randomWord)
            {
                MessageBox.Show("You won!");
                for (int i = 0; i < randomWord.Length; i++)
                    lbls[i].Content = randomWord[i];
                QuitGame();
            }
            else
            {
                points -= 10;
                Show_Points();
                MessageBox.Show("Wrong word!");
                textbox.Text = "";
                MistakeWasMade();
            }
        }   
        public void Show_Lines()
        {
            int CvLeft = 11;
            int CvTop = 60;
            string textRounds = "Rounds left: ";
            string countRoundStr = countRounds.ToString();
            textRounds += countRounds;

            for (int i = 0; i < randomWord.Length; i++)
            {
                //Instanz erzeugen
                lbls[i] = new Label
                {
                    //Eigenschaften setzen u_ä.
                    Content = "_",
                    Name = "Label" + i
                };

                Canvas.SetTop(lbls[i], CvTop);
                Canvas.SetLeft(lbls[i], CvLeft);

                //Label zur Form hinzcfügen
                canvas.Children.Add(lbls[i]);
                CvLeft += 42;
            }
            lblRounds[0] = new Label
            {
                Content = textRounds,
                Name = "Rounds",
                FontSize = 40,

                Width = 250,
                Height = 60
            };
            Canvas.SetTop(lblRounds[0], 0);
            Canvas.SetLeft(lblRounds[0], 0);
            canvas.Children.Add(lblRounds[0]);

            lblPoints[0] = new Label
            {
                Content = points,
                Name = "Points",
                Width = 200,
                Height = 200
            };
            Canvas.SetTop(lblPoints[0], 210);
            Canvas.SetLeft(lblPoints[0], 230);
            canvas.Children.Add(lblPoints[0]);
        }
        public void Show_Buttons()
        {
            int CvLeft = 4;
            double CvTop = 406.4;
            int count = 0;
            bool flanke = false;
            Button[] btn = new Button[26];
            char[] Alphabet = {'a', 'b', 'c', 'd', 'e',
                        'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
                        'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w',
                        'x', 'y', 'z'};
            for (int i = 0; i < 26; i++)
            {
                //Instanz erzeugen
                btn[i] = new Button
                {
                    //Eigenschaften setzen uä.
                    Content = Alphabet[i],
                    Name = "Button" + i,
                    Background = Brushes.LightGreen
            };

                Canvas.SetTop(btn[i], CvTop);
                Canvas.SetLeft(btn[i], CvLeft);

                btn[i].Click += Button_Click;

                //Label zur Form hinzufügen
                canvas.Children.Add(btn[i]);

                CvLeft += 64;
                count++;
                if (count > 12 && !flanke)
                {
                    CvLeft = 4;
                    CvTop = 470.4;
                    flanke = true;
                }
            }
        }
        public void Show_Image()
        {
            //Bild abhängig von Fehler auswählen
            bild.Source = GetImage(@"D:\ProjektMultiplayer\Hangman\Hangman\Images\" + anzfehler + ".png");
        }
        static public BitmapImage GetImage(string imageUri)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(imageUri, UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();
            return bitmapImage;
        }
        public void Show_Points()
        {
            lblPoints[0].Content = points;
            Check_Bonus();
        }
        public void Check_Bonus()
        {
            if(points >= 150 && !BonusUsed)
            {
                btnPoints[0] = new Button
                {
                    Name = "btn_Points",
                    Content = "Bonus",
                    Background = Brushes.Yellow,
                    Height = 65,
                    Width = 100
                };
                btnPoints[0].Click += Button_Click;
                Canvas.SetTop(btnPoints[0], 225);
                Canvas.SetLeft(btnPoints[0], 350);
                canvas.Children.Add(btnPoints[0]);
                ShowPopUp("Congratulations! The Bonus sets your mistakes back to 0!");
            }
            else if (points < 150)
            {
                BonusUsed = false;
                canvas.Children.Remove(btnPoints[0]);
            }
            else if(points >= 150 && BonusUsed)
            { 
                canvas.Children.Remove(btnPoints[0]);
            }
        }
        public void QuitGame()
        {
            countRounds--;
            for (int i = 0; i < randomWord.Length; i++)   //alle lbls beenden
            {
                canvas.Children.Remove(lbls[i]);
            }
            canvas.Children.Remove(lblPoints[0]);
            canvas.Children.Remove(lblRounds[0]);
            if (countRounds == 0)
            {
                MainWindow mw = (MainWindow)Application.Current.MainWindow;
                mw.Content = new Hangman_menu();
            }
            else
                Start();
        }
        public void MenuItem_Click_Neu(object sender, RoutedEventArgs e)
        {
            //aktuelles Spiel beendet & neues Spiel starten
            for (int i = 0; i < randomWord.Length; i++)   //alle lbls beenden
            {
                canvas.Children.Remove(lbls[i]);
            }
            Start();
        }
        public void MenuItem_Click_Quit(object sender, RoutedEventArgs e)
        {
            //alles beenden
            System.Windows.Application.Current.Shutdown();
        }
        public void MenuItem_Click_Menu(object sender, RoutedEventArgs e)
        {
            //MenuPage aufrufen
            QuitGame();
        }
        public void MenuItem_Click_Settings(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < randomWord.Length; i++)   //alle lbls beenden
            {
                canvas.Children.Remove(lbls[i]);
            }
            canvas.Children.Remove(lblPoints[0]);
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            mw.Content = new Settings();
        }
        public void ShowPopUp(string content, string color = "white")
        {
            BrushConverter bc = new BrushConverter();
            Brush brush = (Brush)bc.ConvertFrom(color);
            brush.Freeze();
            popupcontent.Background = brush;
            popupcontent.Text = content;
            popup.IsOpen = true;

            aTimer.Enabled = false;
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 2000;
            aTimer.Enabled = true;           
        }
        public void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() => {
                popup.IsOpen = false;
            }));
        }
    }
}
