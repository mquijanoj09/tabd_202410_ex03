using IAEA_CS_REST_API.Models;

namespace IAEA_CS_REST_API.Interfaces
{
    public interface ITipoRepository
    {
        public Task<IEnumerable<Tipo>> GetAllAsync();

        public Task<Tipo> GetByIdAsync(int tipo_id);

    }
}