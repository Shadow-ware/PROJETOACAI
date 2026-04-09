using System;
using System.Collections.Generic;
using System.Text;

using acaigalatico.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Diagnostics;

namespace acaigalatico.Infrastructure.Context
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        // Construtor: Passa as opções (como a string de conexão) para a classe base
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        // Aqui registramos as tabelas
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<ItemVenda> ItensVenda { get; set; }
        public DbSet<ItemVendaAdicional> ItensVendaAdicionais { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<SiteContent> SiteContents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplica configurações automáticas (se houverem arquivos de configuração separados)
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            // Exemplo: Garantir que deletar uma Categoria não apague os produtos em cascata sem querer
            // (Isso é segurança de dados)
            modelBuilder.Entity<Produto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Produtos)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração para ItemVenda -> Produto (Evitar Cascata)
            modelBuilder.Entity<ItemVenda>()
                .HasOne(i => i.Produto)
                .WithMany()
                .HasForeignKey(i => i.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração para ItemVendaAdicional -> Produto (Evitar Cascata)
            modelBuilder.Entity<ItemVendaAdicional>()
                .HasOne(i => i.Produto)
                .WithMany()
                .HasForeignKey(i => i.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração para Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.HasIndex(e => e.Email).IsUnique(); // Email deve ser único
                entity.Property(e => e.SenhaHash).IsRequired();
            });
        }
    }
}
