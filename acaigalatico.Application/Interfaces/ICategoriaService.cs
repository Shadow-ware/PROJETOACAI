using System.Collections.Generic;
using System.Threading.Tasks;
using acaigalatico.Application.DTOs;

namespace acaigalatico.Application.Interfaces
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaDTO>> GetCategoriasAsync();
        Task<CategoriaDTO> GetByIdAsync(int? id);
        Task AddAsync(CategoriaDTO categoriaDto);
        Task UpdateAsync(CategoriaDTO categoriaDto);
        Task RemoveAsync(int id);
    }
}
