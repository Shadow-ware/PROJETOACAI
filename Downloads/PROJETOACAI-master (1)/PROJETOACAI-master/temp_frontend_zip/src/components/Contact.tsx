import { MapPin, Phone, Mail, Clock, Instagram, Facebook, MessageCircle } from 'lucide-react';
import { ImageWithFallback } from './figma/ImageWithFallback';

export function Contact() {
  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
      <div className="text-center mb-12">
        <h1 className="bg-gradient-to-r from-purple-300 via-pink-300 to-blue-300 bg-clip-text text-transparent mb-4 drop-shadow-lg">
          Fale Conosco
        </h1>
        <p className="text-purple-200">
          Estamos aqui para te atender! Entre em contato ou nos visite. 🌟
        </p>
      </div>

      <div className="grid lg:grid-cols-2 gap-8 mb-12">
        {/* Informações de Contato */}
        <div className="space-y-6">
          <div className="bg-purple-900/40 backdrop-blur-md rounded-2xl p-8 border-2 border-purple-400/30 shadow-xl shadow-purple-500/20">
            <h2 className="text-purple-100 mb-6">Informações de Contato</h2>
            
            <div className="space-y-4">
              <div className="flex items-start gap-4">
                <div className="w-12 h-12 rounded-full bg-purple-500/30 flex items-center justify-center flex-shrink-0 border border-purple-400/30">
                  <MapPin className="w-6 h-6 text-purple-300" />
                </div>
                <div>
                  <div className="text-purple-200 mb-1">Endereço</div>
                  <p className="text-purple-100">
                    Rua das Galáxias, 777<br />
                    Centro - Cidade Espacial<br />
                    CEP: 12345-678
                  </p>
                </div>
              </div>

              <div className="flex items-start gap-4">
                <div className="w-12 h-12 rounded-full bg-purple-500/30 flex items-center justify-center flex-shrink-0 border border-purple-400/30">
                  <Phone className="w-6 h-6 text-purple-300" />
                </div>
                <div>
                  <div className="text-purple-200 mb-1">Telefone</div>
                  <p className="text-purple-100">
                    (11) 99999-9999<br />
                    (11) 3333-4444
                  </p>
                </div>
              </div>

              <div className="flex items-start gap-4">
                <div className="w-12 h-12 rounded-full bg-purple-500/30 flex items-center justify-center flex-shrink-0 border border-purple-400/30">
                  <Mail className="w-6 h-6 text-purple-300" />
                </div>
                <div>
                  <div className="text-purple-200 mb-1">E-mail</div>
                  <p className="text-purple-100">
                    contato@acaigalactico.com.br<br />
                    vendas@acaigalactico.com.br
                  </p>
                </div>
              </div>

              <div className="flex items-start gap-4">
                <div className="w-12 h-12 rounded-full bg-purple-500/30 flex items-center justify-center flex-shrink-0 border border-purple-400/30">
                  <Clock className="w-6 h-6 text-purple-300" />
                </div>
                <div>
                  <div className="text-purple-200 mb-1">Horário de Funcionamento</div>
                  <div className="text-purple-100 space-y-1">
                    <p>Segunda a Sexta: 10h - 22h</p>
                    <p>Sábado: 10h - 23h</p>
                    <p>Domingo: 11h - 21h</p>
                  </div>
                </div>
              </div>
            </div>
          </div>

          {/* Redes Sociais */}
          <div className="bg-purple-900/40 backdrop-blur-md rounded-2xl p-8 border-2 border-purple-400/30 shadow-xl shadow-purple-500/20">
            <h2 className="text-purple-100 mb-6">Redes Sociais</h2>
            <div className="grid grid-cols-3 gap-4">
              <a 
                href="https://instagram.com/acaigalactico"
                target="_blank"
                rel="noopener noreferrer"
                className="flex flex-col items-center gap-3 p-4 bg-purple-500/20 rounded-xl hover:bg-purple-500/30 transition-all border-2 border-purple-500/30 hover:border-purple-400/50 shadow-lg shadow-purple-500/10"
              >
                <Instagram className="w-8 h-8 text-pink-300" />
                <span className="text-purple-200">Instagram</span>
              </a>
              <a 
                href="https://facebook.com/acaigalactico"
                target="_blank"
                rel="noopener noreferrer"
                className="flex flex-col items-center gap-3 p-4 bg-purple-500/20 rounded-xl hover:bg-purple-500/30 transition-all border-2 border-purple-500/30 hover:border-purple-400/50 shadow-lg shadow-purple-500/10"
              >
                <Facebook className="w-8 h-8 text-blue-300" />
                <span className="text-purple-200">Facebook</span>
              </a>
              <a 
                href="https://wa.me/5511999999999"
                target="_blank"
                rel="noopener noreferrer"
                className="flex flex-col items-center gap-3 p-4 bg-purple-500/20 rounded-xl hover:bg-purple-500/30 transition-all border-2 border-purple-500/30 hover:border-purple-400/50 shadow-lg shadow-purple-500/10"
              >
                <MessageCircle className="w-8 h-8 text-green-300" />
                <span className="text-purple-200">WhatsApp</span>
              </a>
            </div>
          </div>
        </div>

        {/* Formulário de Contato */}
        <div className="bg-purple-900/40 backdrop-blur-md rounded-2xl p-8 border-2 border-purple-400/30 shadow-xl shadow-purple-500/20">
          <h2 className="text-purple-100 mb-6">Envie uma Mensagem</h2>
          <form className="space-y-4">
            <div>
              <label className="block text-purple-200 mb-2">Nome</label>
              <input 
                type="text"
                className="w-full px-4 py-3 rounded-lg bg-purple-900/50 border-2 border-purple-500/40 text-white placeholder-purple-300 focus:outline-none focus:border-purple-400 backdrop-blur-sm"
                placeholder="Seu nome"
              />
            </div>
            
            <div>
              <label className="block text-purple-200 mb-2">E-mail</label>
              <input 
                type="email"
                className="w-full px-4 py-3 rounded-lg bg-purple-900/50 border-2 border-purple-500/40 text-white placeholder-purple-300 focus:outline-none focus:border-purple-400 backdrop-blur-sm"
                placeholder="seu@email.com"
              />
            </div>
            
            <div>
              <label className="block text-purple-200 mb-2">Telefone</label>
              <input 
                type="tel"
                className="w-full px-4 py-3 rounded-lg bg-purple-900/50 border-2 border-purple-500/40 text-white placeholder-purple-300 focus:outline-none focus:border-purple-400 backdrop-blur-sm"
                placeholder="(11) 99999-9999"
              />
            </div>
            
            <div>
              <label className="block text-purple-200 mb-2">Assunto</label>
              <select className="w-full px-4 py-3 rounded-lg bg-purple-900/50 border-2 border-purple-500/40 text-white focus:outline-none focus:border-purple-400 backdrop-blur-sm">
                <option>Dúvida</option>
                <option>Sugestão</option>
                <option>Reclamação</option>
                <option>Elogio</option>
                <option>Pedido/Delivery</option>
                <option>Outro</option>
              </select>
            </div>
            
            <div>
              <label className="block text-purple-200 mb-2">Mensagem</label>
              <textarea 
                rows={5}
                className="w-full px-4 py-3 rounded-lg bg-purple-900/50 border-2 border-purple-500/40 text-white placeholder-purple-300 focus:outline-none focus:border-purple-400 resize-none backdrop-blur-sm"
                placeholder="Digite sua mensagem..."
              />
            </div>
            
            <button 
              type="submit"
              className="w-full px-6 py-3 bg-purple-600/80 hover:bg-purple-500 rounded-lg transition-all shadow-lg shadow-purple-500/40 backdrop-blur-sm border border-purple-400/30"
            >
              Enviar Mensagem 🚀
            </button>
          </form>
        </div>
      </div>

      {/* Mapa */}
      <div className="bg-purple-900/40 backdrop-blur-md rounded-2xl p-8 border-2 border-purple-400/30 shadow-xl shadow-purple-500/20">
        <h2 className="text-purple-100 mb-6">Nossa Localização</h2>
        <div className="rounded-xl overflow-hidden border-2 border-purple-400/40 shadow-xl shadow-purple-500/20">
          <ImageWithFallback 
            src="https://images.unsplash.com/photo-1648808694138-6706c5efc80a?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&ixid=M3w3Nzg4Nzd8MHwxfHNlYXJjaHwxfHxtb2Rlcm4lMjBjYWZlJTIwaW50ZXJpb3J8ZW58MXx8fHwxNzY0NjgzNTA3fDA&ixlib=rb-4.1.0&q=80&w=1080"
            alt="Interior da loja"
            className="w-full h-96 object-cover"
          />
        </div>
        <p className="text-purple-200 mt-4 text-center">
          📍 Venha nos visitar! Estamos localizados no coração da cidade, fácil acesso e estacionamento disponível.
        </p>
      </div>
    </div>
  );
}