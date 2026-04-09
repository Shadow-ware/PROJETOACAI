using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using acaigalatico.Application.DTOs;
using acaigalatico.Application.Interfaces;
using acaigalatico.Domain.Interfaces;

namespace acaigalatico.Application.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<IEnumerable<CategoriaDTO>> GetCategoriasAsync()
        {
            var categoriasEntity = await _categoriaRepository.GetCategoriasAsync();
            return categoriasEntity.Select(c => new CategoriaDTO
            {
                Id = c.Id,
                Nome = c.Nome
            });
        }

        public async Task<CategoriaDTO> GetByIdAsync(int? id)
        {
            if (id == null) return null;
            var categoriaEntity = await _categoriaRepository.GetByIdAsync(id.Value);
            if (categoriaEntity == null) return null;
            return new CategoriaDTO
            {
                Id = categoriaEntity.Id,
                Nome = categoriaEntity.Nome
            };
        }

        public async Task AddAsync(CategoriaDTO categoriaDto)
        {
            var categoriaEntity = new acaigalatico.Domain.Entities.Categoria { Nome = categoriaDto.Nome };
            await _categoriaRepository.AddAsync(categoriaEntity);
        }

        public async Task UpdateAsync(CategoriaDTO categoriaDto)
        {
            var categoriaEntity = await _categoriaRepository.GetByIdAsync(categoriaDto.Id);
            if (categoriaEntity != null)
            {
                categoriaEntity.Nome = categoriaDto.Nome;
                await _categoriaRepository.UpdateAsync(categoriaEntity);
            }
        }

        public async Task RemoveAsync(int id)
        {
            var categoriaEntity = await _categoriaRepository.GetByIdAsync(id);
            if (categoriaEntity != null)
            {
                await _categoriaRepository.RemoveAsync(categoriaEntity);
            }
        }
    }
}
