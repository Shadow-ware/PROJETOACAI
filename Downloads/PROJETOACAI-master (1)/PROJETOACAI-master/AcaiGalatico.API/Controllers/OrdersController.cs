using acaigalatico.Application.Interfaces;
using acaigalatico.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AcaiGalatico.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IVendaService _vendaService;

        public OrdersController(IVendaService vendaService)
        {
            _vendaService = vendaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Venda>>> Get()
        {
            var vendas = await _vendaService.GetVendasAsync();
            return Ok(vendas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Venda>> Get(int id)
        {
            var venda = await _vendaService.GetByIdAsync(id);
            if (venda == null) return NotFound();
            return Ok(venda);
        }

        [HttpPost]
        public async Task<ActionResult<Venda>> Post([FromBody] Venda venda)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _vendaService.AddAsync(venda);
            return CreatedAtAction(nameof(Get), new { id = venda.Id }, venda);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Venda venda)
        {
            if (id != venda.Id) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _vendaService.UpdateAsync(venda);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var venda = await _vendaService.GetByIdAsync(id);
            if (venda == null) return NotFound();
            await _vendaService.RemoveAsync(id);
            return NoContent();
        }
    }
}
