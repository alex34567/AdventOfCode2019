using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019
{
    internal class Day6 : ISolution
    {
        public int DayN => 6;

        public (string, string?) GetAns(string[] input)
        {
            var orbits = new Dictionary<string, Orbit>();
            foreach (var line in input)
            {
                var splitMark = line.IndexOf(')');
                var orbitFromStr = line.Substring(0, splitMark);
                var orbitToStr = line.Substring(splitMark + 1);

                if (!orbits.TryGetValue(orbitFromStr, out var orbitFrom))
                {
                    orbitFrom = new Orbit();
                    orbits.Add(orbitFromStr, orbitFrom);
                }

                if (!orbits.TryGetValue(orbitToStr, out var orbitTo))
                {
                    orbitTo = new Orbit();
                    orbits.Add(orbitToStr, orbitTo);
                }

                orbitTo.AddOrbit(orbitFrom);
            }

            var part1 = orbits.Sum(orbit => orbit.Value.CountOrbits());
            orbits.TryGetValue("SAN", out var santa);
            orbits.TryGetValue("YOU", out var me);
            if (santa == null || me == null) return (part1.ToString(), null);
            var santaPath = santa.PathToCom().ToList();
            var commonSet = new HashSet<Orbit?>(santaPath);

            var distanceToCommonNode = 0;
            var myCurrentPos = me.Parent;
            while (commonSet.Add(myCurrentPos))
            {
                if (myCurrentPos == null) throw new InvalidOperationException();
                myCurrentPos = myCurrentPos.Parent;
                distanceToCommonNode++;
            }

            var santaCurrentPos = santa.Parent;
            while (!ReferenceEquals(santaCurrentPos, myCurrentPos))
            {
                if (santaCurrentPos == null) throw new InvalidOperationException();
                distanceToCommonNode++;
                santaCurrentPos = santaCurrentPos.Parent;
            }

            return (part1.ToString(), distanceToCommonNode.ToString());
        }

        private class Orbit
        {
            public Orbit? Parent { get; private set; }

            public void AddOrbit(Orbit orbit)
            {
                if (Parent != null) throw new InvalidOperationException();

                Parent = orbit;
            }

            public int CountOrbits()
            {
                var count = 0;
                if (Parent == null) return count;
                count++;
                count += Parent.CountOrbits();

                return count;
            }

            public IEnumerable<Orbit> PathToCom()
            {
                yield return this;

                if (Parent == null) yield break;
                foreach (var orbit in Parent.PathToCom()) yield return orbit;
            }

            public Orbit Advance(int n)
            {
                if (n != 0 && Parent == null) throw new InvalidOperationException();

                return n == 0 ? this : Parent!.Advance(n - 1);
            }
        }
    }
}