using System;
using Microsoft.AspNetCore.Mvc;
using IAEA_CS_REST_API.Services;
using IAEA_CS_REST_API.Helpers;
using IAEA_CS_REST_API.Models;

namespace IAEA_CS_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UbicacionesController(UbicacionService ubicacionService) : Controller
    {
        private readonly UbicacionService _ubicacionService = ubicacionService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var lasUbicaciones = await _ubicacionService
                .GetAllAsync();

            return Ok(lasUbicaciones);
        }

        [HttpGet("{ubicacion_id:int}")]
        public async Task<IActionResult> GetByIdAsync(int ubicacion_id)
        {
            try
            {
                var unaUbicacion = await _ubicacionService
                    .GetByIdAsync(ubicacion_id);

                return Ok(unaUbicacion);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpGet("{ubicacion_id:int}/Reactores")]
        public async Task<IActionResult> GetReactoresUbicacionAsync(int ubicacion_id)
        {
            try
            {
                var losReactoresUbicacion = await _ubicacionService
                    .GetReactoresUbicacionAsync(ubicacion_id);

                return Ok(losReactoresUbicacion);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

    }
}