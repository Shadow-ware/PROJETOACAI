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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges37 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges38 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges35 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges36 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges27 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges28 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges17 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges18 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges19 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges20 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges21 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges22 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges23 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges24 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges25 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges26 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges33 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges34 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges29 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges30 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges31 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges32 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            borderlessForm = new Guna2BorderlessForm(components);
            shadowForm = new Guna2ShadowForm(components);
            tmrAtualizacao = new System.Windows.Forms.Timer(components);
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
            txtItens = new Guna2TextBox();
            lblItens = new Guna2HtmlLabel();
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
            pnlHeader = new Guna2Panel();
            btnFecharPopup = new Guna2Button();
            btnAtualizar = new Guna2Button();
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
            // tmrAtualizacao
            // 
            tmrAtualizacao.Interval = 10000;
            tmrAtualizacao.Tick += tmrAtualizacao_Tick;
            // 
            // pnlPagina
            // 
            pnlPagina.BackColor = Color.FromArgb(245, 243, 255);
            pnlPagina.Controls.Add(pnlCard);
            pnlPagina.CustomizableEdges = customizableEdges37;
            pnlPagina.Dock = DockStyle.Fill;
            pnlPagina.FillColor = Color.FromArgb(245, 243, 255);
            pnlPagina.Location = new Point(0, 0);
            pnlPagina.Name = "pnlPagina";
            pnlPagina.Padding = new Padding(24);
            pnlPagina.ShadowDecoration.CustomizableEdges = customizableEdges38;
            pnlPagina.Size = new Size(1098, 600);
            pnlPagina.TabIndex = 0;
            // 
            // pnlCard
            // 
            pnlCard.BackColor = Color.Transparent;
            pnlCard.BorderRadius = 18;
            pnlCard.Controls.Add(splitMain);
            pnlCard.Controls.Add(pnlHeader);
            pnlCard.CustomizableEdges = customizableEdges35;
            pnlCard.Dock = DockStyle.Fill;
            pnlCard.FillColor = Color.White;
            pnlCard.Location = new Point(24, 24);
            pnlCard.Name = "pnlCard";
            pnlCard.ShadowDecoration.BorderRadius = 18;
            pnlCard.ShadowDecoration.CustomizableEdges = customizableEdges36;
            pnlCard.ShadowDecoration.Depth = 10;
            pnlCard.ShadowDecoration.Enabled = true;
            pnlCard.Size = new Size(1050, 552);
            pnlCard.TabIndex = 0;
            // 
            // splitMain
            // 
            splitMain.Dock = DockStyle.Fill;
            splitMain.Location = new Point(0, 122);
            splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            splitMain.Panel1.Controls.Add(pnlTabela);
            // 
            // splitMain.Panel2
            // 
            splitMain.Panel2.Controls.Add(pnlEditor);
            splitMain.Size = new Size(1050, 430);
            splitMain.SplitterDistance = 625;
            splitMain.TabIndex = 1;
            // 
            // pnlTabela
            // 
            pnlTabela.Controls.Add(dgvPedidos);
            pnlTabela.CustomizableEdges = customizableEdges1;
            pnlTabela.Dock = DockStyle.Fill;
            pnlTabela.FillColor = Color.Transparent;
            pnlTabela.Location = new Point(0, 0);
            pnlTabela.Name = "pnlTabela";
            pnlTabela.Padding = new Padding(18, 0, 12, 18);
            pnlTabela.ShadowDecoration.CustomizableEdges = customizableEdges2;
            pnlTabela.Size = new Size(625, 430);
            pnlTabela.TabIndex = 0;
            // 
            // dgvPedidos
            // 
            dgvPedidos.AllowUserToAddRows = false;
            dgvPedidos.AllowUserToDeleteRows = false;
            dgvPedidos.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(249, 247, 255);
            dgvPedidos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
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
            dgvPedidos.Size = new Size(595, 412);
            dgvPedidos.TabIndex = 0;
            dgvPedidos.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(249, 247, 255);
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
            // 
            // colId
            // 
            colId.FillWeight = 75F;
            colId.HeaderText = "ID";
            colId.Name = "colId";
            colId.ReadOnly = true;
            // 
            // colData
            // 
            colData.FillWeight = 130F;
            colData.HeaderText = "DATA";
            colData.Name = "colData";
            colData.ReadOnly = true;
            // 
            // colStatus
            // 
            colStatus.FillWeight = 120F;
            colStatus.HeaderText = "STATUS";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            // 
            // colCliente
            // 
            colCliente.FillWeight = 150F;
            colCliente.HeaderText = "CLIENTE";
            colCliente.Name = "colCliente";
            colCliente.ReadOnly = true;
            // 
            // colTotal
            // 
            colTotal.FillWeight = 120F;
            colTotal.HeaderText = "TOTAL";
            colTotal.Name = "colTotal";
            colTotal.ReadOnly = true;
            // 
            // colEditar
            // 
            colEditar.FillWeight = 80F;
            colEditar.HeaderText = "";
            colEditar.Name = "colEditar";
            colEditar.ReadOnly = true;
            colEditar.Text = "Editar";
            colEditar.UseColumnTextForButtonValue = true;
            // 
            // colExcluir
            // 
            colExcluir.FillWeight = 80F;
            colExcluir.HeaderText = "";
            colExcluir.Name = "colExcluir";
            colExcluir.ReadOnly = true;
            colExcluir.Text = "Excluir";
            colExcluir.UseColumnTextForButtonValue = true;
            // 
            // pnlEditor
            // 
            pnlEditor.AutoScroll = true;
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
            pnlEditor.CustomizableEdges = customizableEdges27;
            pnlEditor.Dock = DockStyle.Fill;
            pnlEditor.FillColor = Color.White;
            pnlEditor.Location = new Point(0, 0);
            pnlEditor.Name = "pnlEditor";
            pnlEditor.Padding = new Padding(14, 0, 18, 18);
            pnlEditor.ShadowDecoration.CustomizableEdges = customizableEdges28;
            pnlEditor.Size = new Size(421, 430);
            pnlEditor.TabIndex = 0;
            // 
            // txtItens
            // 
            txtItens.BorderRadius = 10;
            txtItens.Cursor = Cursors.IBeam;
            txtItens.CustomizableEdges = customizableEdges3;
            txtItens.DefaultText = "";
            txtItens.Font = new Font("Segoe UI", 9F);
            txtItens.Location = new Point(16, 416);
            txtItens.Multiline = true;
            txtItens.Name = "txtItens";
            txtItens.PlaceholderText = "Nenhum item detalhado.";
            txtItens.ReadOnly = true;
            txtItens.ScrollBars = ScrollBars.Vertical;
            txtItens.SelectedText = "";
            txtItens.ShadowDecoration.CustomizableEdges = customizableEdges4;
            txtItens.Size = new Size(386, 70);
            txtItens.TabIndex = 21;
            // 
            // lblItens
            // 
            lblItens.BackColor = Color.Transparent;
            lblItens.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblItens.ForeColor = Color.FromArgb(70, 81, 102);
            lblItens.Location = new Point(16, 396);
            lblItens.Name = "lblItens";
            lblItens.Size = new Size(92, 17);
            lblItens.TabIndex = 22;
            lblItens.Text = "Itens Detalhados";
            // 
            // btnExcluir
            // 
            btnExcluir.BorderRadius = 10;
            btnExcluir.CustomizableEdges = customizableEdges5;
            btnExcluir.FillColor = Color.FromArgb(231, 76, 60);
            btnExcluir.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnExcluir.ForeColor = Color.White;
            btnExcluir.Location = new Point(219, 500);
            btnExcluir.Name = "btnExcluir";
            btnExcluir.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnExcluir.Size = new Size(180, 40);
            btnExcluir.TabIndex = 20;
            btnExcluir.Text = "Excluir pedido";
            btnExcluir.Click += btnExcluir_Click;
            // 
            // btnSalvar
            // 
            btnSalvar.BorderRadius = 10;
            btnSalvar.CustomizableEdges = customizableEdges7;
            btnSalvar.FillColor = Color.FromArgb(155, 89, 182);
            btnSalvar.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnSalvar.ForeColor = Color.White;
            btnSalvar.Location = new Point(16, 500);
            btnSalvar.Name = "btnSalvar";
            btnSalvar.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnSalvar.Size = new Size(180, 40);
            btnSalvar.TabIndex = 19;
            btnSalvar.Text = "Salvar alterações";
            btnSalvar.Click += btnSalvar_Click;
            // 
            // txtObservacao
            // 
            txtObservacao.BorderRadius = 10;
            txtObservacao.Cursor = Cursors.IBeam;
            txtObservacao.CustomizableEdges = customizableEdges9;
            txtObservacao.DefaultText = "";
            txtObservacao.Font = new Font("Segoe UI", 10F);
            txtObservacao.Location = new Point(14, 346);
            txtObservacao.Multiline = true;
            txtObservacao.Name = "txtObservacao";
            txtObservacao.PlaceholderText = "Detalhes do pedido";
            txtObservacao.SelectedText = "";
            txtObservacao.ShadowDecoration.CustomizableEdges = customizableEdges10;
            txtObservacao.Size = new Size(386, 42);
            txtObservacao.TabIndex = 18;
            // 
            // lblObservacao
            // 
            lblObservacao.BackColor = Color.Transparent;
            lblObservacao.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblObservacao.ForeColor = Color.FromArgb(70, 81, 102);
            lblObservacao.Location = new Point(14, 326);
            lblObservacao.Name = "lblObservacao";
            lblObservacao.Size = new Size(65, 17);
            lblObservacao.TabIndex = 17;
            lblObservacao.Text = "Observação";
            // 
            // txtBairro
            // 
            txtBairro.BorderRadius = 10;
            txtBairro.Cursor = Cursors.IBeam;
            txtBairro.CustomizableEdges = customizableEdges11;
            txtBairro.DefaultText = "";
            txtBairro.Font = new Font("Segoe UI", 10F);
            txtBairro.Location = new Point(217, 276);
            txtBairro.Name = "txtBairro";
            txtBairro.PlaceholderText = "Bairro";
            txtBairro.SelectedText = "";
            txtBairro.ShadowDecoration.CustomizableEdges = customizableEdges12;
            txtBairro.Size = new Size(180, 40);
            txtBairro.TabIndex = 16;
            // 
            // lblBairro
            // 
            lblBairro.BackColor = Color.Transparent;
            lblBairro.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblBairro.ForeColor = Color.FromArgb(70, 81, 102);
            lblBairro.Location = new Point(217, 256);
            lblBairro.Name = "lblBairro";
            lblBairro.Size = new Size(34, 17);
            lblBairro.TabIndex = 15;
            lblBairro.Text = "Bairro";
            // 
            // txtEndereco
            // 
            txtEndereco.BorderRadius = 10;
            txtEndereco.Cursor = Cursors.IBeam;
            txtEndereco.CustomizableEdges = customizableEdges13;
            txtEndereco.DefaultText = "";
            txtEndereco.Font = new Font("Segoe UI", 10F);
            txtEndereco.Location = new Point(14, 276);
            txtEndereco.Name = "txtEndereco";
            txtEndereco.PlaceholderText = "Rua, número...";
            txtEndereco.SelectedText = "";
            txtEndereco.ShadowDecoration.CustomizableEdges = customizableEdges14;
            txtEndereco.Size = new Size(180, 40);
            txtEndereco.TabIndex = 14;
            // 
            // lblEndereco
            // 
            lblEndereco.BackColor = Color.Transparent;
            lblEndereco.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblEndereco.ForeColor = Color.FromArgb(70, 81, 102);
            lblEndereco.Location = new Point(14, 256);
            lblEndereco.Name = "lblEndereco";
            lblEndereco.Size = new Size(52, 17);
            lblEndereco.TabIndex = 13;
            lblEndereco.Text = "Endereço";
            // 
            // cboPagamento
            // 
            cboPagamento.BackColor = Color.Transparent;
            cboPagamento.BorderRadius = 10;
            cboPagamento.CustomizableEdges = customizableEdges15;
            cboPagamento.DrawMode = DrawMode.OwnerDrawFixed;
            cboPagamento.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPagamento.FocusedColor = Color.Empty;
            cboPagamento.Font = new Font("Segoe UI", 10F);
            cboPagamento.ForeColor = Color.FromArgb(68, 88, 112);
            cboPagamento.ItemHeight = 34;
            cboPagamento.Location = new Point(217, 206);
            cboPagamento.Name = "cboPagamento";
            cboPagamento.ShadowDecoration.CustomizableEdges = customizableEdges16;
            cboPagamento.Size = new Size(180, 40);
            cboPagamento.TabIndex = 12;
            // 
            // lblPagamento
            // 
            lblPagamento.BackColor = Color.Transparent;
            lblPagamento.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblPagamento.ForeColor = Color.FromArgb(70, 81, 102);
            lblPagamento.Location = new Point(217, 186);
            lblPagamento.Name = "lblPagamento";
            lblPagamento.Size = new Size(64, 17);
            lblPagamento.TabIndex = 11;
            lblPagamento.Text = "Pagamento";
            // 
            // txtValor
            // 
            txtValor.BorderRadius = 10;
            txtValor.Cursor = Cursors.IBeam;
            txtValor.CustomizableEdges = customizableEdges17;
            txtValor.DefaultText = "";
            txtValor.Font = new Font("Segoe UI", 10F);
            txtValor.Location = new Point(14, 206);
            txtValor.Name = "txtValor";
            txtValor.PlaceholderText = "0,00";
            txtValor.SelectedText = "";
            txtValor.ShadowDecoration.CustomizableEdges = customizableEdges18;
            txtValor.Size = new Size(180, 40);
            txtValor.TabIndex = 10;
            // 
            // lblValor
            // 
            lblValor.BackColor = Color.Transparent;
            lblValor.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblValor.ForeColor = Color.FromArgb(70, 81, 102);
            lblValor.Location = new Point(14, 186);
            lblValor.Name = "lblValor";
            lblValor.Size = new Size(31, 17);
            lblValor.TabIndex = 9;
            lblValor.Text = "Valor";
            // 
            // cboCliente
            // 
            cboCliente.BackColor = Color.Transparent;
            cboCliente.BorderRadius = 10;
            cboCliente.CustomizableEdges = customizableEdges19;
            cboCliente.DrawMode = DrawMode.OwnerDrawFixed;
            cboCliente.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCliente.FocusedColor = Color.Empty;
            cboCliente.Font = new Font("Segoe UI", 10F);
            cboCliente.ForeColor = Color.FromArgb(68, 88, 112);
            cboCliente.ItemHeight = 34;
            cboCliente.Location = new Point(210, 136);
            cboCliente.Name = "cboCliente";
            cboCliente.ShadowDecoration.CustomizableEdges = customizableEdges20;
            cboCliente.Size = new Size(180, 40);
            cboCliente.TabIndex = 8;
            // 
            // lblCliente
            // 
            lblCliente.BackColor = Color.Transparent;
            lblCliente.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblCliente.ForeColor = Color.FromArgb(70, 81, 102);
            lblCliente.Location = new Point(210, 116);
            lblCliente.Name = "lblCliente";
            lblCliente.Size = new Size(39, 17);
            lblCliente.TabIndex = 7;
            lblCliente.Text = "Cliente";
            // 
            // cboStatus
            // 
            cboStatus.BackColor = Color.Transparent;
            cboStatus.BorderRadius = 10;
            cboStatus.CustomizableEdges = customizableEdges21;
            cboStatus.DrawMode = DrawMode.OwnerDrawFixed;
            cboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cboStatus.FocusedColor = Color.Empty;
            cboStatus.Font = new Font("Segoe UI", 10F);
            cboStatus.ForeColor = Color.FromArgb(68, 88, 112);
            cboStatus.ItemHeight = 34;
            cboStatus.Location = new Point(14, 136);
            cboStatus.Name = "cboStatus";
            cboStatus.ShadowDecoration.CustomizableEdges = customizableEdges22;
            cboStatus.Size = new Size(180, 40);
            cboStatus.TabIndex = 6;
            // 
            // lblStatus
            // 
            lblStatus.BackColor = Color.Transparent;
            lblStatus.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblStatus.ForeColor = Color.FromArgb(70, 81, 102);
            lblStatus.Location = new Point(14, 116);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(36, 17);
            lblStatus.TabIndex = 5;
            lblStatus.Text = "Status";
            // 
            // dtpData
            // 
            dtpData.BorderRadius = 10;
            dtpData.Checked = true;
            dtpData.CustomizableEdges = customizableEdges23;
            dtpData.FillColor = Color.FromArgb(244, 247, 250);
            dtpData.Font = new Font("Segoe UI", 9.75F);
            dtpData.Format = DateTimePickerFormat.Custom;
            dtpData.Location = new Point(210, 66);
            dtpData.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
            dtpData.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
            dtpData.Name = "dtpData";
            dtpData.ShadowDecoration.CustomizableEdges = customizableEdges24;
            dtpData.Size = new Size(180, 40);
            dtpData.TabIndex = 4;
            dtpData.Value = new DateTime(2026, 4, 14, 19, 57, 57, 38);
            // 
            // lblData
            // 
            lblData.BackColor = Color.Transparent;
            lblData.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblData.ForeColor = Color.FromArgb(70, 81, 102);
            lblData.Location = new Point(210, 46);
            lblData.Name = "lblData";
            lblData.Size = new Size(28, 17);
            lblData.TabIndex = 3;
            lblData.Text = "Data";
            // 
            // txtCodigo
            // 
            txtCodigo.BorderRadius = 10;
            txtCodigo.Cursor = Cursors.IBeam;
            txtCodigo.CustomizableEdges = customizableEdges25;
            txtCodigo.DefaultText = "";
            txtCodigo.Font = new Font("Segoe UI", 10F);
            txtCodigo.Location = new Point(16, 66);
            txtCodigo.Name = "txtCodigo";
            txtCodigo.PlaceholderText = "";
            txtCodigo.ReadOnly = true;
            txtCodigo.SelectedText = "";
            txtCodigo.ShadowDecoration.CustomizableEdges = customizableEdges26;
            txtCodigo.Size = new Size(180, 40);
            txtCodigo.TabIndex = 2;
            // 
            // lblCodigo
            // 
            lblCodigo.BackColor = Color.Transparent;
            lblCodigo.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblCodigo.ForeColor = Color.FromArgb(70, 81, 102);
            lblCodigo.Location = new Point(17, 46);
            lblCodigo.Name = "lblCodigo";
            lblCodigo.Size = new Size(50, 17);
            lblCodigo.TabIndex = 1;
            lblCodigo.Text = "Pedido #";
            // 
            // lblEditorTitulo
            // 
            lblEditorTitulo.BackColor = Color.Transparent;
            lblEditorTitulo.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold);
            lblEditorTitulo.ForeColor = Color.FromArgb(26, 36, 56);
            lblEditorTitulo.Location = new Point(18, 8);
            lblEditorTitulo.Name = "lblEditorTitulo";
            lblEditorTitulo.Size = new Size(182, 30);
            lblEditorTitulo.TabIndex = 0;
            lblEditorTitulo.Text = "Detalhes do pedido";
            // 
            // pnlHeader
            // 
            pnlHeader.Controls.Add(btnFecharPopup);
            pnlHeader.Controls.Add(btnAtualizar);
            pnlHeader.Controls.Add(lblApiInfo);
            pnlHeader.Controls.Add(lblResumo);
            pnlHeader.Controls.Add(lblDescricao);
            pnlHeader.Controls.Add(lblTitulo);
            pnlHeader.CustomizableEdges = customizableEdges33;
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.FillColor = Color.Transparent;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.ShadowDecoration.CustomizableEdges = customizableEdges34;
            pnlHeader.Size = new Size(1050, 122);
            pnlHeader.TabIndex = 0;
            // 
            // btnFecharPopup
            // 
            btnFecharPopup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnFecharPopup.BorderRadius = 10;
            btnFecharPopup.CustomizableEdges = customizableEdges29;
            btnFecharPopup.FillColor = Color.FromArgb(231, 76, 60);
            btnFecharPopup.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnFecharPopup.ForeColor = Color.White;
            btnFecharPopup.Location = new Point(976, 12);
            btnFecharPopup.Name = "btnFecharPopup";
            btnFecharPopup.ShadowDecoration.CustomizableEdges = customizableEdges30;
            btnFecharPopup.Size = new Size(44, 40);
            btnFecharPopup.TabIndex = 6;
            btnFecharPopup.Text = "X";
            btnFecharPopup.Visible = false;
            // 
            // btnAtualizar
            // 
            btnAtualizar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAtualizar.BorderRadius = 10;
            btnAtualizar.CustomizableEdges = customizableEdges31;
            btnAtualizar.FillColor = Color.FromArgb(155, 89, 182);
            btnAtualizar.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnAtualizar.ForeColor = Color.White;
            btnAtualizar.Location = new Point(818, 66);
            btnAtualizar.Name = "btnAtualizar";
            btnAtualizar.ShadowDecoration.CustomizableEdges = customizableEdges32;
            btnAtualizar.Size = new Size(204, 40);
            btnAtualizar.TabIndex = 5;
            btnAtualizar.Text = "Atualizar";
            btnAtualizar.Click += btnAtualizar_Click;
            // 
            // lblApiInfo
            // 
            lblApiInfo.BackColor = Color.Transparent;
            lblApiInfo.ForeColor = Color.FromArgb(107, 114, 128);
            lblApiInfo.Location = new Point(24, 97);
            lblApiInfo.Name = "lblApiInfo";
            lblApiInfo.Size = new Size(142, 17);
            lblApiInfo.TabIndex = 3;
            lblApiInfo.Text = "API: http://localhost:5207/";
            // 
            // lblResumo
            // 
            lblResumo.BackColor = Color.Transparent;
            lblResumo.ForeColor = Color.FromArgb(34, 197, 94);
            lblResumo.Location = new Point(24, 78);
            lblResumo.Name = "lblResumo";
            lblResumo.Size = new Size(100, 17);
            lblResumo.TabIndex = 2;
            lblResumo.Text = "Carregando dados";
            // 
            // lblDescricao
            // 
            lblDescricao.BackColor = Color.Transparent;
            lblDescricao.ForeColor = Color.FromArgb(107, 114, 128);
            lblDescricao.Location = new Point(24, 57);
            lblDescricao.Name = "lblDescricao";
            lblDescricao.Size = new Size(275, 17);
            lblDescricao.TabIndex = 1;
            lblDescricao.Text = "Visualize e gerencie os pedidos feitos pelos usuários";
            // 
            // lblTitulo
            // 
            lblTitulo.BackColor = Color.Transparent;
            lblTitulo.Font = new Font("Microsoft Sans Serif", 20F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(48, 43, 99);
            lblTitulo.Location = new Point(24, 15);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(250, 33);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Gestão de Pedidos";
            // 
            // frmPedidos
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1098, 600);
            Controls.Add(pnlPagina);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmPedidos";
            StartPosition = FormStartPosition.CenterParent;
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
