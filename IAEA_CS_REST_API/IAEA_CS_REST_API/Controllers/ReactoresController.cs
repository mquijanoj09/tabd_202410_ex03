using System;
using Microsoft.AspNetCore.Mvc;
using IAEA_CS_REST_API.Services;
using IAEA_CS_REST_API.Helpers;

namespace IAEA_CS_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReactoresController(ReactorService reactorService) : Controller
    {
        private readonly ReactorService _reactorService = reactorService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var lasReactores = await _reactorService
                .GetAllAsync();

            return Ok(lasReactores);
        }

        [HttpGet("{fruta_id:int}")]
        public async Task<IActionResult> GetByIdAsync(int fruta_id)
        {
            try
            {
                var unaFruta = await _reactorService
                    .GetByIdAsync(fruta_id);

                return Ok(unaFruta);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }
    }
}