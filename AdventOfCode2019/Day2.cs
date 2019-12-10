using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019
{
    internal class Day2 : ISolution
    {
        public int DayN => 2;

        public (string, string?) GetAns(string[] input)
        {
            var program = input[0].Split(',').Select(int.Parse).ToList();
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

        private static int RunIter(IEnumerable<int> inProgram, int noun, int verb)
        {
            var program = new List<int>(inProgram)
            {
                [1] = noun,
                [2] = verb
            };

            var index = 0;

            while (index < program.Count)
            {
                switch (program[index])
                {
                    case 1:
                        program[program[index + 3]] = program[program[index + 1]] + program[program[index + 2]];
                        break;
                    case 2:
                        program[program[index + 3]] = program[program[index + 1]] * program[program[index + 2]];
                        break;
                    case 99:
                        index = -1;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (index < 0) break;
                index += 4;
            }

            return program[0];
        }
    }
}