using System.Numerics;
using IAEA_CS_REST_API.Models;

namespace IAEA_CS_REST_API.Interfaces
{
    public interface IReactorRepository
    {
        public Task<IEnumerable<Reactor>> GetAllAsync();

        public Task<Reactor> GetByIdAsync(int reactor_id);

        public Task<Reactor> GetByNameAsync(string reactor_nombre);

        public Task<bool> CreateAsync(Reactor unaReactor);
    }
}