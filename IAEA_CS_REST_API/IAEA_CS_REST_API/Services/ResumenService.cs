using IAEA_CS_REST_API.Interfaces;
using IAEA_CS_REST_API.Models;

namespace IAEA_CS_REST_API.Services
{
    public class ResumenService
    {
        private readonly IResumenRepository _resumenRepository;

        public ResumenService(IResumenRepository resumenRepository)
        {
            _resumenRepository = resumenRepository;
        }

        public async Task<Resumen> GetAllAsync()
        {
            return await _resumenRepository
                .GetAllAsync();
        }
    }
}