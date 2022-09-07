using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class AES : CryptographicTechnique
    {
        List<string[,]> keys = new List<string[,]>();
        public string[,] Shift_Rows_Dec(string[,] plain_text)
        {
            for (int i = 0; i < 4; i++)
            {
                List<string> end = new List<string>(i);
                List<string> start = new List<string>(4 - i);
                int cnt = i;
                for (int j = 3; cnt > 0; j--)
                {
                    end.Add(plain_text[i, j]);
                    cnt--;
                }
                end.Reverse();
                for (int j = 0; j < 4 - i; j++)
                    start.Add(plain_text[i, j]);

                int st = 0;
                for (int j = 0; j < end.Count; j++)
                    plain_text[i, st++] = end[j];

                for (int j = 0; j < start.Count; j++)
                    plain_text[i, st++] = start[j];
            }
            return plain_text;
        }
        public string get_R_C(string tmp)
        {
            string[,] SBOX =
               {
            {"63", "7C", "77", "7B", "F2", "6B", "6F", "C5", "30", "01", "67", "2B", "FE", "D7", "AB", "76" },
            {"CA", "82", "C9", "7D", "FA", "59", "47", "F0", "AD", "D4", "A2", "AF", "9C", "A4", "72", "C0" },
            {"B7", "FD", "93", "26", "36", "3F", "F7", "CC", "34", "A5", "E5", "F1", "71", "D8", "31", "15" },
            {"04", "C7", "23", "C3", "18", "96", "05", "9A", "07", "12", "80", "E2", "EB", "27", "B2", "75" },
            {"09", "83", "2C", "1A", "1B", "6E", "5A", "A0", "52", "3B", "D6", "B3", "29", "E3", "2F", "84"},
            {"53", "D1", "00", "ED", "20", "FC", "B1", "5B", "6A", "CB", "BE", "39", "4A", "4C", "58", "CF"},
            {"D0", "EF", "AA", "FB", "43", "4D", "33", "85", "45", "F9", "02", "7F", "50", "3C", "9F", "A8"},
            {"51", "A3", "40", "8F", "92", "9D", "38", "F5", "BC", "B6", "DA", "21", "10", "FF", "F3", "D2"},
            {"CD", "0C", "13", "EC", "5F", "97", "44", "17", "C4", "A7", "7E", "3D", "64", "5D", "19", "73"},
            {"60", "81", "4F", "DC", "22", "2A", "90", "88", "46", "EE", "B8", "14", "DE", "5E", "0B", "DB"},
            {"E0", "32", "3A", "0A", "49", "06", "24", "5C", "C2", "D3", "AC", "62", "91", "95", "E4", "79"},
            {"E7", "C8", "37", "6D", "8D", "D5", "4E", "A9", "6C", "56", "F4", "EA", "65", "7A", "AE", "08"},
            {"BA", "78", "25", "2E", "1C", "A6", "B4", "C6", "E8", "DD", "74", "1F", "4B", "BD", "8B", "8A"},
            {"70", "3E", "B5", "66", "48", "03", "F6", "0E", "61", "35", "57", "B9", "86", "C1", "1D", "9E"},
            {"E1", "F8", "98", "11", "69", "D9", "8E", "94", "9B", "1E", "87", "E9", "CE", "55", "28", "DF"},
            {"8C", "A1", "89", "0D", "BF", "E6", "42", "68", "41", "99", "2D", "0F", "B0", "54", "BB", "16"}
        };
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    if (SBOX[i, j] == tmp)
                    {
                        string hex1 = i.ToString("X1");
                        string hex2 = j.ToString("X1");
                        return hex1 + hex2;
                    }
                }
            }
            return "";
        }
        public string[,] Sub_Bytes_Dec(string[,] cipher_text)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    string tmp = cipher_text[i, j];
                    cipher_text[i, j] = get_R_C(tmp);
                }
            }
            return cipher_text;
        }
        public string[,] Mix_Columns_Dec(string[,] plain_Text)
        {

            string[,] tmp = {
                {"0E","0B","0D","09"},
                {"09","0E","0B","0D"},
                { "0D","09","0E","0B"},
                { "0B","0D","09","0E"}
            };
            string[,] res = new string[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int[] arr = new int[4];
                    for (int k = 0; k < 4; k++)
                    {
                        arr[k] = mul_Dec(tmp[i, k], plain_Text[k, j]);
                    }
                    int x = (arr[0] ^ arr[1] ^ arr[2] ^ arr[3]);
                    res[i, j] = x.ToString("X2");
                    Console.WriteLine(res[i, j]);
                }
                Console.WriteLine(" ");
            }
            Console.WriteLine("---------------------------");
            return res;
        }
        public int shift_left(int x)
        {
            x *= 2;
            if (x > 255)
                x ^= 27;
            //101010001010
            string bin = Convert.ToString(x, 2);
            string t = "";
            for (int i = 0; i < 8 - bin.Length; i++)
            {
                t += "0";
            }
            bin = t + bin;
            bin = bin.Substring(bin.Length - 8, 8);
            x = Convert.ToInt32(bin, 2);
            return x;
        }
        public int mul_Dec(string s1, string s2)
        {
            s1 = s1.ToUpper();
            s2 = s2.ToUpper();

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


            string res1 = "", res2 = "";
            for (int i = 0; i < s1.Length; i++)
            {
                res1 += data[s1[i]];
                res2 += data[s2[i]];
            }
            int tmp1 = Convert.ToInt32(res1, 2);
            int tmp2 = Convert.ToInt32(res2, 2);

            if (tmp1 == 9)
            {
                int a = shift_left(tmp2);
                a = shift_left(a);
                a = shift_left(a);
                a ^= tmp2;
                return a;
            }
            else if (tmp1 == 11)
            {
                int a = shift_left(tmp2);
                a = shift_left(a);
                a ^= tmp2;
                a = shift_left(a);
                a ^= tmp2;
                return a;
            }
            else if (tmp1 == 13)
            {
                int a = shift_left(tmp2);
                a ^= tmp2;
                a = shift_left(a);
                a = shift_left(a);
                a ^= tmp2;
                return a;
            }
            else if (tmp1 == 14)
            {
                int a = shift_left(tmp2);
                a ^= tmp2;
                a = shift_left(a);
                a ^= tmp2;
                a = shift_left(a);
                return a;
            }
            return 0;
        }
        public override string Decrypt(string cipherText, string key)
        {
            string[,] CT = new string[4, 4];
            string[,] k = new string[4, 4];
            int idx = 2;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    CT[j, i] = (cipherText[idx].ToString() + cipherText[idx + 1].ToString());
                    k[j, i] = (key[idx].ToString() + key[idx + 1].ToString());
                    idx += 2;
                }
            }
            for (int i = 0; i < 10; i++)
            {
                keys.Add(new string[4, 4]);
            }
            keys[0] = get_key_Round(k, 0);

            for (int i = 1; i <= 9; i++)
            {

                keys[i] = get_key_Round(keys[i - 1], i);
            }

            //Round 0
            CT = xor_2mat(CT, keys[9]);
            CT = Sub_Bytes_Dec(Shift_Rows_Dec(CT));


            //Round for 9 times
            string[,] s = new string[4, 4];
            for (int i = 1; i <= 9; i++)
            {
                s = xor_2mat(keys[9 - i], CT);
                s = Mix_Columns_Dec(s);
                /* for (int l = 0; l < 4; l++)
                 {
                     for (int j = 0; j < 4; j++)
                         Console.WriteLine(s[l, j]);
                     Console.WriteLine(" ");
                 }
                 Console.WriteLine("-------------------------------------");*/
                s = Shift_Rows_Dec(s);
                s = Sub_Bytes_Dec(s);
                for (int m = 0; m < 4; m++)
                {
                    for (int q = 0; q < 4; q++)
                        CT[m, q] = s[m, q];
                }
            }

            //last Round
            CT = xor_2mat(k, CT);

            string final = "";

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    final += CT[j, i];
                }
            }
            return "0x" + final;
        }
        public string[,] Sub_Bytes(string[,] plain_text)
        {
            string[,] SBOX =
                {
            {"63", "7C", "77", "7B", "F2", "6B", "6F", "C5", "30", "01", "67", "2B", "FE", "D7", "AB", "76" },
            {"CA", "82", "C9", "7D", "FA", "59", "47", "F0", "AD", "D4", "A2", "AF", "9C", "A4", "72", "C0" },
            {"B7", "FD", "93", "26", "36", "3F", "F7", "CC", "34", "A5", "E5", "F1", "71", "D8", "31", "15" },
            {"04", "C7", "23", "C3", "18", "96", "05", "9A", "07", "12", "80", "E2", "EB", "27", "B2", "75" },
            {"09", "83", "2C", "1A", "1B", "6E", "5A", "A0", "52", "3B", "D6", "B3", "29", "E3", "2F", "84"},
            {"53", "D1", "00", "ED", "20", "FC", "B1", "5B", "6A", "CB", "BE", "39", "4A", "4C", "58", "CF"},
            {"D0", "EF", "AA", "FB", "43", "4D", "33", "85", "45", "F9", "02", "7F", "50", "3C", "9F", "A8"},
            {"51", "A3", "40", "8F", "92", "9D", "38", "F5", "BC", "B6", "DA", "21", "10", "FF", "F3", "D2"},
            {"CD", "0C", "13", "EC", "5F", "97", "44", "17", "C4", "A7", "7E", "3D", "64", "5D", "19", "73"},
            {"60", "81", "4F", "DC", "22", "2A", "90", "88", "46", "EE", "B8", "14", "DE", "5E", "0B", "DB"},
            {"E0", "32", "3A", "0A", "49", "06", "24", "5C", "C2", "D3", "AC", "62", "91", "95", "E4", "79"},
            {"E7", "C8", "37", "6D", "8D", "D5", "4E", "A9", "6C", "56", "F4", "EA", "65", "7A", "AE", "08"},
            {"BA", "78", "25", "2E", "1C", "A6", "B4", "C6", "E8", "DD", "74", "1F", "4B", "BD", "8B", "8A"},
            {"70", "3E", "B5", "66", "48", "03", "F6", "0E", "61", "35", "57", "B9", "86", "C1", "1D", "9E"},
            {"E1", "F8", "98", "11", "69", "D9", "8E", "94", "9B", "1E", "87", "E9", "CE", "55", "28", "DF"},
            {"8C", "A1", "89", "0D", "BF", "E6", "42", "68", "41", "99", "2D", "0F", "B0", "54", "BB", "16"}
        };
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    string tmp = plain_text[i, j];
                    int r = int.Parse(tmp[0].ToString(), System.Globalization.NumberStyles.HexNumber);
                    int c = int.Parse(tmp[1].ToString(), System.Globalization.NumberStyles.HexNumber);
                    plain_text[i, j] = SBOX[r, c];
                }
            }
            return plain_text;
        }
        public string[,] Shift_Rows(string[,] plain_text)
        {
            for (int i = 0; i < 4; i++)
            {
                List<string> end = new List<string>(4 - i);
                List<string> start = new List<string>(i);
                int cnt = 4 - i;
                for (int j = 3; cnt > 0; j--)
                {
                    end.Add(plain_text[i, j]);
                    cnt--;
                }
                end.Reverse();
                for (int j = 0; j < i; j++)
                    start.Add(plain_text[i, j]);
                int st = 0;
                for (int j = 0; j < end.Count; j++)
                    plain_text[i, st++] = end[j];
                for (int j = 0; j < start.Count; j++)
                    plain_text[i, st++] = start[j];
            }
            return plain_text;
        }
        public string get_binary_str(string hexa)
        {
            hexa = hexa.ToUpper();
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

            string ans = data[hexa[0]] + data[hexa[1]];

            return ans;

        }
        public string mul(string s1, string s2)
        {
            s1 = s1.ToUpper();
            s2 = s2.ToUpper();

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

            string res1 = "", res2 = "";
            for (int i = 0; i < s1.Length; i++)
            {
                res1 += data[s1[i]];
                res2 += data[s2[i]];
            }
            int tmp1 = Convert.ToInt32(res1, 2);
            int tmp2 = Convert.ToInt32(res2, 2);
            if (s1 == "03")
            {
                int ans2 = tmp2 * 2;
                ans2 ^= tmp2;
                string binary2 = Convert.ToString(ans2, 2);
                return binary2;
            }
            int ans = tmp1 * tmp2;
            string binary = Convert.ToString(ans, 2);
            return binary;
        }
        public string X_OR(string s1, string s2, string s3, string s4)
        {
            int tmp1 = Convert.ToInt32(s1, 2);
            int tmp2 = Convert.ToInt32(s2, 2);
            int tmp3 = Convert.ToInt32(s3, 2);
            int tmp4 = Convert.ToInt32(s4, 2);

            int ans = (tmp1 ^ tmp2 ^ tmp3 ^ tmp4);

            string binary = Convert.ToString(ans, 2);

            int sta = 283;
            if (binary.Length > 8)
                ans ^= sta;

            binary = Convert.ToString(ans, 2);
            string temp = "";

            for (int i = 0; i < 8 - binary.Length; i++)
                temp += '0';

            return (temp + binary);
        }
        public string get_hexa(string s)
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
            string tmp1 = s.Substring(0, 4);
            string tmp2 = s.Substring(4, 4);
            return (dict[tmp1] + dict[tmp2]);
        }
        public string[,] Mix_Columns(string[,] plain_Text)
        {
            string[,] tmp = {
                {"02","03","01","01"},
                {"01","02","03","01"},
                { "01","01","02","03"},
                { "03","01","01","02"}
            };
            string[,] res = new string[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    string[] arr = new string[4];
                    for (int k = 0; k < 4; k++)
                    {
                        arr[k] = mul(tmp[i, k], plain_Text[k, j]);
                    }

                    res[i, j] = get_hexa(X_OR(arr[0], arr[1], arr[2], arr[3]));
                }
            }
            return res;
        }
        public string x_or(string x, string y)
        {
            string res = "";
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] == y[i])
                    res += '0';
                else
                    res += '1';
            }
            return res;
        }
        public string[,] get_key_Round(string[,] last, int num)
        {
            string[] F_column = new string[4];
            string[] C1 = new string[4];

            //Get first column and last column
            for (int i = 0; i < 4; i++)
            {
                F_column[i] = last[i, 3];
                C1[i] = last[i, 0];
            }

            //rotate it
            string tmp = F_column[0];
            for (int i = 1; i < 4; i++)
                F_column[i - 1] = F_column[i];
            F_column[3] = tmp;

            //substdude 
            string[,] SBOX = {
            {"63", "7C", "77", "7B", "F2", "6B", "6F", "C5", "30", "01", "67", "2B", "FE", "D7", "AB", "76" },
            {"CA", "82", "C9", "7D", "FA", "59", "47", "F0", "AD", "D4", "A2", "AF", "9C", "A4", "72", "C0" },
            {"B7", "FD", "93", "26", "36", "3F", "F7", "CC", "34", "A5", "E5", "F1", "71", "D8", "31", "15" },
            {"04", "C7", "23", "C3", "18", "96", "05", "9A", "07", "12", "80", "E2", "EB", "27", "B2", "75" },
            {"09", "83", "2C", "1A", "1B", "6E", "5A", "A0", "52", "3B", "D6", "B3", "29", "E3", "2F", "84"},
            {"53", "D1", "00", "ED", "20", "FC", "B1", "5B", "6A", "CB", "BE", "39", "4A", "4C", "58", "CF"},
            {"D0", "EF", "AA", "FB", "43", "4D", "33", "85", "45", "F9", "02", "7F", "50", "3C", "9F", "A8"},
            {"51", "A3", "40", "8F", "92", "9D", "38", "F5", "BC", "B6", "DA", "21", "10", "FF", "F3", "D2"},
            {"CD", "0C", "13", "EC", "5F", "97", "44", "17", "C4", "A7", "7E", "3D", "64", "5D", "19", "73"},
            {"60", "81", "4F", "DC", "22", "2A", "90", "88", "46", "EE", "B8", "14", "DE", "5E", "0B", "DB"},
            {"E0", "32", "3A", "0A", "49", "06", "24", "5C", "C2", "D3", "AC", "62", "91", "95", "E4", "79"},
            {"E7", "C8", "37", "6D", "8D", "D5", "4E", "A9", "6C", "56", "F4", "EA", "65", "7A", "AE", "08"},
            {"BA", "78", "25", "2E", "1C", "A6", "B4", "C6", "E8", "DD", "74", "1F", "4B", "BD", "8B", "8A"},
            {"70", "3E", "B5", "66", "48", "03", "F6", "0E", "61", "35", "57", "B9", "86", "C1", "1D", "9E"},
            {"E1", "F8", "98", "11", "69", "D9", "8E", "94", "9B", "1E", "87", "E9", "CE", "55", "28", "DF"},
            {"8C", "A1", "89", "0D", "BF", "E6", "42", "68", "41", "99", "2D", "0F", "B0", "54", "BB", "16"}
        };
            for (int i = 0; i < 4; i++)
            {
                int r = int.Parse(F_column[i][0].ToString(), System.Globalization.NumberStyles.HexNumber);
                int c = int.Parse(F_column[i][1].ToString(), System.Globalization.NumberStyles.HexNumber);
                F_column[i] = SBOX[r, c];
            }

            //xor
            int Rcon = num;

            string[,] Con =
            {
                { "01", "02","04","08","10","20","40","80","1b","36"},
                { "00", "00","00","00","00","00","00","00","00","00"},
                { "00", "00","00","00","00","00","00","00","00","00"},
                { "00", "00","00","00","00","00","00","00","00","00"}
            };

            string[] C2 = new string[4];
            for (int i = 0; i < 4; i++)
                C2[i] = Con[i, Rcon];

            for (int i = 0; i < 4; i++)
                C1[i] = get_hexa(x_or(get_binary_str(C1[i]), get_binary_str(C2[i])));

            string[,] ans = new string[4, 4];

            //fill first column
            for (int i = 0; i < 4; i++)
                ans[i, 0] = get_hexa(x_or(get_binary_str(C1[i]), get_binary_str(F_column[i])));

            //fill all columns
            for (int i = 1; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    ans[j, i] = get_hexa(x_or(get_binary_str(last[j, i]), get_binary_str(ans[j, i - 1])));
                }
            }
            return ans;
        }
        string[,] xor_2mat(string[,] a, string[,] b)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    a[i, j] = get_hexa(x_or(get_binary_str(a[i, j]), get_binary_str(b[i, j])));
                }
            }
            return a;
        }
        public override string Encrypt(string plainText, string key)
        {
            string[,] PT = new string[4, 4];
            string[,] k = new string[4, 4];
            int idx = 2;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    PT[j, i] = (plainText[idx].ToString() + plainText[idx + 1].ToString());
                    k[j, i] = (key[idx].ToString() + key[idx + 1].ToString());
                    idx += 2;
                }
            }
            //Round 0

            PT = xor_2mat(PT, k);

            //Round for 9 times
            string[,] s = new string[4, 4];
            string[,] temp_k = new string[4, 4];
            for (int i = 1; i <= 9; i++)
            {
                s = Sub_Bytes(PT);
                s = Shift_Rows(s);
                s = Mix_Columns(s);
                temp_k = get_key_Round(k, i - 1);
                for (int d = 0; d < 4; d++)
                {
                    for (int j = 0; j < 4; j++)
                        k[d, j] = temp_k[d, j];
                }
                PT = xor_2mat(temp_k, s);
            }

            //last Round
            PT = xor_2mat(get_key_Round(k, 9), Shift_Rows(Sub_Bytes(PT)));

            string final = "";

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    final += PT[j, i];
                }
            }
            return "0x" + final;
        }
    }
}