using acaigalatico.Domain.Entities;

namespace acaigalatico.Domain.Interfaces
{
    public interface IVendaRepository
    {
        Task<IEnumerable<Venda>> GetVendasAsync();
        Task<Venda?> GetByIdAsync(int id);
        Task<Venda> AddAsync(Venda venda);
        Task<Venda> UpdateAsync(Venda venda);
        Task<Venda> RemoveAsync(Venda venda);
    }
}
