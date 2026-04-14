using System.Globalization;
using System.Windows.Forms;
using acaiGalatico.UI.Models;
using acaiGalatico.UI.Services;

namespace acaiGalatico.UI.Forms
{
    public partial class frmPedidos : Form
    {
        private readonly PedidosApiService _pedidosApiService;
        private readonly CultureInfo _culture = CultureInfo.GetCultureInfo("pt-BR");
        private List<PedidoDto> _pedidos = new();
        private List<ClienteDto> _clientes = new();
        private bool _carregandoFormulario;

        public frmPedidos()
        {
            _pedidosApiService = AppServices.PedidosApiService;
            InitializeComponent();
            InicializarControles();
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            await CarregarDadosAsync();
            tmrAtualizacao.Start();
        }

        private bool _estaAtualizando;

        private async void tmrAtualizacao_Tick(object sender, EventArgs e)
        {
            if (_estaAtualizando) return;
            
            try 
            {
                _estaAtualizando = true;
                await CarregarDadosAsync(int.TryParse(txtCodigo.Text, out var id) ? id : (int?)null, true);
            }
            finally 
            {
                _estaAtualizando = false;
            }
        }

        private void InicializarControles()
        {
            dtpData.CustomFormat = "dd/MM/yyyy HH:mm";
            cboStatus.DataSource = Enum.GetValues<StatusVendaDto>();
            cboPagamento.DataSource = Enum.GetValues<TipoPagamentoDto>();
            cboStatus.Format += ComboStatus_Format;
            cboPagamento.Format += ComboPagamento_Format;
            lblApiInfo.Text = $"API: {_pedidosApiService.BaseUrl}";

            // Desabilita campos que vêm do site (apenas leitura no desktop)
            dtpData.Enabled = false;
            cboCliente.Enabled = false;
            cboPagamento.Enabled = false;
            txtValor.ReadOnly = true;
            txtEndereco.ReadOnly = true;
            txtBairro.ReadOnly = true;
            txtObservacao.ReadOnly = true;
        }

        private async Task CarregarDadosAsync(int? pedidoParaSelecionar = null, bool isAutoRefresh = false)
        {
            try
            {
                // Se for atualização automática, não mostramos o cursor de espera nem mudamos o estado do formulário de forma visível
                if (!isAutoRefresh) AlternarCarregamento(true);
                else lblResumo.Text = "Sincronizando...";

                var clientesTask = _pedidosApiService.GetClientesAsync();
                var pedidosTask = _pedidosApiService.GetPedidosAsync();

                await Task.WhenAll(clientesTask, pedidosTask);

                _clientes = clientesTask.Result.OrderBy(c => c.Nome).ToList();
                _pedidos = pedidosTask.Result.OrderByDescending(p => p.DataVenda).ToList();

                // Só atualiza os componentes visíveis se necessário ou se não estiver no meio de uma edição
                if (!_carregandoFormulario)
                {
                    PopularClientes();
                    PopularGrid();
                    AtualizarResumo();

                    var idParaSelecionar = pedidoParaSelecionar ?? (_pedidos.FirstOrDefault()?.Id);

                    if (idParaSelecionar.HasValue)
                    {
                        var pedido = _pedidos.FirstOrDefault(p => p.Id == idParaSelecionar.Value);
                        if (pedido != null)
                        {
                            CarregarPedidoNoFormulario(pedido);
                            SelecionarPedidoNaGrid(idParaSelecionar.Value);
                        }
                    }
                    else
                    {
                        LimparFormulario();
                    }
                }
            }
            catch (Exception ex)
            {
                lblResumo.Text = "API indisponível";
                
                // Só mostra MessageBox se for um erro manual, não no timer
                if (!isAutoRefresh)
                {
                    MessageBox.Show(
                        $"ERRO DE CONEXÃO DESKTOP -> API\n\n" +
                        $"Base URL: http://localhost:5207/\n" +
                        $"Endpoint: api/pedidos\n\n" +
                        $"Detalhes: {ex.Message}",
                        "Falha ao carregar pedidos",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            finally
            {
                if (!isAutoRefresh) AlternarCarregamento(false);
            }
        }

        private void PopularClientes()
        {
            var opcoes = new List<ClienteOption>
            {
                new(0, "Cliente Avulso")
            };

            opcoes.AddRange(_clientes.Select(c => new ClienteOption(c.Id, c.Nome)));

            cboCliente.DataSource = opcoes;
            cboCliente.DisplayMember = nameof(ClienteOption.Nome);
            cboCliente.ValueMember = nameof(ClienteOption.Id);
        }

        private void PopularGrid()
        {
            dgvPedidos.Rows.Clear();

            foreach (var pedido in _pedidos)
            {
                var indice = dgvPedidos.Rows.Add(
                    $"#{pedido.Id}",
                    pedido.DataVenda.ToString("dd/MM/yyyy HH:mm"),
                    ObterStatusLabel(pedido.Status),
                    pedido.Cliente?.Nome ?? "Cliente Avulso",
                    pedido.ValorTotal.ToString("C2", _culture));

                var row = dgvPedidos.Rows[indice];
                row.Tag = pedido;

                var statusCell = row.Cells[colStatus.Name];
                statusCell.Style.BackColor = ObterCorStatus(pedido.Status);
                statusCell.Style.ForeColor = pedido.Status == StatusVendaDto.Pendente ? Color.FromArgb(62, 45, 0) : Color.White;
                statusCell.Style.SelectionBackColor = statusCell.Style.BackColor;
                statusCell.Style.SelectionForeColor = statusCell.Style.ForeColor;

                row.Cells[colEditar.Name].Style.BackColor = Color.FromArgb(243, 232, 255); // Light purple
                row.Cells[colEditar.Name].Style.ForeColor = Color.FromArgb(155, 89, 182); // Purple
                row.Cells[colExcluir.Name].Style.BackColor = Color.FromArgb(254, 242, 242); // Light red
                row.Cells[colExcluir.Name].Style.ForeColor = Color.FromArgb(220, 38, 38); // Red
            }
        }

        private void AtualizarResumo()
        {
            lblResumo.Text = $"{_pedidos.Count} pedido(s) carregado(s) · atualização {DateTime.Now:dd/MM/yyyy HH:mm}";
        }

        private void CarregarPedidoNoFormulario(PedidoDto pedido)
        {
            _carregandoFormulario = true;

            try
            {
                lblEditorTitulo.Text = $"Detalhes do pedido #{pedido.Id}";
                txtCodigo.Text = pedido.Id.ToString();
                dtpData.Value = pedido.DataVenda < dtpData.MinDate ? dtpData.MinDate : pedido.DataVenda;
                cboStatus.SelectedItem = pedido.Status;
                cboPagamento.SelectedItem = pedido.FormaPagamento;
                cboCliente.SelectedValue = pedido.ClienteId ?? 0;
                txtValor.Text = pedido.ValorTotal.ToString("N2", _culture);
                txtEndereco.Text = pedido.EnderecoEntrega ?? string.Empty;
                txtBairro.Text = pedido.BairroEntrega ?? string.Empty;
                txtObservacao.Text = pedido.Observacao ?? string.Empty;

                if (pedido.Itens != null && pedido.Itens.Count > 0)
                {
                    txtItens.Text = string.Join(Environment.NewLine, pedido.Itens.Select(i => $"• {i.Quantidade}x {i.DisplayNome} - {i.PrecoTotal:C2}"));
                }
                else
                {
                    txtItens.Text = "Nenhum item detalhado.";
                }

                btnSalvar.Text = "Salvar alterações";
                btnExcluir.Enabled = true;
            }
            finally
            {
                _carregandoFormulario = false;
            }
        }

        private void LimparFormulario()
        {
            _carregandoFormulario = true;

            try
            {
                lblEditorTitulo.Text = "Selecione um pedido";
                txtCodigo.Text = string.Empty;
                dtpData.Value = DateTime.Now;
                cboStatus.SelectedIndex = -1;
                cboPagamento.SelectedIndex = -1;
                cboCliente.SelectedIndex = -1;
                txtValor.Text = string.Empty;
                txtEndereco.Text = string.Empty;
                txtBairro.Text = string.Empty;
                txtObservacao.Text = string.Empty;
                txtItens.Text = string.Empty;
                btnSalvar.Text = "Salvar";
                btnExcluir.Enabled = false;
                dgvPedidos.ClearSelection();
            }
            finally
            {
                _carregandoFormulario = false;
            }
        }

        private void SelecionarPedidoNaGrid(int id)
        {
            foreach (DataGridViewRow row in dgvPedidos.Rows)
            {
                if (row.Tag is PedidoDto pedido && pedido.Id == id)
                {
                    row.Selected = true;
                    dgvPedidos.CurrentCell = row.Cells[colId.Name];
                    break;
                }
            }
        }

        private void AlternarCarregamento(bool carregando)
        {
            UseWaitCursor = carregando;
            btnAtualizar.Enabled = !carregando;
            btnSalvar.Enabled = !carregando && !string.IsNullOrEmpty(txtCodigo.Text);
            btnExcluir.Enabled = !carregando && !string.IsNullOrEmpty(txtCodigo.Text);
        }

        private async void btnAtualizar_Click(object sender, EventArgs e)
        {
            await CarregarDadosAsync();
        }

        private async void btnSalvar_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtCodigo.Text, out var id))
            {
                return;
            }

            try
            {
                AlternarCarregamento(true);
                var pedido = ConstruirPedidoDoFormulario();
                pedido.Id = id;

                await _pedidosApiService.UpdatePedidoAsync(pedido);
                MessageBox.Show($"Status do pedido #{pedido.Id} atualizado com sucesso.", "Pedidos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CarregarDadosAsync(pedido.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Falha ao salvar pedido", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                AlternarCarregamento(false);
            }
        }

        private async void btnExcluir_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtCodigo.Text, out var id))
            {
                return;
            }

            await ExcluirPedidoAsync(id);
        }

        private async Task ExcluirPedidoAsync(int id)
        {
            var confirmacao = MessageBox.Show(
                $"Deseja realmente excluir o pedido #{id}?",
                "Excluir pedido",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmacao != DialogResult.Yes)
            {
                return;
            }

            try
            {
                AlternarCarregamento(true);
                await _pedidosApiService.DeletePedidoAsync(id);
                MessageBox.Show("Pedido excluído com sucesso.", "Pedidos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CarregarDadosAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Falha ao excluir pedido", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                AlternarCarregamento(false);
            }
        }

        private PedidoDto ConstruirPedidoDoFormulario()
        {
            if (!decimal.TryParse(txtValor.Text, NumberStyles.Currency | NumberStyles.Number, _culture, out var valor) &&
                !decimal.TryParse(txtValor.Text.Replace(".", ",").Trim(), NumberStyles.Number, _culture, out valor))
            {
                throw new InvalidOperationException("Informe um valor total válido.");
            }

            var clienteId = cboCliente.SelectedValue is int selectedClienteId && selectedClienteId > 0
                ? selectedClienteId
                : (int?)null;

            return new PedidoDto
            {
                DataVenda = dtpData.Value,
                ValorTotal = valor,
                FormaPagamento = cboPagamento.SelectedItem is TipoPagamentoDto pagamento ? pagamento : TipoPagamentoDto.Dinheiro,
                ClienteId = clienteId,
                Status = cboStatus.SelectedItem is StatusVendaDto status ? status : StatusVendaDto.Pendente,
                EnderecoEntrega = txtEndereco.Text.Trim(),
                BairroEntrega = txtBairro.Text.Trim(),
                Observacao = txtObservacao.Text.Trim()
            };
        }

        private void dgvPedidos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvPedidos.Rows.Count)
            {
                return;
            }

            if (dgvPedidos.Rows[e.RowIndex].Tag is not PedidoDto pedido)
            {
                return;
            }

            var columnName = dgvPedidos.Columns[e.ColumnIndex].Name;

            if (columnName == colExcluir.Name)
            {
                _ = ExcluirPedidoAsync(pedido.Id);
                return;
            }

            CarregarPedidoNoFormulario(pedido);
            SelecionarPedidoNaGrid(pedido.Id);
        }

        private void dgvPedidos_SelectionChanged(object sender, EventArgs e)
        {
            if (_carregandoFormulario)
            {
                return;
            }

            if (dgvPedidos.CurrentRow?.Tag is PedidoDto pedido)
            {
                CarregarPedidoNoFormulario(pedido);
            }
        }

        private void ComboStatus_Format(object? sender, ListControlConvertEventArgs e)
        {
            if (e.ListItem is StatusVendaDto status)
            {
                e.Value = ObterStatusLabel(status);
            }
        }

        private void ComboPagamento_Format(object? sender, ListControlConvertEventArgs e)
        {
            if (e.ListItem is TipoPagamentoDto pagamento)
            {
                e.Value = pagamento switch
                {
                    TipoPagamentoDto.Dinheiro => "Dinheiro",
                    TipoPagamentoDto.Pix => "Pix",
                    TipoPagamentoDto.Cartao => "Cartão",
                    TipoPagamentoDto.Fiado => "Fiado",
                    _ => pagamento.ToString()
                };
            }
        }

        private static string ObterStatusLabel(StatusVendaDto status) => status switch
         {
             StatusVendaDto.Pendente => "🕒 Pendente",
             StatusVendaDto.Preparando => "🥣 Em andamento",
             StatusVendaDto.SaiuParaEntrega => "✅ Pronto",
             StatusVendaDto.Entregue => "🚀 Entregue",
             StatusVendaDto.Cancelado => "❌ Cancelado",
             _ => status.ToString()
         };

        private static Color ObterCorStatus(StatusVendaDto status) => status switch
        {
            StatusVendaDto.Pendente => Color.FromArgb(255, 193, 7), // Yellow
            StatusVendaDto.Preparando => Color.FromArgb(155, 89, 182), // Purple
            StatusVendaDto.SaiuParaEntrega => Color.FromArgb(40, 167, 69), // Green
            StatusVendaDto.Entregue => Color.FromArgb(0, 123, 255), // Blue
            StatusVendaDto.Cancelado => Color.FromArgb(220, 53, 69), // Red
            _ => Color.Gray
        };

        private sealed record ClienteOption(int Id, string Nome);
    }
}
