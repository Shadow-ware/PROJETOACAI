using System;
using System.Collections.Generic;
using System.Text;

namespace acaigalatico.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty; // inserindo string.Empty para evitar null reference exceptions, garantindo que o nome seja sempre inicializado 

        public string Email { get; set; } = string.Empty; // Armazena o email do usuário, que deve ser único 

        public string SenhaHash { get; set; } = string.Empty;// Armazena o hash da senha para segurança 

        public string? FotoPerfil { get; set; } // Armazena o caminho ou URL da foto de perfil do usuário, pode ser nulo se o usuário não tiver uma foto 

        public string? Telefone { get; set; }

        public string? Endereco { get; set; }


    }
}
