using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordSearch
{
    class WordSearchFunctions
    {
        public static WordSearchEntry LoadWords(IEnumerable<string> words)
        {
            var root = new WordSearchEntry("", false);
            foreach (var word in words)
            {
                if (word.Length > 3)
                    root.AddWordTail(word.ToLower());
            }
            return root;
        }

        public static WordSearchEntry LoadWords(IEnumerable<string> words, HashSet<char> chars)
        {
            var root = new WordSearchEntry("", false);
            foreach (var word in words)
            {
                var theword = word.ToLower();
                if (word.Length > 3 && theword.All(chars.Contains))
                    root.AddWordTail(theword);
            }
            return root;
        }

        public static IEnumerable<WordSearchSol> FindWords(char[,] chars, WordSearchEntry dictionary)
        {
            for (int x = 0; x < chars.GetLength(0); x++)
                for (int y = 0; y < chars.GetLength(1); y++)
                {
                    foreach (var word in FindWords(
                            chars,
                            new bool[chars.GetLength(0), chars.GetLength(1)],
                            dictionary,
                            new Stack<Point>(),
                            x,
                            y
                            ))
                        yield return word;
                }
        }

        private static IEnumerable<WordSearchSol> FindWords(char[,] chars, bool[,] visited, WordSearchEntry lastStep, Stack<Point> path, int x, int y)
        {
            if (x < 0 || y < 0 || x >= chars.GetLength(0) || y >= chars.GetLength(0) || visited[x, y])
                yield break;
            var nextstep = lastStep[chars[x, y]];
            if (nextstep == null)  
                yield break;
            path.Push(new Point { X = x, Y = y });
            if (nextstep.IsWord) 
                yield return new WordSearchSol(nextstep.Word, path);
            // Return possible longer words.
            var newVisited = new bool[chars.GetLength(0), chars.GetLength(1)];
            Array.Copy(visited, newVisited, visited.Length);
            newVisited[x, y] = true;
            foreach (var word in
                FindWords(chars, newVisited, nextstep, path, x + 1, y).Concat(
                FindWords(chars, newVisited, nextstep, path, x, y + 1)).Concat(
                FindWords(chars, newVisited, nextstep, path, x - 1, y)).Concat(
                FindWords(chars, newVisited, nextstep, path, x, y - 1)))
                yield return word;
            path.Pop();
        }
    }
}
