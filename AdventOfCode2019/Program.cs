using AdventOfCode2019.Common;

namespace AdventOfCode2019
{
    public static class Program
    {
        public static void Main(string[] input)
        {
            var solutions = new ISolution[]
            {
                new Day1(), new Day2(), new Day3(), new Day4(), new Day5(), new Day6(), new Day7(), new Day8(),
                new Day9(), new Day10(), new Day11(), new Day12(), new Day13(), new Day14()
            };
            foreach (var solution in solutions) Utility.Run(solution, input[0]);
        }
    }
}