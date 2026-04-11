using acaigalatico.Application.Interfaces;
using acaigalatico.Domain.Entities;
using acaigalatico.Domain.Interfaces;

namespace acaigalatico.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<IEnumerable<Cliente>> GetClientesAsync()
        {
            return await _clienteRepository.GetClientesAsync();
        }

        public async Task<Cliente?> GetByIdAsync(int? id)
        {
            if (id == null) return null;
            return await _clienteRepository.GetByIdAsync(id.Value);
        }

        public async Task AddAsync(Cliente cliente)
        {
            await _clienteRepository.AddAsync(cliente);
        }

        public async Task UpdateAsync(Cliente cliente)
        {
            await _clienteRepository.UpdateAsync(cliente);
        }

        public async Task RemoveAsync(int id)
        {
            var cliente = await _clienteRepository.GetByIdAsync(id);
            if (cliente != null)
            {
                await _clienteRepository.RemoveAsync(cliente);
            }
        }
    }
}
