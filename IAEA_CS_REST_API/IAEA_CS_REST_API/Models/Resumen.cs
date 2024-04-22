using System.Text.Json.Serialization;

namespace IAEA_CS_REST_API.Models
{
    public class Resumen
    {
        [JsonPropertyName("reactores")]
        public int Reactores { get; set; } = 0;

        [JsonPropertyName("tipos")]
        public int Tipos { get; set; } = 0;

        [JsonPropertyName("ubicaciones")]
        public int Ubicaciones { get; set; } = 0;

    }
}