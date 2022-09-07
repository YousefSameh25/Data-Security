using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.RSA
{
    public class RSA
    {
        public int fast_power(int n, int p, int mod)
        {
            if (p == 1)
                return n;
            long tmp = fast_power(n, p / 2, mod);
            tmp = ((tmp % mod) * (tmp % mod)) % mod;
            if (p % 2 != 0)
                tmp = ((tmp % mod) * (n % mod)) % mod;
            return (int)tmp;
        }
        public int get_d(int e, int phi)
        {
            for (int i = 0; ; i++)
            {
                if ((e * i) % phi == 1)
                    return i;
            }
        }
        public int Encrypt(int p, int q, int M, int e)
        {
            int n = p * q;
            int c = fast_power(M, e, n);
            return c;
        }
        public int Decrypt(int p, int q, int C, int e)
        {
            int n = p * q;
            int phi = (p - 1) * (q - 1);
            int d = get_d(e, phi);
            int M = fast_power(C, d, n);
            return M;
        }
    }
}