namespace WhichCam;

public class CameraInformations
{
    public string? Maker { get; set; }
    public string? Model { get; set; }
}

public class PictureInformationsModel
{
    public required bool Success { get; set; }
    public required string PictureFullName { get; set; }
    public required CameraInformations Detected { get; set; }
}
