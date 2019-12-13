namespace AdventOfCode2019
{
    internal class Day4 : ISolution
    {
        public int DayN => 4;

        public (string, string?) GetAns(string[] input)
        {
            var splitMarker = input[0].IndexOf('-');
            var lowBound = int.Parse(input[0].Substring(0, splitMarker));
            var upperBound = int.Parse(input[0].Substring(splitMarker + 1));
            var goodPart1 = 0;
            var goodPart2 = 0;

            while (upperBound >= lowBound)
            {
                var n = lowBound;
                var lastDigit = 10;
                var twoAdjacent = false;
                var validPart1 = true;
                var onlyTwoAdjacent = false;
                var inAdjacentGroup = false;
                var tooManyInAdjacentGroup = false;

                while (n > 0)
                {
                    var digit = n % 10;
                    n /= 10;
                    if (digit == lastDigit)
                    {
                        twoAdjacent = true;

                        if (inAdjacentGroup)
                            tooManyInAdjacentGroup = true;
                        else
                            inAdjacentGroup = true;
                    }
                    else
                    {
                        if (inAdjacentGroup && !tooManyInAdjacentGroup) onlyTwoAdjacent = true;

                        inAdjacentGroup = false;
                        tooManyInAdjacentGroup = false;
                    }

                    if (digit > lastDigit)
                    {
                        validPart1 = false;
                        break;
                    }

                    lastDigit = digit;
                }

                if (!twoAdjacent) validPart1 = false;
                if (validPart1)
                {
                    goodPart1++;
                    if (onlyTwoAdjacent || inAdjacentGroup && !tooManyInAdjacentGroup) goodPart2++;
                }

                lowBound++;
            }

            return (goodPart1.ToString(), goodPart2.ToString());
        }
    }
}