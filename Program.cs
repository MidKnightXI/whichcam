using System.CommandLine;

internal class Program
{
    private static void Main(string[] args)
    {
        var rootCommand = new RootCommand
        {
            new Option<string>("--target", "The directory containing the images to analyze."),
            new Option<string>("--output", "The output file path.")
        };
    }
}