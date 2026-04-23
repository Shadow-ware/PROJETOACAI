#nullable disable
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using acaiGalatico.UI.Models;
using acaiGalatico.UI.Services;
using Guna.UI2.WinForms;

namespace acaiGalatico.UI.Forms
{
    public partial class frmVendas : Form
    {
        private readonly PedidosApiService _pedidosApiService;
        private List<PedidoDto> _pedidos = new();
        private bool _estaCarregando;

        public frmVendas()
        {
            _pedidosApiService = AppServices.PedidosApiService;
            InitializeComponent();
            
            // Habilita DoubleBuffering para evitar flicker
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            flowPedidos.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)?.SetValue(flowPedidos, true);
            
            // Ajustar largura dos itens quando a tela for redimensionada
            flowPedidos.Resize += (s, e) => {
                foreach (Control ctrl in flowPedidos.Controls)
                {
                    ctrl.Width = flowPedidos.Width - 45;
                }
            };
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Inicia o carregamento sem bloquear a UI
            _ = Task.Run(async () => await CarregarPedidosAtivosAsync());
            tmrAtualizacao.Start();
        }

        private async void tmrAtualizacao_Tick(object sender, EventArgs e)
        {
            if (_estaCarregando) return;
            await CarregarPedidosAtivosAsync(true);
        }

        private async Task CarregarPedidosAtivosAsync(bool isAutoRefresh = false)
        {
            if (_estaCarregando) return;

            try
            {
                _estaCarregando = true;

                var todosPedidos = await _pedidosApiService.GetPedidosAsync();
                
                // 🔄 ORDEM DOS PEDIDOS: Mais novos primeiro
                var pedidosAtivos = todosPedidos
                    .Where(p => p.Status != StatusVendaDto.Entregue && p.Status != StatusVendaDto.Cancelado)
                    .OrderByDescending(p => p.Id)
                    .ToList();

                // Detecta se houve mudança para evitar redraw desnecessário
                bool houveMudanca = _pedidos.Count != pedidosAtivos.Count || 
                                   !_pedidos.Select(p => p.Id).SequenceEqual(pedidosAtivos.Select(p => p.Id)) ||
                                   !_pedidos.Select(p => p.Status).SequenceEqual(pedidosAtivos.Select(p => p.Status));

                if (houveMudanca || !isAutoRefresh)
                {
                    _pedidos = pedidosAtivos;
                    
                    this.Invoke(new Action(() => {
                        AtualizarCards(pedidosAtivos);
                        lblStatusFila.Text = $"{pedidosAtivos.Count} pedidos em produção";
                    }));
                }
            }
            catch (Exception ex)
            {
                if (!isAutoRefresh)
                {
                    MessageBox.Show($"Erro ao carregar pedidos: {ex.Message}");
                }
            }
            finally
            {
                _estaCarregando = false;
            }
        }

        private void AtualizarCards(List<PedidoDto> pedidos)
        {
            flowPedidos.SuspendLayout();
            flowPedidos.Controls.Clear();

            foreach (var pedido in pedidos)
            {
                flowPedidos.Controls.Add(CriarLinhaPedido(pedido));
            }
            
            flowPedidos.ResumeLayout();
        }

        private Control CriarLinhaPedido(PedidoDto pedido)
        {
            var pnlLinha = new Guna2Panel
            {
                Width = flowPedidos.Width - 45,
                Height = 60,
                FillColor = Color.White,
                BorderRadius = 8,
                Margin = new Padding(0, 0, 0, 6)
            };

            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 9,
                BackColor = Color.Transparent,
                Padding = new Padding(10, 0, 10, 0)
            };

            // Colunas limpas conforme o formato solicitado
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 55F));   // #ID
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));    // Cliente
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));    // Tamanho
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));   // Qtd
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));    // Frutas
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));    // Acompanhamentos
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));    // Pagamento
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));    // Valor
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));  // [FINALIZAR]

            Label CriarLabel(string texto, Font fonte, Color cor) => new Label
            {
                Text = texto,
                Font = fonte,
                ForeColor = cor,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true
            };

            var boldFont = new Font("Segoe UI", 11, FontStyle.Bold);
            var regularFont = new Font("Segoe UI", 10, FontStyle.Bold);

            // Dados Parseados (Regex)
            string cliente = pedido.ClienteNomeDisplay;
            string tamanho = string.IsNullOrEmpty(pedido.TamanhoParsed) ? "Açaí" : pedido.TamanhoParsed;
            string qtd = string.IsNullOrEmpty(pedido.QuantidadeParsed) ? "1" : pedido.QuantidadeParsed;
            string frutas = string.IsNullOrEmpty(pedido.FrutasParsed) ? "S/ Frutas" : pedido.FrutasParsed;
            string acomp = string.IsNullOrEmpty(pedido.AcompanhamentosParsed) ? "S/ Adicionais" : pedido.AcompanhamentosParsed;
            string pagamento = pedido.PagamentoLimpo;

            table.Controls.Add(CriarLabel($"#{pedido.Id}", boldFont, Color.FromArgb(155, 89, 182)), 0, 0);
            table.Controls.Add(CriarLabel(cliente, boldFont, Color.Black), 1, 0);
            table.Controls.Add(CriarLabel(tamanho, regularFont, Color.FromArgb(43, 52, 69)), 2, 0);
            table.Controls.Add(CriarLabel($"x{qtd}", regularFont, Color.FromArgb(43, 52, 69)), 3, 0);
            table.Controls.Add(CriarLabel(frutas, regularFont, Color.Gray), 4, 0);
            table.Controls.Add(CriarLabel(acomp, regularFont, Color.Gray), 5, 0);
            table.Controls.Add(CriarLabel(pagamento, regularFont, Color.FromArgb(39, 174, 96)), 6, 0);
            table.Controls.Add(CriarLabel(pedido.ValorTotal.ToString("C2"), boldFont, Color.FromArgb(48, 43, 99)), 7, 0);

            var btnFinalizar = new Guna2Button
            {
                Text = "FINALIZAR",
                Size = new Size(100, 35),
                Anchor = AnchorStyles.None,
                BorderRadius = 6,
                FillColor = Color.FromArgb(46, 204, 113),
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                Animated = true
            };

            btnFinalizar.Click += async (s, e) => {
                btnFinalizar.Enabled = false;
                btnFinalizar.Text = "...";
                try {
                    await _pedidosApiService.FinalizarPedidoAsync(pedido.Id);
                    await CarregarPedidosAtivosAsync(true);
                } catch {
                    btnFinalizar.Enabled = true;
                    btnFinalizar.Text = "FINALIZAR";
                }
            };

            table.Controls.Add(btnFinalizar, 8, 0);
            pnlLinha.Controls.Add(table);

            return pnlLinha;
        }

        private async Task AtualizarStatus(int id, StatusVendaDto novoStatus)
        {
            try 
            {
                await _pedidosApiService.UpdateStatusAsync(id, novoStatus);
                await CarregarPedidosAtivosAsync(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar status: {ex.Message}");
            }
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            _ = CarregarPedidosAtivosAsync();
        }
    }
}
