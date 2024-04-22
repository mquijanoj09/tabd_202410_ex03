using System;
using IAEA_CS_REST_API.Helpers;
using IAEA_CS_REST_API.Interfaces;
using IAEA_CS_REST_API.Models;
using System.Runtime.Intrinsics.X86;

namespace IAEA_CS_REST_API.Services
{
    public class TipoService(ITipoRepository tipoRepository)
    {
        private readonly ITipoRepository _tipoRepository = tipoRepository;

        public async Task<IEnumerable<Tipo>> GetAllAsync()
        {
            return await _tipoRepository
                .GetAllAsync();
        }

        public async Task<Tipo> GetByIdAsync(int tipo_id)
        {
            Tipo unaTipo = await _tipoRepository
                .GetByIdAsync(tipo_id);

            if (unaTipo.Id == 0)
                throw new AppValidationException($"Tipo no encontrada con el id {tipo_id}");

            return unaTipo;
        }

    }
}
