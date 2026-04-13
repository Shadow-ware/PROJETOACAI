using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using acaigalatico.Application.DTOs;
using acaigalatico.Application.Interfaces;
using acaigalatico.Domain.Entities;
using acaigalatico.Domain.Interfaces;

namespace acaigalatico.Application.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<IEnumerable<ProdutoDTO>> GetProdutosAsync()
        {
            var produtos = await _produtoRepository.GetProdutosAsync();
            
            return produtos.Select(p => new ProdutoDTO
            {
                Id = p.Id,
                Nome = p.Nome,
                Descricao = p.Descricao,
                PrecoCusto = p.PrecoCusto,
                PrecoVenda = p.PrecoVenda,
                CategoriaId = p.CategoriaId,
                CategoriaNome = p.Categoria?.Nome,
                EstoqueAtual = p.EstoqueAtual,
                EstoqueMinimo = p.EstoqueMinimo,
                Ativo = p.Ativo,
                ImagemUrl = p.ImagemUrl
            });
        }

        public async Task AddAsync(ProdutoDTO produtoDto)
        {
            var produto = new Produto
            {
                Nome = produtoDto.Nome,
                Descricao = produtoDto.Descricao ?? "",
                PrecoCusto = produtoDto.PrecoCusto,
                PrecoVenda = produtoDto.PrecoVenda,
                CategoriaId = produtoDto.CategoriaId,
                EstoqueAtual = produtoDto.EstoqueAtual,
                EstoqueMinimo = produtoDto.EstoqueMinimo,
                Ativo = produtoDto.Ativo,
                EhParaVenda = true,
                ImagemUrl = string.IsNullOrEmpty(produtoDto.ImagemUrl) ? "/images/default-acai.png" : produtoDto.ImagemUrl
            };

            await _produtoRepository.CreateAsync(produto);
        }

        public async Task<ProdutoDTO?> GetByIdAsync(int? id)
        {
            if (id == null) return null;
            var p = await _produtoRepository.GetByIdAsync(id.Value);
            if (p == null) return null;
            return new ProdutoDTO
            {
                Id = p.Id,
                Nome = p.Nome,
                Descricao = p.Descricao,
                PrecoCusto = p.PrecoCusto,
                PrecoVenda = p.PrecoVenda,
                CategoriaId = p.CategoriaId,
                CategoriaNome = p.Categoria?.Nome,
                EstoqueAtual = p.EstoqueAtual,
                EstoqueMinimo = p.EstoqueMinimo,
                Ativo = p.Ativo,
                ImagemUrl = p.ImagemUrl
            };
        }

        public async Task UpdateAsync(ProdutoDTO produtoDto)
        {
            var existing = await _produtoRepository.GetByIdAsync(produtoDto.Id);
            if (existing == null) return;
            
            existing.Nome = produtoDto.Nome;
            existing.Descricao = produtoDto.Descricao ?? "";
            existing.PrecoCusto = produtoDto.PrecoCusto;
            existing.PrecoVenda = produtoDto.PrecoVenda;
            existing.CategoriaId = produtoDto.CategoriaId;
            existing.EstoqueAtual = produtoDto.EstoqueAtual;
            existing.EstoqueMinimo = produtoDto.EstoqueMinimo;
            existing.Ativo = produtoDto.Ativo;
            existing.ImagemUrl = string.IsNullOrEmpty(produtoDto.ImagemUrl) ? existing.ImagemUrl : produtoDto.ImagemUrl;
            
            await _produtoRepository.UpdateAsync(existing);
        }

        public async Task RemoveAsync(int id)
        {
            var produto = await _produtoRepository.GetByIdAsync(id);
            if (produto != null)
            {
                await _produtoRepository.RemoveAsync(produto);
            }
        }
    }
}
