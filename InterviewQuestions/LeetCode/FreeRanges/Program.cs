using System.Collections.Generic;

namespace FreeRanges
{
    class Range
    {
        public int Start { get; set; }
        public int Finish { get; set; }

        public Range(int start, int finish)
        {
            Start = start;
            Finish = finish;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Test cases
            TestOpenRangesWithinInterval();
            TestWithInputBeforeInterval();
            TestWithInputOverlappingInterval();
        }

        private static List<Range> GetFreeRanges(List<Range> used, int beginInterval, int endInterval)
        {
            var arrayLength = endInterval - beginInterval + 1;
            var lookup = new bool[arrayLength];

            foreach(var range in used)
            {
                if (range.Finish < beginInterval)
                    continue;
                if (range.Start > endInterval)
                    continue;

                var start = range.Start < beginInterval ? beginInterval : range.Start;
                var end = range.Finish > endInterval ? endInterval : range.Finish;
                for (var i = start; i <= end; ++i)
                {
                    var index = i - beginInterval;
                    lookup[index] = true;
                }
            }

            // Now identify all falses and that's the free range.
            var result = new List<Range>();
            var freeStart = beginInterval;
            var freeEnd = endInterval;
            var startFound = false;
            for (var index = 0; index < arrayLength; ++index)
            {
                // Identify the start
                if (!startFound && lookup[index] == false)
                {
                    startFound = true;
                    freeStart = index + beginInterval;
                    continue;
                }

                // Indentify the end
                if (startFound && lookup[index] == true)
                {
                    freeEnd = index - 1 + beginInterval;
                    var range = new Range(freeStart, freeEnd);
                    result.Add(range);
                    startFound = false;
                }
            }

            if (startFound)
            {
                var range = new Range(freeStart, endInterval);
                result.Add(range);
            }
            return result;
        }


        private static void TestOpenRangesWithinInterval()
        {
            var input = new List<Range>();
            input.Add(new Range(10, 12));
            input.Add(new Range(15, 17));
            var result = GetFreeRanges(input, 10, 17);
        }

        private static void TestWithInputBeforeInterval()
        {
            var input = new List<Range>();
            input.Add(new Range(5,7));
            input.Add(new Range(9,10));
            input.Add(new Range(15, 17));
            var result = GetFreeRanges(input, 10, 17);
        }

        private static void TestWithInputOverlappingInterval()
        {
            var input = new List<Range>();
            input.Add(new Range(5,7));
            input.Add(new Range(9,11));
            input.Add(new Range(14, 16));
            input.Add(new Range(14, 16));
            input.Add(new Range(18, 19));
            var result = GetFreeRanges(input, 10, 17);
        }

    }
}
