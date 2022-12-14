using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class TripleDES : ICryptographicTechnique<string, List<string>>
    {

        public List<KeyValuePair<string, string>> CD = new List<KeyValuePair<string, string>>();
        public List<KeyValuePair<string, string>> LR = new List<KeyValuePair<string, string>>();
        public List<string> keys = new List<string>();
        public string generate_LR(string PT)
        {
            string IP = create_IP(PT);
            string L0 = IP.Substring(0, 32);
            string R0 = IP.Substring(32, 32);
            LR.Add(new KeyValuePair<string, string>(L0, R0));

            for (int i = 1; i <= 16; i++)
            {
                string L = LR[i - 1].Value;
                string R = get_R(LR[i - 1].Key, LR[i - 1].Value, keys[i]);
                LR.Add(new KeyValuePair<string, string>(L, R));
            }

            string RL = LR[16].Value + LR[16].Key;
            int[] IP_1 = new int[64] { 40, 8, 48, 16, 56, 24, 64, 32, 39, 7, 47, 15, 55, 23, 63, 31, 38, 6, 46, 14, 54, 22, 62, 30, 37, 5, 45, 13, 53, 21, 61, 29, 36, 4, 44, 12, 52, 20, 60, 28, 35, 3, 43, 11, 51, 19, 59, 27, 34, 2, 42, 10, 50, 18, 58, 26, 33, 1, 41, 9, 49, 17, 57, 25 };
            string final = "";
            for (int i = 0; i < 64; i++)
                final += RL[IP_1[i] - 1];

            string ct = "0x" + Convert.ToInt64(final, 2).ToString("X");

            return ct;
        }
        public string get_R(string L, string R, string k)
        {
            string ER = expand(R);
            string tmp_f = X_OR(ER, k);
            int[,] s1 = new int[4, 16] { { 14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7 }, { 0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8 }, { 4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0 }, { 15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13 } };
            int[,] s2 = new int[4, 16] { { 15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10 }, { 3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5 }, { 0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15 }, { 13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9 } };
            int[,] s3 = new int[4, 16] { { 10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8 }, { 13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1 }, { 13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7 }, { 1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12 } };
            int[,] s4 = new int[4, 16] { { 7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15 }, { 13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9 }, { 10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4 }, { 3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14 } };
            int[,] s5 = new int[4, 16] { { 2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9 }, { 14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6 }, { 4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14 }, { 11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3 } };
            int[,] s6 = new int[4, 16] { { 12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11 }, { 10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8 }, { 9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6 }, { 4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13 } };
            int[,] s7 = new int[4, 16] { { 4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1 }, { 13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6 }, { 1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2 }, { 6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12 } };
            int[,] s8 = new int[4, 16] { { 13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7 }, { 1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2 }, { 7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8 }, { 2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11 } };


            List<string> Blocks = new List<string>();

            int cnt = 0;
            string tmp_B = "";
            for (int i = 0; i < tmp_f.Length; i++)
            {
                if (cnt == 6)
                {
                    Blocks.Add(tmp_B);
                    tmp_B = "";
                    cnt = 0;
                }
                tmp_B += tmp_f[i];
                cnt++;
            }
            Blocks.Add(tmp_B);
            string s = "";
            for (int i = 0; i < Blocks.Count; i++)
            {
                int row = get_pos((Blocks[i][0].ToString() + Blocks[i][5].ToString()));
                int col = get_pos((Blocks[i].Substring(1, 4)).ToString());
                int sb = 0;
                if (i == 0)
                    sb = s1[row, col];
                if (i == 1)
                    sb = s2[row, col];
                if (i == 2)
                    sb = s3[row, col];
                if (i == 3)
                    sb = s4[row, col];
                if (i == 4)
                    sb = s5[row, col];
                if (i == 5)
                    sb = s6[row, col];
                if (i == 6)
                    sb = s7[row, col];
                if (i == 7)
                    sb = s8[row, col];
                s += ToBinary(sb).ToString();
            }

            int[] P = new int[32] { 16, 7, 20, 21, 29, 12, 28, 17, 1, 15, 23, 26, 5, 18, 31, 10, 2, 8, 24, 14, 32, 27, 3, 9, 19, 13, 30, 6, 22, 11, 4, 25 };
            string f = "";

            for (int i = 0; i < 32; i++)
                f += s[P[i] - 1];

            string New_R = X_OR(L, f);
            return New_R;
        }
        public string ToBinary(int n)
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();
            dict.Add(0, "0000");
            dict.Add(1, "0001");
            dict.Add(2, "0010");
            dict.Add(3, "0011");
            dict.Add(4, "0100");
            dict.Add(5, "0101");
            dict.Add(6, "0110");
            dict.Add(7, "0111");
            dict.Add(8, "1000");
            dict.Add(9, "1001");
            dict.Add(10, "1010");
            dict.Add(11, "1011");
            dict.Add(12, "1100");
            dict.Add(13, "1101");
            dict.Add(14, "1110");
            dict.Add(15, "1111");
            return dict[n];
        }
        public int get_pos(string pos)
        {
            int ans = 0;
            int idx = 0;
            for (int i = pos.Length - 1; i >= 0; i--)
                ans += ((int)Math.Pow(2, idx++) * (pos[i] - '0'));
            return ans;
        }
        public string expand(string R)
        {
            int[] E = new int[48] { 32, 1, 2, 3, 4, 5, 4, 5, 6, 7, 8, 9, 8, 9, 10, 11, 12, 13, 12, 13, 14, 15, 16, 17, 16, 17, 18, 19, 20, 21, 20, 21, 22, 23, 24, 25, 24, 25, 26, 27, 28, 29, 28, 29, 30, 31, 32, 1 };
            string New_R = "";
            for (int i = 0; i < 48; i++)
                New_R += R[E[i] - 1];
            return New_R;
        }
        public string X_OR(string a, string b)
        {
            string res = "";
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == b[i])
                    res += '0';
                else
                    res += '1';
            }
            return res;
        }
        public string swap_1(string s)
        {
            char c = s[0];
            string res = "";
            for (int i = 1; i < s.Length; i++)
                res += s[i];
            res += c;
            return res;
        }
        public string swap_2(string s)
        {
            char c1 = s[0], c2 = s[1];
            string res = "";
            for (int i = 2; i < s.Length; i++)
                res += s[i];
            res += c1;
            res += c2;
            return res;
        }
        public void generate_CD(string key)
        {
            string tmp = binary(key);
            string New_key = get_key(tmp);
            string C0 = New_key.Substring(0, 28);
            string D0 = New_key.Substring(28, 28);
            keys.Add(generate_key(C0 + D0));
            CD.Add(new KeyValuePair<string, string>(C0, D0));
            for (int i = 1; i <= 16; i++)
            {
                if (i == 1 || i == 2 || i == 9 || i == 16)
                {
                    string tmp1 = swap_1(CD[i - 1].Key);
                    string tmp2 = swap_1(CD[i - 1].Value);
                    CD.Add(new KeyValuePair<string, string>(tmp1, tmp2));
                    keys.Add(generate_key(tmp1 + tmp2));
                }
                else
                {
                    string tmp1 = swap_2(CD[i - 1].Key);
                    string tmp2 = swap_2(CD[i - 1].Value);
                    CD.Add(new KeyValuePair<string, string>(tmp1, tmp2));
                    keys.Add(generate_key(tmp1 + tmp2));
                }
            }
        }
        public string get_key(string key)
        {
            int[] PC1 = new int[56] { 57, 49, 41, 33, 25, 17, 9, 1, 58, 50, 42, 34, 26, 18, 10, 2, 59, 51, 43, 35, 27, 19, 11, 3, 60, 52, 44, 36, 63, 55, 47, 39, 31, 23, 15, 7, 62, 54, 46, 38, 30, 22, 14, 6, 61, 53, 45, 37, 29, 21, 13, 5, 28, 20, 12, 4 };
            string New_key = "";
            for (int i = 0; i < 56; i++)
                New_key += key[PC1[i] - 1];
            return New_key;
        }
        public string generate_key(string key)
        {
            int[] PC2 = new int[48] { 14, 17, 11, 24, 1, 5, 3, 28, 15, 6, 21, 10, 23, 19, 12, 4, 26, 8, 16, 7, 27, 20, 13, 2, 41, 52, 31, 37, 47, 55, 30, 40, 51, 45, 33, 48, 44, 49, 39, 56, 34, 53, 46, 42, 50, 36, 29, 32 };
            string New_key = "";
            for (int i = 0; i < 48; i++)
                New_key += key[PC2[i] - 1];
            return New_key;
        }
        public string binary(string hexa)
        {
            Dictionary<char, string> data = new Dictionary<char, string>();
            data.Add('0', "0000");
            data.Add('1', "0001");
            data.Add('2', "0010");
            data.Add('3', "0011");
            data.Add('4', "0100");
            data.Add('5', "0101");
            data.Add('6', "0110");
            data.Add('7', "0111");
            data.Add('8', "1000");
            data.Add('9', "1001");
            data.Add('A', "1010");
            data.Add('B', "1011");
            data.Add('C', "1100");
            data.Add('D', "1101");
            data.Add('E', "1110");
            data.Add('F', "1111");
            string res = "";
            for (int i = 2; i < hexa.Length; i++)
                res += data[hexa[i]];
            return res;
        }
        public string create_IP(string M)
        {
            int[] IP = new int[64] { 58, 50, 42, 34, 26, 18, 10, 2, 60, 52, 44, 36, 28, 20, 12, 4, 62, 54, 46, 38, 30, 22, 14, 6, 64, 56, 48, 40, 32, 24, 16, 8, 57, 49, 41, 33, 25, 17, 9, 1, 59, 51, 43, 35, 27, 19, 11, 3, 61, 53, 45, 37, 29, 21, 13, 5, 63, 55, 47, 39, 31, 23, 15, 7 };
            string tmp = binary(M);
            string New_M = "";
            for (int i = 0; i < 64; i++)
                New_M += tmp[IP[i] - 1];
            return New_M;
        }
        public static string TOHEXA(string n)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("0000", "0");
            dict.Add("0001", "1");
            dict.Add("0010", "2");
            dict.Add("0011", "3");
            dict.Add("0100", "4");
            dict.Add("0101", "5");
            dict.Add("0110", "6");
            dict.Add("0111", "7");
            dict.Add("1000", "8");
            dict.Add("1001", "9");
            dict.Add("1010", "A");
            dict.Add("1011", "B");
            dict.Add("1100", "C");
            dict.Add("1101", "D");
            dict.Add("1110", "E");
            dict.Add("1111", "F");
            return dict[n];
        }
        public string generate_LR2(string CT)
        {
            string IP = create_IP(CT);
            string L0 = IP.Substring(0, 32);
            string R0 = IP.Substring(32, 32);
            LR.Add(new KeyValuePair<string, string>(L0, R0));
            for (int i = 1; i <= 16; i++)
            {
                string L = LR[i - 1].Value;
                string R = get_R(LR[i - 1].Key, LR[i - 1].Value, keys[keys.Count - i]);
                LR.Add(new KeyValuePair<string, string>(L, R));
            }
            string RL = LR[16].Value + LR[16].Key;
            int[] IP_1 = new int[64] { 40, 8, 48, 16, 56, 24, 64, 32, 39, 7, 47, 15, 55, 23, 63, 31, 38, 6, 46, 14, 54, 22, 62, 30, 37, 5, 45, 13, 53, 21, 61, 29, 36, 4, 44, 12, 52, 20, 60, 28, 35, 3, 43, 11, 51, 19, 59, 27, 34, 2, 42, 10, 50, 18, 58, 26, 33, 1, 41, 9, 49, 17, 57, 25 };
            string final = "";
            for (int i = 0; i < 64; i++)
                final += RL[IP_1[i] - 1];
            string pt = "0x";
            for (int i = 0; i < 64; i += 4)
            {
                string temp = TOHEXA(final.Substring(i, 4));
                pt += temp;
            }
            return pt;
        }
        public string Decrypt(string cipherText, List<string> key)
        {
            generate_CD(key[0]);
            string tmp1 = generate_LR2(cipherText);
            CD.Clear();
            LR.Clear();
            keys.Clear();
            /*
              generate_CD(key[1]);
              string tmp2 = generate_LR2(tmp1);
              CD.Clear();
              LR.Clear();
              keys.Clear();
              generate_CD(key[2]);
              string tmp3 = generate_LR2(tmp2);
            */
            return tmp1;
        }

        public string Encrypt(string plainText, List<string> key)
        {
            generate_CD(key[0]);
            string tmp1 = generate_LR(plainText);
            CD.Clear();
            LR.Clear();
            keys.Clear();
            /*
              generate_CD(key[1]);
              string tmp2 = generate_LR(tmp1);
              CD.Clear();
              LR.Clear();
              keys.Clear();
              generate_CD(key[2]);
              string tmp3 = generate_LR(tmp2);
            */
            return tmp1;
        }

        public List<string> Analyse(string plainText,string cipherText)
        {
            throw new NotSupportedException();
        }

    }
}
