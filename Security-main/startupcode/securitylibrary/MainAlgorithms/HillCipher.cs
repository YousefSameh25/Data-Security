using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{


    /// <summary>
    /// The List<int> is row based. Which means that the key is given in row based manner.
    /// </summary>
    public class HillCipher : ICryptographicTechnique<string, string>, ICryptographicTechnique<List<int>, List<int>>
    {
        List<int> dec_3(List<int> key, List<int> cipherText)
        {
            int[,] mat = new int[3, 3];
            int idx = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    mat[i, j] = key[idx++];
                }
            }

            // calculate detramint 3*3
            int temp1 = mat[0, 0] * (mat[1, 1] * mat[2, 2] - mat[1, 2] * mat[2, 1]);
            int temp2 = mat[0, 1] * (mat[1, 0] * mat[2, 2] - mat[1, 2] * mat[2, 0]);
            int temp3 = mat[0, 2] * (mat[1, 0] * mat[2, 1] - mat[1, 1] * mat[2, 0]);
            int det = (temp1 - temp2 + temp3);
            det = (26 + (det % 26)) % 26;
            // calculate b
            int x = 26 - det;
            int c;
            for (int i = 1; ; i += 26)
            {
                if ((i) % x == 0)
                {
                    c = (i) / x;
                    break;
                }

            }
            int b = 26 - c;
            //cout << b << endl;
            int[,] res = new int[3, 3];
            res[0, 0] = (b * (1) * (mat[1, 1] * mat[2, 2] - mat[1, 2] * mat[2, 1]));
            res[0, 0] = (26 + (res[0, 0] % 26)) % 26;
            res[0, 1] = (b * (-1) * (mat[1, 0] * mat[2, 2] - mat[1, 2] * mat[2, 0]));
            res[0, 1] = (26 + (res[0, 1] % 26)) % 26;
            res[0, 2] = (b * (1) * (mat[1, 0] * mat[2, 1] - mat[1, 1] * mat[2, 0]));
            res[0, 2] = (26 + (res[0, 2] % 26)) % 26;
            res[1, 0] = (b * (-1) * (mat[0, 1] * mat[2, 2] - mat[2, 1] * mat[0, 2]));
            res[1, 0] = (26 + (res[1, 0] % 26)) % 26;
            res[1, 1] = (b * (1) * (mat[0, 0] * mat[2, 2] - mat[0, 2] * mat[2, 0]));
            res[1, 1] = (26 + (res[1, 1] % 26)) % 26;
            res[1, 2] = (b * (-1) * (mat[0, 0] * mat[2, 1] - mat[0, 1] * mat[2, 0]));
            res[1, 2] = (26 + (res[1, 2] % 26)) % 26;
            res[2, 0] = (b * (1) * (mat[0, 1] * mat[1, 2] - mat[0, 2] * mat[1, 1]));
            res[2, 0] = (26 + (res[2, 0] % 26)) % 26;
            res[2, 1] = (b * (-1) * (mat[0, 0] * mat[1, 2] - mat[0, 2] * mat[1, 0]));
            res[2, 1] = (26 + (res[2, 1] % 26)) % 26;
            res[2, 2] = (b * (1) * (mat[0, 0] * mat[1, 1] - mat[0, 1] * mat[1, 0]));
            res[2, 2] = (26 + (res[2, 2] % 26)) % 26;
            int[,] inv_key = new int[3, 3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    inv_key[i, j] = res[j, i];

            ////////////////////////////////////////////////////////////////////////////

            int m = 3;

            int idx2 = 0, colu = (cipherText.Count + (m - 1)) / m;

            int[,] cipher_2d = new int[m, colu];
            for (int i = 0; i < colu; i++)
            {
                for (int j = 0; j < m; j++)
                {

                    cipher_2d[j, i] = cipherText[idx2++];
                }
            }
            int[,] ans = new int[m, colu];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < colu; j++)
                {
                    ans[i, j] = 0;
                    for (int k = 0; k < m; k++)
                    {
                        ans[i, j] += inv_key[i, k] * cipher_2d[k, j];
                    }
                }
            }
            var end = new List<int>();
            for (int j = 0; j < colu; j++)
            {
                for (int i = 0; i < m; i++)
                {
                    end.Add(ans[i, j] % 26);
                }
            }

            return end;
        }

        List<int> dec_2(List<int> key, List<int> cipherText)
        {
            double[,] mat = new double[2, 2];
            int idx = 0;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    mat[i, j] = key[idx++];
                }
            }
            if ((mat[0, 0] * mat[1, 1] - mat[0, 1] * mat[1, 0]) > 26 || (mat[0, 0] * mat[1, 1] - mat[0, 1] * mat[1, 0]) == 0)
                throw new SystemException();
            double det = 1 / (mat[0, 0] * mat[1, 1] - mat[0, 1] * mat[1, 0]);

            double temp = mat[0, 0];
            mat[0, 0] = mat[1, 1];
            mat[1, 1] = temp;
            mat[0, 1] *= -1;
            mat[1, 0] *= -1;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {

                    mat[i, j] *= det;
                    mat[i, j] = ((26 + (((int)mat[i, j] + 26) % 26))) % 26;
                }
            }
            //////////////////////////////////////||||||||||||/////////////

            int m = 2;

            int idx2 = 0, colu = (cipherText.Count + (m - 1)) / m;
            int[,] cipher_2d = new int[m, colu];
            for (int i = 0; i < colu; i++)
            {
                for (int j = 0; j < m; j++)
                {

                    cipher_2d[j, i] = cipherText[idx2++];
                }
            }
            double[,] ans = new double[m, colu];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < colu; j++)
                {
                    ans[i, j] = 0;
                    for (int k = 0; k < m; k++)
                    {
                        ans[i, j] += mat[i, k] * cipher_2d[k, j];
                    }
                }
            }
            var end = new List<int>();
            for (int j = 0; j < colu; j++)
            {
                for (int i = 0; i < m; i++)
                {
                    end.Add((int)ans[i, j] % 26);
                }
            }

            return end;
        }
    
        public string Analyse(string plainText, string cipherText)
        {
            throw new NotImplementedException();
        }

        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            if (key.Count == 4)
                return dec_2(key, cipherText);
            return dec_3(key, cipherText);
        }

        public string Decrypt(string cipherText, string key)
        {
            throw new NotImplementedException();
        }

        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            int m = key.Count;
            m = (int)Math.Sqrt(m);
            int idx2 = 0, colu = (plainText.Count + (m - 1)) / m;

            int[,] plain_2d = new int[m, colu];
            for (int i = 0; i < colu; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (idx2 >= plainText.Count)
                        plain_2d[j, i] = 0;
                    else
                        plain_2d[j, i] = plainText[idx2++];
                }
            }

            int[,] key_2d = new int[m, m];
            int idx = 0;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    key_2d[i, j] = key[idx++];
                }
            }

            int[,] res = new int[m, colu];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < colu; j++)
                {
                    res[i, j] = 0;
                    for (int k = 0; k < m; k++)
                    {
                        res[i, j] += key_2d[i, k] * plain_2d[k, j];
                    }
                }
            }

            var ans = new List<int>();
            for (int j = 0; j < colu; j++)
            {
                for (int i = 0; i < m; i++)
                {
                    ans.Add(res[i, j] % 26);
                }
            }
            return ans;
        }

        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            //throw new NotImplementedException();

            List<int> test = new List<int>();
            bool isEqual = false;
            for (int i = 0; i < 26; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    for (int k = 0; k < 26; k++)
                    {
                        for (int l = 0; l < 26; l++)
                        {
                            test = Encrypt(plainText, new List<int> { l, k, j, i });
                            isEqual = Enumerable.SequenceEqual(test, cipherText);
                            if (isEqual)
                            {
                                return new List<int> { l, k, j, i };
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }
            }
            if (!isEqual)
                throw new InvalidAnlysisException();
            return test;


        }
        public string Encrypt(string plainText, string key)
        {
           throw new NotImplementedException();
        }
        List<int> analyse(List<int> plain, List<int> cipher, int idx)
        {
            var key = new List<int>();
            bool f = true;
            for (int i = 0; i < 26; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    for (int l = 0; l < 26; l++)
                    {
                        f = true;
                        for (int k = 0; k < plain.Count - 1; k += 3)
                        {

                            int x = (i * plain[k]) + (j * plain[k + 1]) + (l * plain[k + 2]);
                            x = x % 26;
                            if (x != cipher[idx + k])
                            {
                                f = false;
                                break;
                            }

                        }
                        if (f == true)
                        {
                            key.Add(i);
                            key.Add(j);
                            key.Add(l);
                            return key;
                        }
                    }
                }
            }
            return key;
        }
        public List<int> Analyse3By3Key(List<int> plain3, List<int> cipher3)
        {
            List<int> k1 = analyse(plain3, cipher3, 0);
            List<int> k2 = analyse(plain3, cipher3, 1);
            List<int> k3 = analyse(plain3, cipher3, 2);
            for (int i = 0; i < 3; i++)
                k1.Add(k2[i]);
            for (int i = 0; i < 3; i++)
                k1.Add(k3[i]);
            return k1;
        }

        public string Analyse3By3Key(string plain3, string cipher3)
        {
            throw new NotImplementedException();
        }
    }
}
