using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace Word_Bearer
{
    class Program
    {
        public const int WordMaxLength = 40;
        public const string Alphabet = "0123456789qwertyuiopasdfghjklzxcvbnmйцукенгшщзхъфывапролджэячсмитьбюёßäüöQWERTYUIOPASDFGHJKLZXCVBNMЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮЁÄÜÖ";
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("To process one file or many, you can simply throw them onto the WordBearer.exe;\nEnter the full or relative path to file to process:");
                var file = Console.ReadLine();
                Process(file);
            }
            else foreach (string file in args)
            {
                Process(file);
            }
        }

        private static void Process(string file)
        {
            List<WordData> wordsFound;
            FindWords(file, out wordsFound);

            var builderResultWords = new StringBuilder("File: " + file + "\nWords(word count): ");
            var builderResultLiterals = new StringBuilder("\n\nLiterals(maybe): ");

            foreach (WordData word in wordsFound)
                AnalizeWord(builderResultWords, builderResultLiterals, word);

            var result = builderResultWords.Append(builderResultLiterals).ToString();
            var fi = new FileInfo(file);
            WriteResult(fi.DirectoryName + "\\WB" + fi.Name, result);
        }

        private static void FindWords(string file, out List<WordData> wordsFound)
        {
            wordsFound = new List<WordData>();
            var oldFile = new StreamReader(file);

            var builder = new StringBuilder();
            var content = oldFile.ReadToEnd().GetEnumerator();
            oldFile.Close();
            while (true)
            {
                var end = !content.MoveNext();
                char c = end ? (char)0 : content.Current;
                if (!Alphabet.Contains(c))
                {
                    SaveWord(wordsFound, builder.ToString());
                    builder = new StringBuilder();
                }
                else builder.Append(c);
                if (end) break;
            }
            wordsFound.Sort();
        }

        private static void SaveWord(List<WordData> wordsFound, string temp)
        {
            if (temp.Length > 0 && temp.Length < WordMaxLength)
            {
                if (!wordsFound.Contains(new WordData(temp.ToLower())))
                    wordsFound.Add(new WordData(temp.ToLower()));
                wordsFound[wordsFound.IndexOf(new WordData(temp.ToLower()))].Occur(temp);
            }
        }

        private static void AnalizeWord(StringBuilder builderResultWords, StringBuilder builderResultLiterals, WordData word)
        {
            int maxCount = 0;
            string maxWord = word.word;
            foreach (var wordCountPair in word.cases)
                if (wordCountPair.Value > maxCount) 
                { maxCount = wordCountPair.Value; maxWord = wordCountPair.Key; }
            if (maxWord != word.word)
                builderResultLiterals.Append(maxWord + " " + word.occuredTimes + "; ");
            builderResultWords.Append(maxWord + " " + word.occuredTimes + "; ");
        }

        private static void WriteResult(string file, string result)
        {
            var dest = new FileInfo(file);
            if (dest.Exists) throw new Exception("File already exists; can't write");
            var newFile = dest.CreateText();
            newFile.Write(result);
            newFile.Flush();
            newFile.Close();
        }
    }
}
