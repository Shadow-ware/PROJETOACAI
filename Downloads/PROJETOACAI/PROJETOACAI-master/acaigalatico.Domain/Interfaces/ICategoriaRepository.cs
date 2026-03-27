using System.Collections.Generic;
using System.Threading.Tasks;
using acaigalatico.Domain.Entities;

namespace acaigalatico.Domain.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<IEnumerable<Categoria>> GetCategoriasAsync();
        Task<Categoria?> GetByIdAsync(int id);
        Task<Categoria> AddAsync(Categoria categoria);
        Task<Categoria> UpdateAsync(Categoria categoria);
        Task<Categoria> RemoveAsync(Categoria categoria);
    }
}
