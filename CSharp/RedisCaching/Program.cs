using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace RedisCaching
{
    static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var connection = ConnectionMultiplexer.Connect("localhost,ssl=true");

            }
            catch (Exception ex)
            {
                var oldForgroundColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error ");

            }

        }

        
    }
}
