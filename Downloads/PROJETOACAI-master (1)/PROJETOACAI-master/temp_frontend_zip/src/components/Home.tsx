import { ImageWithFallback } from './figma/ImageWithFallback';
import { Sparkles, Clock, Award, MapPin } from 'lucide-react';
import logo from 'figma:asset/ca936e7013ed0128385226a28398cc0a09450e7e.png';
import logoText from 'figma:asset/4985f1f0a459d5aaa3c853f077923075fdcc0e8c.png';

export function Home() {
  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
      {/* Hero Section */}
      <div className="text-center mb-16 space-y-6">
        <div className="inline-block">
          <div className="flex items-center justify-center gap-6 mb-6">
            <img 
              src={logo} 
              alt="Açaí Galático Logo" 
              className="w-32 h-32 sm:w-40 sm:h-40 animate-pulse drop-shadow-2xl" 
            />
          </div>
          <img 
            src={logoText} 
            alt="Açaí Galático" 
            className="w-full max-w-2xl mx-auto drop-shadow-[0_0_30px_rgba(168,85,247,0.8)] mb-6"
          />
          <div className="h-1 bg-gradient-to-r from-transparent via-purple-400 to-transparent shadow-lg shadow-purple-500/50"></div>
        </div>
        <p className="text-xl text-purple-100 max-w-2xl mx-auto drop-shadow-lg">
          Uma experiência intergaláctica de sabor! Açaí de qualidade premium que vai te levar às estrelas. 🚀
        </p>
      </div>

      {/* Featured Image */}
      <div className="mb-16 rounded-2xl overflow-hidden border-4 border-purple-400/40 shadow-2xl shadow-purple-500/40 backdrop-blur-sm">
        <ImageWithFallback 
          src="https://images.unsplash.com/photo-1681325655248-fddbd7921715?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&ixid=M3w3Nzg4Nzd8MHwxfHNlYXJjaHwxfHxhY2FpJTIwYm93bCUyMHB1cnBsZXxlbnwxfHx8fDE3NjQ4MDMzMzl8MA&ixlib=rb-4.1.0&q=80&w=1080"
          alt="Açaí Bowl"
          className="w-full h-96 object-cover"
        />
      </div>

      {/* Quem Somos */}
      <section className="mb-16 bg-purple-900/40 backdrop-blur-md rounded-2xl p-8 border-2 border-purple-400/30 shadow-xl shadow-purple-500/20">
        <div className="flex items-center gap-3 mb-6">
          <Sparkles className="w-8 h-8 text-purple-300" />
          <h2 className="text-purple-200">Quem Somos</h2>
        </div>
        <div className="space-y-4 text-purple-100">
          <p>
            O <span className="text-purple-300">Açaí Galático</span> nasceu da paixão por proporcionar 
            experiências únicas e saborosas. Somos mais do que uma simples açaiteria - somos um portal 
            para uma jornada de sabores cósmicos!
          </p>
          <p>
            Utilizamos apenas açaí premium, selecionado diretamente da Amazônia, e combinamos com 
            acompanhamentos fresquinhos e de alta qualidade. Cada tigela é preparada com amor e 
            dedicação, pensando em proporcionar o melhor para nossos clientes.
          </p>
          <p>
            Nossa missão é transformar cada visita em uma experiência memorável, onde sabor, 
            qualidade e atendimento se encontram em perfeita harmonia. Venha explorar o universo 
            de sabores do Açaí Galático!
          </p>
        </div>
      </section>

      {/* Features Grid */}
      <div className="grid md:grid-cols-3 gap-6 mb-16">
        <div className="bg-gradient-to-br from-purple-800/50 to-pink-800/50 backdrop-blur-md rounded-xl p-6 border-2 border-purple-400/30 hover:border-purple-400/50 transition-all shadow-xl shadow-purple-500/20 hover:shadow-purple-500/40">
          <div className="w-12 h-12 rounded-full bg-purple-500/40 flex items-center justify-center mb-4 backdrop-blur-sm border border-purple-400/30">
            <Clock className="w-6 h-6 text-purple-200" />
          </div>
          <h3 className="text-purple-100 mb-2">Horário de Funcionamento</h3>
          <div className="text-purple-200 space-y-1">
            <p>Segunda a Sexta: 10h - 22h</p>
            <p>Sábado: 10h - 23h</p>
            <p>Domingo: 11h - 21h</p>
          </div>
        </div>

        <div className="bg-gradient-to-br from-purple-800/50 to-pink-800/50 backdrop-blur-md rounded-xl p-6 border-2 border-purple-400/30 hover:border-purple-400/50 transition-all shadow-xl shadow-purple-500/20 hover:shadow-purple-500/40">
          <div className="w-12 h-12 rounded-full bg-purple-500/40 flex items-center justify-center mb-4 backdrop-blur-sm border border-purple-400/30">
            <Award className="w-6 h-6 text-purple-200" />
          </div>
          <h3 className="text-purple-100 mb-2">Qualidade Premium</h3>
          <p className="text-purple-200">
            Açaí 100% orgânico da Amazônia, sem conservantes ou aditivos artificiais. 
            Fresco e batido na hora!
          </p>
        </div>

        <div className="bg-gradient-to-br from-purple-800/50 to-pink-800/50 backdrop-blur-md rounded-xl p-6 border-2 border-purple-400/30 hover:border-purple-400/50 transition-all shadow-xl shadow-purple-500/20 hover:shadow-purple-500/40">
          <div className="w-12 h-12 rounded-full bg-purple-500/40 flex items-center justify-center mb-4 backdrop-blur-sm border border-purple-400/30">
            <MapPin className="w-6 h-6 text-purple-200" />
          </div>
          <h3 className="text-purple-100 mb-2">Nossa Localização</h3>
          <p className="text-purple-200">
            Rua das Galáxias, 777<br />
            Centro - Cidade Espacial<br />
            CEP: 12345-678
          </p>
        </div>
      </div>

      {/* CTA Section */}
      <div className="text-center bg-gradient-to-r from-purple-600/30 via-pink-600/30 to-purple-600/30 backdrop-blur-md rounded-2xl p-12 border-2 border-purple-400/40 shadow-2xl shadow-purple-500/30">
        <h2 className="text-purple-100 mb-4">Pronto para decolar? 🚀</h2>
        <p className="text-purple-200 mb-6 max-w-2xl mx-auto">
          Visite nossa loja e embarque nesta jornada galáctica de sabores. 
          Temos delivery para levar o universo até você!
        </p>
        <div className="flex gap-4 justify-center flex-wrap">
          <a 
            href="tel:+5511999999999" 
            className="px-6 py-3 bg-purple-600/80 hover:bg-purple-500 rounded-lg transition-all shadow-lg shadow-purple-500/40 backdrop-blur-sm border border-purple-400/30"
          >
            📞 (11) 99999-9999
          </a>
          <a 
            href="https://instagram.com/acaigalactico" 
            target="_blank"
            rel="noopener noreferrer"
            className="px-6 py-3 bg-pink-600/80 hover:bg-pink-500 rounded-lg transition-all shadow-lg shadow-pink-500/40 backdrop-blur-sm border border-pink-400/30"
          >
            📱 @acaigalactico
          </a>
        </div>
      </div>
    </div>
  );
}