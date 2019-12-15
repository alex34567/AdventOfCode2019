using System.Linq;
using AdventOfCode2019.Common.Intcode;

namespace AdventOfCode2019
{
    internal class Day5 : ISolution
    {
        public int DayN => 5;

        public (string, string?) GetAns(string[] input)
        {
            var program = Intcode.ParseProgram(input[0]);

            var part1 = Intcode.RunProgram(program, new[] {1L}).SkipWhile(x => x == 0).Single();
            var part2 = Intcode.RunProgram(program, new[] {5L}).Single();

            return (part1.ToString(), part2.ToString());
        }
    }
}