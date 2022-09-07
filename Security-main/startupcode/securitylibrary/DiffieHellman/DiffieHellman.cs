using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DiffieHellman
{
    public class DiffieHellman
    {
        public List<int> GetKeys(int q, int alpha, int xa, int xb)
        {

            List<int> keys = new List<int>();
            int ya = fast_power(alpha, xa, q);
            int yb = fast_power(alpha, xb, q);
            int ky = fast_power(ya, xb, q);
            int kx = fast_power(yb, xa, q);
            keys.Add(kx);
            keys.Add(ky);
            return keys;
        }
        public int fast_power(int n, int p, int q)
        {
            if (p == 1)
                return n;
            int tmp = fast_power(n, p / 2, q);
            tmp = ((tmp % q) * (tmp % q)) % q;
            if (p % 2 != 0)
                tmp = ((tmp % q) * (n % q)) % q;
            return tmp;
        }
    }
}