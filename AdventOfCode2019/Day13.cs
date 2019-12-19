using System;
using System.Collections.Generic;
using AdventOfCode2019.Common.Intcode;

namespace AdventOfCode2019
{
    internal class Day13 : ISolution
    {
        public int DayN => 13;

        public (string, string?) GetAns(string[] input)
        {
            var program = Intcode.ParseProgram(input[0]);
            program[0] = 2;

            var grid = new Tile[37, 20];

            int? part1 = null;
            var ballX = new[] {0};
            var paddleX = new[] {0};
            var score = 0L;
            using var intcode = Intcode.RunProgram(program, IntcodeInput(ballX, paddleX)).GetEnumerator();
            while (intcode.MoveNext())
            {
                var x = intcode.Current;
                if (!intcode.MoveNext()) throw new InvalidOperationException();
                var y = intcode.Current;
                if (!intcode.MoveNext()) throw new InvalidOperationException();
                if (x < 0)
                {
                    score = intcode.Current;
                    continue;
                }

                var tile = (Tile) intcode.Current;
                if (part1 == null && tile == Tile.Empty && grid[x, y] == Tile.Block)
                {
                    part1 = 0;
                    for (var i = 0; i < grid.GetLength(0); i++)
                    for (var j = 0; j < grid.GetLength(1); j++)
                        if (grid[i, j] == Tile.Block)
                            part1++;
                }

                switch (tile)
                {
                    case Tile.Ball:
                        ballX[0] = (int) x;
                        break;
                    case Tile.Paddle:
                        paddleX[0] = (int) x;
                        break;
                }

                grid[x, y] = tile;
            }

            return ($"{part1}", score.ToString());
        }

        private static IEnumerable<long> IntcodeInput(int[] ballX, int[] paddleX)
        {
            while (true)
            {
                var paddleDir = ballX[0].CompareTo(paddleX[0]);
                if (paddleDir > 0)
                    yield return 1;
                else if (paddleDir < 0)
                    yield return -1;
                else
                    yield return 0;
            }
        }

        private enum Tile
        {
            Empty,
            Wall,
            Block,
            Paddle,
            Ball
        }
    }
}