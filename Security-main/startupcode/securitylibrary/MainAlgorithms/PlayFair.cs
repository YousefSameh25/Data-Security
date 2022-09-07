using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographicTechnique<string, string>
    {
        /// <summary>
        /// The most common diagrams in english (sorted): TH, HE, AN, IN, ER, ON, RE, ED, ND, HA, AT, EN, ES, OF, NT, EA, TI, TO, IO, LE, IS, OU, AR, AS, DE, RT, VE
        /// </summary>
        /// <param plainText="plainText"></param>
        /// <param plainText="cipherText"></param>
        /// <returns></returns>
        public string Analyse(string plainText)
        {
            throw new NotImplementedException();
        }

        public string Analyse(string plainText, string cipherText)
        {
            throw new NotSupportedException();
        }

        public string Decrypt(string cipherText, string key)
        {

            cipherText = cipherText.ToLower();
            key = key.ToLower();
            StringBuilder sb1 = new StringBuilder("");
            StringBuilder sb2 = new StringBuilder(key);
            for (int i = 0; i < sb2.Length; i++)
            {
                if (sb2[i] == '*')
                {
                    continue;
                }
                sb1.Append(sb2[i]);
                for (int j = i + 1; j < sb2.Length; j++)
                {
                    if (sb2[i] == 'j' || sb2[i] == 'i')
                    {
                        if (sb2[j] == 'i' || sb2[j] == 'j')
                        {
                            sb2[j] = '*';
                        }
                    }
                    else if (sb2[i] == sb2[j])
                    {
                        sb2[j] = '*';
                    }
                }
            }
            for (char n = 'a'; n <= 'z'; n++)
            {
                bool chk2 = false;
                for (int i = 0; i < sb1.Length; i++)
                {
                    if (n == sb1[i])
                    {
                        chk2 = true;
                        break;
                    }
                }
                if (chk2)
                {
                    continue;
                }
                else
                {
                    sb1.Append(n);
                }
            }
            char[,] arr = new char[5, 5];
            int idx = 0;
            bool chkjj = true;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if ((sb1[idx] == 'i' || sb1[idx] == 'j'))
                    {
                        if (chkjj)
                        {
                            arr[i, j] = sb1[idx];
                            chkjj = false;
                            idx++;
                            continue;
                        }
                        idx++;
                    }
                    arr[i, j] = sb1[idx];
                    idx++;
                }
            }
            string temp = "";
            for (int i = 0; i < cipherText.Length;)
            {
                char a1, a2;
                int x1 = 0, x2 = 0, y1 = 0, y2 = 0;
                if (i == cipherText.Length - 1 || cipherText[i] == cipherText[i + 1])
                {
                    a1 = cipherText[i];
                    a2 = 'x';
                }
                else
                {
                    a1 = cipherText[i];
                    a2 = cipherText[i + 1];
                }
                for (int j = 0; j < 5; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        if (a1 == arr[j, k])
                        {
                            x1 = j;
                            y1 = k;
                        }
                        else if (a2 == arr[j, k])
                        {
                            x2 = j;
                            y2 = k;
                        }
                    }
                }
                if (x1 == x2)
                {
                    if (y1 > 0 && y2 == 0)
                    {
                        temp += arr[x1, y1 - 1];
                        temp += arr[x2, 4];

                    }
                    if (y1 == 0 && y2 > 0)
                    {
                        temp += arr[x1, 4];
                        temp += arr[x2, y2 - 1];

                    }
                    if (y1 > 0 && y2 > 0)
                    {
                        temp += arr[x1, y1 - 1];
                        temp += arr[x2, y2 - 1];
                    }
                }
                else if (y1 == y2)
                {
                    if (x1 > 0 && x2 == 0)
                    {
                        temp += arr[x1 - 1, y1];
                        temp += arr[4, y2];
                    }
                    if (x1 == 0 && x2 > 0)
                    {
                        temp += arr[4, y1];
                        temp += arr[x2 - 1, y2];
                    }
                    if (x1 > 0 && x2 > 0)
                    {
                        temp += arr[x1 - 1, y1];
                        temp += arr[x2 - 1, y2];
                    }
                }
                else
                {
                    temp += arr[x1, y2];
                    temp += arr[x2, y1];
                }
                if (i != cipherText.Length - 1 && cipherText[i] == cipherText[i + 1])
                    i++;
                else
                    i += 2;
            }
            string result = "";
            for (int i = 0; i < temp.Length - 1; i += 2)
            {
                result += temp[i];
                if (temp[i + 1] != 'x')
                    result += temp[i + 1];
                else if (i + 2 < temp.Length && temp[i + 1] == 'x' && temp[i] != temp[i + 2])
                    result += temp[i + 1];
            }
            return result;
        }


        public string Encrypt(string plainText, string key)
        {

            plainText = plainText.ToLower();
            key = key.ToLower();
            StringBuilder sb1 = new StringBuilder("");
            StringBuilder sb2 = new StringBuilder(key);
            for (int i = 0; i < sb2.Length; i++)
            {
                if (sb2[i] == '*')
                {
                    continue;
                }
                sb1.Append(sb2[i]);
                for (int j = i + 1; j < sb2.Length; j++)
                {
                    if (sb2[i] == 'j' || sb2[i] == 'i')
                    {
                        if (sb2[j] == 'i' || sb2[j] == 'j')
                        {
                            sb2[j] = '*';
                        }
                    }
                    else if (sb2[i] == sb2[j])
                    {
                        sb2[j] = '*';
                    }
                }
            }
            for (char n = 'a'; n <= 'z'; n++)
            {
                bool chk2 = false;
                for (int i = 0; i < sb1.Length; i++)
                {
                    if (n == sb1[i])
                    {
                        chk2 = true;
                        break;
                    }
                }
                if (chk2)
                {
                    continue;
                }
                else
                {
                    sb1.Append(n);
                }
            }
            char[,] arr = new char[5, 5];
            int idx = 0;
            bool chkjj = true;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if ((sb1[idx] == 'i' || sb1[idx] == 'j'))
                    {
                        if (chkjj)
                        {
                            arr[i, j] = sb1[idx];
                            chkjj = false;
                            idx++;
                            continue;
                        }
                        idx++;
                    }
                    arr[i, j] = sb1[idx];
                    idx++;
                }
            }
            string temp = "";
            for (int i = 0; i < plainText.Length;)
            {
                char a1, a2;
                int x1 = 0, x2 = 0, y1 = 0, y2 = 0;
                if (i == plainText.Length - 1 || plainText[i] == plainText[i + 1])
                {
                    a1 = plainText[i];
                    a2 = 'x';
                }
                else
                {
                    a1 = plainText[i];
                    a2 = plainText[i + 1];
                }
                for (int j = 0; j < 5; j++)
                {
                    for (int lo = 0; lo < 5; lo++)
                    {
                        if (a1 == arr[j, lo])
                        {
                            x1 = j;
                            y1 = lo;
                        }
                        else if (a2 == arr[j, lo])
                        {
                            x2 = j;
                            y2 = lo;
                        }
                    }
                }
                if (x1 == x2)
                {
                    if (y1 < 4 && y2 == 4)
                    {
                        temp += arr[x1, y1 + 1];
                        temp += arr[x2, 0];

                    }
                    if (y1 == 4 && y2 < 4)
                    {
                        temp += arr[x1, 0];
                        temp += arr[x2, y2 + 1];

                    }
                    if (y1 < 4 && y2 < 4)
                    {
                        temp += arr[x1, y1 + 1];
                        temp += arr[x2, y2 + 1];
                    }
                }
                else if (y1 == y2)
                {
                    if (x1 < 4 && x2 == 4)
                    {
                        temp += arr[x1 + 1, y1];
                        temp += arr[0, y2];
                    }
                    if (x1 == 4 && x2 < 4)
                    {
                        temp += arr[0, y1];
                        temp += arr[x2 + 1, y2];
                    }
                    if (x1 < 4 && x2 < 4)
                    {
                        temp += arr[x1 + 1, y1];
                        temp += arr[x2 + 1, y2];
                    }
                }
                else
                {
                    temp += arr[x1, y2];
                    temp += arr[x2, y1];
                }
                if (i != plainText.Length - 1 && plainText[i] == plainText[i + 1])
                    i++;
                else
                    i += 2;
            }
            return temp.ToUpper();
        }
    }
}
