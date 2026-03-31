import { useState } from 'react';
import { Lock, LogOut, Package, FileText, Users, BarChart3, Edit, Trash2, Plus } from 'lucide-react';

interface AdminProps {
  isAuthenticated: boolean;
  onLogin: (value: boolean) => void;
}

export function Admin({ isAuthenticated, onLogin }: AdminProps) {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [activeTab, setActiveTab] = useState<'products' | 'invoices' | 'customers' | 'analytics'>('products');

  const handleLogin = (e: React.FormEvent) => {
    e.preventDefault();
    // Mock login - Em produção, isso seria validado no backend
    if (username === 'admin' && password === 'galactico123') {
      onLogin(true);
    } else {
      alert('Credenciais inválidas!');
    }
  };

  const handleLogout = () => {
    onLogin(false);
    setUsername('');
    setPassword('');
  };

  // Mock data
  const products = [
    { id: 1, name: 'Açaí P (300ml)', price: 12.00, stock: 50, active: true },
    { id: 2, name: 'Açaí M (500ml)', price: 18.00, stock: 45, active: true },
    { id: 3, name: 'Açaí G (700ml)', price: 24.00, stock: 38, active: true },
    { id: 4, name: 'Açaí GG (1L)', price: 32.00, stock: 25, active: true },
    { id: 5, name: 'Combo Nebulosa', price: 28.00, stock: 20, active: true },
  ];

  const invoices = [
    { id: 1001, customer: 'João Silva', date: '03/12/2025', total: 36.00, status: 'Pago' },
    { id: 1002, customer: 'Maria Santos', date: '03/12/2025', total: 28.00, status: 'Pago' },
    { id: 1003, customer: 'Pedro Oliveira', date: '02/12/2025', total: 52.00, status: 'Pendente' },
    { id: 1004, customer: 'Ana Costa', date: '02/12/2025', total: 24.00, status: 'Pago' },
    { id: 1005, customer: 'Carlos Souza', date: '01/12/2025', total: 42.00, status: 'Pago' },
  ];

  const customers = [
    { id: 1, name: 'João Silva', email: 'joao@email.com', phone: '(11) 99999-0001', orders: 15 },
    { id: 2, name: 'Maria Santos', email: 'maria@email.com', phone: '(11) 99999-0002', orders: 23 },
    { id: 3, name: 'Pedro Oliveira', email: 'pedro@email.com', phone: '(11) 99999-0003', orders: 8 },
    { id: 4, name: 'Ana Costa', email: 'ana@email.com', phone: '(11) 99999-0004', orders: 31 },
    { id: 5, name: 'Carlos Souza', email: 'carlos@email.com', phone: '(11) 99999-0005', orders: 12 },
  ];

  if (!isAuthenticated) {
    return (
      <div className="max-w-md mx-auto px-4 py-20">
        <div className="bg-purple-900/40 backdrop-blur-md rounded-2xl p-8 border-2 border-purple-400/30 shadow-xl shadow-purple-500/20">
          <div className="text-center mb-8">
            <div className="w-16 h-16 mx-auto rounded-full bg-purple-500/30 flex items-center justify-center mb-4 border border-purple-400/30">
              <Lock className="w-8 h-8 text-purple-300" />
            </div>
            <h1 className="text-purple-100 mb-2">Área Restrita</h1>
            <p className="text-purple-300">Acesso apenas para funcionários autorizados</p>
          </div>

          <form onSubmit={handleLogin} className="space-y-4">
            <div>
              <label className="block text-purple-200 mb-2">Usuário</label>
              <input 
                type="text"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                className="w-full px-4 py-3 rounded-lg bg-purple-900/50 border-2 border-purple-500/40 text-white placeholder-purple-300 focus:outline-none focus:border-purple-400 backdrop-blur-sm"
                placeholder="Digite seu usuário"
                required
              />
            </div>
            
            <div>
              <label className="block text-purple-200 mb-2">Senha</label>
              <input 
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className="w-full px-4 py-3 rounded-lg bg-purple-900/50 border-2 border-purple-500/40 text-white placeholder-purple-300 focus:outline-none focus:border-purple-400 backdrop-blur-sm"
                placeholder="Digite sua senha"
                required
              />
            </div>
            
            <button 
              type="submit"
              className="w-full px-6 py-3 bg-purple-600/80 hover:bg-purple-500 rounded-lg transition-all shadow-lg shadow-purple-500/40 backdrop-blur-sm border border-purple-400/30"
            >
              Entrar
            </button>
          </form>

          <div className="mt-6 p-4 bg-purple-500/20 rounded-lg border-2 border-purple-500/30">
            <p className="text-purple-200 text-sm text-center">
              <strong>Demo:</strong> usuário: admin / senha: galactico123
            </p>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
      <div className="flex justify-between items-center mb-8">
        <div>
          <h1 className="bg-gradient-to-r from-purple-300 via-pink-300 to-blue-300 bg-clip-text text-transparent mb-2 drop-shadow-lg">
            Painel Administrativo
          </h1>
          <p className="text-purple-200">Gerencie todos os aspectos do Açaí Galático</p>
        </div>
        <button 
          onClick={handleLogout}
          className="flex items-center gap-2 px-4 py-2 bg-red-600/30 hover:bg-red-600/40 text-red-200 rounded-lg border-2 border-red-500/40 transition-all shadow-lg shadow-red-500/20 backdrop-blur-sm"
        >
          <LogOut className="w-4 h-4" />
          Sair
        </button>
      </div>

      {/* Stats Cards */}
      <div className="grid md:grid-cols-4 gap-4 mb-8">
        <div className="bg-purple-900/40 backdrop-blur-md rounded-xl p-6 border-2 border-purple-500/30 shadow-xl shadow-purple-500/20">
          <div className="flex items-center gap-3">
            <div className="w-10 h-10 rounded-full bg-purple-500/30 flex items-center justify-center border border-purple-400/30">
              <Package className="w-5 h-5 text-purple-300" />
            </div>
            <div>
              <div className="text-purple-300">Produtos</div>
              <div className="text-purple-100">24</div>
            </div>
          </div>
        </div>

        <div className="bg-purple-900/40 backdrop-blur-md rounded-xl p-6 border-2 border-purple-500/30 shadow-xl shadow-purple-500/20">
          <div className="flex items-center gap-3">
            <div className="w-10 h-10 rounded-full bg-green-500/30 flex items-center justify-center border border-green-400/30">
              <FileText className="w-5 h-5 text-green-300" />
            </div>
            <div>
              <div className="text-purple-300">Vendas Hoje</div>
              <div className="text-purple-100">32</div>
            </div>
          </div>
        </div>

        <div className="bg-purple-900/40 backdrop-blur-md rounded-xl p-6 border-2 border-purple-500/30 shadow-xl shadow-purple-500/20">
          <div className="flex items-center gap-3">
            <div className="w-10 h-10 rounded-full bg-blue-500/30 flex items-center justify-center border border-blue-400/30">
              <Users className="w-5 h-5 text-blue-300" />
            </div>
            <div>
              <div className="text-purple-300">Clientes</div>
              <div className="text-purple-100">156</div>
            </div>
          </div>
        </div>

        <div className="bg-purple-900/40 backdrop-blur-md rounded-xl p-6 border-2 border-purple-500/30 shadow-xl shadow-purple-500/20">
          <div className="flex items-center gap-3">
            <div className="w-10 h-10 rounded-full bg-pink-500/30 flex items-center justify-center border border-pink-400/30">
              <BarChart3 className="w-5 h-5 text-pink-300" />
            </div>
            <div>
              <div className="text-purple-300">Faturamento</div>
              <div className="text-purple-100">R$ 1.2k</div>
            </div>
          </div>
        </div>
      </div>

      {/* Tabs */}
      <div className="flex gap-2 mb-6 overflow-x-auto">
        <button
          onClick={() => setActiveTab('products')}
          className={`flex items-center gap-2 px-4 py-2 rounded-lg transition-all whitespace-nowrap backdrop-blur-sm ${
            activeTab === 'products'
              ? 'bg-purple-500/40 text-purple-100 shadow-lg shadow-purple-500/30 border-2 border-purple-400/50'
              : 'text-purple-200 hover:bg-purple-500/20 border-2 border-transparent hover:border-purple-400/30'
          }`}
        >
          <Package className="w-4 h-4" />
          Produtos
        </button>
        <button
          onClick={() => setActiveTab('invoices')}
          className={`flex items-center gap-2 px-4 py-2 rounded-lg transition-all whitespace-nowrap backdrop-blur-sm ${
            activeTab === 'invoices'
              ? 'bg-purple-500/40 text-purple-100 shadow-lg shadow-purple-500/30 border-2 border-purple-400/50'
              : 'text-purple-200 hover:bg-purple-500/20 border-2 border-transparent hover:border-purple-400/30'
          }`}
        >
          <FileText className="w-4 h-4" />
          Notas Fiscais
        </button>
        <button
          onClick={() => setActiveTab('customers')}
          className={`flex items-center gap-2 px-4 py-2 rounded-lg transition-all whitespace-nowrap backdrop-blur-sm ${
            activeTab === 'customers'
              ? 'bg-purple-500/40 text-purple-100 shadow-lg shadow-purple-500/30 border-2 border-purple-400/50'
              : 'text-purple-200 hover:bg-purple-500/20 border-2 border-transparent hover:border-purple-400/30'
          }`}
        >
          <Users className="w-4 h-4" />
          Clientes
        </button>
        <button
          onClick={() => setActiveTab('analytics')}
          className={`flex items-center gap-2 px-4 py-2 rounded-lg transition-all whitespace-nowrap backdrop-blur-sm ${
            activeTab === 'analytics'
              ? 'bg-purple-500/40 text-purple-100 shadow-lg shadow-purple-500/30 border-2 border-purple-400/50'
              : 'text-purple-200 hover:bg-purple-500/20 border-2 border-transparent hover:border-purple-400/30'
          }`}
        >
          <BarChart3 className="w-4 h-4" />
          Análises
        </button>
      </div>

      {/* Content */}
      <div className="bg-purple-900/40 backdrop-blur-md rounded-2xl border-2 border-purple-400/30 overflow-hidden shadow-xl shadow-purple-500/20">
        {activeTab === 'products' && (
          <div className="p-6">
            <div className="flex justify-between items-center mb-6">
              <h2 className="text-purple-100">Gerenciar Produtos</h2>
              <button className="flex items-center gap-2 px-4 py-2 bg-purple-600/80 hover:bg-purple-500 rounded-lg transition-all shadow-lg shadow-purple-500/40 backdrop-blur-sm border border-purple-400/30">
                <Plus className="w-4 h-4" />
                Novo Produto
              </button>
            </div>
            <div className="overflow-x-auto">
              <table className="w-full">
                <thead>
                  <tr className="border-b-2 border-purple-500/30">
                    <th className="text-left py-3 px-4 text-purple-200">ID</th>
                    <th className="text-left py-3 px-4 text-purple-200">Nome</th>
                    <th className="text-left py-3 px-4 text-purple-200">Preço</th>
                    <th className="text-left py-3 px-4 text-purple-200">Estoque</th>
                    <th className="text-left py-3 px-4 text-purple-200">Status</th>
                    <th className="text-right py-3 px-4 text-purple-200">Ações</th>
                  </tr>
                </thead>
                <tbody>
                  {products.map((product) => (
                    <tr key={product.id} className="border-b border-purple-500/20 hover:bg-purple-500/10">
                      <td className="py-3 px-4 text-purple-100">#{product.id}</td>
                      <td className="py-3 px-4 text-purple-100">{product.name}</td>
                      <td className="py-3 px-4 text-purple-100">R$ {product.price.toFixed(2)}</td>
                      <td className="py-3 px-4 text-purple-100">{product.stock} un</td>
                      <td className="py-3 px-4">
                        <span className={`px-2 py-1 rounded text-sm ${
                          product.active 
                            ? 'bg-green-500/30 text-green-200 border border-green-400/30' 
                            : 'bg-red-500/30 text-red-200 border border-red-400/30'
                        }`}>
                          {product.active ? 'Ativo' : 'Inativo'}
                        </span>
                      </td>
                      <td className="py-3 px-4">
                        <div className="flex gap-2 justify-end">
                          <button className="p-2 hover:bg-purple-500/30 rounded transition-all">
                            <Edit className="w-4 h-4 text-purple-300" />
                          </button>
                          <button className="p-2 hover:bg-red-500/30 rounded transition-all">
                            <Trash2 className="w-4 h-4 text-red-300" />
                          </button>
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        )}

        {activeTab === 'invoices' && (
          <div className="p-6">
            <h2 className="text-purple-100 mb-6">Notas Fiscais</h2>
            <div className="overflow-x-auto">
              <table className="w-full">
                <thead>
                  <tr className="border-b-2 border-purple-500/30">
                    <th className="text-left py-3 px-4 text-purple-200">NF</th>
                    <th className="text-left py-3 px-4 text-purple-200">Cliente</th>
                    <th className="text-left py-3 px-4 text-purple-200">Data</th>
                    <th className="text-left py-3 px-4 text-purple-200">Total</th>
                    <th className="text-left py-3 px-4 text-purple-200">Status</th>
                    <th className="text-right py-3 px-4 text-purple-200">Ações</th>
                  </tr>
                </thead>
                <tbody>
                  {invoices.map((invoice) => (
                    <tr key={invoice.id} className="border-b border-purple-500/20 hover:bg-purple-500/10">
                      <td className="py-3 px-4 text-purple-100">#{invoice.id}</td>
                      <td className="py-3 px-4 text-purple-100">{invoice.customer}</td>
                      <td className="py-3 px-4 text-purple-100">{invoice.date}</td>
                      <td className="py-3 px-4 text-purple-100">R$ {invoice.total.toFixed(2)}</td>
                      <td className="py-3 px-4">
                        <span className={`px-2 py-1 rounded text-sm ${
                          invoice.status === 'Pago' 
                            ? 'bg-green-500/30 text-green-200 border border-green-400/30' 
                            : 'bg-yellow-500/30 text-yellow-200 border border-yellow-400/30'
                        }`}>
                          {invoice.status}
                        </span>
                      </td>
                      <td className="py-3 px-4">
                        <div className="flex gap-2 justify-end">
                          <button className="px-3 py-1 bg-purple-600/40 hover:bg-purple-600/60 rounded transition-all text-purple-100 border border-purple-400/30">
                            Ver Detalhes
                          </button>
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        )}

        {activeTab === 'customers' && (
          <div className="p-6">
            <div className="flex justify-between items-center mb-6">
              <h2 className="text-purple-100">Clientes Cadastrados</h2>
              <button className="flex items-center gap-2 px-4 py-2 bg-purple-600/80 hover:bg-purple-500 rounded-lg transition-all shadow-lg shadow-purple-500/40 backdrop-blur-sm border border-purple-400/30">
                <Plus className="w-4 h-4" />
                Novo Cliente
              </button>
            </div>
            <div className="overflow-x-auto">
              <table className="w-full">
                <thead>
                  <tr className="border-b-2 border-purple-500/30">
                    <th className="text-left py-3 px-4 text-purple-200">ID</th>
                    <th className="text-left py-3 px-4 text-purple-200">Nome</th>
                    <th className="text-left py-3 px-4 text-purple-200">E-mail</th>
                    <th className="text-left py-3 px-4 text-purple-200">Telefone</th>
                    <th className="text-left py-3 px-4 text-purple-200">Pedidos</th>
                    <th className="text-right py-3 px-4 text-purple-200">Ações</th>
                  </tr>
                </thead>
                <tbody>
                  {customers.map((customer) => (
                    <tr key={customer.id} className="border-b border-purple-500/20 hover:bg-purple-500/10">
                      <td className="py-3 px-4 text-purple-100">#{customer.id}</td>
                      <td className="py-3 px-4 text-purple-100">{customer.name}</td>
                      <td className="py-3 px-4 text-purple-100">{customer.email}</td>
                      <td className="py-3 px-4 text-purple-100">{customer.phone}</td>
                      <td className="py-3 px-4 text-purple-100">{customer.orders}</td>
                      <td className="py-3 px-4">
                        <div className="flex gap-2 justify-end">
                          <button className="p-2 hover:bg-purple-500/30 rounded transition-all">
                            <Edit className="w-4 h-4 text-purple-300" />
                          </button>
                          <button className="p-2 hover:bg-red-500/30 rounded transition-all">
                            <Trash2 className="w-4 h-4 text-red-300" />
                          </button>
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        )}

        {activeTab === 'analytics' && (
          <div className="p-6">
            <h2 className="text-purple-100 mb-6">Análises e Relatórios</h2>
            <div className="grid md:grid-cols-2 gap-6">
              <div className="bg-purple-500/20 rounded-xl p-6 border-2 border-purple-500/30 shadow-lg shadow-purple-500/10">
                <h3 className="text-purple-200 mb-4">Vendas por Período</h3>
                <div className="space-y-3">
                  <div className="flex justify-between items-center">
                    <span className="text-purple-300">Hoje</span>
                    <span className="text-purple-100">R$ 1.248,00</span>
                  </div>
                  <div className="flex justify-between items-center">
                    <span className="text-purple-300">Esta Semana</span>
                    <span className="text-purple-100">R$ 8.756,00</span>
                  </div>
                  <div className="flex justify-between items-center">
                    <span className="text-purple-300">Este Mês</span>
                    <span className="text-purple-100">R$ 34.892,00</span>
                  </div>
                </div>
              </div>

              <div className="bg-purple-500/20 rounded-xl p-6 border-2 border-purple-500/30 shadow-lg shadow-purple-500/10">
                <h3 className="text-purple-200 mb-4">Produtos Mais Vendidos</h3>
                <div className="space-y-3">
                  <div className="flex justify-between items-center">
                    <span className="text-purple-300">Açaí M (500ml)</span>
                    <span className="text-purple-100">142 un</span>
                  </div>
                  <div className="flex justify-between items-center">
                    <span className="text-purple-300">Combo Nebulosa</span>
                    <span className="text-purple-100">98 un</span>
                  </div>
                  <div className="flex justify-between items-center">
                    <span className="text-purple-300">Açaí G (700ml)</span>
                    <span className="text-purple-100">87 un</span>
                  </div>
                </div>
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}