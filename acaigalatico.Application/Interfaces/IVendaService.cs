using acaigalatico.Domain.Entities;

namespace acaigalatico.Application.Interfaces
{
    public interface IVendaService
    {
        Task<IEnumerable<Venda>> GetVendasAsync();
        Task<Venda?> GetByIdAsync(int? id);
        Task AddAsync(Venda venda);
        Task UpdateAsync(Venda venda);
        Task RemoveAsync(int id);
    }
}
