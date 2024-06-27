using System.CommandLine;

namespace WhichCam;

internal static class Program
{
    private static int Main(string[] args)
    {
        var rootCommand = new RootCommand("WhichCam - Camera Model/Maker detector");
        var targetArgument = new Argument<DirectoryInfo>(
            name: "target",
            description: "The directory containing the images to analyze.")
        {
            Arity = ArgumentArity.ExactlyOne
        };
        var outputOption = new Option<DirectoryInfo>(
            name: "--output",
            description: "Where to copy the files",
            getDefaultValue: () => new DirectoryInfo("./"))
        {
            Arity = ArgumentArity.ExactlyOne,
        };
        var orderByOption = new Option<string>(
            name: "--orderBy",
            description: "Determine if it needs to orders medias by maker or model",
            getDefaultValue: () => "maker"
        )
        {
            Arity = ArgumentArity.ExactlyOne
        };

        targetArgument.AddValidator(arg =>
        {
            var value = arg.GetValueOrDefault() as DirectoryInfo;
            if (value is not null || value!.Exists is false)
            {
                throw new ArgumentException("Target directory {0} does not exist", value.FullName);
            }
            var picturesPaths = value!.GetFiles().Any(f => InfosExtractor.validImageFormats.Contains(f.Extension.ToLower()));
            if (picturesPaths is false)
            {
                throw new ArgumentException("Target directory {0} has no valid files", value.FullName);
            }
        });
        outputOption.AddValidator(arg =>
        {
            if (arg.GetValueOrDefault() is not DirectoryInfo value || value.Exists is false)
            {
                throw new ArgumentException($"Output directory is invalid or does not exist.");
            }
        });
        orderByOption.AddValidator(arg =>
        {
            var value = arg.GetValueOrDefault();
            if (value is not "maker" && value is not "model")
            {
                throw new ArgumentException("The value for --orderBy must be 'maker' or 'model'. (default 'maker')");
            }
        });

        rootCommand.AddArgument(targetArgument);
        rootCommand.AddOption(outputOption);
        rootCommand.AddOption(orderByOption);

        rootCommand.SetHandler((target, output, orderBy) =>
        {
            var infos = InfosExtractor.RetrieveInformation(target);
            InfosExtractor.OrderPictures(infos, output, orderBy);
        }, targetArgument, outputOption, orderByOption);

        return rootCommand.Invoke(args);
    }
}