using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2019
{
    internal class Day14 : ISolution
    {
        private static Regex ParsingRegex { get; } = new Regex(@"(?:(\d+) (\w+), )*(\d+) (\w+) => (\d+) (\w+)");

        public int DayN => 14;

        public (string, string?) GetAns(string[] input)
        {
            var upperBound = 1000000000000;
            var lowerBound = 0L;
            while (lowerBound + 1 != upperBound)
            {
                var halfWay = (upperBound - lowerBound) / 2 + lowerBound;
                var res = RunIter(halfWay, input);
                if (res != null)
                {
                    lowerBound = halfWay;
                }
                else
                {
                    upperBound = halfWay;
                }
            }

            return ((RunIter(1, input) ?? 0).ToString() , lowerBound.ToString());
        }

        private static long? RunIter(long fuelCount, string[] input)
        {
            var ore = new Resource("ORE") {Count = 1000000000000};
            var fuel = new Resource("FUEL");
            var map = new Dictionary<string, Resource>
            {
                {"ORE", ore},
                {"FUEL", fuel}
            };

            foreach (var line in input)
            {
                var match = ParsingRegex.Match(line);

                var requirements = match.Groups[1].Captures
                    .Zip(match.Groups[2].Captures).Append((match.Groups[3], match.Groups[4])).Select(x =>
                    {
                        var (costCapture, nameCapture) = x;
                        var cost = int.Parse(costCapture.Value);
                        var requirementName = nameCapture.Value;

                        if (map.TryGetValue(requirementName, out var requirementResource)) return new Job(requirementResource, cost);
                        requirementResource = new Resource(requirementName);
                        map.Add(requirementName, requirementResource);

                        return new Job(requirementResource, cost);
                    }).ToList();

                var makeCount = int.Parse(match.Groups[5].Value);
                var name = match.Groups[6].Value;
                if (!map.TryGetValue(name, out var resource))
                {
                    resource = new Resource(name);
                    map.Add(name, resource);
                }

                resource.Requirements = requirements;
                resource.MakeCount = makeCount;
            }

            var stack = new Stack<Job>();
            stack.Push(new Job(fuel, fuelCount));

            while (stack.TryPop(out var job))
            {
                if (job.Resource.Count >= job.Count)
                {
                    job.Resource.Count -= job.Count;
                }
                else
                {
                    if (job.Resource.MakeCount == 0) return null;
                    var needToMake = job.Count - job.Resource.Count;
                    var recipeInvocations = Math.DivRem(needToMake, job.Resource.MakeCount, out var rem);
                    if (rem > 0)
                    {
                        recipeInvocations++;
                        job.Resource.Count = job.Resource.MakeCount - rem;
                    }
                    else
                    {
                        job.Resource.Count = 0;
                    }

                    foreach (var req in job.Resource.Requirements)
                    {
                        stack.Push(req * recipeInvocations);
                    }
                }
            }

            return 1000000000000 - ore.Count;
        }

        private struct Job
        {
            public Job(Resource resource, long count)
            {
                Resource = resource;
                Count = count;
            }

            public static Job operator *(Job job, long scale)
            {
                return new Job(job.Resource, job.Count * scale);
            }

            public Resource Resource { get; }
            public long Count { get; }
        }

        private class Resource
        {
            public Resource(string name)
            {
                Name = name;
            }

            public string Name { get; }

            public int MakeCount { get; set; }
            public List<Job> Requirements { get; set; } = new List<Job>();

            public long Count { get; set; }
        }
    }
}