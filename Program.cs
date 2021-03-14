using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace WordSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            var field = GenerateField(5, 5);
            DisplayField(field);

            var fileToRead = "words.txt";
            Console.WriteLine("Reading from file {0}", fileToRead);
            var words = File.ReadAllLines(fileToRead);
            Console.WriteLine("Loading dictionary {0}", fileToRead);
            var dictionary = WordSearchFunctions.LoadWords(words, UniqueCharsIn(field));
            Console.WriteLine("Loaded {0} words from file {1}", words.Length, fileToRead);
            Console.WriteLine("Press enter to list the words found...");
            Console.ReadLine();
            var wordsInField = WordSearchFunctions.FindWords(field, dictionary).ToArray();
            Console.WriteLine("Number of words found {0} : ", wordsInField.Length);

            foreach (var foundword in wordsInField.Select(w => w.Word).OrderBy(w => w))
                Console.WriteLine(foundword);

            string word;
            do
            {
                word = Console.ReadLine();
                var wordToDisplay = wordsInField.FirstOrDefault(w => w.Word == word);
                if (wordToDisplay == null) 
                {
                    // either its not a word or its not in field.
                    Console.WriteLine("Sorry!, '{0}' is not a word in the field.", word);
                }
                else
                {
                    DisplayWord(wordToDisplay, field);
                }
            } while (!String.IsNullOrEmpty(word));
        }

        static void DisplayField(char[,] chars)
        {
            for (int y = 0; y < chars.GetLength(1); y++)
            {
                for (int x = 0; x < chars.GetLength(0); x++)
                {
                    Console.Write(chars[x, y]);
                    Console.Write(' ');
                }
                Console.WriteLine();
            }
        }

        private static char[,] GenerateField(int xmax, int ymax)
        {
            // English scrabble distribution for generation of field.
            // This is used for the frequency of letter usage in the language
            var chars =
                new string('a', 9) +
                new string('b', 2) +
                new string('c', 2) +
                new string('d', 4) +
                new string('e', 12) +
                new string('f', 2) +
                new string('g', 3) +
                new string('h', 2) +
                new string('i', 9) +
                new string('j', 1) +
                new string('k', 1) +
                new string('l', 4) +
                new string('m', 2) +
                new string('n', 6) +
                new string('o', 8) +
                new string('p', 2) +
                new string('q', 1) +
                new string('r', 6) +
                new string('s', 4) +
                new string('t', 6) +
                new string('u', 4) +
                new string('v', 2) +
                new string('w', 2) +
                new string('x', 1) +
                new string('y', 2) +
                new string('z', 1);
            var field = new char[xmax, ymax];
            var random = new Random();
            for (int x = 0; x < xmax; x++)
                for (int y = 0; y < ymax; y++)
                {
                    field[x, y] = chars[random.Next(chars.Length)];
                }
            return field;
        }

        private static HashSet<char> UniqueCharsIn(char[,] chars)
        {
            return new HashSet<char>(chars.Cast<char>().Distinct());
        }

        private static void DisplayWord(WordSearchSol word, char[,] field)
        {
            for (int y = 0; y < field.GetLength(1); y++)
            {
                for (int x = 0; x < field.GetLength(0); x++)
                {
                    var thispoint = new Point { X = x, Y = y };
                    if (word.Path.Contains(thispoint))
                        Console.BackgroundColor = ConsoleColor.White;
                    Console.Write(field[x, y].ToString().ToUpper());
                    Console.BackgroundColor = ConsoleColor.White;
                    if (word.PathContains(thispoint, new Point { X = x + 1, Y = y }))
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.Write("  ");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.WriteLine();
                for (int x = 0; x < field.GetLength(0); x++)
                {
                    var thispoint = new Point { X = x, Y = y };
                    if (word.PathContains(thispoint, new Point { X = x, Y = y + 1 }))
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write("  ");
                }
                Console.WriteLine();
            }
        }

    }
}
