using System;
using System.IO;

namespace Lab_5
{

    public class LFSR
    {
        bool[] bits;
        bool[] seq;
        int[] tab_taps;
        int tap_vol;

        public LFSR(int bitCount, string seed, int tap_count, int[] taps)
        {
            bits = new bool[bitCount];
            tab_taps = new int[tap_count];
            tap_vol = tap_count;

            for (int i = 0; i < bitCount; i++)
                bits[i] = seed[i] == '1' ? true : false;

            for (int i = 0; i < tap_count; i++)
                tab_taps[i] = taps[i];
        }

        public string generate(int seq_size)
        {
            seq = new bool[seq_size];

            for (int i = 0; i < seq_size; i++)
            {
                seq[i] = this.Shift();
            }

            string generated_seq = "";

            for (int i = 0; i < seq_size; i++)
            {
                if (seq[i] == true)
                {
                    generated_seq += "1,";
                }
                else
                {
                    generated_seq += "0,";
                }
            }

            return generated_seq;
        }

        public bool Shift()
        {
            //bool bnew = bits[bits.Length - 1] ^ bits[bits.Length - 2];
            //bnew = bnew ^ bits[bits.Length - 3];
            //Console.Write(tab_taps.Length + "\n");

            bool bnew = bits[0];

            if (tab_taps.Length == 1)
            {
                bnew = bits[tab_taps[0]];
            }
            else
            {
                for (int i = 0; i < tab_taps.Length - 1; i++)
                {
                    bool temp;

                    if (i == 0)
                    {
                        temp = bits[tab_taps[i]] ^ bits[tab_taps[i + 1]];
                        bnew = temp;
                    } else
                    {
                        bnew = bnew ^ bits[tab_taps[i + 1]];
                    }
                }
            }

            for (int i = 0; i < bits.Length - 1; i++)
            {
                bits[i] = bits[i + 1];
            }

            bits[bits.Length - 1] = bnew;

            return bnew;
        }

        public void toFile(string seq)
        {
            string path = @"D:\lfsr.txt";

            string appendText = seq + Environment.NewLine;
            File.AppendAllText(path, appendText);
        }
    }

    class Program
    {
        public static int BerlekampMassey(byte[] s)
        {
            int L, N, m, d;
            int n = s.Length;
            byte[] c = new byte[n];
            byte[] b = new byte[n];
            byte[] t = new byte[n];
            b[0] = c[0] = 1;
            N = L = 0;
            m = -1;
            while (N < n)
            {
                d = s[N];
                for (int i = 1; i <= L; i++)
                    d ^= c[i] & s[N - i];           
                if (d == 1)
                {
                    Array.Copy(c, t, n);    

                    for (int i = 0; (i + N - m) < n; i++)
                        c[i + N - m] ^= b[i];

                    if (L <= (N >> 1))
                    {
                        L = N + 1 - L;
                        m = N;
                        Array.Copy(t, b, n);    
                    }
                }
                N++;
            }

            Console.Write("Polynomial: ");
            for (int i = 0; i <= L; i++)
            {
                if(c[i] == 1)
                {
                    if(i == L)
                    {
                        Console.Write("1");
                    }
                    else
                    {
                        Console.Write(" x^" + (L - i) + " + ");
                    }
                }   
            }

            Console.Write("\nLinear complexity: " + L);

            return L;
        }

        
        static void Main(string[] args)
        {
            LFSR lfsr = new LFSR(16, "0110110101101101", 2, new int[] { 3,5 });
            string gen = lfsr.generate(32);

            Console.Write("Generated sequence: " + gen);

            lfsr.toFile(gen);

            Console.Write("\n");
            BerlekampMassey(new byte[] { 1, 1, 0, 0, 0, 1, 1, 0, 1, 1, 0, 1, 0, 1, 1, 0, 1, 1, 1, 0, 1, 1, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0 });

           
            Console.ReadKey();
        }
    }
}
