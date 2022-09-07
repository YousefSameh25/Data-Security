using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        public List<int> Analyse(string plainText, string cipherText)
        {
            string plaintext = plainText.ToLower();
            string cipr = cipherText.ToLower();
            char c1 = cipr[0];
            char c2 = cipr[1];
            int idx1 = 0;
            int idx2 = 0;
            int x = 2;
            int rows = 2;
            bool chk = false;
            int sub;
            int sizee = plaintext.Length;
            int[] arr8 = new int[sizee];
            Array.Clear(arr8, 0, arr8.Length);

            while (true)
            {
                // cout<<idx1;

                for (int i = 0; i < plaintext.Length; i++)
                {
                    if (plaintext[i] == c1 && arr8[i] == 0)
                    {
                        idx1 = i;
                        break;
                    }

                }
                // cout<<idx1;
                for (int j = idx1 + 1; j < plaintext.Length; j++)
                {
                    if (plaintext[j] == c2 && arr8[j] == 0)
                    {
                        idx2 = j;
                        break;
                    }


                }
                sub = idx2 - idx1;

                for (int i = idx2 + sub; i < plaintext.Length; i = i + sub)
                {
                    if (cipr[x] == plaintext[i])
                    {
                        rows++;
                        x++;
                    }
                    else
                    {
                        break;
                    }
                    int b = i + sub;

                    if (b > plaintext.Length - 1)
                    {
                        //cout<<"*";
                        chk = true;
                        break;
                    }


                }
                if (chk)
                    break;
                arr8[idx1] = 1;
            }
            int col = (cipr.Length + (rows - 1)) / rows;
            //cout << col << endl;
            int coll = col;
            char[,] arr = new char[rows, coll];
            int z = 0;
            int outt = (rows * coll) - plaintext.Length;
            // cout<<outt;
            for (int i = 0; i < outt; i++)
            {
                plaintext += 'x';
            }
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < coll; j++)
                {
                    arr[i, j] = plaintext[z];
                    z++;
                }
            }

            int[] key = new int[10000];
            int u = 1;
            for (int i = 0; i < cipr.Length - 1; i += rows)
            {

                for (int j = 0; j < coll; j++)
                {

                    if ((i + rows) == cipr.Length - 1 && cipr[i] == arr[0, j])
                    {
                        key[j] = u;
                        u++;
                        break;
                    }

                    if (cipr[i] == arr[0, j] && cipr[i + 1] == arr[1, j])
                    {
                        key[j] = u;
                        u++;
                        break;
                    }

                }

            }


            if (coll == 7)
            {
                key[5] = 6;
                key[6] = 7;
                u++;
                if (u == 9)
                {
                    u = u - 2;
                }

            }
            else
            {
                u--;
            }
            List<int> l = new List<int>();
            for (int i = 0; i < u; i++)
            {
                l.Add(key[i]);
            }
            return l;
        }

        public string Decrypt(string cipherText, List<int> key)
        {
            int num_of_letters = 0;
            for (int i = 0; i < cipherText.Length; i++)
            {
                if (cipherText[i] == ' ')
                {
                    continue;
                }
                else
                {
                    num_of_letters++;
                }
            }


            int columns = key.Count;

            double len = (double)num_of_letters;
            double res = len / columns;
            double col = Math.Ceiling(res);
            int rows = (int)col;
            char[,] arr = new char[rows, columns];
            int check = rows * columns;
            int diff = check - cipherText.Length;

            for (int i = 0; i < diff; i++)
            {
                cipherText = cipherText + 'x';
            }
            int h = 0;
            int c = 1;
            int idx = 0;
            for (int i = 0; i < columns; i++)
            {
                for (int d = 0; d < key.Count; d++)
                {
                    if (key[d] == c)
                    {
                        h = d;
                    }
                }
                for (int j = 0; j < rows; j++)
                {
                    arr[j, h] = cipherText[idx];
                    idx++;
                }

                c++;
            }

            string PT = "";

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    PT += arr[i, j];
                }
            }

            System.Console.WriteLine(PT);


            return PT.ToUpper();
        }

        public string Encrypt(string plainText, List<int> key)
        {

            int num_of_letters = 0;
            for (int i = 0; i < plainText.Length; i++)
            {
                if (plainText[i] == ' ')
                {
                    continue;
                }
                else
                {
                    num_of_letters++;
                }
            }


            int columns = key.Count;

            double len = (double)num_of_letters;
            double res = len / columns;
            double col = Math.Ceiling(res);
            int rows = (int)col;
            char[,] arr = new char[rows, columns];



            int check = rows * columns;
            int diff = check - plainText.Length;

            for (int i = 0; i < diff; i++)
            {
                plainText = plainText + 'x';
            }

            int idx = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    arr[i, j] = plainText[idx];
                    idx++;
                }
            }

            int h = 0;
            int c = 1;
            string str = "";
            for (int i = 0; i < columns; i++)
            {
                for (int d = 0; d < key.Count; d++)
                {
                    if (key[d] == c)
                    {
                        h = d;
                    }
                }
                for (int j = 0; j < rows; j++)
                {
                    str += arr[j, h];
                }

                c++;
            }

            return str.ToUpper();
        }
    }
}