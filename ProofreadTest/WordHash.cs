using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProofreadTest
{
    public struct WordHash
    {
        /// <summary>
        /// 文字列のハッシュ値を生成する。
        /// </summary>
        /// <param name="word">文字列</param>
        public WordHash(string word)
        {
            sjis = Encoding.GetEncoding("shift_jis");

            val1 = 0;
            val2 = 0;
            foreach (var ch in word)
            {
                if (sjis.GetByteCount(new char[] { ch }) == 1)
                {
                    val1 |= (ulong)1 << (int)ch % 64 - 1;
                }
                else
                {
                    val2 |= (ulong)1 << (int)ch % 64 - 1;
                }
            }
        }

        /// <summary>
        /// 2つのハッシュ値の距離を計算する。
        /// </summary>
        /// <param name="a">ハッシュ値</param>
        /// <param name="b">ハッシュ値</param>
        /// <returns>ハッシュ値の距離</returns>
        public static int MeasureDistance(WordHash a, WordHash b)
        {
            var ab1 = a.val1 & b.val1;
            var ab2 = a.val2 & b.val2;
            if (ab1 == 0 && ab2 == 0)
            {
                return int.MaxValue;
            }
            else
            {
                return Math.Max(
                    countBits(a.val1 - ab1) + countBits(a.val2 - ab2),
                    countBits(b.val1 - ab1) + countBits(b.val2 - ab2)
                    );
            }
        }

        /// <summary>
        /// 1のビット数を数える。
        /// </summary>
        /// <param name="bits">非負整数値</param>
        /// <returns>ビット数</returns>
        private static int countBits(ulong bits)
        {
            bits = (bits & 0x5555555555555555u) + (bits >> 1 & 0x5555555555555555u);
            bits = (bits & 0x3333333333333333u) + (bits >> 2 & 0x3333333333333333u);
            bits = (bits & 0x0f0f0f0f0f0f0f0fu) + (bits >> 4 & 0x0f0f0f0f0f0f0f0fu);
            bits = (bits & 0x00ff00ff00ff00ffu) + (bits >> 8 & 0x00ff00ff00ff00ffu);
            bits = (bits & 0x0000ffff0000ffffu) + (bits >> 16 & 0x0000ffff0000ffffu);
            bits = (bits & 0x00000000ffffffffu) + (bits >> 32 & 0x00000000ffffffffu);
            return (int)bits;
        }

        private Encoding sjis;
        private ulong val1, val2;
    }
}
