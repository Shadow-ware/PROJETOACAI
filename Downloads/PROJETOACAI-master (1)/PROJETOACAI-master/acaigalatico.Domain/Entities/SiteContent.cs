using System;
using System.ComponentModel.DataAnnotations;

namespace acaigalatico.Domain.Entities
{
    public class SiteContent
    {
        [Key]
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Page { get; set; } = string.Empty; // Inicio, Cardapio, etc.
        public string Type { get; set; } = "Text"; // Text, Image, Description
    }
}
