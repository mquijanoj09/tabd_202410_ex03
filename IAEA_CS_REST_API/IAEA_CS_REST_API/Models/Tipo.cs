using System.Text.Json.Serialization;

namespace IAEA_CS_REST_API.Models
{
    public class Tipo
    {
        [JsonPropertyName("id")]
        public int Id { get; set; } = 0;

        [JsonPropertyName("tipo_nombre")]
        public string? Tipo_nombre { get; set; } = string.Empty;

    }
}
