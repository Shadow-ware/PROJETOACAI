import { Plus } from 'lucide-react';

export function Menu() {
  const sizes = [
    { size: 'P (300ml)', price: 'R$ 12,00', description: 'Ideal para um lanche rápido' },
    { size: 'M (500ml)', price: 'R$ 18,00', description: 'O mais pedido!' },
    { size: 'G (700ml)', price: 'R$ 24,00', description: 'Para quem tem fome de estrelas' },
    { size: 'GG (1L)', price: 'R$ 32,00', description: 'Experiência galáctica completa' },
  ];

  const toppings = [
    { name: 'Granola', price: 'Grátis', category: 'Inclusos' },
    { name: 'Leite em Pó', price: 'Grátis', category: 'Inclusos' },
    { name: 'Banana', price: '+ R$ 2,00', category: 'Frutas' },
    { name: 'Morango', price: '+ R$ 3,00', category: 'Frutas' },
    { name: 'Kiwi', price: '+ R$ 3,00', category: 'Frutas' },
    { name: 'Manga', price: '+ R$ 2,50', category: 'Frutas' },
    { name: 'Leite Condensado', price: '+ R$ 2,00', category: 'Extras' },
    { name: 'Nutella', price: '+ R$ 4,00', category: 'Extras' },
    { name: 'Paçoca', price: '+ R$ 2,50', category: 'Extras' },
    { name: 'Amendoim', price: '+ R$ 2,00', category: 'Extras' },
    { name: 'Chocolate Granulado', price: '+ R$ 2,00', category: 'Extras' },
    { name: 'Confete', price: '+ R$ 2,00', category: 'Extras' },
    { name: 'M&Ms', price: '+ R$ 3,50', category: 'Extras' },
    { name: 'Kit Kat', price: '+ R$ 3,50', category: 'Extras' },
    { name: 'Oreo', price: '+ R$ 3,00', category: 'Extras' },
  ];

  const specialCombos = [
    { 
      name: 'Combo Nebulosa', 
      price: 'R$ 28,00',
      description: 'Açaí G + Morango + Nutella + Granola',
      popular: true
    },
    { 
      name: 'Combo Supernova', 
      price: 'R$ 26,00',
      description: 'Açaí G + Banana + Kit Kat + Leite Condensado',
      popular: true
    },
    { 
      name: 'Combo Kids Espacial', 
      price: 'R$ 15,00',
      description: 'Açaí P + Confete + Chocolate Granulado',
      popular: false
    },
    { 
      name: 'Combo Fit Galáctica', 
      price: 'R$ 22,00',
      description: 'Açaí M + Frutas variadas + Granola',
      popular: false
    },
  ];

  const groupedToppings = toppings.reduce((acc, topping) => {
    if (!acc[topping.category]) {
      acc[topping.category] = [];
    }
    acc[topping.category].push(topping);
    return acc;
  }, {} as Record<string, typeof toppings>);

  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
      <div className="text-center mb-12">
        <h1 className="bg-gradient-to-r from-purple-300 via-pink-300 to-blue-300 bg-clip-text text-transparent mb-4 drop-shadow-lg">
          Cardápio Galático
        </h1>
        <p className="text-purple-200">
          Monte seu açaí perfeito e embarque nesta jornada de sabores! ✨
        </p>
      </div>

      {/* Tamanhos */}
      <section className="mb-12">
        <h2 className="text-purple-200 mb-6 flex items-center gap-2">
          <span className="text-2xl">🪐</span> Tamanhos
        </h2>
        <div className="grid md:grid-cols-2 lg:grid-cols-4 gap-4">
          {sizes.map((item) => (
            <div 
              key={item.size}
              className="bg-purple-900/40 backdrop-blur-md rounded-xl p-6 border-2 border-purple-400/30 hover:border-purple-400/50 transition-all shadow-xl shadow-purple-500/20 hover:shadow-purple-500/40"
            >
              <div className="mb-2">
                <span className="text-purple-100">{item.size}</span>
              </div>
              <div className="text-pink-400 mb-2">{item.price}</div>
              <p className="text-purple-300">{item.description}</p>
            </div>
          ))}
        </div>
      </section>

      {/* Combos Especiais */}
      <section className="mb-12">
        <h2 className="text-purple-200 mb-6 flex items-center gap-2">
          <span className="text-2xl">⭐</span> Combos Especiais
        </h2>
        <div className="grid md:grid-cols-2 gap-4">
          {specialCombos.map((combo) => (
            <div 
              key={combo.name}
              className="bg-gradient-to-br from-purple-800/50 to-pink-800/50 backdrop-blur-md rounded-xl p-6 border-2 border-purple-400/30 hover:border-purple-400/50 transition-all relative overflow-hidden shadow-xl shadow-purple-500/20 hover:shadow-purple-500/40"
            >
              {combo.popular && (
                <div className="absolute top-0 right-0 bg-pink-500 text-white px-3 py-1 text-sm rounded-bl-lg shadow-lg">
                  ⭐ Popular
                </div>
              )}
              <h3 className="text-purple-100 mb-2">{combo.name}</h3>
              <div className="text-pink-400 mb-3">{combo.price}</div>
              <p className="text-purple-200">{combo.description}</p>
            </div>
          ))}
        </div>
      </section>

      {/* Acompanhamentos */}
      <section>
        <h2 className="text-purple-200 mb-6 flex items-center gap-2">
          <span className="text-2xl">🌟</span> Acompanhamentos
        </h2>
        
        {Object.entries(groupedToppings).map(([category, items]) => (
          <div key={category} className="mb-8">
            <h3 className="text-purple-300 mb-4">{category}</h3>
            <div className="grid sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-5 gap-3">
              {items.map((topping) => (
                <div 
                  key={topping.name}
                  className="bg-purple-900/30 backdrop-blur-md rounded-lg p-4 border-2 border-purple-500/20 hover:border-purple-400/40 transition-all flex flex-col items-center text-center shadow-lg shadow-purple-500/10"
                >
                  <div className="w-10 h-10 rounded-full bg-purple-500/30 flex items-center justify-center mb-2 border border-purple-400/30">
                    <Plus className="w-5 h-5 text-purple-300" />
                  </div>
                  <div className="text-purple-100 mb-1">{topping.name}</div>
                  <div className={`${topping.price === 'Grátis' ? 'text-green-400' : 'text-purple-300'}`}>
                    {topping.price}
                  </div>
                </div>
              ))}
            </div>
          </div>
        ))}
      </section>

      {/* Info adicional */}
      <div className="mt-12 bg-purple-900/40 backdrop-blur-md rounded-xl p-6 border-2 border-purple-400/30 text-center shadow-xl shadow-purple-500/20">
        <p className="text-purple-200">
          💫 Monte seu açaí do seu jeito! Cada tamanho já inclui granola e leite em pó. 
          Adicione quantos acompanhamentos quiser!
        </p>
        <p className="text-purple-300 mt-2">
          🚀 Delivery disponível! Entre em contato para fazer seu pedido.
        </p>
      </div>
    </div>
  );
}