using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        public int Analyse(string plainText, string cipherText)
        {
            cipherText = cipherText.ToLower();
            List<int> keys = new List<int>();
            char x = cipherText[1];
            for (int i = 0; i < plainText.Length; i++)
            {
                if (plainText[i] == x)
                    keys.Add(i);
            }

            for (int i = 0; i < keys.Count; i++)
            {
                string s = Encrypt(plainText, keys[i]).ToLower();

                if (String.Equals(cipherText, s))
                {
                    return keys[i];
                }
            }

            return -1;
        }

        public string Encrypt(string plainText, int key)
        {
            double len = (double)plainText.Length;
            double res = len / key;
            double col = Math.Ceiling(res);
            int columns = (int)col;


            char[,] arr = new char[key, columns];
            int count = 0;
            for (int i = 0; i < plainText.Length; i++)
            {
                if (plainText[i] == ' ')
                    continue;
                else
                    count++;
            }

            int check = key * columns;
            int diff = check - plainText.Length;

            for (int i = 0; i < diff; i++)
            {
                plainText = plainText + '*';
            }

            int x = 0;
            for (int j = 0; j < columns; j++)
            {
                for (int i = 0; i < key; i++)

                {
                    arr[i, j] = plainText[x];
                    x++;
                }
            }

            string y = "";
            for (int i = 0; i < key; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    y += arr[i, j];

                }
            }

            string z = "";

            for (int i = 0; i < y.Length; i++)
            {
                if (y[i] == '*')
                {
                    continue;
                }

                z += y[i];

            }

            return z.ToUpper();
        }

        public string Decrypt(string cipherText, int key)
        {
            double len = (double)cipherText.Length;
            double res = len / key;
            double col = Math.Ceiling(res);
            int columns = (int)col;


            char[,] arr = new char[key, columns];
            int count = 0;
            for (int i = 0; i < cipherText.Length; i++)
            {
                if (cipherText[i] == ' ')
                    continue;
                else
                    count++;
            }

            int check = key * columns;
            int diff = check - cipherText.Length;

            for (int i = 0; i < diff; i++)
            {
                cipherText = cipherText + '*';
            }

            int x = 0;
            for (int i = 0; i < key; i++)
            {
                for (int j = 0; j < columns; j++)

                {
                    arr[i, j] = cipherText[x];
                    x++;
                }
            }

            string y = "";
            for (int j = 0; j < columns; j++)
            {
                for (int i = 0; i < key; i++)
                {
                    y += arr[i, j];

                }
            }

            string z = "";

            for (int i = 0; i < y.Length; i++)
            {
                if (y[i] == '*')
                {
                    continue;
                }

                z += y[i];

            }

            return z.ToUpper();

        }
    }
}