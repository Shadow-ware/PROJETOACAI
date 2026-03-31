using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using acaigalatico.Domain.Entities;
using acaigalatico.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;

namespace acaigalatico.Infrastructure
{
    public class SeedingService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedingService(AppDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            // 0. Seed Roles
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // 1. Seed Admin User
            var adminUser = await _userManager.FindByNameAsync("Adminacai");
            if (adminUser == null)
            {
                var user = new IdentityUser
                {
                    UserName = "Adminacai",
                    Email = "admin@acaigalatico.com",
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, "NKL0029*");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }
            //opa
            // Kk
            else
            {
                if (!adminUser.EmailConfirmed)
                {
                    adminUser.EmailConfirmed = true;
                    await _userManager.UpdateAsync(adminUser);
                }

                if (!await _userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // 1.1 Sincroniza Admin na tabela personalizada Usuarios
            if (!_context.Usuarios.Any(u => u.Email == "admin@acaigalatico.com"))
            {
                _context.Usuarios.Add(new Usuario
                {
                    Nome = "Administrador Açaí",
                    Email = "admin@acaigalatico.com",
                    SenhaHash = "********",
                    FotoPerfil = "/images/admin-profile.png"
                });
                await _context.SaveChangesAsync();
            }

            // 2. Se já tem dados, não faz nada (para não duplicar produtos/categorias)
            if (_context.Produtos.Any() || _context.Categorias.Any())
                return;

            // 3. Criar as Categorias
            var catAcompanhamentos = new Categoria { Nome = "Acompanhamentos" };
            var catBebidas = new Categoria { Nome = "Bebidas" };
            var catSalgadinhos = new Categoria { Nome = "Salgadinhos" };
            var catInsumos = new Categoria { Nome = "Uso Interno / Insumos" }; // Luvas, Toucas, etc.

            // 4. Criar os Produtos (Agora TODOS têm Descrição e ImagemUrl padrão)
            var produtos = new List<Produto>
            {
                // --- ACOMPANHAMENTOS ---
                new Produto { Nome = "Canudinho de Sorvete", Descricao = "Unidade avulsa", PrecoCusto = 0.20m, PrecoVenda = 0.50m, EstoqueAtual = 200, Categoria = catAcompanhamentos, EhParaVenda = true, ImagemUrl = "/images/default-acai.png" },
                new Produto { Nome = "Ovomaltine 700g", Descricao = "Pote para uso interno", PrecoCusto = 35.90m, PrecoVenda = null, EstoqueAtual = 1, Categoria = catAcompanhamentos, EhParaVenda = false, ImagemUrl = "/images/default-acai.png" },
                new Produto { Nome = "Leite em pó (Aurora/Italac) 400g", Descricao = "Pacote de 400g", PrecoCusto = 20.00m, EstoqueAtual = 5, Categoria = catAcompanhamentos, EhParaVenda = false, ImagemUrl = "/images/default-acai.png" },
                new Produto { Nome = "Granulado (Dori/Dona Jura) 70g", Descricao = "Pacote de 70g", PrecoCusto = 4.00m, EstoqueAtual = 10, Categoria = catAcompanhamentos, EhParaVenda = false, ImagemUrl = "/images/default-acai.png" },
                new Produto { Nome = "Leite Condensado Mococa", Descricao = "Caixinha 395g", PrecoCusto = 4.00m, EstoqueAtual = 20, Categoria = catAcompanhamentos, EhParaVenda = false, ImagemUrl = "/images/default-acai.png" },

                // --- BEBIDAS (Revenda) ---
                new Produto { Nome = "Coca Cola 2L", Descricao = "Garrafa PET 2 Litros", PrecoCusto = 10.00m, PrecoVenda = 13.00m, EstoqueAtual = 10, Categoria = catBebidas, EhParaVenda = true, ImagemUrl = "/images/default-acai.png" },
                new Produto { Nome = "Guaraná Antártica 2L", Descricao = "Garrafa PET 2 Litros", PrecoCusto = 10.00m, PrecoVenda = 11.00m, EstoqueAtual = 10, Categoria = catBebidas, EhParaVenda = true, ImagemUrl = "/images/default-acai.png" },
                new Produto { Nome = "Refrigerante 600ml", Descricao = "Coca, Convenção ou Sukita", PrecoCusto = 5.00m, PrecoVenda = 8.00m, EstoqueAtual = 24, Categoria = catBebidas, EhParaVenda = true, ImagemUrl = "/images/default-acai.png" },
                
                // Conversão de Fardo para Unidade
                new Produto { Nome = "Coca Cola Caçulinha", Descricao = "Unidade Pequena", PrecoCusto = 1.83m, PrecoVenda = 3.00m, EstoqueAtual = 24, Categoria = catBebidas, EhParaVenda = true, ImagemUrl = "/images/default-acai.png" },

                new Produto { Nome = "Coca Cola Lata 350ml", Descricao = "Lata de alumínio", PrecoCusto = 3.08m, PrecoVenda = 5.00m, EstoqueAtual = 24, Categoria = catBebidas, EhParaVenda = true, ImagemUrl = "/images/default-acai.png" },

                new Produto { Nome = "Suco Frupic", Descricao = "Sabores Variados", PrecoCusto = 2.00m, PrecoVenda = 4.00m, EstoqueAtual = 30, Categoria = catBebidas, EhParaVenda = true, ImagemUrl = "/images/default-acai.png" },

                new Produto { Nome = "Gatorade", Descricao = "Isotônico 500ml", PrecoCusto = 5.00m, PrecoVenda = 7.00m, EstoqueAtual = 15, Categoria = catBebidas, EhParaVenda = true, ImagemUrl = "/images/default-acai.png" },

                // --- SALGADINHOS ---
                new Produto { Nome = "Salgadinho Fofura", Descricao = "Pacote Pequeno", PrecoCusto = 1.80m, PrecoVenda = 3.00m, EstoqueAtual = 20, Categoria = catSalgadinhos, EhParaVenda = true, ImagemUrl = "/images/default-acai.png" },

                new Produto { Nome = "Salgadinho Lobits", Descricao = "Pacote Pequeno", PrecoCusto = 1.30m, PrecoVenda = 2.00m, EstoqueAtual = 20, Categoria = catSalgadinhos, EhParaVenda = true, ImagemUrl = "/images/default-acai.png" },

                // --- USO INTERNO (Insumos) ---
                new Produto { Nome = "Colheres Descartáveis", Descricao = "Pacote 500un", PrecoCusto = 42.00m, EstoqueAtual = 1, Categoria = catInsumos, EhParaVenda = false, ImagemUrl = "/images/default-acai.png" },
                new Produto { Nome = "Luvas Tam M", Descricao = "Caixa 100un", PrecoCusto = 16.50m, EstoqueAtual = 1, Categoria = catInsumos, EhParaVenda = false, ImagemUrl = "/images/default-acai.png" },
                new Produto { Nome = "Toucas Descartáveis", Descricao = "Pacote 100un", PrecoCusto = 16.99m, EstoqueAtual = 1, Categoria = catInsumos, EhParaVenda = false, ImagemUrl = "/images/default-acai.png" }
            };

            // 5. Salvar tudo no banco
            _context.Categorias.AddRange(catAcompanhamentos, catBebidas, catSalgadinhos, catInsumos);
            _context.Produtos.AddRange(produtos);

            await _context.SaveChangesAsync();

            // 7. Seed Site Content
            if (!_context.SiteContents.Any())
            {
                var contents = new List<SiteContent>
                {
                    // Página Inicial
                    new SiteContent { Key = "Home_Hero_Title", Value = "O Melhor Açaí da Galáxia!", Page = "Inicio", Type = "Text" },
                    new SiteContent { Key = "Home_Hero_Subtitle", Value = "Sabor refrescante e energia pura para o seu dia.", Page = "Inicio", Type = "Text" },
                    new SiteContent { Key = "Home_Hero_Image", Value = "/images/hero-acai.png", Page = "Inicio", Type = "Image" },
                    new SiteContent { Key = "Home_About_Text", Value = "Nascemos com a missão de levar o sabor autêntico do açaí para todos os cantos, com ingredientes selecionados e muito carinho.", Page = "Inicio", Type = "Description" },

                    // Cardápio
                    new SiteContent { Key = "Menu_Title", Value = "Nosso Cardápio", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Subtitle", Value = "Escolha suas combinações favoritas.", Page = "Cardapio", Type = "Text" },

                    // Contato
                    new SiteContent { Key = "Contact_Phone", Value = "(11) 99999-9999", Page = "Contato", Type = "Text" },
                    new SiteContent { Key = "Contact_Email", Value = "contato@acaigalatico.com", Page = "Contato", Type = "Text" },
                    new SiteContent { Key = "Contact_Address", Value = "Rua Galáctica, 123 - Centro", Page = "Contato", Type = "Text" }
                };
                _context.SiteContents.AddRange(contents);
                await _context.SaveChangesAsync();
            }
        }
    }
}
