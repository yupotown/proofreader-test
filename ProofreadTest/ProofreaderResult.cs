using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProofreadTest
{
    public class ProofreaderResult
    {
        public ProofreaderResult(string original, IEnumerable<string> words)
        {
            this.OriginalWord = original;
            this.CorrectWords = new HashSet<string>();
            foreach (var word in words)
            {
                this.CorrectWords.Add(word);
            }
        }

        public string OriginalWord { get; private set; }
        public HashSet<string> CorrectWords { get; private set; }
    }
}
