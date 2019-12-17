using System;
using System.Collections.Generic;
using AdventOfCode2019.Common;

namespace AdventOfCode2019
{
    internal class Day10 : ISolution
    {
        public int DayN => 10;

        public (string, string?) GetAns(string[] input)
        {
            var height = input.Length;
            var width = input[0].Length;
            var grid = new bool[width, height];
            for (var i = 0; i < height; i++)
            for (var j = 0; j < width; j++)
                grid[j, i] = input[i][j] == '#';

            var mostAsteroids = 0;
            var bestPos = new Vec2();

            for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
            {
                if (!grid[i, j]) continue;
                var asteroidsInView = 0;
                for (var k = 0; k < width; k++)
                for (var l = 0; l < height; l++)
                {
                    if (!grid[k, l] || k == i && l == j) continue;
                    var deltaX = k - i;
                    var deltaY = l - j;
                    var gcd = Utility.Gcd(deltaX, deltaY);
                    deltaX /= gcd;
                    deltaY /= gcd;
                    var currentX = i + deltaX;
                    var currentY = j + deltaY;
                    var canSee = true;
                    while (currentX != k || currentY != l)
                    {
                        if (grid[currentX, currentY])
                        {
                            canSee = false;
                            break;
                        }

                        currentX += deltaX;
                        currentY += deltaY;
                    }

                    if (canSee) asteroidsInView++;
                }

                if (asteroidsInView <= mostAsteroids) continue;
                mostAsteroids = asteroidsInView;
                bestPos = new Vec2(i, j);
            }

            var lines = new SortedSet<Vec2>(ClockWiseComparer.Instance);
            for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
            {
                if (!grid[i, j] || i == bestPos.X && j == bestPos.Y) continue;
                var deltaX = i - bestPos.X;
                var deltaY = j - bestPos.Y;
                var gcd = Utility.Gcd(deltaX, deltaY);
                deltaX /= gcd;
                deltaY /= gcd;
                lines.Add(new Vec2(deltaX, deltaY));
            }

            bool destroyedAsteroid;
            var asteroidCount = 0;
            var twoHundredthPos = new Vec2();
            do
            {
                destroyedAsteroid = false;
                foreach (var line in lines)
                {
                    var currentX = bestPos.X + line.X;
                    var currentY = bestPos.Y + line.Y;
                    while (currentX >= 0 && currentY >= 0 && currentX < width && currentY < height)
                    {
                        if (grid[currentX, currentY])
                        {
                            asteroidCount++;
                            destroyedAsteroid = true;
                            grid[currentX, currentY] = false;
                            if (asteroidCount == 200) twoHundredthPos = new Vec2(currentX, currentY);
                            break;
                        }

                        currentX += line.X;
                        currentY += line.Y;
                    }
                }
            } while (destroyedAsteroid);

            return (mostAsteroids.ToString(), (twoHundredthPos.X * 100 + twoHundredthPos.Y).ToString());
        }

        private class ClockWiseComparer : IComparer<Vec2>
        {
            private ClockWiseComparer()
            {
            }

            public static ClockWiseComparer Instance { get; } = new ClockWiseComparer();

            public int Compare(Vec2 a, Vec2 b)
            {
                var aAngle = Math.Atan2(a.X, -a.Y);
                var bAngle = Math.Atan2(b.X, -b.Y);
                if (aAngle < 0) aAngle += Math.PI * 2;
                if (bAngle < 0) bAngle += Math.PI * 2;

                return aAngle.CompareTo(bAngle);
            }
        }
    }
}