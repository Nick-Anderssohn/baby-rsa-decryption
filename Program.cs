using System;
using System.Numerics;
using System.Collections.Generic;

namespace PrivateKeyCracker
{
    class Program
    {
        private static BigInteger p, q, d, n, e;
        private static Dictionary<char, char> babyAsciiTable = new Dictionary<char, char>()
        {
            {'A', '1'},
            {'E', '2'},
            {'G', '3'},
            {'I', '4'},
            {'O', '5'},
            {'R', '6'},
            {'T', '7'},
            {'X', '8'},
            {'!', '9'},
        };

        static void Main(string[] args)
        {
            e = 49;
            n = 10539750919;
            Console.WriteLine("e: " + e.ToString());
            Console.WriteLine("n: " + n.ToString());
            Console.WriteLine("please be patient...this takes a bit to run");
            FindPQ();
            FindD();
            Console.WriteLine("\np: " + p.ToString());
            Console.WriteLine("q: " + q.ToString());
            Console.WriteLine("d: " + d.ToString());
            Console.WriteLine("decrypting please wait...");
            string c = "ITG!AAEXEX IRRG!IGRXI OIXGEREAGO";
            Console.WriteLine("cipher text: " + c);
            Console.WriteLine("numeric version: " + LettersToNumbers(c));
            // String m1 = Decrypt("ITG!AAEXEX");
            // Console.WriteLine("M1: " + m1.ToString());
            // String m2 = Decrypt("IRRG!IGRXI");
            // Console.WriteLine("M2: " + m2.ToString());
            // String m3 = Decrypt("OIXGEREAGO");
            // Console.WriteLine("M3: " + m3.ToString());
            String m1 = NumbersToLetters("36217");
            String m2 = NumbersToLetters("98483");
            String m3 = NumbersToLetters("57847");
            Console.WriteLine("m1: " + m1);
            Console.WriteLine("m2: " + m2);
            Console.WriteLine("m3: " + m3);
            Console.WriteLine(m1 + m2 + m3);
        }


        static void FindPQ()
        {
            for (p = 2; p < n; p++)
            {
                q = n / p;
                if (BigInteger.GreatestCommonDivisor(e, (p - 1) * (q - 1)) == 1 && IsPrime(p) && IsPrime(q) && p * q == n)
                {
                    return;
                }
            }
        }

        //can be very slow depending on size of checkMe...fine for this hw assignment
        static bool IsPrime(BigInteger checkMe)
        {
            for (int i = 2; i < checkMe; i++)
            {
                if (checkMe % i == 0)
                    return false;
            }
            return true;
        }

        static void FindD()
        {
            BigInteger pq = (p - 1) * (q - 1);
            //d = BigInteger.ModPow(e, e, pq);
            d = ModInverse(e, pq);
        }

        //http://stackoverflow.com/questions/7483706/c-sharp-modinverse-function
        static BigInteger ModInverse(BigInteger a, BigInteger n)
        {
            BigInteger i = n, v = 0, d = 1;
            while (a > 0)
            {
                BigInteger t = i / a, x = a;
                a = i % x;
                i = x;
                x = d;
                d = v - t * x;
                v = x;
            }
            v %= n;
            if (v < 0) v = (v + n) % n;
            return v;
        }

        static string LettersToNumbers(string str)
        {
            foreach (var pair in babyAsciiTable)
            {
                str = str.Replace(pair.Key, pair.Value);
            }
            return str;
        }

        static string NumbersToLetters(string str)
        {
            foreach (var pair in babyAsciiTable)
            {
                str = str.Replace(pair.Value, pair.Key);
            }
            return str;
        }

        //currently runs out of memory...
        static string Decrypt(string c)
        {
            c = LettersToNumbers(c);
            BigInteger numeric = BigInteger.Parse(c);
            BigInteger raised = BigInteger.Pow(numeric, int.Parse(d.ToString()));
            // BigInteger raised = BigInteger.Parse(numeric.ToString());
            // for (int i = 0; i < d; i++)
            // {
            //     Console.WriteLine(i);
            //     raised *= numeric;
            // }
            numeric = raised % n;
            c = numeric.ToString();
            c = NumbersToLetters(c);
            return c;
        }
    }
}
