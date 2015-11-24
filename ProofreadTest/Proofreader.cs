using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProofreadTest
{
    public class Proofreader
    {
        public Proofreader()
        {
            CorrectWords = new HashSet<string>();
        }

        public ProofreaderResult Proofread(string word)
        {
            // 元から正しい単語ならそのまま返す
            if (CorrectWords.Contains(word))
            {
                return new ProofreaderResult(word, new List<string> { word });
            }

            // レーベンシュタイン距離が最も近いものを返す
            var min = int.MaxValue;
            var res = new List<string>();
            foreach (var cw in CorrectWords)
            {
                var dist = MeasureLevenshtein(word, cw);
                if (dist < min)
                {
                    min = dist;
                    res = new List<string> { cw };
                }
                else if (dist == min)
                {
                    res.Add(cw);
                }
            }
            return new ProofreaderResult(word, res);
        }

        public int MeasureLevenshtein(string a, string b)
        {
            int[,] d = new int[a.Length + 1, b.Length + 1];
            for (var i = 0; i <= a.Length; ++i) d[i, 0] = i;
            for (var j = 0; j <= b.Length; ++j) d[0, j] = j;
            for (var i = 1; i <= a.Length; ++i)
            {
                for (var j = 1; j <= b.Length; ++j)
                {
                    d[i, j] = d[i - 1, j - 1] + (a[i - 1] == b[j - 1] ? 0 : 1);
                    d[i, j] = Math.Min(d[i, j], d[i - 1, j] + 1);
                    d[i, j] = Math.Min(d[i, j], d[i, j - 1] + 1);
                }
            }
            return d[a.Length, b.Length];
        }

        public HashSet<string> CorrectWords { get; private set; }
    }
}
