using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace Word_Bearer
{
    class Program
    {
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
            var wordsFound = new List<WordData>();
            var res = new FileInfo(file);
            var dest = new FileInfo(res.DirectoryName + "\\WB" + res.Name);
            var oldFile = new StreamReader(file);
            if (dest.Exists) throw new Exception("File already exists; can't write");

            var builder = new StringBuilder();
            var content = oldFile.ReadToEnd().GetEnumerator();
            oldFile.Close();
            while (true)
            {
                var end = !content.MoveNext();
                char c = end ? (char)0 : content.Current;
                if (!Alphabet.Contains(c))
                {
                    var temp = builder.ToString();
                    if (temp.Length > 0 && temp.Length < 40)
                    {
                        if (!wordsFound.Contains(new WordData(temp.ToLower())))
                            wordsFound.Add(new WordData(temp.ToLower()));
                        wordsFound[wordsFound.IndexOf(new WordData(temp.ToLower()))].Occur(temp);
                    }
                    builder = new StringBuilder();
                }
                else builder.Append(c);
                if (end) break;
            }
            var builderResultWords = new StringBuilder("File: " + file + "\nWords(word count): ");
            var builderResultLiterals = new StringBuilder("\n\nLiterals(maybe): ");
            wordsFound.Sort();
            foreach (WordData word in wordsFound)
            {
                int maxCount = 0;
                string maxWord = word.word;
                foreach (var wordCountPair in word.cases)
                    if (wordCountPair.Value > maxCount) { maxCount = wordCountPair.Value; maxWord = wordCountPair.Key; }
                if (maxWord != word.word)
                    builderResultLiterals.Append(maxWord + " " + word.occuredTimes + "; ");
                builderResultWords.Append(maxWord + " " + word.occuredTimes + "; ");
            }
            var newFile = dest.CreateText();
            var result = builderResultWords.Append(builderResultLiterals).ToString();

            newFile.Write(result);
            newFile.Flush();
            newFile.Close();
        }
    }
}
