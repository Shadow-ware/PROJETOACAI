using acaigalatico.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace acaigalatico.Application.Interfaces
{
    public interface IConteudoSiteService
    {
        ConteudoInicio GetInicio();
        void UpdateInicio(ConteudoInicio conteudo);
        
        ConteudoCardapio GetCardapioConteudo();
        void UpdateCardapioConteudo(ConteudoCardapio conteudo);

        // Métodos Cardápio
        List<ItemCardapio> GetCardapio();
        void AddItemCardapio(ItemCardapio item);
        void UpdateItemCardapio(ItemCardapio item);
        void DeleteItemCardapio(Guid id);
        ItemCardapio? GetItemCardapioById(Guid id);

        Task<string> SaveImageAsync(IFormFile file);
        Task SeedInitialDataAsync();
    }
}
