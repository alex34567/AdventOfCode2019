namespace AdventOfCode2019
{
    public interface ISolution
    {
        int DayN { get; }
        (string, string?) GetAns(string[] input);
    }
}