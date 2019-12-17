using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.Common.Intcode
{
    public static class Intcode
    {
        private static long FetchData(IList<long> program, int mode, ref Registers registers)
        {
            long ret;
            switch (mode)
            {
                case 0:
                    ret = program[checked((int) program[registers.ProgramCounter])];
                    registers.ProgramCounter++;
                    break;
                case 1:
                    ret = program[registers.ProgramCounter];
                    registers.ProgramCounter++;
                    break;
                case 2:
                    ret = program[checked((int) program[registers.ProgramCounter]) + registers.RelativeBase];
                    registers.ProgramCounter++;
                    break;
                default:
                    throw new InvalidOpcodeException();
            }

            return ret;
        }

        private static IEnumerable<long> RunProgramInternal(IList<long> program, IEnumerable<long> inputNums)
        {
            using var inputs = inputNums.GetEnumerator();
            var registers = new Registers();

            while (registers.ProgramCounter < program.Count && registers.ProgramCounter >= 0)
            {
                var longIns = program[registers.ProgramCounter];
                if (longIns > int.MaxValue) throw new InvalidOpcodeException();
                var ins = (int) longIns;
                registers.ProgramCounter++;
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
                    9 => 1,
                    99 => 0,
                    _ => 2
                };
                var outputMode = inputCount switch
                {
                    1 when mode3 != 0 => throw new InvalidOpcodeException(),
                    1 => mode2,
                    0 when mode3 != 0 || mode2 != 0 => throw new InvalidOpcodeException(),
                    0 => mode1,
                    _ => mode3
                };

                var param1 = 0L;
                var param2 = 0L;
                if (inputCount > 0) param1 = FetchData(program, mode1, ref registers);

                if (inputCount > 1) param2 = FetchData(program, mode2, ref registers);

                var res = 0L;
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
                        if (param1 != 0) registers.ProgramCounter = checked((int) param2);
                        break;
                    case 6:
                        if (param1 == 0) registers.ProgramCounter = checked((int) param2);
                        break;
                    case 7:
                        res = param1 < param2 ? 1 : 0;
                        break;
                    case 8:
                        res = param1 == param2 ? 1 : 0;
                        break;
                    case 9:
                        registers.RelativeBase += checked((int) param1);
                        break;
                    case 99:
                        registers.ProgramCounter = -1;
                        break;
                    default:
                        throw new InvalidOpcodeException();
                }

                var hasOutput = opcode switch
                {
                    4 => false,
                    5 => false,
                    6 => false,
                    9 => false,
                    99 => false,
                    _ => true
                };

                if (hasOutput)
                    switch (outputMode)
                    {
                        case 0:
                            program[checked((int) program[registers.ProgramCounter])] = res;
                            registers.ProgramCounter++;
                            break;
                        case 2:
                            program[checked((int) program[registers.ProgramCounter]) + registers.RelativeBase] = res;
                            registers.ProgramCounter++;
                            break;
                        default:
                            throw new InvalidOpcodeException();
                    }
            }
        }

        public static IList<long> ParseProgram(string program)
        {
            return program.Split(',').Select(long.Parse).ToList();
        }

        public static long RunDay2Program(IEnumerable<long> inProgram)
        {
            var program = new List<long>(inProgram);

            if (RunProgramInternal(program, new long[0]).Any()) throw new InvalidOutputException();

            return program[0];
        }

        public static IEnumerable<long> RunProgram(IEnumerable<long> inProgram, IEnumerable<long> input)
        {
            const int programSize = 1024 * 5;
            var program = new List<long>(programSize);
            program.AddRange(inProgram);
            for (var i = program.Count; i < programSize; i++) program.Add(0);
            return RunProgramInternal(program, input);
        }

        private struct Registers
        {
            public int ProgramCounter;
            public int RelativeBase;
        }
    }
}