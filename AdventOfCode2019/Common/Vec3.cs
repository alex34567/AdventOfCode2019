using System;

namespace AdventOfCode2019.Common
{
    public struct Vec3 : IComparable<Vec3>, IComparable, IEquatable<Vec3>
    {
        public bool Equals(Vec3 other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override bool Equals(object? obj)
        {
            return obj is Vec3 other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (X, Y, Z).GetHashCode();
        }

        public static bool operator ==(Vec3 left, Vec3 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vec3 left, Vec3 right)
        {
            return !left.Equals(right);
        }

        public int CompareTo(object? obj)
        {
            if (obj is null) return 1;
            return obj is Vec3 other
                ? CompareTo(other)
                : throw new ArgumentException($"Object must be of type {nameof(Vec3)}");
        }

        public static bool operator <(Vec3 left, Vec3 right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(Vec3 left, Vec3 right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(Vec3 left, Vec3 right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(Vec3 left, Vec3 right)
        {
            return left.CompareTo(right) >= 0;
        }

        public int CompareTo(Vec3 other)
        {
            var xComparision = X.CompareTo(other.X);
            if (xComparision != 0)
                return xComparision;
            var yComparison = Y.CompareTo(other.Y);
            return yComparison != 0 ? yComparison : Z.CompareTo(other.Z);
        }

        public Vec3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vec3 operator +(Vec3 a, Vec3 b)
        {
            return new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vec3 operator -(Vec3 a, Vec3 b)
        {
            return new Vec3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public int DistanceTo(Vec3 other)
        {
            return Math.Abs(X - other.X) + Math.Abs(Y - other.Y) + Math.Abs(Z - other.Z);
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
    }
}