using System.Linq;
using AdventOfCode2019.Common.Intcode;

namespace AdventOfCode2019
{
    internal class Day9 : ISolution
    {
        public int DayN => 9;

        public (string, string?) GetAns(string[] input)
        {
            var program = Intcode.ParseProgram(input[0]);

            var part1 = Intcode.RunProgram(program, new[] {1L}).Single();
            var part2 = Intcode.RunProgram(program, new[] {2L}).Single();

            return (part1.ToString(), part2.ToString());
        }
    }
}