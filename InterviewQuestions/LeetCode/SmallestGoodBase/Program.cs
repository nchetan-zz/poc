using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallestGoodBase
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.main(args);
        }

        private void main(string[] args)
        {
            var result = smallestGoodBase("13");
            Console.WriteLine($"{result}");
        }

        private string smallestGoodBase(string n)
        {
            long num = Convert.ToInt64(n);

            long bit = 1;
            for (int power = 64; power >= 1; power--)
            {
                if ((bit << power) < num)
                {
                    long k = helper(num, power);
                    if (k != -1) return k.ToString();
                }
            }
            return (num - 1).ToString();
        }

        private long helper(long num, int power)
        {
            long left = 1, right = (long)(Math.Pow(num, 1.0 / power) + 1);
            while (left < right)
            {
                long mid = left + (right - left) / 2;
                long sum = 0, cur = 1;
                for (int i = 0; i <= power; i++)
                {
                    sum += cur;
                    cur *= mid;
                }
                if (sum == num) return mid;
                else if (sum > num) right = mid;
                else left = mid + 1;
            }
            return -1;
        }
    }
}
