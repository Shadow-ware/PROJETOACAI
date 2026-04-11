using acaigalatico.Application.DTOs;
using acaigalatico.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AcaiGalatico.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProdutoService _produtoService;

        public ProductsController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get()
        {
            var produtos = await _produtoService.GetProdutosAsync();
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoDTO>> Get(int id)
        {
            var produto = await _produtoService.GetByIdAsync(id);
            if (produto == null) return NotFound();
            return Ok(produto);
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoDTO>> Post([FromBody] ProdutoDTO produtoDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _produtoService.AddAsync(produtoDto);
            return CreatedAtAction(nameof(Get), new { id = produtoDto.Id }, produtoDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProdutoDTO produtoDto)
        {
            if (id != produtoDto.Id) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _produtoService.UpdateAsync(produtoDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var produto = await _produtoService.GetByIdAsync(id);
            if (produto == null) return NotFound();
            await _produtoService.RemoveAsync(id);
            return NoContent();
        }
    }
}
