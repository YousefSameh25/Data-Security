using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {
        int get_pos(char c)
        {
            string letters = "abcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < letters.Length; i++)
                if (letters[i] == c)
                    return i;
            return 0;
        }
        public string Encrypt(string plainText, int key)
        {
            string letters = "abcdefghijklmnopqrstuvwxyz";
            string cipherText = "";
            for (int i = 0; i < plainText.Length; i++)
            {
                char c = letters[(get_pos(plainText[i]) + key) % 26];
                cipherText += c;
            }
            return cipherText;
        }

        public string Decrypt(string cipherText, int key)
        {
            string letters = "abcdefghijklmnopqrstuvwxyz";
            string plainText = "";
            string cipherText2 = cipherText.ToLower();
            for (int i = 0; i < cipherText2.Length; i++)
            {
                int idx = (get_pos(cipherText2[i]) - key) % 26;
                if (idx < 0)
                    idx += 26;
                plainText += letters[idx];
            }
            if (Char.IsUpper(cipherText[0]))
                return plainText.ToUpper();
            return plainText;
        }

        public int Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            int key;
            if (get_pos(plainText[0]) > get_pos(cipherText[0]))
            {
                key = 26 - get_pos(plainText[0]) + get_pos(cipherText[0]);
            }
            else
            {
                key = get_pos(cipherText[0]) - get_pos(plainText[0]);
            }
            return key;
        }
    }
}