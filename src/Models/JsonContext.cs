using System.Text.Json.Serialization;

namespace WhichCam.Model.JsonContext;

[JsonSourceGenerationOptions(WriteIndented = true,
                             PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
[JsonSerializable(typeof(List<PictureInformationsModel>))]
internal partial class Context : JsonSerializerContext
{
}
