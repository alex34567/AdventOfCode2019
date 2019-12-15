using System.Text;

namespace AdventOfCode2019
{
    internal class Day8 : ISolution
    {
        private const int Width = 25;
        private const int Height = 6;
        private const int Area = Height * Width;

        public int DayN => 8;

        public (string, string?) GetAns(string[] input)
        {
            var layerCount = input[0].Length / Area;
            var layers = new byte[layerCount, Height, Width];
            for (var i = 0; i < layerCount; i++)
            for (var j = 0; j < Height; j++)
            for (var k = 0; k < Width; k++)
                layers[i, j, k] = (byte) (input[0][i * Area + j * Width + k] - '0');

            var bestLayer = 0;
            var bestLayerCount = int.MaxValue;

            for (var i = 0; i < layerCount; i++)
            {
                var zeroCount = 0;
                for (var j = 0; j < Height; j++)
                for (var k = 0; k < Width; k++)
                    if (layers[i, j, k] == 0)
                        zeroCount++;

                if (zeroCount > bestLayerCount) continue;
                bestLayer = i;
                bestLayerCount = zeroCount;
            }

            var oneCount = 0;
            var twoCount = 0;
            for (var i = 0; i < Height; i++)
            for (var j = 0; j < Width; j++)
                if (layers[bestLayer, i, j] == 1) oneCount++;
                else if (layers[bestLayer, i, j] == 2) twoCount++;

            var part2 = new StringBuilder();
            part2.AppendLine();

            for (var i = 0; i < Height; i++)
            {
                for (var j = 0; j < Width; j++)
                {
                    var pixelIsWhite = false;
                    for (var k = 0; k < layerCount; k++)
                    {
                        if (layers[k, i, j] == 2) continue;

                        if (layers[k, i, j] == 1) pixelIsWhite = true;
                        break;
                    }

                    part2.Append(pixelIsWhite ? '#' : ' ');
                }

                part2.AppendLine();
            }

            return ((oneCount * twoCount).ToString(), part2.ToString());
        }
    }
}