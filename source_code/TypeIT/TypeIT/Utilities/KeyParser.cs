using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeIT.Utilities
{
    class KeyParser
    {
        public static int TotalMistakes { get; set; }
        public static double AverageTypingSpeed { get; set; }
        public static double AverageAccuracy { get; set; }
        public static DateTime StartTime { get; set; }
        public static void Main()
        {
            string text = File.ReadAllText("../../20.txt");
            string first = "";
            first += text[0];
            text = text[1..];
            int i = 0;

            WaitForFirst(first);

            StartTime = DateTime.Now;

            do
            {
                while (GetWord(text, i) == "")
                {
                    i++;
                }
                TypeWord(GetWord(text, i));
                CalculateTypingSpeed(i);
                CalculateAccuracy(text.Length);
                i++;
            } while (i < GetNumberOfWords(text));
        }

        public static void WaitForFirst(string first)
        {
            ConsoleKeyInfo key;
            string typedFirst = "";
            do
            {
                key = Console.ReadKey();

                if (key.Key != ConsoleKey.Backspace)
                {
                    typedFirst += key.KeyChar;
                }
                else
                {
                    if (typedFirst.Length > 0)
                    {
                        typedFirst = typedFirst.Substring(0, typedFirst.Length - 1);
                    }
                }

            } while (typedFirst != first);

        }

        public static int CalculateErrorSpace(string word)
        {
            int minSpace = 5;
            int calculatedSpace = (int)((word.Length * 1.5) / 2);

            return minSpace > calculatedSpace ? minSpace : calculatedSpace;
        }

        public static void CalculateTypingSpeed(int wordNumber)
        {
            AverageTypingSpeed = Math.Round((wordNumber / (DateTime.Now - StartTime).TotalSeconds) * 60, 2);
        }

        public static void CalculateAccuracy(int totalChars)
        {
            AverageAccuracy = Math.Round(((Double)(totalChars - TotalMistakes) / totalChars) * 100, 2);
        }

        public static void TypeWord(string word)
        {
            int currentMistakes = 0;
            int maxCurrentMistakes = CalculateErrorSpace(word);

            ConsoleKeyInfo keyInfo;

            string text = "";

            int i = 0;

            do
            {
                keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.Spacebar)
                {
                    if (text.Equals(word))
                    {
                        return;
                    }
                }

                if (keyInfo.Key != ConsoleKey.Backspace && currentMistakes < maxCurrentMistakes)
                {
                    text += keyInfo.KeyChar;
                }


                if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (text.Length > 0)
                    {
                        if (currentMistakes > 0)
                        {
                            currentMistakes--;
                        }
                        text = text[0..^1];
                    }
                    i--;
                }
                else if (keyInfo.KeyChar != word[i])
                {
                    if (currentMistakes < maxCurrentMistakes)
                    {
                        currentMistakes++;
                    }
                    TotalMistakes++;
                    i--;
                }

                if (i < word.Length - 1)
                {
                    i++;
                }
            } while (true);
        }

        public static string GetWord(string text, int wordIndex)
        {
            string[] words = text.Split(' ');

            if (wordIndex < GetNumberOfWords(text))
            {
                return words[wordIndex];
            }
            return null;
        }

        public static int GetNumberOfWords(string text)
        {
            string[] words = text.Split(' ');

            return words.Length;
        }
    }
}
