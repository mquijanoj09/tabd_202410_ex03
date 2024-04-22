using IAEA_CS_REST_API.Models;

namespace IAEA_CS_REST_API.Interfaces
{
    public interface IReactorRepository
    {
        public Task<IEnumerable<Reactor>> GetAllAsync();
    }
}