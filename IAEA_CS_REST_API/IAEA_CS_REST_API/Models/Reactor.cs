using System.Text.Json.Serialization;

namespace IAEA_CS_REST_API.Models
{
    public class Reactor
    {
        [JsonPropertyName("id")]
        public int Id { get; set; } = 0;

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;

        [JsonPropertyName("url_wikipedia")]
        public string? Url_Wikipedia { get; set; } = string.Empty;

        [JsonPropertyName("url_imagen")]
        public string? Url_Imagen { get; set; } = string.Empty;
    }
}