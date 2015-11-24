using System;
using System.Collections.Generic;
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

            // 正しい単語一覧
            pr.CorrectWords.AddRange(new List<string>
            {
                "apple",
                "tomorrow",
                "white",
                "while",
                "りんご",
                "ごりら",
                "†漆黒のらっぱ†",
            });

            // テストケース
            var cases = new Dictionary<string, List<string>>();
            cases.Add("apple", new List<string> { "apple" });
            cases.Add("tommorow", new List<string> { "tomorrow" });
            cases.Add("whie", new List<string> { "white", "while" });
            cases.Add("whiteご", new List<string> { "white" });
            cases.Add("りwhile", new List<string> { "while" });
            cases.Add("りんご", new List<string> { "りんご" });
            cases.Add("ごりｒあ", new List<string> { "ごりら" });
            cases.Add("†漆黒のラッパ†", new List<string> { "†漆黒のらっぱ†" });

            // テスト
            foreach (var cas in cases)
            {
                var res = pr.Proofread(cas.Key);
                Console.WriteLine("{0} : {1} -> {2}",
                    cas.Value.Contains(res.Word),
                    cas.Key, res.Word);
            }
        }
    }
}
