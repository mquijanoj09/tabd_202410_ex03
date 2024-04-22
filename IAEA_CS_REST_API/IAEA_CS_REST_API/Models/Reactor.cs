using System.Text.Json.Serialization;

namespace IAEA_CS_REST_API.Models
{
    public class Reactor
    {
        [JsonPropertyName("id")]
        public int Id { get; set; } = 0;

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;

        [JsonPropertyName("potencia")]
        public float? Potencia { get; set; } = 0;

        [JsonPropertyName("estado")]
        public string? Estado { get; set; } = string.Empty;

        [JsonPropertyName("fecha")]
        public DateTime? Fecha { get; set; } = new DateTime(2000, 1, 1);
    }
}