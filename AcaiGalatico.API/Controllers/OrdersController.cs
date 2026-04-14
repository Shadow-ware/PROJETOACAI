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
                Console.WriteLine($"[API-INFO] Recebido pedido GET para listar vendas em {DateTime.Now}");
                var vendas = await _vendaService.GetVendasAsync();
                var list = vendas.ToList();
                Console.WriteLine($"[API-INFO] Retornando {list.Count} vendas.");
                return Ok(list);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[API-ERROR] Erro no GET vendas: {ex.Message}");
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Venda>> Get(int id)
        {
            Console.WriteLine($"[API-INFO] Recebido pedido GET para venda ID {id}");
            var venda = await _vendaService.GetByIdAsync(id);
            if (venda == null) return NotFound();
            return Ok(venda);
        }

        [HttpPost]
        public async Task<ActionResult<Venda>> Post([FromBody] Venda venda)
        {
            Console.WriteLine($"[API-INFO] Recebido novo pedido via POST em {DateTime.Now}");
            
            if (!ModelState.IsValid) 
            {
                Console.WriteLine("[API-WARN] ModelState inválido para o pedido recebido.");
                return BadRequest(ModelState);
            }
            
            try 
            {
                // Reset do ID para garantir que o banco gere um novo
                venda.Id = 0; 
                
                // Se houver itens, garante que o ID da venda neles também seja resetado
                if (venda.Itens != null)
                {
                    foreach (var item in venda.Itens)
                    {
                        item.Id = 0;
                        item.VendaId = 0;
                        // Garantir que não estamos tentando adicionar produtos ou vendas existentes
                        item.Produto = null;
                        item.Venda = null;
                    }
                }
                
                // Garantir que não estamos tentando adicionar um cliente existente como novo
                venda.Cliente = null;

                var novaVenda = await _vendaService.AddAsync(venda);
                Console.WriteLine($"[API-SUCCESS] Pedido salvo com sucesso! Novo ID: {novaVenda.Id}");
                return CreatedAtAction(nameof(Get), new { id = novaVenda.Id }, novaVenda);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[API-ERROR] Erro ao salvar pedido: {ex.Message}");
                if (ex.InnerException != null) Console.WriteLine($"[API-INNER-ERROR] {ex.InnerException.Message}");
                return StatusCode(500, $"Erro ao salvar pedido: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Venda venda)
        {
            if (id != venda.Id) return BadRequest("ID divergente.");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var vendaOriginal = await _vendaService.GetByIdAsync(id);
            if (vendaOriginal == null) return NotFound("Pedido não encontrado.");

            // Atualiza o status e demais campos permitidos pelo Desktop
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
