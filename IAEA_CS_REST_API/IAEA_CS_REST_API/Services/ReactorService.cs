using System;
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
    }
}
