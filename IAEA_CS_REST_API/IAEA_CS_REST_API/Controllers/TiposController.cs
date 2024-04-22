﻿using System;
using Microsoft.AspNetCore.Mvc;
using IAEA_CS_REST_API.Services;
using IAEA_CS_REST_API.Helpers;

namespace IAEA_CS_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiposController(TipoService tipoService) : Controller
    {
        private readonly TipoService _tipoService = tipoService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var lasTipos = await _tipoService
                .GetAllAsync();

            return Ok(lasTipos);
        }

        [HttpGet("{tipo_id:int}")]
        public async Task<IActionResult> GetByIdAsync(int tipo_id)
        {
            try
            {
                var unaTipo = await _tipoService
                    .GetByIdAsync(tipo_id);

                return Ok(unaTipo);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }

    }
}