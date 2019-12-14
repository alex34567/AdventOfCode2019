using System;
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

            int part1;
            try
            {
                part1 = Intcode.RunProgram(program, new[] {1}).SkipWhile(x => x == 0).Single();
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOutputException();
            }

            int part2;
            try
            {
                part2 = Intcode.RunProgram(program, new[] {5}).Single();
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOutputException();
            }

            return (part1.ToString(), part2.ToString());
        }
    }
}