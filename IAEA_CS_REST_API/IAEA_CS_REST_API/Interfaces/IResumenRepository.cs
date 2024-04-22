using IAEA_CS_REST_API.Models;

namespace IAEA_CS_REST_API.Interfaces
{
    public interface IResumenRepository
    {
        public Task<Resumen> GetAllAsync();
    }
}