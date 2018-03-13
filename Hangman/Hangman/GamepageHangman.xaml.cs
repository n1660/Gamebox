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
<<<<<<< HEAD
using System.Timers;
=======
using System.Net.Cache;
>>>>>>> 5a2a099b581764b14508286c9208c113dc4e1d05

namespace Hangman
{
    public partial class GamepageHangman : Page
    {
        string randomWord = "";
        string checkrand = "";
        string solution;
        string alreadyUsed = "";
        static int maxfehler = 9;
        int StartPoints = Settings.points;
        int anzfehler = 0;
        bool BonusUsed = false;
        bool BonusPopUp = false;
        Label[] lbls = new Label[15];
        Label[] lblPoints = new Label[3];
        Label[] lblRounds = new Label[1];
        Button[] btnBonus = new Button[1];
        System.Timers.Timer TimerPopUp = new System.Timers.Timer();
        System.Timers.Timer TimerPoints = new System.Timers.Timer();

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
                                path = "lists/3-5Buchstaben.txt";
                                break;
                            case 2:
                                path = "lists/3-5Letters.txt";
                                break;
                            default:
                                break;
                        }
                        break;
                    case 2:
                        switch (Settings.language)
                        {
                            case 1:
                                path = "lists/6-8Buchstaben.txt";
                                break;
                            case 2:
                                path = "lists/6-8Letters.txt";
                                break;
                            default:
                                break;
                        }                  
                        break;
                    case 3:
                        switch (Settings.language)
                        {
                            case 1:
                                path =  "lists/Extrem.txt";
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
            if(letter != "Hint" && letter != "Bonus")   //Buchstabe gedrückt
            {
                btn.Background = Brushes.Red;
                bool containsLetter = randomWord.Contains(letter);
                bool letterUsed = alreadyUsed.Contains(letter);
                if (containsLetter && !letterUsed)      //Buchstabe richtig & noch nicht gewählt
                {
                    alreadyUsed += letter;
                    ShowNewWord(letter);
                }
                else
                {
                    alreadyUsed += letter;
                    if (letterUsed)      //Buchstabe bereits gewählt
                    {
                        ResetPopUp();
                        ShowPopUp("Letter already chosen!", "Red");
                    }
                    else                //Buchstabe nicht enthalten
                    {
                        ResetPopUp();
                        ShowPopUp("Letter not included!", "Red");
                        MistakeWasMade();
                    }
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
                Settings.points -= 10;
                MistakeWasMade();
            }
            else if(letter == "Bonus")      //Bonus wurde gedrückt
            {
                canvas.Children.Remove(btnBonus[0]);
                anzfehler = 0;
                Show_Image();
                BonusUsed = true;
            }
        }
        public void ShowNewWord(string letter)
        {
            int pos = randomWord.IndexOf(letter);
            lbls[pos].Content = letter;
            Settings.points += 10;
            checkrand += pos;
            int pos2 = randomWord.IndexOf(letter, pos + 1);
            if (pos2 != -1)  // weiterer Buchstabe gefunden (6 mal überprüfen)
            {
                lbls[pos2].Content = letter;
                Settings.points += 10;
                checkrand += pos2;
                int pos3 = randomWord.IndexOf(letter, pos2 + 1);
                if (pos3 != -1)
                {
                    lbls[pos3].Content = letter;
                    Settings.points += 10;
                    checkrand += pos3;
                    int pos4 = randomWord.IndexOf(letter, pos3 + 1);
                    if (pos4 != -1)
                    { 
                        lbls[pos4].Content = letter;
                        Settings.points += 10;
                        checkrand += pos4;
                        int pos5 = randomWord.IndexOf(letter, pos4 + 1);
                        if(pos5 != -1)
                        {
                            lbls[pos5].Content = letter;
                            Settings.points += 10;
                            checkrand += pos5;
                            int pos6 = randomWord.IndexOf(letter, 5 + 1);
                            if(pos6 != -1)
                            {
                                lbls[pos6].Content = letter;
                                Settings.points += 10;
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
                    Settings.points += 50;
                MessageBox.Show(message);
                QuitGame();
            }
        }
        public void MistakeWasMade()
        {
            anzfehler++;
            Show_Image();
            Settings.points = Settings.points - 10;
            Show_Points();
            if (anzfehler == maxfehler)
            {
                for(int i = 0; i < randomWord.Length; i++)
                {
                    lbls[i].Content = randomWord[i];
                }
                MessageBox.Show("You lost!");
                Settings.points -= 20;
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
                Settings.points -= 10;
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
            string countRoundStr = Settings.countRounds.ToString();
            textRounds += Settings.countRounds;

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

                Width = 300,
                Height = 60
            };
            Canvas.SetTop(lblRounds[0], 0);
            Canvas.SetLeft(lblRounds[0], 0);
            canvas.Children.Add(lblRounds[0]);

            lblPoints[0] = new Label
            {
                Content = Settings.points,
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

            btnBonus[0] = new Button
            {
                Name = "btn_Points",
                Content = "Bonus",
                Background = Brushes.Yellow,
                Height = 65,
                Width = 100,
                Visibility = Visibility.Hidden
            };
            btnBonus[0].Click += Button_Click;
            Canvas.SetTop(btnBonus[0], 225);
            Canvas.SetLeft(btnBonus[0], 350);
            canvas.Children.Add(btnBonus[0]);
        }
        public void Show_Image()
        {
            //Bild abhängig von Fehler auswählen
            bild.Source = GetImage("pics/" + anzfehler.ToString() + ".png");
        }
        static public BitmapImage GetImage(string imageUri)
        {
            var bitmapImage = new BitmapImage();

            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.UriSource = new Uri(imageUri, UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();
            return bitmapImage;
        }
        public void Show_Points()
        {
            lblPoints[0].Content = Settings.points;
            Check_Bonus();
        }
        public void Check_Bonus()
        {
            switch(StartPoints)     //Bonus abhängig von StartPunkte
            { 
                case 100:
                    if (Settings.points >= 150 && !BonusUsed)
                    {
                        btnBonus[0].Visibility = Visibility.Visible;
                        if (!BonusPopUp)
                        {
                            BonusPopUp = true;
                            popup.Width = 700;
                            popupcontent.Width = 700;
                            ShowPopUp("Congratulations, the Bonus sets your mistakes back to 0!");
                        }
                    }
                    else if (Settings.points == 0)
                    {
                        MessageBox.Show("You lost!");
                        Settings.countRounds = 1;
                        QuitGame();
                    }
                    else if (Settings.points < 150)
                        BonusUsed = false;
                    else
                    {
                        btnBonus[0].Visibility = Visibility.Hidden;
                        BonusPopUp = false;
                    }
                    break;
                case 200:
                    if (Settings.points >= 300 && !BonusUsed)
                    {
                        btnBonus[0].Visibility = Visibility.Visible;
                        if (!BonusPopUp)
                        {
                            BonusPopUp = true;
                            popup.Width = 700;
                            popupcontent.Width = 700;
                            ShowPopUp("Congratulations, the Bonus sets your mistakes back to 0!");
                        }
                    }
                    else if (Settings.points == 0)
                    {
                        MessageBox.Show("You lost!");
                        Settings.countRounds = 1;
                        QuitGame();
                    }
                    else if (Settings.points < 300)
                        BonusUsed = false;
                    else
                    {
                        btnBonus[0].Visibility = Visibility.Hidden;
                        BonusPopUp = false;
                    }
                    break;
                default:
                    break;
            }
        }
        public void QuitGame()
        {
            Settings.countRounds--;
            for (int i = 0; i < randomWord.Length; i++)   //alle lbls entfernen
            {
                canvas.Children.Remove(lbls[i]);
            }
            canvas.Children.Remove(lblPoints[0]);
            canvas.Children.Remove(lblRounds[0]);          
            if (Settings.countRounds == 0)   // nach Rundenanzahl ins Menü
            {
                canvas.Children.Clear();


                lblPoints[1] = new Label
                {
                    Content = "Your Score:",
                    Name = "End_Points",
                    Width = 500,
                    Height = 500,
                    FontSize = 60                    
                };
                Canvas.SetTop(lblPoints[1], 100);
                Canvas.SetLeft(lblPoints[1], 250);
                canvas.Children.Add(lblPoints[1]);

                lblPoints[2] = new Label
                {
                    Content = Settings.points,
                    Name = "End_Points",
                    Width = 500,
                    Height = 500,
                    FontSize = 60
                };
                Canvas.SetTop(lblPoints[2], 200);
                Canvas.SetLeft(lblPoints[2], 350);
                canvas.Children.Add(lblPoints[2]);

                TimerPoints.Enabled = false;
                TimerPoints.Elapsed += new ElapsedEventHandler(PointsTime);
                TimerPoints.Interval = 3000;
                TimerPoints.Enabled = true;
            }
            else                    //oder neue Runde
            {
                Start();
            }

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
            Settings.countRounds = 1;
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

            TimerPopUp.Enabled = false;
            TimerPopUp.Elapsed += new ElapsedEventHandler(PopUpTime);
            if (BonusPopUp)         //BonusPopUp aktiv -> 3 Sek anzeigen
                TimerPopUp.Interval = 3000;
            else                    //BuchstabenPopUp aktiv -> 2 Sek anzeigen
                TimerPopUp.Interval = 2000;
            TimerPopUp.Enabled = true;
        }
        public void PopUpTime(object source, ElapsedEventArgs e)    //Aufgerufene Fkt nach den PopUps
        {
            Dispatcher.BeginInvoke(new Action(() => {
                popup.IsOpen = false;
            }));
        }
        public void PointsTime(object source, ElapsedEventArgs e)   //Aufgerufene Fkt nach Punktanzeige am Ende
        {
            Dispatcher.BeginInvoke(new Action(() => {
                canvas.Children.Clear();
                MainWindow mw = (MainWindow)Application.Current.MainWindow;
                mw.Content = new Hangman_menu();
            }));
        }
        public void ResetPopUp()
        {
            popup.Width = 300;
            popupcontent.Width = 300;
        }                                 //Größe von PopUp wieder anpassen
    }
}
