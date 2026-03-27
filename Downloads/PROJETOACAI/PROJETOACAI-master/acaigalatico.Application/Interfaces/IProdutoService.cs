using System.Collections.Generic;
using System.Threading.Tasks;
using acaigalatico.Application.DTOs;

namespace acaigalatico.Application.Interfaces
{
    public interface IProdutoService
    {
        Task<IEnumerable<ProdutoDTO>> GetProdutosAsync();
        Task AddAsync(ProdutoDTO produtoDto);
        Task<ProdutoDTO?> GetByIdAsync(int? id);
        Task UpdateAsync(ProdutoDTO produtoDto);
        Task RemoveAsync(int id);
    }
}
