using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WordSearch
{
    public class WordSearchEntry
    {
            private Dictionary<char, WordSearchEntry> _next;
            public bool IsWord { get; private set; }
            public string Word { get; private set; }

            public WordSearchEntry(string sofar, bool word)
            {
                Word = sofar;
                IsWord = word;
            }

            private Dictionary<char, WordSearchEntry> Next
            {
                get { return _next ?? (_next = new Dictionary<char, WordSearchEntry>(1)); }
            }

            public WordSearchEntry this[char next]
            {
                get
                {
                    WordSearchEntry nextChar;
                    if (!Next.TryGetValue(next, out nextChar))
                        return null;
                    return nextChar;
                }
                set
                {
                    Debug.Assert(this[next] == null);
                    Next[next] = value;
                }
            }
            public void AddWordTail(string tail)
            {
            WordSearchEntry nextChar = this[tail[0]];
                var nextIsWord = tail.Length == 1;
                if (nextChar == null)
                {
                    nextChar = new WordSearchEntry(Word + tail[0], nextIsWord);
                    this[tail[0]] = nextChar;
                }
                if (!nextIsWord) 
                {
                    nextChar.AddWordTail(tail.Substring(1)); // get the character and do some recursion
                }
            }
        }
 
}
