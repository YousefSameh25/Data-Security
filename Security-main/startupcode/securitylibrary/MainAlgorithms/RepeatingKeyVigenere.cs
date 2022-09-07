using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
    {
        int get_pos(char c)
        {
            string letters = "abcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < letters.Length; i++)
                if (letters[i] == c)
                    return i;
            return 0;
        }
        public string Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            string key = "";
            string letters = "abcdefghijklmnopqrstuvwxyz";
            int index;
            int x = 0;
            for (int i = 0; i < cipherText.Length; i++)
            {
                index = (get_pos(cipherText[i]) - get_pos(plainText[i])) % 26;
                if (index < 0)
                {
                    index += 26;
                }
                key += letters[index];
            }
            int tmp = 0;
            for (int i = 1; i < key.Length; i++)
            {
                if (key[0] == key[i] && key[1] == key[i + 1] && key[2] == key[i + 2])   // 3 letters
                {
                    tmp = i;
                    break;
                }
            }
            string newkey = "";
            for (int i = 0; i < tmp; i++)
            {
                newkey += key[i];
            }
            return newkey.ToLower();

            // throw new NotImplementedException();
        }

        public string Decrypt(string cipherText, string key)
        {
            cipherText = cipherText.ToLower();
            string plainText = "";
            string letters = "abcdefghijklmnopqrstuvwxyz";
            int index;
            int x = 0;
            while (key.Length < cipherText.Length)
            {
                key += key[x];
                x++;
            }
            for (int i = 0; i < cipherText.Length; i++)
            {
                index = (get_pos(cipherText[i]) - get_pos(key[i])) % 26;
                if (index < 0)
                {
                    index += 26;
                }
                plainText += letters[index];
            }
            return plainText;
            // throw new NotImplementedException();
        }

        public string Encrypt(string plainText, string key)
        {
            string cipherText = "";
            string letters = "abcdefghijklmnopqrstuvwxyz";
            int x = 0;
            while (key.Length < plainText.Length)
            {
                key += key[x];
                x++;
            }
            for (int i = 0; i < plainText.Length; i++)
            {
                cipherText += letters[(get_pos(plainText[i]) + get_pos(key[i])) % 26];
            }
            return cipherText;
        }
    }
}