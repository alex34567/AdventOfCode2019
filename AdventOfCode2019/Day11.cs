using System;
using System.Collections.Generic;
using System.Text;
using AdventOfCode2019.Common;
using AdventOfCode2019.Common.Intcode;

namespace AdventOfCode2019
{
    internal class Day11 : ISolution
    {
        public int DayN => 11;

        public (string, string?) GetAns(string[] input)
        {
            var program = Intcode.ParseProgram(input[0]);
            var part2Map = GenMap(program, true);
            var smallestX = int.MaxValue;
            var smallestY = int.MaxValue;
            var biggestX = int.MinValue;
            var biggestY = int.MinValue;
            foreach (var pos in part2Map.Keys)
            {
                if (pos.X < smallestX) smallestX = pos.X;
                if (pos.X > biggestX) biggestX = pos.X;
                if (pos.Y < smallestY) smallestY = pos.Y;
                if (pos.Y > biggestY) biggestY = pos.Y;
            }

            var part2 = new StringBuilder();
            part2.AppendLine();
            for (var i = biggestY; i >= smallestY; i--)
            {
                for (var j = smallestX; j <= biggestX; j++)
                    if (part2Map.TryGetValue(new Vec2(j, i), out var isWhite) && isWhite)
                        part2.Append('#');
                    else
                        part2.Append(' ');

                part2.AppendLine();
            }

            return (GenMap(program, false).Count.ToString(), part2.ToString());
        }

        private static Dictionary<Vec2, bool> GenMap(IEnumerable<long> program, bool isPart2)
        {
            var map = new Dictionary<Vec2, bool>();
            var overWhiteTile = new[] {0L};
            var oldPos = new Vec2();
            var currentPos = new Vec2();
            var currentRotation = Rotation.Up;
            if (isPart2)
            {
                map[currentPos] = true;
                overWhiteTile[0] = 1;
            }

            using var intcode = Intcode.RunProgram(program, Intcode.InputHelper(overWhiteTile)).GetEnumerator();
            while (intcode.MoveNext())
            {
                var paintWhite = intcode.Current == 1;
                if (!intcode.MoveNext()) throw new InvalidOperationException();
                var rotateDir = intcode.Current;
                currentRotation = rotateDir == 0 ? RotateLeft(currentRotation) : RotateRight(currentRotation);
                currentPos = Move(currentRotation, currentPos);
                map[oldPos] = paintWhite;
                overWhiteTile[0] = 0;
                if (map.TryGetValue(currentPos, out var overWhite) && overWhite) overWhiteTile[0] = 1;
                oldPos = currentPos;
            }

            return map;
        }

        private static Rotation RotateLeft(Rotation rotation)
        {
            var rotated = rotation + 1;
            if (rotated > Rotation.Right) rotated = Rotation.Up;
            return rotated;
        }

        private static Rotation RotateRight(Rotation rotation)
        {
            var rotated = rotation - 1;
            if (rotated < Rotation.Up) rotated = Rotation.Right;
            return rotated;
        }

        private static Vec2 Move(Rotation rotation, Vec2 pos)
        {
            return rotation switch
            {
                Rotation.Up => new Vec2(pos.X, pos.Y + 1),
                Rotation.Left => new Vec2(pos.X - 1, pos.Y),
                Rotation.Down => new Vec2(pos.X, pos.Y - 1),
                Rotation.Right => new Vec2(pos.X + 1, pos.Y),
                _ => throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null)
            };
        }

        private enum Rotation
        {
            Up,
            Left,
            Down,
            Right
        }
    }
}