import { Menu, Home, Newspaper, Phone, Lock } from 'lucide-react';
import logo from 'figma:asset/471ddfe1d8350f337e600011ec6f84ff26442d39.png';

interface NavigationProps {
  currentPage: 'home' | 'menu' | 'news' | 'contact' | 'admin';
  onNavigate: (page: 'home' | 'menu' | 'news' | 'contact' | 'admin') => void;
  isAdminAuthenticated: boolean;
}

export function Navigation({ currentPage, onNavigate, isAdminAuthenticated }: NavigationProps) {
  const navItems = [
    { id: 'home' as const, label: 'Início', icon: Home },
    { id: 'menu' as const, label: 'Cardápio', icon: Menu },
    { id: 'news' as const, label: 'Novidades', icon: Newspaper },
    { id: 'contact' as const, label: 'Contato', icon: Phone },
  ];

  return (
    <nav className="bg-gradient-to-r from-green-800 to-green-700 backdrop-blur-xl border-b border-green-500/30 sticky top-0 z-50 shadow-lg shadow-green-500/20">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex items-center justify-between h-24">
          <button 
            onClick={() => onNavigate('home')}
            className="flex items-center group"
          >
            <img 
              src={logo} 
              alt="Açaí Galáctico" 
               className="h-20 sm:h-24 object-contain drop-shadow-lg group-hover:scale-105 transition-all" 
             />
          </button>
          
          <div className="flex gap-1">
            {navItems.map((item) => {
              const Icon = item.icon;
              return (
                <button
                  key={item.id}
                  onClick={() => onNavigate(item.id)}
                  className={`flex items-center gap-2 px-4 py-2 rounded-lg transition-all backdrop-blur-sm ${
                    currentPage === item.id
                      ? 'bg-green-600/60 text-white shadow-lg shadow-green-500/30 border border-green-400/50'
                      : 'text-green-50 hover:bg-green-600/40 border border-transparent hover:border-green-400/30'
                  }`}
                >
                  <Icon className="w-4 h-4" />
                  <span className="hidden sm:inline">{item.label}</span>
                </button>
              );
            })}
            <button
              onClick={() => onNavigate('admin')}
              className={`flex items-center gap-2 px-4 py-2 rounded-lg transition-all backdrop-blur-sm ${
                currentPage === 'admin'
                  ? 'bg-green-600/60 text-white shadow-lg shadow-green-500/30 border border-green-400/50'
                  : 'text-green-50 hover:bg-green-600/40 border border-transparent hover:border-green-400/30'
              }`}
            >
              <Lock className="w-4 h-4" />
              <span className="hidden sm:inline">
                {isAdminAuthenticated ? 'Admin' : 'Login'}
              </span>
            </button>
          </div>
        </div>
      </div>
    </nav>
  );
}