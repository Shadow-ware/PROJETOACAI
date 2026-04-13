using acaigalatico.Application.Interfaces;
using acaigalatico.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AcaiGalatico.API.Controllers
{
    [Route("api/pedidos")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IVendaService _vendaService;

        public OrdersController(IVendaService vendaService)
        {
            _vendaService = vendaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Venda>>> GetPedidos()
        {
            try 
            {
                var vendas = await _vendaService.GetVendasAsync();
                return Ok(vendas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
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
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            
            // Se o ID vier preenchido do Web, zeramos para o banco gerar um novo
            // e evitar conflitos de chave primária.
            venda.Id = 0; 
            
            await _vendaService.AddAsync(venda);
            return CreatedAtAction(nameof(GetPedidos), new { id = venda.Id }, venda);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Venda venda)
        {
            if (id != venda.Id) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            // Busca a venda original para preservar itens e cliente
            var vendaOriginal = await _vendaService.GetByIdAsync(id);
            if (vendaOriginal == null) return NotFound();

            // Atualiza apenas os campos permitidos pelo Desktop
            vendaOriginal.Status = venda.Status;
            vendaOriginal.EnderecoEntrega = venda.EnderecoEntrega;
            vendaOriginal.BairroEntrega = venda.BairroEntrega;
            vendaOriginal.Observacao = venda.Observacao;
            vendaOriginal.FormaPagamento = venda.FormaPagamento;
            vendaOriginal.ValorTotal = venda.ValorTotal;

            await _vendaService.UpdateAsync(vendaOriginal);
            return NoContent();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> PatchStatus(int id, [FromBody] StatusVenda status)
        {
            var venda = await _vendaService.GetByIdAsync(id);
            if (venda == null) return NotFound();

            venda.Status = status;
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
