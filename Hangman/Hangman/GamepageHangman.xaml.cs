using System;
using System.IO;
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
using System.Diagnostics;

namespace Hangman
{
    public partial class GamepageHangman : Page
    {
        string randomWord = Spiel.NewWord();
        string checkrand = "";
        string solution;
        string alreadyUsed = "";
        int maxfehler = 9;
        int anzfehler = 0;
        Label[] lbls = new Label[15];

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
                    path = @"3-5Buchstaben.txt";
                    break;
                case 2:
                    path = @"6-8Buchstaben.txt";
                    break;
                case 3:
                    path = @"Extrem.txt";
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
            randomWord = NewWord();
            anzfehler = 0;
            Show_Lines();
            Show_Buttons();
            Show_Image();
        }
        public void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            solution = textbox.Text;
        }
        public void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string letter = btn.Content.ToString();
            if(letter != "Hint")
            { 
                bool containsLetter = randomWord.Contains(letter);
                if (containsLetter)
                {
                    ShowNewWord(letter);
                }
                else
                {
                    bool letterUsed = alreadyUsed.Contains(letter);
                    if(letterUsed)
                    {
                        MessageBox.Show(String.Format("Buchstabe {0} bereits gewählt!", letter));
                    }
                    else
                    { 
                        MessageBox.Show(String.Format("Buchstabe {0} nicht enthalten!", letter));
                        MistakeWasMade();
                    }
                    alreadyUsed += letter;
                }
            }
            else  //Hint wurde gedrückt
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
                CheckIfWordShown("Durch schummeln erraten!");

                checkrand += randString;
                MistakeWasMade();
            }
        }
        public void ShowNewWord(string letter)
        {
            int pos = randomWord.IndexOf(letter);
            lbls[pos].Content = letter;
            int pos2 = randomWord.IndexOf(letter, pos + 1);
            if (pos2 != -1)  // weiterer Buchstabe gefunden
            {
                lbls[pos2].Content = letter;
                int pos3 = randomWord.IndexOf(letter, pos2 + 1);
                if (pos3 != -1)
                {
                    lbls[pos3].Content = letter;
                    int pos4 = randomWord.IndexOf(letter, pos3 + 1);
                    if (pos4 != -1)
                        lbls[pos4].Content = letter;
                }
            }
            CheckIfWordShown("Gewonnen!");
        }
        public void CheckIfWordShown(string Message)
        {
            string lbl = "";
            for (int i = 0; i < randomWord.Length; i++)
            {
                lbl += lbls[i];
            }
            int won = lbl.IndexOf("_");
            if (won == -1)
            {
                MessageBox.Show(Message);
                QuitGame();
            }
        }
        public void MistakeWasMade()
        {
            anzfehler++;
            Show_Image();
            if (anzfehler == maxfehler)
            {
                MessageBox.Show("Leider verloren!");
                QuitGame();
            }
        }
        public void Button_Click_Submit(object sender, RoutedEventArgs e)
        {
            if (solution == randomWord)
            {
                MessageBox.Show("Gewonnen!");
                for (int i = 0; i < randomWord.Length; i++)
                    lbls[i].Content = randomWord[i];
                QuitGame();
            }
            else
            {
                MessageBox.Show("Leider das falsche Wort!");
                textbox.Text = "";
                MistakeWasMade();
            }
        }   
        public void Show_Lines()
        {
            int CvLeft = 11;
            int CvTop = 50;

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
                    Name = "Button" + i
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
        public void QuitGame()
        {
            for (int i = 0; i < randomWord.Length; i++)   //alle lbls beenden
            {
                canvas.Children.Remove(lbls[i]);
            }
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            mw.Content = new Hangman_menu();
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
    }
}
