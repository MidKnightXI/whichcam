using System.Text.Json.Serialization;

namespace WhichCam;

public class CameraInformations
{
    [JsonPropertyName("maker")]
    public required string? Maker { get; set; }

    [JsonPropertyName("model")]
    public required string? Model { get; set; }
}

public class PictureInformationsModel
{
    [JsonPropertyName("success")]
    public required bool Success { get; set; }

    [JsonPropertyName("filename")]
    public required string Filename { get; set; }

    [JsonPropertyName("detected")]
    public required CameraInformations? Detected { get; set; }

    [JsonPropertyName("error_message")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ErrorMessage { get; set; }
}
