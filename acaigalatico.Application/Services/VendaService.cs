using acaigalatico.Application.Interfaces;
using acaigalatico.Domain.Entities;
using acaigalatico.Domain.Interfaces;

namespace acaigalatico.Application.Services
{
    public class VendaService : IVendaService
    {
        private readonly IVendaRepository _vendaRepository;

        public VendaService(IVendaRepository vendaRepository)
        {
            _vendaRepository = vendaRepository;
        }

        public async Task<IEnumerable<Venda>> GetVendasAsync()
        {
            return await _vendaRepository.GetVendasAsync();
        }

        public async Task<Venda?> GetByIdAsync(int? id)
        {
            if (id == null) return null;
            return await _vendaRepository.GetByIdAsync(id.Value);
        }

        public async Task AddAsync(Venda venda)
        {
            await _vendaRepository.AddAsync(venda);
        }

        public async Task UpdateAsync(Venda venda)
        {
            await _vendaRepository.UpdateAsync(venda);
        }

        public async Task RemoveAsync(int id)
        {
            var venda = await _vendaRepository.GetByIdAsync(id);
            if (venda != null)
            {
                await _vendaRepository.RemoveAsync(venda);
            }
        }
    }
}
