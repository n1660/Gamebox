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
using System.Diagnostics;

namespace Hangman
{
    public partial class MainWindow : Window
    {
        string randomWord = Spiel.NewWord();
        string solution;
        int maxfehler = 9;
        int anzfehler = 0;
        Label[] lbls = new Label[15];

        public MainWindow()
        {
            InitializeComponent();
            Start();
        }

        private void Start()
        {
            randomWord = Spiel.NewWord();
            anzfehler = 0;
            Show_Lines();
            Show_Buttons();
            Show_Image();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            solution = textbox.Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string letter = btn.Content.ToString();
            bool containsLetter = randomWord.Contains(letter);
            if(containsLetter)
            {
                ShowNewWord(letter);
                
            }
            else
            { 
                MessageBox.Show(String.Format("Buchstabe {0} nicht enthalten!", letter));
                anzfehler++;
                Show_Image();
                if (anzfehler == maxfehler)
                { 
                    MessageBox.Show("Leider verloren!");
                    QuitGame();
                }
            }
        }

        private void ShowNewWord(string letter)
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
            string lbl = "";
            for(int i = 0; i < randomWord.Length; i++)
            {
                lbl += lbls[i];
            }
            int won = lbl.IndexOf("_");
            if (won == -1)
            {
                MessageBox.Show("Gewonnen!");
                QuitGame();
            }
        }

        private void Button_Click_Submit(object sender, RoutedEventArgs e)
        {
            if(solution == randomWord)
            {
                MessageBox.Show("Gewonnen!");
                for(int i = 0; i < randomWord.Length; i++)
                    lbls[i].Content = randomWord[i];
                //QuitGame();
            }
            else
            {
                MessageBox.Show("Leider das falsche Wort!");
                textbox.Text = "";
                anzfehler++;
                Show_Image();
                if (anzfehler == maxfehler)
                {
                    MessageBox.Show("Leider verloren!");
                    QuitGame();
                }
            }
        }
        private void Show_Lines()
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
        private void Show_Buttons()
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
        private void Show_Image()
        {
            bild.Source = GetImage(@"D:\ProjektMultiplayer\Hangman\Hangman\Images\" + anzfehler +".png");
        }
        private static BitmapImage GetImage(string imageUri)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(imageUri, UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();
            return bitmapImage;
        }
        private void QuitGame()
        {
            for(int i = 0; i< randomWord.Length; i++)
            {
                canvas.Children.Remove(lbls[i]);
            }
            Start();
        }

        
    }
}
