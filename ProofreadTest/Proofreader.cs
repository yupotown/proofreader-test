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
            CorrectWords = new List<string>();
        }

        public ProofreaderResult Proofread(string word)
        {
            return new ProofreaderResult(word, word);
        }

        public List<string> CorrectWords { get; private set; }
    }
}
