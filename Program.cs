using System.CommandLine;
using WhichCam;

internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        var rootCommand = new RootCommand("WhichCam - Camera Model/Maker detector");
        var target = new Option<DirectoryInfo>(
            name: "--target",
            description: "The directory containing the images to analyze."){
            IsRequired = true };
        var output = new Option<FileInfo>(
            name: "--output",
            description: "The output file path."){
            IsRequired = true };

        rootCommand.AddOption(target);
        rootCommand.AddOption(output);

        rootCommand.SetHandler((targ, outp) =>
        {
            var extractor = new InfosExtractor();
            extractor.RetrieveInformations(targ, outp);
        }, target, output);

        return await rootCommand.InvokeAsync(args);
    }
}