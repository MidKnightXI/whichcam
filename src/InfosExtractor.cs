using System.Text.Json;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;

namespace WhichCam;

public class InfosExtractor
{
    private static readonly string[] validImageFormat =
    {
        ".jpg", ".png", ".gif", ".tiff", ".cr2", ".nef", ".arw", ".dng", ".raf",
        ".rw2", ".erf", ".nrw", ".crw", ".3fr", ".sr2", ".k25", ".kc2", ".mef",
        ".cs1", ".orf", ".mos", ".kdc", ".cr3", ".ari", ".srf", ".srw", ".j6i",
        ".fff", ".mrw", ".x3f", ".mdc", ".rwl", ".pef", ".iiq", ".cxi", ".nksc",
    };

    public static List<PictureInformationsModel> RetrieveInformations(DirectoryInfo targetDirectory)
    {
        var outputInformations = new List<PictureInformationsModel>();
        var picturesPaths = targetDirectory.GetFiles()
            .Where(f => validImageFormat.Contains(f.Extension.ToLower()))
            .Select(f => f.FullName);

        foreach (var path in picturesPaths)
        {
            try
            {
                using var stream = File.OpenRead(path);
                var directories = ImageMetadataReader.ReadMetadata(stream);
                var cameraInformations = GetCameraInformations(directories);

                outputInformations.Add(new PictureInformationsModel()
                {
                    Success = cameraInformations is not null,
                    Filename = path,
                    Detected = cameraInformations
                });
            }
            catch (Exception ex)
            {
                outputInformations.Add(new PictureInformationsModel()
                {
                    Success = false,
                    Filename = path,
                    Detected = null,
                    ErrorMessage = ex.Message
                });
            }
        }

        return outputInformations;
    }

    public static bool Check(DirectoryInfo targetDirectory)
    {
        if (targetDirectory.Exists is false)
        {
            Console.Error.WriteLine($"Directory does not exist", targetDirectory.FullName);
            return false;
        }

        var picturesPaths = targetDirectory.GetFiles()
            .Any(f => validImageFormat.Contains(f.Extension.ToLower()));

        if (picturesPaths is false)
        {
            Console.Error.WriteLine("Directory has no valid files", targetDirectory.FullName);
            return false;
        }

        return true;
    }

    private static CameraInformations? GetCameraInformations(IReadOnlyList<MetadataExtractor.Directory> directories)
    {
        if (directories is null)
        {
            return null;
        }

        var ifd0Directory = directories.OfType<ExifIfd0Directory>().FirstOrDefault();

        if (ifd0Directory is not null)
        {
            var maker = ifd0Directory.GetDescription(ExifDirectoryBase.TagMake);
            var model = ifd0Directory.GetDescription(ExifDirectoryBase.TagModel);

            return new CameraInformations() { Maker = maker, Model = model };
        }
        else
        {
            return null;
        }
    }

    public static void SaveOutputInformations(List<PictureInformationsModel> outputInformations,
                                              FileInfo outputFile)
    {
        using var stream = outputFile.CreateText();
        var json = JsonSerializer.Serialize(
            outputInformations,
            new JsonSerializerOptions { WriteIndented = true });

        stream.Write(json);
    }
}
