using System.Text;

namespace FizzBuzz
{
    public class FizzBuzzGenerator
    {
        public string Execute()
        {
            const int firstMultiple = 3;
            const int secondMultiple = 5;
            const int multiple = firstMultiple*secondMultiple;

            var resultBuilder = new StringBuilder();
            for (var currentNumber = 1; currentNumber <= 15; currentNumber++)
            {
                if (currentNumber%multiple == 0)
                {
                    resultBuilder.AppendLine("FizzBuzz");
                    continue;
                }

                if (currentNumber%3 == 0)
                {
                    resultBuilder.AppendLine("Fizz");
                    continue;
                }

                if (currentNumber%5 == 0)
                {
                    resultBuilder.AppendLine("Buzz");
                    continue;
                }

                resultBuilder.AppendLine($"{currentNumber}");
            }

            return resultBuilder.ToString();
        }
    }
}