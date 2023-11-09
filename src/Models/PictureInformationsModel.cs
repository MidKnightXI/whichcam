namespace WhichCam;

public class CameraInformations
{
    public required string? Maker { get; set; }
    public required string? Model { get; set; }
}

public class PictureInformationsModel
{
    public required bool Success { get; set; }
    public required string Filename { get; set; }
    public required CameraInformations? Detected { get; set; }
}
