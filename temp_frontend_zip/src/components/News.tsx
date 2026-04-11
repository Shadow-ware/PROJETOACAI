import { Calendar, TrendingUp } from 'lucide-react';

export function News() {
  const newsItems = [
    {
      id: 1,
      date: '28 Nov 2025',
      title: 'Novo Sabor: Açaí com Creme de Avelã Intergalático! 🌰',
      content: 'Chegou a novidade que todos esperavam! Nosso novo creme de avelã especial feito com ingredientes premium. Venha experimentar essa explosão de sabor!',
      highlight: true,
    },
    {
      id: 2,
      date: '15 Nov 2025',
      title: 'Promoção Black Friday Galáctica! 🎉',
      content: 'De 24 a 30 de novembro: Leve 2 açaís tamanho M e ganhe 20% de desconto! Use o cupom GALAXIA20 no delivery.',
      highlight: true,
    },
    {
      id: 3,
      date: '10 Nov 2025',
      title: 'Novos Horários aos Sábados',
      content: 'Agora abrimos mais cedo aos sábados! A partir deste mês, funcionamos das 10h às 23h. Mais tempo para você curtir o melhor açaí da galáxia!',
      highlight: false,
    },
    {
      id: 4,
      date: '05 Nov 2025',
      title: 'Parceria com App de Delivery',
      content: 'Agora você também pode pedir seu Açaí Galático pelo iFood e Rappi! Mais comodidade para matar aquela vontade de açaí.',
      highlight: false,
    },
    {
      id: 5,
      date: '01 Nov 2025',
      title: 'Programa de Fidelidade Espacial ⭐',
      content: 'A cada 5 açaís, ganhe 1 grátis! Peça seu cartão fidelidade na loja e acumule pontos estelares.',
      highlight: false,
    },
    {
      id: 6,
      date: '25 Out 2025',
      title: 'Linha de Smoothies Chegou!',
      content: 'Experimente nossos novos smoothies de frutas tropicais: Manga Estelar, Morango Cósmico e Abacaxi Lunar. Refrescância intergaláctica!',
      highlight: false,
    },
  ];

  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
      <div className="text-center mb-12">
        <h1 className="bg-gradient-to-r from-purple-300 via-pink-300 to-blue-300 bg-clip-text text-transparent mb-4 drop-shadow-lg">
          Novidades & Promoções
        </h1>
        <p className="text-purple-200">
          Fique por dentro de tudo que está acontecendo no universo Açaí Galático! 🚀
        </p>
      </div>

      <div className="grid lg:grid-cols-3 gap-6 mb-12">
        {/* Destaques */}
        <div className="lg:col-span-2 space-y-6">
          {newsItems
            .filter(item => item.highlight)
            .map((item) => (
              <article 
                key={item.id}
                className="bg-gradient-to-br from-purple-800/50 to-pink-800/50 backdrop-blur-md rounded-2xl p-8 border-2 border-purple-400/30 hover:border-purple-400/50 transition-all relative overflow-hidden shadow-xl shadow-purple-500/30"
              >
                <div className="absolute top-0 right-0 bg-pink-500 text-white px-4 py-1 rounded-bl-xl flex items-center gap-1 shadow-lg">
                  <TrendingUp className="w-4 h-4" />
                  <span>Destaque</span>
                </div>
                <div className="flex items-center gap-2 text-purple-300 mb-3">
                  <Calendar className="w-4 h-4" />
                  <time>{item.date}</time>
                </div>
                <h2 className="text-purple-100 mb-4">{item.title}</h2>
                <p className="text-purple-200 leading-relaxed">{item.content}</p>
              </article>
            ))}
        </div>

        {/* Sidebar - Outras Notícias */}
        <div className="space-y-4">
          <h2 className="text-purple-200">Outras Notícias</h2>
          {newsItems
            .filter(item => !item.highlight)
            .map((item) => (
              <article 
                key={item.id}
                className="bg-purple-900/40 backdrop-blur-md rounded-xl p-4 border-2 border-purple-500/30 hover:border-purple-400/40 transition-all shadow-lg shadow-purple-500/20"
              >
                <div className="flex items-center gap-2 text-purple-300 mb-2">
                  <Calendar className="w-3 h-3" />
                  <time className="text-sm">{item.date}</time>
                </div>
                <h3 className="text-purple-100 mb-2">{item.title}</h3>
                <p className="text-purple-300">{item.content}</p>
              </article>
            ))}
        </div>
      </div>

      {/* Newsletter Section */}
      <div className="bg-gradient-to-r from-purple-600/30 via-pink-600/30 to-purple-600/30 backdrop-blur-md rounded-2xl p-8 border-2 border-purple-400/40 shadow-2xl shadow-purple-500/30">
        <div className="text-center max-w-2xl mx-auto">
          <h2 className="text-purple-100 mb-3">📬 Receba Nossas Novidades</h2>
          <p className="text-purple-200 mb-6">
            Cadastre-se e seja o primeiro a saber sobre promoções, novos sabores e eventos especiais!
          </p>
          <div className="flex gap-3 max-w-md mx-auto">
            <input 
              type="email"
              placeholder="Seu e-mail"
              className="flex-1 px-4 py-3 rounded-lg bg-purple-900/50 border-2 border-purple-500/40 text-white placeholder-purple-300 focus:outline-none focus:border-purple-400 backdrop-blur-sm"
            />
            <button className="px-6 py-3 bg-purple-600/80 hover:bg-purple-500 rounded-lg transition-all shadow-lg shadow-purple-500/40 backdrop-blur-sm border border-purple-400/30">
              Inscrever
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}