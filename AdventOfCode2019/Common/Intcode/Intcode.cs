using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.Common.Intcode
{
    public static class Intcode
    {
        private static int FetchData(IList<int> program, int mode, ref int programCounter)
        {
            int ret;
            switch (mode)
            {
                case 0:
                    ret = program[program[programCounter]];
                    programCounter++;
                    break;
                case 1:
                    ret = program[programCounter];
                    programCounter++;
                    break;
                default:
                    throw new InvalidOpcodeException();
            }

            return ret;
        }

        private static IEnumerable<int> RunProgramInternal(IList<int> program, IEnumerable<int> inputNums)
        {
            using var inputs = inputNums.GetEnumerator();
            var programCounter = 0;

            while (programCounter < program.Count)
            {
                var ins = program[programCounter];
                programCounter++;
                if (ins < 0) throw new InvalidOpcodeException();

                var opcode = ins % 100;
                ins /= 100;
                var mode1 = ins % 10;
                ins /= 10;
                var mode2 = ins % 10;
                ins /= 10;
                var mode3 = ins;
                if (ins > 9) throw new InvalidOpcodeException();

                var inputCount = opcode switch
                {
                    3 => 0,
                    4 => 1,
                    99 => 0,
                    _ => 2
                };

                var param1 = 0;
                var param2 = 0;
                if (inputCount > 0) param1 = FetchData(program, mode1, ref programCounter);

                if (inputCount > 1) param2 = FetchData(program, mode2, ref programCounter);

                var res = 0;
                switch (opcode)
                {
                    case 1:
                        res = param1 + param2;
                        break;
                    case 2:
                        res = param1 * param2;
                        break;
                    case 3:
                        if (!inputs.MoveNext()) throw new NotEnoughInputException();
                        res = inputs.Current;
                        break;
                    case 4:
                        yield return param1;
                        break;
                    case 5:
                        if (param1 != 0) programCounter = param2;
                        break;
                    case 6:
                        if (param1 == 0) programCounter = param2;
                        break;
                    case 7:
                        res = param1 < param2 ? 1 : 0;
                        break;
                    case 8:
                        res = param1 == param2 ? 1 : 0;
                        break;
                    case 99:
                        programCounter = -1;
                        break;
                    default:
                        throw new InvalidOpcodeException();
                }

                var hasOutput = opcode switch
                {
                    4 => false,
                    5 => false,
                    6 => false,
                    99 => false,
                    _ => true
                };

                if (hasOutput)
                    switch (mode3)
                    {
                        case 0:
                            program[program[programCounter]] = res;
                            programCounter++;
                            break;
                        default:
                            throw new InvalidOpcodeException();
                    }

                if (programCounter < 0) break;
            }
        }

        public static IList<int> ParseProgram(string program)
        {
            return program.Split(',').Select(int.Parse).ToList();
        }

        public static int RunDay2Program(IEnumerable<int> inProgram)
        {
            var program = new List<int>(inProgram);

            if (RunProgramInternal(program, new int[0]).Any()) throw new InvalidOutputException();

            return program[0];
        }

        public static IEnumerable<int> RunProgram(IEnumerable<int> program, IEnumerable<int> input)
        {
            return RunProgramInternal(new List<int>(program), input);
        }
    }
}