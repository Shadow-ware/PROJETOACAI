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

            // 6. Seed Clientes e Vendas (Fictícios)
            var clientes = new List<Cliente>
            {
                new Cliente { Nome = "João Silva", Telefone = "(11) 98888-7777", SaldoDevedor = 15.50m },
                new Cliente { Nome = "Maria Oliveira", Telefone = "(11) 97777-6666", SaldoDevedor = 0.00m },
                new Cliente { Nome = "Pedro Santos", Telefone = "(11) 96666-5555", SaldoDevedor = 45.00m }
            };
            _context.Clientes.AddRange(clientes);
            await _context.SaveChangesAsync();

            var vendas = new List<Venda>
            {
                new Venda { ValorTotal = 25.00m, Status = StatusVenda.Pendente, EnderecoEntrega = "Rua das Flores, 123", BairroEntrega = "Jardins", DataVenda = DateTime.Now.AddHours(-2), ClienteId = clientes[0].Id, FormaPagamento = TipoPagamento.Pix, Observacao = "Troco para 50" },
                new Venda { ValorTotal = 18.50m, Status = StatusVenda.Preparando, EnderecoEntrega = "Av. Paulista, 1000", BairroEntrega = "Bela Vista", DataVenda = DateTime.Now.AddHours(-1), ClienteId = clientes[1].Id, FormaPagamento = TipoPagamento.Dinheiro, Observacao = "Sem campainha" },
                new Venda { ValorTotal = 32.00m, Status = StatusVenda.Entregue, EnderecoEntrega = "Rua Augusta, 500", BairroEntrega = "Consolação", DataVenda = DateTime.Now.AddDays(-1), ClienteId = clientes[2].Id, FormaPagamento = TipoPagamento.Cartao, Observacao = "" }
            };
            _context.Vendas.AddRange(vendas);

            await _context.SaveChangesAsync();

            // 7. Seed Site Content
            if (!_context.SiteContents.Any())
            {
                var contents = new List<SiteContent>
                {
                    // Início
                    new SiteContent { Key = "Home_Hero_Title", Value = "AÇAÍ GALÁCTICO", Page = "Inicio", Type = "Text" },
                    new SiteContent { Key = "Home_Hero_Subtitle", Value = "Explore o universo de sabores do melhor açaí da região!", Page = "Inicio", Type = "Text" },
                    new SiteContent { Key = "Home_Hero_Background", Value = "/images/galaxiaa.jpg", Page = "Inicio", Type = "Image" },
                    new SiteContent { Key = "Home_About_Title", Value = "Quem Somos?", Page = "Inicio", Type = "Text" },
                    new SiteContent { Key = "Home_About_Text", Value = "No Açaí Galáctico, somos uma pequena loja familiar apaixonada por entregar sabor e qualidade.", Page = "Inicio", Type = "Description" },
                    new SiteContent { Key = "Home_About_Image", Value = "/images/entrada-acai.jpeg", Page = "Inicio", Type = "Image" },
                    
                    // Exposição Home
                    new SiteContent { Key = "Home_Expo_Img1", Value = "/images/acai-expo-1.jpeg", Page = "Inicio", Type = "Image" },
                    new SiteContent { Key = "Home_Expo_Title1", Value = "Açaí com Galak", Page = "Inicio", Type = "Text" },
                    new SiteContent { Key = "Home_Expo_Img2", Value = "/images/acai-expo-2.jpeg", Page = "Inicio", Type = "Image" },
                    new SiteContent { Key = "Home_Expo_Title2", Value = "Açaí com morango e Nutella", Page = "Inicio", Type = "Text" },
                    new SiteContent { Key = "Home_Expo_Img3", Value = "/images/acai-expo-3.jpeg", Page = "Inicio", Type = "Image" },
                    new SiteContent { Key = "Home_Expo_Title3", Value = "Açaí com M&M", Page = "Inicio", Type = "Text" },

                    // Cardápio
                    new SiteContent { Key = "Menu_Background_Image", Value = "/images/galaxiaa.jpg", Page = "Cardapio", Type = "Image" },
                    new SiteContent { Key = "Menu_Tradicional_Title", Value = "Tradicional", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Price_300ml", Value = "10.00", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Price_400ml", Value = "12.00", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Price_500ml", Value = "15.00", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Price_700ml", Value = "18.00", Page = "Cardapio", Type = "Text" },
                    
                    new SiteContent { Key = "Menu_Trufado_Title", Value = "Trufado", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Trufado_Subtitle", Value = "(creme de chocolate)", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Trufado_Price_300ml", Value = "13.00", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Trufado_Price_400ml", Value = "15.00", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Trufado_Price_500ml", Value = "17.00", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Trufado_Price_700ml", Value = "22.00", Page = "Cardapio", Type = "Text" },

                    new SiteContent { Key = "Menu_Gourmet_Title", Value = "Gourmet", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Gourmet_Subtitle", Value = "(Nutella, Galak com Negresco)", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Gourmet_Price_300ml", Value = "15.00", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Gourmet_Price_400ml", Value = "17.00", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Gourmet_Price_500ml", Value = "20.00", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Gourmet_Price_700ml", Value = "25.00", Page = "Cardapio", Type = "Text" },

                    // Limites Cardápio
                    new SiteContent { Key = "Menu_Limit_300ml_Fruits", Value = "1", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Limit_300ml_Toppings", Value = "2", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Limit_400ml_Fruits", Value = "1", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Limit_400ml_Toppings", Value = "3", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Limit_500ml_Fruits", Value = "2", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Limit_500ml_Toppings", Value = "4", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Limit_700ml_Fruits", Value = "2", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Limit_700ml_Toppings", Value = "5", Page = "Cardapio", Type = "Text" },

                    // Fazer Pedido
                    new SiteContent { Key = "Order_Side_Title", Value = "Sabor de Outro Mundo", Page = "FazerPedido", Type = "Text" },
                    new SiteContent { Key = "Order_Welcome_Text", Value = "Monte o açaí perfeito para sua viagem galáctica.", Page = "FazerPedido", Type = "Text" },
                    new SiteContent { Key = "Order_Background_Image", Value = "/images/galaxiaa.jpg", Page = "FazerPedido", Type = "Image" },
                    new SiteContent { Key = "Order_Side_Image", Value = "/images/acai-expo-1.jpeg", Page = "FazerPedido", Type = "Image" },
                    new SiteContent { Key = "Order_Delivery_Note", Value = "Prezado cliente, informamos que no momento não dispomos de entregadores próprios. Recomendamos solicitar a retirada via Uber Moto.", Page = "FazerPedido", Type = "Description" },
                    new SiteContent { Key = "Order_Delivery_Link", Value = "https://m.uber.com/", Page = "FazerPedido", Type = "Text" },
                    
                    // Configurações de Pedido (Frutas e Acompanhamentos)
                    new SiteContent { Key = "Order_Fruits", Value = "Banana\nMorango\nManga\nKiwi\nAbacaxi\nUva", Page = "FazerPedido", Type = "Description" },
                    new SiteContent { Key = "Order_Toppings", Value = "Creme de Paçoca|Uma camada deliciosa de creme de amendoim.|/images/logo-sem-escrita.png\nCreme de Pitaya|Sabor exótico e cor vibrante.|/images/logo-sem-escrita.png\nCreme de Leite em Pó|Cremosidade extra para seu açaí.|/images/logo-sem-escrita.png\nLeite em Pó|Clássico indispensável.|/images/logo-sem-escrita.png\nGranola|Crocância saudável.|/images/logo-sem-escrita.png\nPaçoca|O sabor do amendoim.|/images/logo-sem-escrita.png", Page = "FazerPedido", Type = "Description" },

                    new SiteContent { Key = "Contact_Phone", Value = "(11) 98259-9249", Page = "Contato", Type = "Text" },
                    new SiteContent { Key = "Contact_Email", Value = "contato@acaigalatico.com", Page = "Contato", Type = "Text" },
                    new SiteContent { Key = "Contact_Address", Value = "Rua das Estrelas, 123 - Centro", Page = "Contato", Type = "Description" },

                    // Extras Home
                    new SiteContent { Key = "Home_Others_Image", Value = "/images/produtos.jpg", Page = "Inicio", Type = "Image" },
                    new SiteContent { Key = "Home_Others_Title", Value = "BEBIDAS, SORVETES E SALGADOS", Page = "Inicio", Type = "Text" }
                };
                _context.SiteContents.AddRange(contents);
                await _context.SaveChangesAsync();
            }
        }
    }
}
