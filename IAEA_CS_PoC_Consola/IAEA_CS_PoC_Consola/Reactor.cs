using System;
using System.Text.Json.Serialization;

namespace IAEA_CS_PoC_Consola
{
    public class Reactor
    {
        public int Id { get; set; } = 0;
        public string? Nombre { get; set; } = string.Empty;
        public float? Potencia { get; set; } = 0;
        public string? Estado { get; set; } = string.Empty;
        public DateTime? Fecha { get; set; } = new DateTime(2000, 1, 1);
    }
}
