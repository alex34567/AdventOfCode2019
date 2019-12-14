using System;
using AdventOfCode2019.Common;

namespace AdventOfCode2019
{
    internal class Day3 : ISolution
    {
        public int DayN => 3;

        public (string, string?) GetAns(string[] input)
        {
            var firstWire = input[0].Split(',');
            var secondWire = input[1].Split(',');

            var bestDistance = int.MaxValue;
            var bestCost = int.MaxValue;
            var firstWirePos = new Vec2();
            var firstWireCost = 0;

            foreach (var firstWireCommand in firstWire)
            {
                var firstWireLine = ParseIns(firstWireCommand, firstWirePos, firstWireCost);
                firstWirePos = firstWireLine.EndPos;
                firstWireCost = firstWireLine.EndCost;

                var secondWirePos = new Vec2();
                var secondWireCost = 0;
                foreach (var secondWireCommand in secondWire)
                {
                    var secondWireLine = ParseIns(secondWireCommand, secondWirePos, secondWireCost);
                    secondWirePos = secondWireLine.EndPos;
                    secondWireCost = secondWireLine.EndCost;

                    var intersect = firstWireLine.GoesThough(secondWireLine);
                    if (intersect == null) continue;
                    var (distance, cost) = intersect.Value;
                    if (cost == 0) continue;
                    if (distance < bestDistance) bestDistance = distance;

                    if (cost < bestCost) bestCost = cost;
                }
            }

            return (bestDistance.ToString(), bestCost.ToString());
        }

        private static Line ParseIns(string command, Vec2 startPos, int oldCost)
        {
            var commandLetter = command[0];
            var commandNumber = int.Parse(command.Substring(1));

            return commandLetter switch
            {
                'U' => new UpDown(commandNumber, startPos, oldCost),
                'D' => new UpDown(-commandNumber, startPos, oldCost),
                'L' => new LeftRight(commandNumber, startPos, oldCost),
                'R' => new LeftRight(-commandNumber, startPos, oldCost),
                _ => throw new ArgumentException(nameof(command))
            };
        }

        private abstract class Line
        {
            public abstract Vec2 StartPos { get; }
            public abstract Vec2 EndPos { get; }
            public abstract int BeginCost { get; }
            public abstract int EndCost { get; }

            public Vec2 FirstPos => StartPos > EndPos ? EndPos : StartPos;
            public Vec2 SecondPos => StartPos <= EndPos ? EndPos : StartPos;
            public abstract (int, int)? GoesThough(Line otherLine);
        }

        private class UpDown : Line
        {
            private readonly int _magnitude;

            public UpDown(int magnitude, Vec2 startPos, int beginCost)
            {
                _magnitude = magnitude;
                BeginCost = beginCost;
                StartPos = startPos;
            }

            public override Vec2 StartPos { get; }
            public override int BeginCost { get; }

            public override Vec2 EndPos => new Vec2(StartPos.X, StartPos.Y + _magnitude);
            public override int EndCost => Math.Abs(_magnitude) + BeginCost;

            public override (int, int)? GoesThough(Line otherLine)
            {
                if (otherLine is UpDown upDown)
                {
                    if (FirstPos.X != upDown.FirstPos.X) return null;

                    var lowBound = Math.Max(FirstPos.Y, otherLine.FirstPos.Y);
                    var upperBound = Math.Min(SecondPos.Y, otherLine.SecondPos.Y);

                    if (lowBound > upperBound) return null;

                    var intersection1 = new Vec2(FirstPos.X, lowBound);
                    var intersection2 = new Vec2(FirstPos.X, upperBound);
                    var cost1 = StartPos.DistanceTo(intersection1) + otherLine.StartPos.DistanceTo(intersection1) +
                                BeginCost + otherLine.BeginCost;
                    var cost2 = StartPos.DistanceTo(intersection2) + otherLine.StartPos.DistanceTo(intersection2) +
                                BeginCost + otherLine.BeginCost;

                    return (intersection1.DistanceTo(new Vec2()), Math.Min(cost1, cost2));
                }

                if (otherLine.FirstPos.X > StartPos.X || otherLine.SecondPos.X < StartPos.X ||
                    FirstPos.Y > otherLine.StartPos.Y || SecondPos.Y < otherLine.EndPos.Y)
                    return null;

                var point = new Vec2(StartPos.X, otherLine.StartPos.Y);
                var cost = point.DistanceTo(StartPos) + point.DistanceTo(otherLine.StartPos) + BeginCost +
                           otherLine.BeginCost;

                return (point.DistanceTo(new Vec2()), cost);
            }
        }

        private class LeftRight : Line
        {
            private readonly int _magnitude;

            public LeftRight(int magnitude, Vec2 startPos, int beginCost)
            {
                _magnitude = magnitude;
                BeginCost = beginCost;
                StartPos = startPos;
            }

            public override int BeginCost { get; }
            public override Vec2 StartPos { get; }

            public override Vec2 EndPos => new Vec2(StartPos.X + _magnitude, StartPos.Y);
            public override int EndCost => Math.Abs(_magnitude) + BeginCost;

            public override (int, int)? GoesThough(Line otherLine)
            {
                if (!(otherLine is LeftRight leftRight)) return otherLine.GoesThough(this);
                if (FirstPos.Y != leftRight.FirstPos.Y) return null;

                var lowBound = Math.Max(FirstPos.X, otherLine.FirstPos.X);
                var upperBound = Math.Min(SecondPos.X, otherLine.SecondPos.X);

                if (lowBound > upperBound) return null;

                var intersection1 = new Vec2(FirstPos.Y, lowBound);
                var intersection2 = new Vec2(FirstPos.Y, upperBound);
                var cost1 = StartPos.DistanceTo(intersection1) + otherLine.StartPos.DistanceTo(intersection1) +
                            BeginCost + otherLine.BeginCost;
                var cost2 = StartPos.DistanceTo(intersection2) + otherLine.StartPos.DistanceTo(intersection2) +
                            BeginCost + otherLine.BeginCost;

                return (intersection1.DistanceTo(new Vec2()), Math.Min(cost1, cost2));
            }
        }
    }
}