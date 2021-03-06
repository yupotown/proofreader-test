﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProofreadTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var pr = new Proofreader();
            //pr.DebugPrint = true;

            // 正しい単語一覧
            foreach (var word in File.ReadAllLines("words.txt"))
            {
                pr.AddCorrectWord(word);
            }

            // テストケース
            var cases = new Dictionary<string, HashSet<string>>();
            cases.Add("apple", new HashSet<string> { "apple" });
            cases.Add("tommorow", new HashSet<string> { "tomorrow" });
            cases.Add("whie", new HashSet<string> { "white", "while" });
            cases.Add("whiteご", new HashSet<string> { "white" });
            cases.Add("りwhile", new HashSet<string> { "while" });
            cases.Add("りんご", new HashSet<string> { "りんご" });
            cases.Add("ごりｒあ", new HashSet<string> { "ごりら" });
            cases.Add("†漆黒のラッパ†", new HashSet<string> { "†漆黒のらっぱ†" });

            // テスト
            foreach (var cas in cases)
            {
                var res = pr.Proofread(cas.Key);
                Console.WriteLine("{0} : {1} -> {2} ({3})",
                    cas.Value.All(word => res.CorrectWords.Contains(word)),
                    cas.Key,
                    ToString(res.CorrectWords),
                    ToString(cas.Value));
            }
        }

        static string ToString(IEnumerable<string> words)
        {
            return string.Format("{{{0}}}", string.Join(", ", words));
        }
    }
}
