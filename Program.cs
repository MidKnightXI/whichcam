using System.CommandLine;

namespace WhichCam;

internal static class Program
{
    private static int Main(string[] args)
    {
        var rootCommand = new RootCommand("WhichCam - Camera Model/Maker detector");
        var target = new Option<DirectoryInfo>(
            name: "--target",
            description: "The directory containing the images to analyze."){
            IsRequired = true };

        rootCommand.AddOption(target);

        rootCommand.SetHandler(targ =>
        {
            if (InfosExtractor.Check(targ) is false)
                return;

            var infos = InfosExtractor.RetrieveInformation(targ);
            InfosExtractor.SaveOutputInformation(infos, new FileInfo(AppContext.BaseDirectory));
        }, target);

        return rootCommand.Invoke(args);
    }
}