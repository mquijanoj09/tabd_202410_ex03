using System;
using Microsoft.AspNetCore.Mvc;
using IAEA_CS_REST_API.Services;

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
    }
}