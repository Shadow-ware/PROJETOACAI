using System.Collections.Generic;

namespace acaigalatico.Domain.Entities
{
    public class ConteudoInicio
    {
        // Seção HERO
        public string Titulo { get; set; } = "AÇAÍ GALÁCTICO";
        public string Subtitulo { get; set; } = "Explore o universo de sabores!";
        public string ImagemUrl { get; set; } = "/images/galaxiaa.jpg";

        // Seção QUEM SOMOS
        public string SobreTitulo { get; set; } = "Quem Somos?";
        public string SobreDescricao { get; set; } = "No Açaí Galáctico, somos uma pequena loja familiar apaixonada por entregar sabor e qualidade.";
        public string SobreImagemUrl { get; set; } = "/images/entrada-acai.jpeg";
        
        // Features (Destaques de Serviço)
        public string Feature1 { get; set; } = "Produtos Fresquinhos";
        public string Feature2 { get; set; } = "Entrega na Velocidade da Luz";
        public string Feature3 { get; set; } = "Feito com Amor";

        // Seção EXPOSIÇÃO
        public string ExpoTitulo { get; set; } = "Exposição dos nossos Açaís";
        public string ExpoSubtitulo { get; set; } = "Confira o que acabou de chegar do espaço sideral para sua tigela!";
        public List<ItemExposicao> ItensExposicao { get; set; } = new();

        // Seção PRODUTOS ALÉM DE AÇAÍ
        public string OutrosTitulo { get; set; } = "Produtos além de Açaí";
        public string OutrosImagemUrl { get; set; } = "/images/produtos.jpg";
        public string OutrosTextoDestaque { get; set; } = "BEBIDAS, SORVETES E SALGADOS";

        // Seção WHATSAPP
        public string WhatsAppTexto { get; set; } = "CHAMAR NO WHATSAPP";
        public string WhatsAppSubtitulo { get; set; } = "Dúvidas? Fale com a gente!";
        public string WhatsAppLink { get; set; } = "https://wa.me/5511982599249";
    }

    public class ConteudoCardapio
    {
        public string BackgroundImage { get; set; } = "/images/galaxiaa.jpg";

        // Tradicional
        public string TradicionalTitulo { get; set; } = "Tradicional";
        public string TradicionalPreco300ml { get; set; } = "10.00";
        public string TradicionalPreco400ml { get; set; } = "12.00";
        public string TradicionalPreco500ml { get; set; } = "15.00";
        public string TradicionalPreco700ml { get; set; } = "18.00";

        // Trufado
        public string TrufadoTitulo { get; set; } = "Trufado";
        public string TrufadoSubtitulo { get; set; } = "(creme de chocolate)";
        public string TrufadoPreco300ml { get; set; } = "13.00";
        public string TrufadoPreco400ml { get; set; } = "15.00";
        public string TrufadoPreco500ml { get; set; } = "17.00";
        public string TrufadoPreco700ml { get; set; } = "22.00";

        // Gourmet
        public string GourmetTitulo { get; set; } = "Gourmet";
        public string GourmetSubtitulo { get; set; } = "(Nutella, Galak com Negresco)";
        public string GourmetPreco300ml { get; set; } = "15.00";
        public string GourmetPreco400ml { get; set; } = "17.00";
        public string GourmetPreco500ml { get; set; } = "20.00";
        public string GourmetPreco700ml { get; set; } = "25.00";

        // Monte Seu Açaí
        public string MonteSeuAcaiTitulo { get; set; } = "MONTE SEU AÇAÍ";
        public string Regras300ml { get; set; } = "1 fruta e 2 acompanhamentos.";
        public string Regras400ml { get; set; } = "1 fruta e 3 acompanhamentos.";
        public string Regras500ml { get; set; } = "2 frutas e 4 acompanhamentos.";
        public string Regras700ml { get; set; } = "2 frutas e 5 acompanhamentos.";
        public string AdicionaisExtrasTexto { get; set; } = "ADICIONAIS EXTRAS COBRADOS À PARTE";
    }

    public class ItemExposicao
    {
        public string Nome { get; set; } = string.Empty;
        public string ImagemUrl { get; set; } = string.Empty;
    }

    public class ItemCardapio
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public string ImagemUrl { get; set; } = string.Empty;
    }
}
