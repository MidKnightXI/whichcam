using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
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

        var picturesPaths = targetDirectory.GetFiles()
            .Where(f => validImageFormat.Contains(f.Extension.ToLower()))
            .Select(f => f.FullName)
            .AsEnumerable();

        if (picturesPaths is null)
        {
            _logger.LogError("Directory has no valid files", targetDirectory.FullName);
            return;
        }

        var outputInformations = new List<PictureInformationsModel>();
        foreach (var path in picturesPaths)
        {
            using var stream = File.OpenRead(path);
            var directories = ImageMetadataReader.ReadMetadata(stream);

            var ifd0Directory = directories.OfType<ExifIfd0Directory>().FirstOrDefault();

            if (ifd0Directory != null)
            {
                var maker = ifd0Directory.GetDescription(ExifDirectoryBase.TagMake);
                var model = ifd0Directory.GetDescription(ExifDirectoryBase.TagModel);

                outputInformations.Add(new PictureInformationsModel()
                {
                    Success = true,
                    Filename = path,
                    Detected = new CameraInformations(){ Maker = maker, Model = model }
                });
            }
            else
            {
                outputInformations.Add(new PictureInformationsModel()
                {
                    Success = false,
                    Filename = path,
                    Detected = null
                });
            }
        }
    }
}
