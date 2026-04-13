#nullable enable
using Guna.UI2.WinForms;

namespace acaiGalatico.UI.Forms
{
    partial class frmPedidos
    {
        private System.ComponentModel.IContainer? components = null;
        private Guna2BorderlessForm borderlessForm;
        private Guna2ShadowForm shadowForm;
        private Guna2Panel pnlPagina;
        private Guna2Panel pnlCard;
        private Guna2Panel pnlHeader;
        private Guna2HtmlLabel lblTitulo;
        private Guna2HtmlLabel lblDescricao;
        private Guna2HtmlLabel lblResumo;
        private Guna2HtmlLabel lblApiInfo;
        private Guna2Button btnAtualizar;
        private SplitContainer splitMain;
        private Guna2Panel pnlTabela;
        private Guna2Panel pnlEditor;
        private Guna2DataGridView dgvPedidos;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colData;
        private DataGridViewTextBoxColumn colStatus;
        private DataGridViewTextBoxColumn colCliente;
        private DataGridViewTextBoxColumn colTotal;
        private DataGridViewButtonColumn colEditar;
        private DataGridViewButtonColumn colExcluir;
        private Guna2HtmlLabel lblEditorTitulo;
        private Guna2HtmlLabel lblCodigo;
        private Guna2TextBox txtCodigo;
        private Guna2HtmlLabel lblData;
        private Guna2DateTimePicker dtpData;
        private Guna2HtmlLabel lblStatus;
        private Guna2ComboBox cboStatus;
        private Guna2HtmlLabel lblCliente;
        private Guna2ComboBox cboCliente;
        private Guna2HtmlLabel lblValor;
        private Guna2TextBox txtValor;
        private Guna2HtmlLabel lblPagamento;
        private Guna2ComboBox cboPagamento;
        private Guna2HtmlLabel lblEndereco;
        private Guna2TextBox txtEndereco;
        private Guna2HtmlLabel lblBairro;
        private Guna2TextBox txtBairro;
        private Guna2HtmlLabel lblObservacao;
        private Guna2TextBox txtObservacao;
        private Guna2HtmlLabel lblItens;
        private Guna2TextBox txtItens;
        private Guna2Button btnFecharPopup;
        private Guna2Button btnSalvar;
        private Guna2Button btnExcluir;

        private System.Windows.Forms.Timer tmrAtualizacao;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            borderlessForm = new Guna2BorderlessForm(components);
            shadowForm = new Guna2ShadowForm(components);
            tmrAtualizacao = new System.Windows.Forms.Timer(components);
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            pnlPagina = new Guna2Panel();
            pnlCard = new Guna2Panel();
            splitMain = new SplitContainer();
            pnlTabela = new Guna2Panel();
            dgvPedidos = new Guna2DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colData = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            colCliente = new DataGridViewTextBoxColumn();
            colTotal = new DataGridViewTextBoxColumn();
            colEditar = new DataGridViewButtonColumn();
            colExcluir = new DataGridViewButtonColumn();
            pnlEditor = new Guna2Panel();
            btnExcluir = new Guna2Button();
            btnSalvar = new Guna2Button();
            txtObservacao = new Guna2TextBox();
            lblObservacao = new Guna2HtmlLabel();
            txtBairro = new Guna2TextBox();
            lblBairro = new Guna2HtmlLabel();
            txtEndereco = new Guna2TextBox();
            lblEndereco = new Guna2HtmlLabel();
            cboPagamento = new Guna2ComboBox();
            lblPagamento = new Guna2HtmlLabel();
            txtValor = new Guna2TextBox();
            lblValor = new Guna2HtmlLabel();
            cboCliente = new Guna2ComboBox();
            lblCliente = new Guna2HtmlLabel();
            cboStatus = new Guna2ComboBox();
            lblStatus = new Guna2HtmlLabel();
            dtpData = new Guna2DateTimePicker();
            lblData = new Guna2HtmlLabel();
            txtCodigo = new Guna2TextBox();
            lblCodigo = new Guna2HtmlLabel();
            lblEditorTitulo = new Guna2HtmlLabel();
            lblItens = new Guna2HtmlLabel();
            txtItens = new Guna2TextBox();
            pnlHeader = new Guna2Panel();
            btnAtualizar = new Guna2Button();
            btnFecharPopup = new Guna2Button();
            lblApiInfo = new Guna2HtmlLabel();
            lblResumo = new Guna2HtmlLabel();
            lblDescricao = new Guna2HtmlLabel();
            lblTitulo = new Guna2HtmlLabel();
            pnlPagina.SuspendLayout();
            pnlCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            pnlTabela.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPedidos).BeginInit();
            pnlEditor.SuspendLayout();
            pnlHeader.SuspendLayout();
            SuspendLayout();
            // 
            // tmrAtualizacao
            // 
            tmrAtualizacao.Interval = 10000;
            tmrAtualizacao.Tick += tmrAtualizacao_Tick;
            // 
            // pnlPagina
            // 
            pnlPagina.BackColor = Color.FromArgb(245, 243, 255); // #f5f3ff
            pnlPagina.Controls.Add(pnlCard);
            pnlPagina.Dock = DockStyle.Fill;
            pnlPagina.FillColor = Color.FromArgb(245, 243, 255);
            pnlPagina.Location = new Point(0, 0);
            pnlPagina.Name = "pnlPagina";
            pnlPagina.Padding = new Padding(24);
            pnlPagina.Size = new Size(1140, 694);
            pnlPagina.TabIndex = 0;
            // 
            // pnlCard
            // 
            pnlCard.BorderRadius = 18;
            pnlCard.Controls.Add(splitMain);
            pnlCard.Controls.Add(pnlHeader);
            pnlCard.Dock = DockStyle.Fill;
            pnlCard.FillColor = Color.White;
            pnlCard.Location = new Point(24, 24);
            pnlCard.Name = "pnlCard";
            pnlCard.ShadowDecoration.BorderRadius = 18;
            pnlCard.ShadowDecoration.Depth = 10;
            pnlCard.ShadowDecoration.Enabled = true;
            pnlCard.Size = new Size(1092, 646);
            pnlCard.TabIndex = 0;
            splitMain.Dock = DockStyle.Fill;
            splitMain.Location = new Point(0, 122);
            splitMain.Name = "splitMain";
            splitMain.Panel1.Controls.Add(pnlTabela);
            splitMain.Panel2.Controls.Add(pnlEditor);
            splitMain.Size = new Size(1092, 524);
            splitMain.SplitterDistance = 650;
            splitMain.TabIndex = 1;
            pnlTabela.Controls.Add(dgvPedidos);
            pnlTabela.Dock = DockStyle.Fill;
            pnlTabela.FillColor = Color.Transparent;
            pnlTabela.Location = new Point(0, 0);
            pnlTabela.Padding = new Padding(18, 0, 12, 18);
            pnlTabela.Size = new Size(650, 524);
            pnlTabela.TabIndex = 0;
            dgvPedidos.AllowUserToAddRows = false;
            dgvPedidos.AllowUserToDeleteRows = false;
            dgvPedidos.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(249, 247, 255);
            dgvPedidos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvPedidos.BackgroundColor = Color.White;
            dgvPedidos.BorderStyle = BorderStyle.None;
            dgvPedidos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvPedidos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(155, 89, 182);
            dataGridViewCellStyle2.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(155, 89, 182);
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvPedidos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvPedidos.ColumnHeadersHeight = 42;
            dgvPedidos.Columns.AddRange(new DataGridViewColumn[] { colId, colData, colStatus, colCliente, colTotal, colEditar, colExcluir });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9.5F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(43, 52, 69);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(237, 242, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(43, 52, 69);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvPedidos.DefaultCellStyle = dataGridViewCellStyle3;
            dgvPedidos.Dock = DockStyle.Fill;
            dgvPedidos.GridColor = Color.FromArgb(233, 238, 245);
            dgvPedidos.Location = new Point(18, 0);
            dgvPedidos.MultiSelect = false;
            dgvPedidos.Name = "dgvPedidos";
            dgvPedidos.ReadOnly = true;
            dgvPedidos.RowHeadersVisible = false;
            dgvPedidos.RowTemplate.Height = 44;
            dgvPedidos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPedidos.Size = new Size(620, 506);
            dgvPedidos.TabIndex = 0;
            dgvPedidos.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(249, 247, 255);
            dgvPedidos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPedidos.ThemeStyle.AlternatingRowsStyle.Font = null;
            dgvPedidos.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dgvPedidos.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dgvPedidos.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dgvPedidos.ThemeStyle.BackColor = Color.White;
            dgvPedidos.ThemeStyle.GridColor = Color.FromArgb(233, 238, 245);
            dgvPedidos.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(155, 89, 182);
            dgvPedidos.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvPedidos.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            dgvPedidos.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvPedidos.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvPedidos.ThemeStyle.HeaderStyle.Height = 42;
            dgvPedidos.ThemeStyle.ReadOnly = true;
            dgvPedidos.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvPedidos.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvPedidos.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9.5F);
            dgvPedidos.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(43, 52, 69);
            dgvPedidos.ThemeStyle.RowsStyle.Height = 44;
            dgvPedidos.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(237, 242, 255);
            dgvPedidos.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(43, 52, 69);
            dgvPedidos.CellClick += dgvPedidos_CellClick;
            dgvPedidos.SelectionChanged += dgvPedidos_SelectionChanged;
            colId.FillWeight = 75F;
            colId.HeaderText = "ID";
            colId.Name = "colId";
            colId.ReadOnly = true;
            colData.FillWeight = 130F;
            colData.HeaderText = "DATA";
            colData.Name = "colData";
            colData.ReadOnly = true;
            colStatus.FillWeight = 120F;
            colStatus.HeaderText = "STATUS";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            colCliente.FillWeight = 150F;
            colCliente.HeaderText = "CLIENTE";
            colCliente.Name = "colCliente";
            colCliente.ReadOnly = true;
            colTotal.FillWeight = 120F;
            colTotal.HeaderText = "TOTAL";
            colTotal.Name = "colTotal";
            colTotal.ReadOnly = true;
            colEditar.FillWeight = 80F;
            colEditar.HeaderText = "";
            colEditar.Name = "colEditar";
            colEditar.ReadOnly = true;
            colEditar.Text = "Editar";
            colEditar.UseColumnTextForButtonValue = true;
            colExcluir.FillWeight = 80F;
            colExcluir.HeaderText = "";
            colExcluir.Name = "colExcluir";
            colExcluir.ReadOnly = true;
            colExcluir.Text = "Excluir";
            colExcluir.UseColumnTextForButtonValue = true;
            pnlEditor.BackColor = Color.Transparent;
            pnlEditor.Controls.Add(txtItens);
            pnlEditor.Controls.Add(lblItens);
            pnlEditor.Controls.Add(btnExcluir);
            pnlEditor.Controls.Add(btnSalvar);
            pnlEditor.Controls.Add(txtObservacao);
            pnlEditor.Controls.Add(lblObservacao);
            pnlEditor.Controls.Add(txtBairro);
            pnlEditor.Controls.Add(lblBairro);
            pnlEditor.Controls.Add(txtEndereco);
            pnlEditor.Controls.Add(lblEndereco);
            pnlEditor.Controls.Add(cboPagamento);
            pnlEditor.Controls.Add(lblPagamento);
            pnlEditor.Controls.Add(txtValor);
            pnlEditor.Controls.Add(lblValor);
            pnlEditor.Controls.Add(cboCliente);
            pnlEditor.Controls.Add(lblCliente);
            pnlEditor.Controls.Add(cboStatus);
            pnlEditor.Controls.Add(lblStatus);
            pnlEditor.Controls.Add(dtpData);
            pnlEditor.Controls.Add(lblData);
            pnlEditor.Controls.Add(txtCodigo);
            pnlEditor.Controls.Add(lblCodigo);
            pnlEditor.Controls.Add(lblEditorTitulo);
            pnlEditor.Dock = DockStyle.Fill;
            pnlEditor.FillColor = Color.White;
            pnlEditor.Location = new Point(0, 0);
            pnlEditor.Name = "pnlEditor";
            pnlEditor.Padding = new Padding(14, 0, 18, 18);
            pnlEditor.Size = new Size(442, 524);
            pnlEditor.TabIndex = 0;
            pnlEditor.AutoScroll = true;
            // 
            // lblEditorTitulo
            // 
            lblEditorTitulo.BackColor = Color.Transparent;
            lblEditorTitulo.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold);
            lblEditorTitulo.ForeColor = Color.FromArgb(26, 36, 56);
            lblEditorTitulo.Location = new Point(24, 8);
            lblEditorTitulo.Name = "lblEditorTitulo";
            lblEditorTitulo.Size = new Size(182, 30);
            lblEditorTitulo.TabIndex = 0;
            lblEditorTitulo.Text = "Detalhes do pedido";
            // 
            // lblCodigo
            // 
            lblCodigo.BackColor = Color.Transparent;
            lblCodigo.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblCodigo.ForeColor = Color.FromArgb(70, 81, 102);
            lblCodigo.Location = new Point(24, 46);
            lblCodigo.Name = "lblCodigo";
            lblCodigo.Size = new Size(52, 17);
            lblCodigo.TabIndex = 1;
            lblCodigo.Text = "Pedido #";
            // 
            // txtCodigo
            // 
            txtCodigo.BorderRadius = 10;
            txtCodigo.Cursor = Cursors.IBeam;
            txtCodigo.DefaultText = "";
            txtCodigo.Font = new Font("Segoe UI", 10F);
            txtCodigo.Location = new Point(24, 66);
            txtCodigo.Name = "txtCodigo";
            txtCodigo.ReadOnly = true;
            txtCodigo.Size = new Size(180, 40);
            txtCodigo.TabIndex = 2;
            // 
            // lblData
            // 
            lblData.BackColor = Color.Transparent;
            lblData.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblData.ForeColor = Color.FromArgb(70, 81, 102);
            lblData.Location = new Point(230, 46);
            lblData.Name = "lblData";
            lblData.Size = new Size(28, 17);
            lblData.TabIndex = 3;
            lblData.Text = "Data";
            // 
            // dtpData
            // 
            dtpData.BorderRadius = 10;
            dtpData.Checked = true;
            dtpData.FillColor = Color.FromArgb(244, 247, 250);
            dtpData.Font = new Font("Segoe UI", 9.75F);
            dtpData.Format = DateTimePickerFormat.Custom;
            dtpData.Location = new Point(230, 66);
            dtpData.Name = "dtpData";
            dtpData.Size = new Size(180, 40);
            dtpData.TabIndex = 4;
            // 
            // lblStatus
            // 
            lblStatus.BackColor = Color.Transparent;
            lblStatus.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblStatus.ForeColor = Color.FromArgb(70, 81, 102);
            lblStatus.Location = new Point(24, 116);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(36, 17);
            lblStatus.TabIndex = 5;
            lblStatus.Text = "Status";
            // 
            // cboStatus
            // 
            cboStatus.BackColor = Color.Transparent;
            cboStatus.BorderRadius = 10;
            cboStatus.DrawMode = DrawMode.OwnerDrawFixed;
            cboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cboStatus.Font = new Font("Segoe UI", 10F);
            cboStatus.ItemHeight = 34;
            cboStatus.Location = new Point(24, 136);
            cboStatus.Name = "cboStatus";
            cboStatus.Size = new Size(180, 40);
            cboStatus.TabIndex = 6;
            // 
            // lblCliente
            // 
            lblCliente.BackColor = Color.Transparent;
            lblCliente.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblCliente.ForeColor = Color.FromArgb(70, 81, 102);
            lblCliente.Location = new Point(230, 116);
            lblCliente.Name = "lblCliente";
            lblCliente.Size = new Size(40, 17);
            lblCliente.TabIndex = 7;
            lblCliente.Text = "Cliente";
            // 
            // cboCliente
            // 
            cboCliente.BackColor = Color.Transparent;
            cboCliente.BorderRadius = 10;
            cboCliente.DrawMode = DrawMode.OwnerDrawFixed;
            cboCliente.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCliente.Font = new Font("Segoe UI", 10F);
            cboCliente.ItemHeight = 34;
            cboCliente.Location = new Point(230, 136);
            cboCliente.Name = "cboCliente";
            cboCliente.Size = new Size(180, 40);
            cboCliente.TabIndex = 8;
            // 
            // lblValor
            // 
            lblValor.BackColor = Color.Transparent;
            lblValor.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblValor.ForeColor = Color.FromArgb(70, 81, 102);
            lblValor.Location = new Point(24, 186);
            lblValor.Name = "lblValor";
            lblValor.Size = new Size(30, 17);
            lblValor.TabIndex = 9;
            lblValor.Text = "Valor";
            // 
            // txtValor
            // 
            txtValor.BorderRadius = 10;
            txtValor.Cursor = Cursors.IBeam;
            txtValor.DefaultText = "";
            txtValor.Font = new Font("Segoe UI", 10F);
            txtValor.Location = new Point(24, 206);
            txtValor.Name = "txtValor";
            txtValor.PlaceholderText = "0,00";
            txtValor.Size = new Size(180, 40);
            txtValor.TabIndex = 10;
            // 
            // lblPagamento
            // 
            lblPagamento.BackColor = Color.Transparent;
            lblPagamento.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblPagamento.ForeColor = Color.FromArgb(70, 81, 102);
            lblPagamento.Location = new Point(230, 186);
            lblPagamento.Name = "lblPagamento";
            lblPagamento.Size = new Size(64, 17);
            lblPagamento.TabIndex = 11;
            lblPagamento.Text = "Pagamento";
            // 
            // cboPagamento
            // 
            cboPagamento.BackColor = Color.Transparent;
            cboPagamento.BorderRadius = 10;
            cboPagamento.DrawMode = DrawMode.OwnerDrawFixed;
            cboPagamento.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPagamento.Font = new Font("Segoe UI", 10F);
            cboPagamento.ItemHeight = 34;
            cboPagamento.Location = new Point(230, 206);
            cboPagamento.Name = "cboPagamento";
            cboPagamento.Size = new Size(180, 40);
            cboPagamento.TabIndex = 12;
            // 
            // lblEndereco
            // 
            lblEndereco.BackColor = Color.Transparent;
            lblEndereco.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblEndereco.ForeColor = Color.FromArgb(70, 81, 102);
            lblEndereco.Location = new Point(24, 256);
            lblEndereco.Name = "lblEndereco";
            lblEndereco.Size = new Size(53, 17);
            lblEndereco.TabIndex = 13;
            lblEndereco.Text = "Endereço";
            // 
            // txtEndereco
            // 
            txtEndereco.BorderRadius = 10;
            txtEndereco.Cursor = Cursors.IBeam;
            txtEndereco.DefaultText = "";
            txtEndereco.Font = new Font("Segoe UI", 10F);
            txtEndereco.Location = new Point(24, 276);
            txtEndereco.Name = "txtEndereco";
            txtEndereco.PlaceholderText = "Rua, número...";
            txtEndereco.Size = new Size(180, 40);
            txtEndereco.TabIndex = 14;
            // 
            // lblBairro
            // 
            lblBairro.BackColor = Color.Transparent;
            lblBairro.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblBairro.ForeColor = Color.FromArgb(70, 81, 102);
            lblBairro.Location = new Point(230, 256);
            lblBairro.Name = "lblBairro";
            lblBairro.Size = new Size(35, 17);
            lblBairro.TabIndex = 15;
            lblBairro.Text = "Bairro";
            // 
            // txtBairro
            // 
            txtBairro.BorderRadius = 10;
            txtBairro.Cursor = Cursors.IBeam;
            txtBairro.DefaultText = "";
            txtBairro.Font = new Font("Segoe UI", 10F);
            txtBairro.Location = new Point(230, 276);
            txtBairro.Name = "txtBairro";
            txtBairro.PlaceholderText = "Bairro";
            txtBairro.Size = new Size(180, 40);
            txtBairro.TabIndex = 16;
            // 
            // lblObservacao
            // 
            lblObservacao.BackColor = Color.Transparent;
            lblObservacao.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblObservacao.ForeColor = Color.FromArgb(70, 81, 102);
            lblObservacao.Location = new Point(24, 326);
            lblObservacao.Name = "lblObservacao";
            lblObservacao.Size = new Size(65, 17);
            lblObservacao.TabIndex = 17;
            lblObservacao.Text = "Observação";
            // 
            // txtObservacao
            // 
            txtObservacao.BorderRadius = 10;
            txtObservacao.Cursor = Cursors.IBeam;
            txtObservacao.DefaultText = "";
            txtObservacao.Font = new Font("Segoe UI", 10F);
            txtObservacao.Location = new Point(24, 346);
            txtObservacao.Multiline = true;
            txtObservacao.Name = "txtObservacao";
            txtObservacao.PlaceholderText = "Detalhes do pedido";
            txtObservacao.Size = new Size(386, 52);
            txtObservacao.TabIndex = 18;
            // 
            // lblItens
            // 
            lblItens.BackColor = Color.Transparent;
            lblItens.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblItens.ForeColor = Color.FromArgb(70, 81, 102);
            lblItens.Location = new Point(24, 408);
            lblItens.Name = "lblItens";
            lblItens.Size = new Size(93, 17);
            lblItens.TabIndex = 22;
            lblItens.Text = "Itens Detalhados";
            // 
            // txtItens
            // 
            txtItens.BorderRadius = 10;
            txtItens.Cursor = Cursors.IBeam;
            txtItens.DefaultText = "";
            txtItens.Font = new Font("Segoe UI", 9F);
            txtItens.Location = new Point(24, 428);
            txtItens.Multiline = true;
            txtItens.Name = "txtItens";
            txtItens.PlaceholderText = "Nenhum item detalhado.";
            txtItens.ReadOnly = true;
            txtItens.ScrollBars = ScrollBars.Vertical;
            txtItens.Size = new Size(386, 80);
            txtItens.TabIndex = 21;
            // 
            // btnSalvar
            // 
            btnSalvar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSalvar.BorderRadius = 10;
            btnSalvar.FillColor = Color.FromArgb(155, 89, 182); // #9b59b2
            btnSalvar.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnSalvar.ForeColor = Color.White;
            btnSalvar.Location = new Point(24, 520);
            btnSalvar.Name = "btnSalvar";
            btnSalvar.Size = new Size(180, 40);
            btnSalvar.TabIndex = 19;
            btnSalvar.Text = "Salvar alterações";
            btnSalvar.Click += btnSalvar_Click;
            // 
            // btnExcluir
            // 
            btnExcluir.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnExcluir.BorderRadius = 10;
            btnExcluir.FillColor = Color.FromArgb(231, 76, 60); // #e74c3c
            btnExcluir.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnExcluir.ForeColor = Color.White;
            btnExcluir.Location = new Point(230, 520);
            btnExcluir.Name = "btnExcluir";
            btnExcluir.Size = new Size(180, 40);
            btnExcluir.TabIndex = 20;
            btnExcluir.Text = "Excluir pedido";
            btnExcluir.Click += btnExcluir_Click;
            // 
            // pnlHeader
            // 
            pnlHeader.Controls.Add(btnFecharPopup);
            pnlHeader.Controls.Add(btnAtualizar);
            pnlHeader.Controls.Add(lblApiInfo);
            pnlHeader.Controls.Add(lblResumo);
            pnlHeader.Controls.Add(lblDescricao);
            pnlHeader.Controls.Add(lblTitulo);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.FillColor = Color.Transparent;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1092, 122);
            pnlHeader.TabIndex = 0;
            // 
            // btnAtualizar
            // 
            btnAtualizar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAtualizar.BorderRadius = 10;
            btnAtualizar.FillColor = Color.FromArgb(155, 89, 182);
            btnAtualizar.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnAtualizar.ForeColor = Color.White;
            btnAtualizar.Location = new Point(860, 68);
            btnAtualizar.Name = "btnAtualizar";
            btnAtualizar.Size = new Size(204, 40);
            btnAtualizar.TabIndex = 5;
            btnAtualizar.Text = "Atualizar";
            btnAtualizar.Click += btnAtualizar_Click;
            // 
            // btnFecharPopup
            // 
            btnFecharPopup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnFecharPopup.BorderRadius = 10;
            btnFecharPopup.FillColor = Color.FromArgb(231, 76, 60); // #e74c3c
            btnFecharPopup.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnFecharPopup.ForeColor = Color.White;
            btnFecharPopup.Location = new Point(1020, 20);
            btnFecharPopup.Name = "btnFecharPopup";
            btnFecharPopup.Size = new Size(44, 40);
            btnFecharPopup.TabIndex = 6;
            btnFecharPopup.Text = "X";
            btnFecharPopup.Visible = false;
            btnFecharPopup.Click += (s, e) => this.Close();
            // 
            // lblApiInfo
            // 
            lblDescricao.BackColor = Color.Transparent;
            lblDescricao.ForeColor = Color.FromArgb(107, 114, 128);
            lblDescricao.Location = new Point(24, 60);
            lblDescricao.Name = "lblDescricao";
            lblDescricao.Size = new Size(281, 19);
            lblDescricao.TabIndex = 1;
            lblDescricao.Text = "Visualize e gerencie os pedidos feitos pelos usuários";
            lblTitulo.BackColor = Color.Transparent;
            lblTitulo.Font = new Font("Segoe UI Bold", 20F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(48, 43, 99);
            lblTitulo.Location = new Point(24, 15);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(230, 39);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Gestão de Pedidos";
            // 
            // lblResumo
            // 
            lblResumo.BackColor = Color.Transparent;
            lblResumo.ForeColor = Color.FromArgb(34, 197, 94);
            lblResumo.Location = new Point(24, 85);
            lblResumo.Name = "lblResumo";
            lblResumo.Size = new Size(104, 19);
            lblResumo.TabIndex = 2;
            lblResumo.Text = "Carregando dados";
            // 
            // lblApiInfo
            // 
            lblApiInfo.ForeColor = Color.FromArgb(107, 114, 128);
            lblApiInfo.Location = new Point(24, 105);
            lblApiInfo.Name = "lblApiInfo";
            lblApiInfo.Size = new Size(127, 19);
            lblApiInfo.TabIndex = 3;
            lblApiInfo.Text = "API: http://localhost:5207/";
            // 
            // borderlessForm
            // 
            borderlessForm.BorderRadius = 24;
            borderlessForm.ContainerControl = this;
            borderlessForm.DockIndicatorTransparencyValue = 0.6D;
            borderlessForm.TransparentWhileDrag = true;
            // 
            // shadowForm
            // 
            shadowForm.TargetForm = this;
            // 
            // AutoScaleDimensions
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(850, 600);
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            Controls.Add(pnlPagina);
            Name = "frmPedidos";
            Text = "frmPedidos";
            pnlPagina.ResumeLayout(false);
            pnlCard.ResumeLayout(false);
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            pnlTabela.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPedidos).EndInit();
            pnlEditor.ResumeLayout(false);
            pnlEditor.PerformLayout();
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ResumeLayout(false);
        }
    }
}
