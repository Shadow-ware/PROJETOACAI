using acaigalatico.Application.Interfaces;
using acaigalatico.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AcaiGalatico.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public CustomersController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> Get()
        {
            var clientes = await _clienteService.GetClientesAsync();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> Get(int id)
        {
            var cliente = await _clienteService.GetByIdAsync(id);
            if (cliente == null) return NotFound();
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> Post([FromBody] Cliente cliente)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _clienteService.AddAsync(cliente);
            return CreatedAtAction(nameof(Get), new { id = cliente.Id }, cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Cliente cliente)
        {
            if (id != cliente.Id) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _clienteService.UpdateAsync(cliente);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _clienteService.GetByIdAsync(id);
            if (cliente == null) return NotFound();
            await _clienteService.RemoveAsync(id);
            return NoContent();
        }
    }
}
