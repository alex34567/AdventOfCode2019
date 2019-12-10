using System.Linq;

namespace AdventOfCode2019
{
    internal class Day1 : ISolution
    {
        public int DayN => 1;

        public (string, string?) GetAns(string[] input)
        {
            var sum = (from line in input select int.Parse(line) into num select num / 3 into num select num - 2).Sum();

            var part2 = 0;
            foreach (var line in input)
            {
                var num = int.Parse(line);
                num /= 3;
                num -= 2;
                while (num > 0)
                {
                    part2 += num;
                    num /= 3;
                    num -= 2;
                }
            }

            return (sum.ToString(), part2.ToString());
        }
    }
}