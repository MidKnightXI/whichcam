using Microsoft.Extensions.Logging;

namespace WhichCam;

public class InfosExtractor
{
    private readonly ILogger<InfosExtractor> _logger;
    private readonly string[] validImageFormat =
    {
        ".jpg", ".png", ".gif", ".tiff", ".cr2", ".nef", ".arw", ".dng", ".raf",
        ".rw2", ".erf", ".nrw", ".crw", ".3fr", ".sr2", ".k25", ".kc2", ".mef",
        ".cs1", ".orf", ".mos", ".kdc", ".cr3", ".ari", ".srf", ".srw", ".j6i",
        ".fff", ".mrw", ".x3f", ".mdc", ".rwl", ".pef", ".iiq", ".cxi", ".nksc",
    };

    public InfosExtractor()
    {
        using var loggerFactory = LoggerFactory.Create(builder => {});
        _logger = loggerFactory.CreateLogger<InfosExtractor>();
    }

    public void RetrieveInformations(DirectoryInfo targetDirectory, FileInfo outputFile)
    {
        if (targetDirectory.Exists is false)
        {
            _logger.LogError($"Directory does not exist", targetDirectory.FullName);
            return;
        }

        var picturesFromTargetDirectory = targetDirectory.GetFiles()
            .Where(f => validImageFormat.Contains(f.Extension.ToLower()))
            .AsEnumerable();

        if (picturesFromTargetDirectory is null)
        {
            _logger.LogError("Directory has no valid files", targetDirectory.FullName);
            return;
        }

        foreach (var pic in picturesFromTargetDirectory)
        {
            var stream = File.OpenRead(pic.FullName);
        }
    }
}
