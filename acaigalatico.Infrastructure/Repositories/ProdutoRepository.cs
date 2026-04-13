using System.Collections.Generic;
using System.Threading.Tasks;
using acaigalatico.Domain.Entities;
using acaigalatico.Domain.Interfaces;
using acaigalatico.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace acaigalatico.Infrastructure.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Produto>> GetProdutosAsync()
        {
            return await _context.Produtos.Include(p => p.Categoria).ToListAsync();
        }

        public async Task<Produto> CreateAsync(Produto produto)
        {
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
            return produto;
        }

        public async Task<Produto> GetByIdAsync(int id)
        {
            return await _context.Produtos.Include(p => p.Categoria).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Produto> UpdateAsync(Produto produto)
        {
            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();
            return produto;
        }

        public async Task<Produto> RemoveAsync(Produto produto)
        {
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
            return produto;
        }
    }
}
