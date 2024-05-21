using System.Text.Json.Serialization;

namespace WhichCam.Model;

public class PictureInformationsModel
{
    public required bool Success { get; set; }
    public required string Filename { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ErrorMessage { get; set; }
}
