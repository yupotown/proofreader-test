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
        }

        /// <summary>
        /// 正しい単語リストの中から、指定した文字列に近い単語を取得する。
        /// 専用のハッシュ値を使って単語を絞ってから、レーベンシュタイン距離を使って処理する。
        /// </summary>
        /// <param name="word">文字列</param>
        /// <param name="candidatesCount">絞る単語の個数の最小値</param>
        /// <returns></returns>
        public ProofreaderResult Proofread(string word, int candidatesCount = 5)
        {
            // 元から正しい単語ならそのまま返す
            if (hashes.ContainsKey(word))
            {
                return new ProofreaderResult(word, new List<string> { word });
            }

            // 辞書の各単語とのハッシュ距離を計算する
            var hash = generateHash(word);
            var allWords = hashes.Keys.ToList(); // うーん
            var hashDists = new int[allWords.Count];
            for (var i = 0; i < allWords.Count; ++i)
            {
                hashDists[i] = measureHashDist(hash, hashes[allWords[i]]);
            }
            var candidates = new List<string>();
            for (var dist = 0; candidates.Count < candidatesCount; ++dist)
            {
                for (var i = 0; i < allWords.Count; ++i)
                {
                    if (hashDists[i] == dist)
                    {
                        candidates.Add(allWords[i]);
                    }
                }
            }

            #region debug print
            if (DebugPrint)
            {
                var distList = Enumerable.Range(0, allWords.Count)
                    .Select(i => Tuple.Create(allWords[i], hashDists[i]))
                    .ToList();
                distList.Sort((a, b) => a.Item2.CompareTo(b.Item2));
                Console.Error.WriteLine("word: {0}", word);
                foreach (var distPair in distList)
                {
                    Console.Error.WriteLine("{0}\t{1}", distPair.Item2, distPair.Item1);
                }
            }
            #endregion

            // 候補の中でレーベンシュタイン距離が最も近いものを返す
            var min = int.MaxValue;
            var res = new List<string>();
            foreach (var cw in candidates)
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

        /// <summary>
        /// 正しい単語リストの中から、指定した文字列に近い単語を取得する。
        /// レーベンシュタイン距離のみを使って処理する。
        /// </summary>
        /// <param name="word">文字列</param>
        /// <returns></returns>
        public ProofreaderResult ProofreadOld(string word)
        {
            // 元から正しい単語ならそのまま返す
            if (correctWordsOld.Contains(word))
            {
                return new ProofreaderResult(word, new List<string> { word });
            }

            // レーベンシュタイン距離が最も近いものを返す
            var min = int.MaxValue;
            var res = new List<string>();
            foreach (var cw in hashes.Keys)
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

        /// <summary>
        /// 2つの文字列のレーベンシュタイン距離(編集距離)を計算する。
        /// </summary>
        /// <param name="a">文字列</param>
        /// <param name="b">文字列</param>
        /// <returns>レーベンシュタイン距離</returns>
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

        /// <summary>
        /// 正しい単語リストに単語を追加する。
        /// </summary>
        /// <param name="word">正しい単語</param>
        public void AddCorrectWord(string word)
        {
            hashes.Add(word, generateHash(word));
            correctWords.Add(word);
            correctWordsOld.Add(word);
        }

        /// <summary>
        /// 1のビット数を数える。
        /// </summary>
        /// <param name="bits">非負整数値</param>
        /// <returns>ビット数</returns>
        public int CountBits(UInt64 bits)
        {
            bits = (bits & 0x5555555555555555u) + (bits >> 1 & 0x5555555555555555u);
            bits = (bits & 0x3333333333333333u) + (bits >> 2 & 0x3333333333333333u);
            bits = (bits & 0x0f0f0f0f0f0f0f0fu) + (bits >> 4 & 0x0f0f0f0f0f0f0f0fu);
            bits = (bits & 0x00ff00ff00ff00ffu) + (bits >> 8 & 0x00ff00ff00ff00ffu);
            bits = (bits & 0x0000ffff0000ffffu) + (bits >> 16 & 0x0000ffff0000ffffu);
            bits = (bits & 0x00000000ffffffffu) + (bits >> 32 & 0x00000000ffffffffu);
            return (int)bits;
        }

        /// <summary>
        /// 1のビット数を数える。
        /// </summary>
        /// <param name="bits">非負整数値</param>
        /// <returns>ビット数</returns>
        public int CountBitsOld(UInt64 bits)
        {
            var res = 0;
            UInt64 mask = 1;
            for (var i = 0; i < 64; ++i)
            {
                if ((bits & mask) != 0)
                {
                    ++res;
                }
                mask <<= 1;
            }
            return res;
        }

        /// <summary>
        /// デバッグ用出力の有無
        /// </summary>
        public bool DebugPrint { get; set; }

        private Dictionary<string, UInt64> hashes = new Dictionary<string,ulong>();
        private List<string> correctWords = new List<string>();
        private HashSet<string> correctWordsOld = new HashSet<string>();

        /// <summary>
        /// 文字列のハッシュ値を生成する。
        /// </summary>
        /// <param name="word">文字列</param>
        /// <returns>ハッシュ値</returns>
        private UInt64 generateHash(string word)
        {
            UInt64 res = 0;
            foreach (var ch in word)
            {
                res |= ((UInt64)1) << (((int)ch) % 64 - 1);
            }
            return res;
        }

        /// <summary>
        /// 2つのハッシュ値の距離を計算する。
        /// </summary>
        /// <param name="a">ハッシュ値</param>
        /// <param name="b">ハッシュ値</param>
        /// <returns>ハッシュ値の距離</returns>
        private int measureHashDist(UInt64 a, UInt64 b)
        {
            var ab = a & b;
            if (ab == 0)
            {
                return int.MaxValue;
            }
            return Math.Max(CountBits(a - ab), CountBits(b - ab));
        }
    }
}
