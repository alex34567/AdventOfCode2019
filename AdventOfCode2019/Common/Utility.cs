using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2019.Common
{
    public static class Utility
    {
        public static void Run(ISolution solution, string path)
        {
            var input = File.ReadAllLines(Path.Combine(path, $"Day{solution.DayN}.txt"));
            var (part1, part2) = solution.GetAns(input);
            Console.WriteLine($"Day{solution.DayN} Part1: {part1}");
            if (part2 != null) Console.WriteLine($"Day{solution.DayN} Part2: {part2}");
        }

        public static bool NextPermutation<T>(List<T> list) where T : IComparable<T>
        {
            var index1 = -1;
            for (var i = 0; i < list.Count - 1; i++)
                if (list[i].CompareTo(list[i + 1]) < 0)
                    index1 = i;

            if (index1 == -1) return false;
            var index2 = index1 + 1;
            for (var i = 0; i < list.Count; i++)
                if (list[index1].CompareTo(list[i]) < 0)
                    index2 = i;

            var tmp = list[index1];
            list[index1] = list[index2];
            list[index2] = tmp;

            list.Reverse(index1 + 1, list.Count - index1 - 1);
            return true;
        }

        public static int Gcd(int n, int m)
        {
            n = Math.Abs(n);
            m = Math.Abs(m);

            while (m != 0)
            {
                var newN = m;
                m = n % m;
                n = newN;
            }

            return n;
        }
    }
}