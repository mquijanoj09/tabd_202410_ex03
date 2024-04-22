using System.Text.Json.Serialization;

namespace IAEA_CS_REST_API.Models
{
    public class Ubicacion
    {
        [JsonPropertyName("id")]
        public int Id { get; set; } = 0;

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;

    }
}
