import { useState } from 'react';
import { Home } from './components/Home';
import { Menu } from './components/Menu';
import { News } from './components/News';
import { Contact } from './components/Contact';
import { Admin } from './components/Admin';
import { Navigation } from './components/Navigation';
import galaxyBg from 'figma:asset/1f8416f6ce9429332660b59a5b590f06fbbb0442.png';

export default function App() {
  const [currentPage, setCurrentPage] = useState<'home' | 'menu' | 'news' | 'contact' | 'admin'>('home');
  const [isAdminAuthenticated, setIsAdminAuthenticated] = useState(false);

  const renderPage = () => {
    switch (currentPage) {
      case 'home':
        return <Home />;
      case 'menu':
        return <Menu />;
      case 'news':
        return <News />;
      case 'contact':
        return <Contact />;
      case 'admin':
        return <Admin 
          isAuthenticated={isAdminAuthenticated} 
          onLogin={setIsAdminAuthenticated}
        />;
      default:
        return <Home />;
    }
  };

  return (
    <div className="min-h-screen relative overflow-hidden">
      {/* Galaxy background image */}
      <div className="fixed inset-0 w-full h-full">
        <img 
          src={galaxyBg} 
          alt="Galaxy background" 
          className="w-full h-full object-cover"
        />
        <div className="absolute inset-0 bg-gradient-to-b from-black/30 via-transparent to-black/50"></div>
      </div>

      {/* Enhanced stars layer */}
      <div className="fixed inset-0 overflow-hidden pointer-events-none">
        {[...Array(100)].map((_, i) => (
          <div
            key={i}
            className="absolute bg-white rounded-full animate-pulse"
            style={{
              width: `${Math.random() * 3 + 1}px`,
              height: `${Math.random() * 3 + 1}px`,
              top: `${Math.random() * 100}%`,
              left: `${Math.random() * 100}%`,
              animationDelay: `${Math.random() * 3}s`,
              animationDuration: `${Math.random() * 2 + 1}s`,
              opacity: Math.random() * 0.8 + 0.2,
              boxShadow: `0 0 ${Math.random() * 4 + 2}px rgba(255, 255, 255, ${Math.random() * 0.5 + 0.3})`,
            }}
          />
        ))}
      </div>

      {/* Nebula effects */}
      <div className="fixed inset-0 overflow-hidden pointer-events-none">
        <div className="absolute top-1/4 left-1/4 w-96 h-96 bg-purple-500/20 rounded-full blur-3xl animate-pulse"></div>
        <div className="absolute bottom-1/4 right-1/4 w-96 h-96 bg-pink-500/20 rounded-full blur-3xl animate-pulse" style={{ animationDelay: '1s' }}></div>
        <div className="absolute top-1/2 right-1/3 w-64 h-64 bg-blue-500/20 rounded-full blur-3xl animate-pulse" style={{ animationDelay: '2s' }}></div>
      </div>

      <div className="relative z-10">
        <Navigation 
          currentPage={currentPage} 
          onNavigate={setCurrentPage}
          isAdminAuthenticated={isAdminAuthenticated}
        />
        {renderPage()}
      </div>
    </div>
  );
}