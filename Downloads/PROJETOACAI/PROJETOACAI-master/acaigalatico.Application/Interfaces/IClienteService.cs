using acaigalatico.Domain.Entities;

namespace acaigalatico.Application.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> GetClientesAsync();
        Task<Cliente?> GetByIdAsync(int? id);
        Task AddAsync(Cliente cliente);
        Task UpdateAsync(Cliente cliente);
        Task RemoveAsync(int id);
    }
}
