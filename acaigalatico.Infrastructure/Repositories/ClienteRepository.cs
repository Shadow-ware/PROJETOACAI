using acaigalatico.Domain.Entities;
using acaigalatico.Domain.Interfaces;
using acaigalatico.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace acaigalatico.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _context;

        public ClienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> GetClientesAsync()
        {
            return await _context.Clientes.ToListAsync();
        }

        public async Task<Cliente?> GetByIdAsync(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        public async Task<Cliente> AddAsync(Cliente cliente)
        {
            _context.Add(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task<Cliente> UpdateAsync(Cliente cliente)
        {
            _context.Update(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task<Cliente> RemoveAsync(Cliente cliente)
        {
            _context.Remove(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }
    }
}
