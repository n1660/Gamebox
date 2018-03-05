using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace Hangman
{
    class Spiel
    {
        static public string NewWord()
        {
            string path = @"Montagsmaler_Liste.txt";

            string readText = File.ReadAllText(path);
            string[] wort = readText.Split('\n');

            Random rand = new Random();
            int zufallszahl = rand.Next(0,97);
            string randomWord = wort[zufallszahl];

            randomWord = randomWord.Remove(randomWord.Length - 1);

            return randomWord;
        }
        static public void Game()
        {
            int anzfehler = 20;
            //Deklaration
            int fehler = 0;
            string geheimwort = NewWord();
            Console.WriteLine(geheimwort);
            //Suchwort bestimmen
            string suchwort = "";
            for (int i = 0; i < geheimwort.Length; i++)
            {
                suchwort += "-";
            }
            //Spielablauf
            Console.WriteLine("Hangman:\n-------------\n");
            Console.WriteLine("Anzahl der Buchstaben: {0}", geheimwort.Length);
            Console.WriteLine("Geheimwort: " + suchwort);
            
            while (fehler < anzfehler && suchwort != geheimwort)
            {
                char eingabe;
                eingabe = Console.ReadKey().KeyChar;
                string kopieSuchwort = "";
                bool treffer = false;
                for (int i = 0; i < geheimwort.Length; i++)
                {
                    if (eingabe == geheimwort[i])
                    {
                        kopieSuchwort += eingabe;
                        treffer = true;
                    }
                    else
                    {
                        kopieSuchwort += suchwort[i];
                    }
                }
                if (!treffer)
                {
                    fehler++;
                    Console.WriteLine("\nFehler: {0}\n", fehler);
                }
                suchwort = kopieSuchwort;
                Console.WriteLine("\nGeheimwort: " + suchwort);
            }
            if (fehler >= anzfehler)
            {
                Console.WriteLine("Verloren");
            }
            else
            {
                Console.WriteLine("Gewonnen!");
            }
        }
    }
}
