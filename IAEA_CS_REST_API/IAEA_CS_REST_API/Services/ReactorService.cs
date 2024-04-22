using System;
using IAEA_CS_REST_API.Helpers;
using IAEA_CS_REST_API.Interfaces;
using IAEA_CS_REST_API.Models;

namespace IAEA_CS_REST_API.Services
{
    public class ReactorService(IReactorRepository reactorRepository)
    {
        private readonly IReactorRepository _reactorRepository = reactorRepository;

        public async Task<IEnumerable<Reactor>> GetAllAsync()
        {
            return await _reactorRepository
                .GetAllAsync();
        }

        public async Task<Reactor> GetByIdAsync(int fruta_id)
        {
            Reactor unaReactor = await _reactorRepository
                .GetByIdAsync(fruta_id);

            if (unaReactor.Id == 0)
                throw new AppValidationException($"Reactor no encontrada con el id {fruta_id}");

            return unaReactor;
        }
    }
}
