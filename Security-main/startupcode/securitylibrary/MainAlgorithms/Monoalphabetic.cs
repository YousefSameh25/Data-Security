using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        int get_pos(char c)
        {
            string letters = "abcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < letters.Length; i++)
            {
                if (c == letters[i])
                    return i;
            }
            return 0;
        }
        public string Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            StringBuilder letters = new StringBuilder("abcdefghijklmnopqrstuvwxyz");
            StringBuilder key = new StringBuilder("                          ");
            for (int i = 0; i < plainText.Length; i++)
            {
                int idx = get_pos(plainText[i]);
                key[idx] = cipherText[i];
                letters[get_pos(cipherText[i])] = '0';
            }
            for (int i = 0; i < letters.Length; i++)
            {
                for (int j = 0; j < key.Length; j++)
                {
                    if (key[j] == ' ' && letters[i] != '0')
                    {
                        key[j] = letters[i];
                        letters[i] = '0';
                        break;
                    }
                }
            }
            return key.ToString();
        }

        public string Decrypt(string cipherText, string key)
        {
            key = key.ToLower();
            cipherText = cipherText.ToLower();
            string plainText = "";
            string letters = "abcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < cipherText.Length; i++)
            {
                for (int j = 0; j < key.Length; j++)
                {
                    if (key[j] == cipherText[i])
                    {
                        plainText += letters[j];
                        break;
                    }
                }
            }
            return plainText.ToUpper();
        }

        public string Encrypt(string plainText, string key)
        {
            key = key.ToLower();
            plainText = plainText.ToLower();
            string cipherText = "";
            string letters = "abcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < plainText.Length; i++)
            {
                for (int j = 0; j < letters.Length; j++)
                {
                    if (letters[j] == plainText[i])
                    {
                        cipherText += key[j];
                        break;
                    }
                }
            }
            return cipherText.ToUpper();
        }
        public string AnalyseUsingCharFrequency(string cipher)
        {
            cipher = cipher.ToLower();
            string key = "etaoinsrhldcumfpgwybvkxjqz";
            int[] freq = new int[124];
            for (int i = 0; i < cipher.Length; i++)
            {
                int pos = (int)cipher[i];
                freq[pos]++;
            }
            string tmp = "";
            for (int i = 0; i < 26; i++)
            {
                int mx = -1, idx = 0;
                for (int j = 97; j < 124; j++)
                {
                    if (freq[j] > mx)
                    {
                        mx = freq[j];
                        idx = j;
                    }
                }
                if (mx != -1)
                    tmp += (char)idx;
                freq[idx] = -1;
            }
            string plain = "";
            for (int i = 0; i < cipher.Length; i++)
            {
                for (int j = 0; j < tmp.Length; j++)
                {
                    if (cipher[i] == tmp[j])
                    {
                        plain += key[j];
                        break;
                    }
                }
            }
            return plain;
        }
    }
}