using System.CommandLine;

namespace WhichCam;

internal static class Program
{
    private static int Main(string[] args)
    {
        var rootCommand = new RootCommand("WhichCam - Camera Model/Maker detector");
        var target = new Argument<DirectoryInfo>(
            name: "target",
            description: "The directory containing the images to analyze.")
        {
            Arity = ArgumentArity.ExactlyOne
        };

        rootCommand.AddArgument(target);

        rootCommand.SetHandler(target =>
        {
            if (InfosExtractor.Check(target) is false)
            {
                return;
            }
            var infos = InfosExtractor.RetrieveInformation(targ);
            var outputPath = Path.Join(AppContext.BaseDirectory, "prediction.json");

            InfosExtractor.SaveOutputInformation(infos, new FileInfo(outputPath));
        }, target);

        return rootCommand.Invoke(args);
    }
}