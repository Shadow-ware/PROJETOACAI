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
        private bool _criandoNovo;
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
        }

        private void InicializarControles()
        {
            dtpData.CustomFormat = "dd/MM/yyyy HH:mm";
            cboStatus.DataSource = Enum.GetValues<StatusVendaDto>();
            cboPagamento.DataSource = Enum.GetValues<TipoPagamentoDto>();
            cboStatus.Format += ComboStatus_Format;
            cboPagamento.Format += ComboPagamento_Format;
            lblApiInfo.Text = $"API: {_pedidosApiService.BaseUrl}";
        }

        private async Task CarregarDadosAsync(int? pedidoParaSelecionar = null)
        {
            try
            {
                AlternarCarregamento(true);

                var clientesTask = _pedidosApiService.GetClientesAsync();
                var pedidosTask = _pedidosApiService.GetPedidosAsync();

                await Task.WhenAll(clientesTask, pedidosTask);

                _clientes = clientesTask.Result.OrderBy(c => c.Nome).ToList();
                _pedidos = pedidosTask.Result.OrderByDescending(p => p.DataVenda).ToList();

                PopularClientes();
                PopularGrid();
                AtualizarResumo();

                var pedidoSelecionado = pedidoParaSelecionar.HasValue
                    ? _pedidos.FirstOrDefault(p => p.Id == pedidoParaSelecionar.Value)
                    : _pedidos.FirstOrDefault();

                if (pedidoSelecionado != null)
                {
                    CarregarPedidoNoFormulario(pedidoSelecionado);
                    SelecionarPedidoNaGrid(pedidoSelecionado.Id);
                }
                else
                {
                    PrepararNovoPedido();
                }
            }
            catch (Exception ex)
            {
                lblResumo.Text = "API indisponível";
                MessageBox.Show(
                    $"Não foi possível conectar à API de pedidos em {_pedidosApiService.BaseUrl}\n\n{ex.Message}",
                    "Falha ao carregar pedidos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                AlternarCarregamento(false);
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

                row.Cells[colEditar.Name].Style.BackColor = Color.FromArgb(239, 246, 255);
                row.Cells[colEditar.Name].Style.ForeColor = Color.FromArgb(37, 99, 235);
                row.Cells[colExcluir.Name].Style.BackColor = Color.FromArgb(254, 242, 242);
                row.Cells[colExcluir.Name].Style.ForeColor = Color.FromArgb(220, 38, 38);
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
                _criandoNovo = false;
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
                btnSalvar.Text = "Salvar alterações";
                btnExcluir.Enabled = true;
            }
            finally
            {
                _carregandoFormulario = false;
            }
        }

        private void PrepararNovoPedido()
        {
            _carregandoFormulario = true;

            try
            {
                _criandoNovo = true;
                lblEditorTitulo.Text = "Novo pedido";
                txtCodigo.Text = "Novo";
                dtpData.Value = DateTime.Now;
                cboStatus.SelectedItem = StatusVendaDto.Pendente;
                cboPagamento.SelectedItem = TipoPagamentoDto.Dinheiro;
                cboCliente.SelectedValue = 0;
                txtValor.Text = "0,00";
                txtEndereco.Text = string.Empty;
                txtBairro.Text = string.Empty;
                txtObservacao.Text = string.Empty;
                btnSalvar.Text = "Criar pedido";
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
            btnNovo.Enabled = !carregando;
            btnSalvar.Enabled = !carregando;
            btnExcluir.Enabled = !carregando && !_criandoNovo;
        }

        private async void btnAtualizar_Click(object sender, EventArgs e)
        {
            await CarregarDadosAsync();
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            PrepararNovoPedido();
        }

        private async void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                var pedido = ConstruirPedidoDoFormulario();
                AlternarCarregamento(true);

                if (_criandoNovo)
                {
                    var created = await _pedidosApiService.CreatePedidoAsync(pedido);
                    MessageBox.Show("Pedido criado com sucesso.", "Pedidos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await CarregarDadosAsync(created.Id);
                }
                else
                {
                    pedido.Id = int.Parse(txtCodigo.Text);
                    await _pedidosApiService.UpdatePedidoAsync(pedido);
                    MessageBox.Show($"Pedido #{pedido.Id} atualizado com sucesso.", "Pedidos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await CarregarDadosAsync(pedido.Id);
                }
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
            if (_criandoNovo || !int.TryParse(txtCodigo.Text, out var id))
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

        private static string ObterStatusLabel(StatusVendaDto status)
        {
            return status switch
            {
                StatusVendaDto.Pendente => "Novo pedido",
                StatusVendaDto.Preparando => "Em andamento",
                StatusVendaDto.SaiuParaEntrega => "Saiu para entrega",
                StatusVendaDto.Entregue => "Entregue",
                StatusVendaDto.Cancelado => "Cancelado",
                _ => "Novo pedido"
            };
        }

        private static Color ObterCorStatus(StatusVendaDto status)
        {
            return status switch
            {
                StatusVendaDto.Pendente => Color.FromArgb(250, 204, 21),
                StatusVendaDto.Preparando => Color.FromArgb(56, 189, 248),
                StatusVendaDto.SaiuParaEntrega => Color.FromArgb(59, 130, 246),
                StatusVendaDto.Entregue => Color.FromArgb(34, 197, 94),
                StatusVendaDto.Cancelado => Color.FromArgb(239, 68, 68),
                _ => Color.FromArgb(148, 163, 184)
            };
        }

        private sealed record ClienteOption(int Id, string Nome);
    }
}
