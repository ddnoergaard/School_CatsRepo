using CatsRepo.DTO.Cat;
using CatsRepo.Models;
using CatsRepo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Data.SqlClient;

namespace CatsRepo.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/[controller]")]
    [ApiController]
    public class CatsController : ControllerBase
    {
        private readonly ICatsService _catsService;

        public CatsController(ICatsService catsService)
        {
            _catsService = catsService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateDTO dto)
        {
            if (dto is null) return BadRequest();

            try
            {
                Cat cat = await _catsService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = cat.Id }, cat);
            } catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0) return BadRequest();

            try
            {
                Cat cat = await _catsService.GetById(id);
                return Ok(cat);
            } catch (KeyNotFoundException ex)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromQuery] string? name = null, [FromQuery] int? minWeight = null)
        {
            List<Cat> cats = (await _catsService.Get(name, minWeight)).ToList();

            if (cats.Count == 0) return BadRequest("List not found");

            return Ok(cats);
        }

    }
}
