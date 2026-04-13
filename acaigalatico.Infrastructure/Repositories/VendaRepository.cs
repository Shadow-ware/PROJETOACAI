using acaigalatico.Domain.Entities;
using acaigalatico.Domain.Interfaces;
using acaigalatico.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace acaigalatico.Infrastructure.Repositories
{
    public class VendaRepository : IVendaRepository
    {
        private readonly AppDbContext _context;

        public VendaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Venda>> GetVendasAsync()
        {
            return await _context.Vendas
                .Include(v => v.Cliente)
                .Include(v => v.Itens)
                    .ThenInclude(i => i.Produto)
                .ToListAsync();
        }

        public async Task<Venda?> GetByIdAsync(int id)
        {
            return await _context.Vendas
                .Include(v => v.Cliente)
                .Include(v => v.Itens)
                    .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Venda> AddAsync(Venda venda)
        {
            try 
            {
                // Garante que o contexto ignore qualquer ID de navegação que possa vir do Web
                if (venda.Cliente != null) _context.Entry(venda.Cliente).State = EntityState.Unchanged;
                
                if (venda.Itens != null)
                {
                    foreach (var item in venda.Itens)
                    {
                        if (item.Produto != null) _context.Entry(item.Produto).State = EntityState.Unchanged;
                    }
                }

                _context.Vendas.Add(venda);
                await _context.SaveChangesAsync();
                return venda;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB-ERROR] Erro ao salvar venda: {ex.Message}");
                throw;
            }
        }

        public async Task<Venda> UpdateAsync(Venda venda)
        {
            _context.Update(venda);
            await _context.SaveChangesAsync();
            return venda;
        }

        public async Task<Venda> RemoveAsync(Venda venda)
        {
            _context.Remove(venda);
            await _context.SaveChangesAsync();
            return venda;
        }
    }
}
