using System;
using System.Collections.Generic;
using System.Text;

namespace Word_Bearer
{
    class WordData : IComparable
    {
        public string word;
        public Dictionary<string, int> cases = new Dictionary<string, int>();
        public int occuredTimes;
        public WordData(string s) => word = s;
        public void Occur(string word)
        {
            if (!cases.ContainsKey(word))
                cases[word] = 0;
            cases[word]++;
            occuredTimes++;
        }
        public static bool operator ==(WordData wdl, WordData wdr)
        { return wdl.word == wdr.word; }
        public static bool operator !=(WordData wdl, WordData wdr)
        { return !(wdl == wdr); }
        public override bool Equals(Object wd) => this == (WordData)wd;
        public override int GetHashCode() => word.GetHashCode();
        public int CompareTo(object? obj)
        {
            if (((WordData)obj).occuredTimes == this.occuredTimes) return 0;
            return ((WordData)obj).occuredTimes < this.occuredTimes ? -1 : 1;
        }
    }
}
