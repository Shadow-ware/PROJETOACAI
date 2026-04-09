using acaigalatico.Application.DTOs;
using acaigalatico.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

//github.com/acaigalatico/acaigalatico-api.git

namespace AcaiGalatico.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriesController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
        {
            var categorias = await _categoriaService.GetCategoriasAsync();
            return Ok(categorias);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            var categoria = await _categoriaService.GetByIdAsync(id);
            if (categoria == null) return NotFound();
            return Ok(categoria);
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> Post([FromBody] CategoriaDTO categoriaDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _categoriaService.AddAsync(categoriaDto);
            return CreatedAtAction(nameof(Get), new { id = categoriaDto.Id }, categoriaDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.Id) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _categoriaService.UpdateAsync(categoriaDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var categoria = await _categoriaService.GetByIdAsync(id);
            if (categoria == null) return NotFound();
            await _categoriaService.RemoveAsync(id);
            return NoContent();
        }
    }
}
