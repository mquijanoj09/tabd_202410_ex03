using IAEA_CS_REST_API.Models;

namespace IAEA_CS_REST_API.Interfaces
{
    public interface IUbicacionRepository
    {
        public Task<IEnumerable<Ubicacion>> GetAllAsync();

        public Task<Ubicacion> GetByIdAsync(int ubicacion_id); 

        public Task<Ubicacion> GetReactoresUbicacionAsync(int ubicacion_id);
    }
}