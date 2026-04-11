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
            return await _context.Vendas.Include(v => v.Cliente).ToListAsync();
        }

        public async Task<Venda?> GetByIdAsync(int id)
        {
            return await _context.Vendas.Include(v => v.Cliente).FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Venda> AddAsync(Venda venda)
        {
            _context.Add(venda);
            await _context.SaveChangesAsync();
            return venda;
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
