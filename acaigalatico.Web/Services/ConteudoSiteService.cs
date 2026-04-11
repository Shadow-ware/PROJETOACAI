using acaigalatico.Application.Interfaces;
using acaigalatico.Domain.Entities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System;
using System.IO;

namespace acaigalatico.Web.Services
{
    public class ConteudoSiteService : IConteudoSiteService
    {
        private readonly IWebHostEnvironment _environment;

        public ConteudoSiteService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        // Armazenamento em memória (estático para persistir entre requisições enquanto o app roda)
        private static ConteudoInicio _conteudoInicio = new ConteudoInicio
        {
            ItensExposicao = new List<ItemExposicao>
            {
                new ItemExposicao { Nome = "Açaí com Galak", ImagemUrl = "/images/acai-expo-1.jpeg" },
                new ItemExposicao { Nome = "Açaí com morango e Nutella", ImagemUrl = "/images/acai-expo-2.jpeg" },
                new ItemExposicao { Nome = "Açaí com M&M", ImagemUrl = "/images/acai-expo-3.jpeg" }
            }
        };

        private static ConteudoCardapio _conteudoCardapio = new ConteudoCardapio();

        private static List<ItemCardapio> _cardapio = new List<ItemCardapio>
        {
            new ItemCardapio { Nome = "Açaí Tradicional", Descricao = "Açaí puro com granola e banana.", Preco = 15.00m, ImagemUrl = "/images/produtos/default.png" },
            new ItemCardapio { Nome = "Açaí Galáctico", Descricao = "Açaí com leite em pó, leite condensado e morango.", Preco = 22.00m, ImagemUrl = "/images/produtos/default.png" }
        };

        public ConteudoInicio GetInicio()
        {
            return _conteudoInicio;
        }

        public void UpdateInicio(ConteudoInicio conteudo)
        {
            _conteudoInicio = conteudo;
        }

        public ConteudoCardapio GetCardapioConteudo() => _conteudoCardapio;
        public void UpdateCardapioConteudo(ConteudoCardapio conteudo) => _conteudoCardapio = conteudo;

        // Métodos Cardápio
        public List<ItemCardapio> GetCardapio() => _cardapio;

        public ItemCardapio? GetItemCardapioById(Guid id) => _cardapio.FirstOrDefault(i => i.Id == id);

        public void AddItemCardapio(ItemCardapio item)
        {
            _cardapio.Add(item);
        }

        public void UpdateItemCardapio(ItemCardapio item)
        {
            var index = _cardapio.FindIndex(i => i.Id == item.Id);
            if (index != -1)
            {
                _cardapio[index] = item;
            }
        }

        public void DeleteItemCardapio(Guid id)
        {
            var item = _cardapio.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                _cardapio.Remove(item);
            }
        }

        public async Task<string> SaveImageAsync(IFormFile file)
        {
            var uploadDir = Path.Combine(_environment.WebRootPath, "uploads", "site");
            if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return "/uploads/site/" + fileName;
        }

        public Task SeedInitialDataAsync()
        {
            // Já inicializado no campo estático
            return Task.CompletedTask;
        }
    }
}
