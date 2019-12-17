using System.Collections.Generic;
using System.Linq;
using AdventOfCode2019.Common;
using AdventOfCode2019.Common.Intcode;

namespace AdventOfCode2019
{
    internal class Day7 : ISolution
    {
        public int DayN => 7;

        public (string, string?) GetAns(string[] input)
        {
            var program = Intcode.ParseProgram(input[0]);
            var settings = new List<long>
            {
                0, 1, 2, 3, 4
            };

            var feedbackSettings = new List<long>
            {
                5, 6, 7, 8, 9
            };

            return (GetMaxPower(program, settings).ToString(), GetMaxPower(program, feedbackSettings).ToString());
        }


        private static long GetMaxPower(IList<long> program, List<long> settings)
        {
            var maxPower = long.MinValue;
            do
            {
                var currentFeedBack = new[] {0L};
                var lastAmplifier =
                    Intcode.RunProgram(program, new[] {settings[0]}.Concat(Intcode.InputHelper(currentFeedBack)));
                for (var i = 1; i < 5; i++)
                    lastAmplifier = Intcode.RunProgram(program, new[] {settings[i]}.Concat(lastAmplifier));

                foreach (var value in lastAmplifier) currentFeedBack[0] = value;

                if (currentFeedBack[0] > maxPower) maxPower = currentFeedBack[0];
            } while (Utility.NextPermutation(settings));

            return maxPower;
        }
    }
}