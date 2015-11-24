using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProofreadTest
{
    public class ProofreaderResult
    {
        public ProofreaderResult(string original, string word)
        {
            this.OriginalWord = original;
            this.Word = word;
        }

        public string OriginalWord { get; private set; }
        public string Word { get; private set; }
    }
}
