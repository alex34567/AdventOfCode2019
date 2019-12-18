using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode2019.Common;

namespace AdventOfCode2019
{
    internal class Day12 : ISolution
    {
        private static Regex ParsingRegex { get; } = new Regex(@"\<x=(?<X>-?\d+), y=(?<Y>-?\d+), z=(?<Z>-?\d+)\>");

        public int DayN => 12;

        public (string, string?) GetAns(string[] input)
        {
            var moons = (from moon in input
                select ParsingRegex.Match(moon)
                into parsedMoon
                let x = int.Parse(parsedMoon.Groups["X"].Value)
                let y = int.Parse(parsedMoon.Groups["Y"].Value)
                let z = int.Parse(parsedMoon.Groups["Z"].Value)
                select new Moon(new Vec3(x, y, z))).ToList();
            var xInitial = moons.Select(x => (x.Pos.X, x.Velocity.X)).ToList();
            var yInitial = moons.Select(x => (x.Pos.Y, x.Velocity.Y)).ToList();
            var zInitial = moons.Select(x => (x.Pos.Z, x.Velocity.Z)).ToList();
            var xPeriod = -1;
            var yPeriod = -1;
            var zPeriod = -1;
            var totalEnergy = -1;

            for (var i = 0; i < int.MaxValue; i++)
            {
                if (i != 0)
                {
                    if (xPeriod < 0 && moons.Select(x => (x.Pos.X, x.Velocity.X)).Zip(xInitial)
                            .All(x => x.First == x.Second))
                        xPeriod = i;

                    if (yPeriod < 0 && moons.Select(x => (x.Pos.Y, x.Velocity.Y)).Zip(yInitial)
                            .All(x => x.First == x.Second))
                        yPeriod = i;

                    if (zPeriod < 0 && moons.Select(x => (x.Pos.Z, x.Velocity.Z)).Zip(zInitial)
                            .All(x => x.First == x.Second))
                        zPeriod = i;
                }

                if (xPeriod >= 0 && yPeriod >= 0 && zPeriod >= 0) break;

                for (var j = 0; j < moons.Count; j++)
                for (var k = moons.Count - 1; k >= 0; k--)
                {
                    if (j == k) break;
                    var moonA = moons[j];
                    var velocityA = moonA.Velocity;
                    var moonB = moons[k];
                    var velocityB = moonB.Velocity;

                    if (moonA.Pos.X < moonB.Pos.X)
                    {
                        velocityA.X++;
                        velocityB.X--;
                    }
                    else if (moonA.Pos.X > moonB.Pos.X)
                    {
                        velocityA.X--;
                        velocityB.X++;
                    }

                    if (moonA.Pos.Y < moonB.Pos.Y)
                    {
                        velocityA.Y++;
                        velocityB.Y--;
                    }
                    else if (moonA.Pos.Y > moonB.Pos.Y)
                    {
                        velocityA.Y--;
                        velocityB.Y++;
                    }

                    if (moonA.Pos.Z < moonB.Pos.Z)
                    {
                        velocityA.Z++;
                        velocityB.Z--;
                    }
                    else if (moonA.Pos.Z > moonB.Pos.Z)
                    {
                        velocityA.Z--;
                        velocityB.Z++;
                    }

                    moonA.Velocity = velocityA;
                    moonB.Velocity = velocityB;
                    moons[j] = moonA;
                    moons[k] = moonB;
                }

                for (var j = 0; j < moons.Count; j++)
                {
                    var moon = moons[j];
                    moon.Pos += moon.Velocity;
                    moons[j] = moon;
                }

                if (i == 1000)
                    totalEnergy = (from moon in moons
                        let potential = moon.Pos.DistanceTo(new Vec3())
                        let kinetic = moon.Velocity.DistanceTo(new Vec3())
                        select potential * kinetic).Sum();
            }

            return (totalEnergy.ToString(), Utility.Lcm(xPeriod, Utility.Lcm((long) yPeriod, zPeriod)).ToString());
        }

        private struct Moon
        {
            public Vec3 Pos { get; set; }
            public Vec3 Velocity { get; set; }

            public Moon(Vec3 pos)
            {
                Pos = pos;
                Velocity = new Vec3();
            }
        }
    }
}