using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.Common.Intcode
{
    public static class Intcode
    {
        private static IEnumerable<int> RunProgramInternal(IList<int> program, IEnumerable<int> input)
        {
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
                        throw new InvalidOpcodeException();
                }

                if (index < 0) break;
                index += 4;
            }

            yield break;
        }

        public static int RunDay2Program(IEnumerable<int> inProgram)
        {
            var program = new List<int>(inProgram);

            if (RunProgramInternal(program, new int[0]).Any()) throw new TooMuchOutputException();

            return program[0];
        }
    }
}