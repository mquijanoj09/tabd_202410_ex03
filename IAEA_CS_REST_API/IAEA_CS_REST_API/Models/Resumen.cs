using System.Text.Json.Serialization;

namespace IAEA_CS_REST_API.Models
{
    public class Resumen
    {
        [JsonPropertyName("reactores")]
        public int Reactores { get; set; } = 0;

    }
}