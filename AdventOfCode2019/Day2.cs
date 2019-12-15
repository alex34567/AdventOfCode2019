using System.Collections.Generic;
using AdventOfCode2019.Common.Intcode;

namespace AdventOfCode2019
{
    internal class Day2 : ISolution
    {
        public int DayN => 2;

        public (string, string?) GetAns(string[] input)
        {
            var program = Intcode.ParseProgram(input[0]);
            const int part2Key = 19690720;
            var noun = 0;
            var verb = 0;

            while (noun < 100)
            {
                var isGoodNoun = false;
                verb = 0;
                while (verb < 100)
                {
                    if (RunIter(program, noun, verb) == part2Key)
                    {
                        isGoodNoun = true;
                        break;
                    }

                    verb++;
                }

                if (isGoodNoun) break;
                noun++;
            }

            return (RunIter(program, 12, 2).ToString(), (100 * noun + verb).ToString());
        }

        private static long RunIter(IEnumerable<long> inProgram, long noun, long verb)
        {
            var program = new List<long>(inProgram)
            {
                [1] = noun,
                [2] = verb
            };

            return Intcode.RunDay2Program(program);
        }
    }
}