using System.CommandLine;

internal class Program
{

    private static async Task RetrieveInformations(DirectoryInfo targetDirectory, FileInfo outputFile)
    {
        Console.WriteLine(targetDirectory.Name);
        Console.WriteLine(outputFile.Name);
    }

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

        rootCommand.SetHandler((targ, outp) => RetrieveInformations(targ, outp), target, output);

        return await rootCommand.InvokeAsync(args);
    }
}