namespace acaiGalatico.UI.Forms
{
    partial class frmVendas
    {
        private System.ComponentModel.IContainer components = null;
        private Guna.UI2.WinForms.Guna2Panel pnlHeader;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTitulo;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblStatusFila;
        private Guna.UI2.WinForms.Guna2Button btnAtualizar;
        private System.Windows.Forms.FlowLayoutPanel flowPedidos;
        private System.Windows.Forms.Timer tmrAtualizacao;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTitulo = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblStatusFila = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.btnAtualizar = new Guna.UI2.WinForms.Guna2Button();
            this.flowPedidos = new System.Windows.Forms.FlowLayoutPanel();
            this.tmrAtualizacao = new System.Windows.Forms.Timer(this.components);
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Controls.Add(this.btnAtualizar);
            this.pnlHeader.Controls.Add(this.lblStatusFila);
            this.pnlHeader.Controls.Add(this.lblTitulo);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1000, 70);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblTitulo
            // 
            this.lblTitulo.BackColor = System.Drawing.Color.Transparent;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.lblTitulo.Location = new System.Drawing.Point(24, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(248, 34);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Monitor de Produção";
            // 
            // lblStatusFila
            // 
            this.lblStatusFila.BackColor = System.Drawing.Color.Transparent;
            this.lblStatusFila.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblStatusFila.ForeColor = System.Drawing.Color.Gray;
            this.lblStatusFila.Location = new System.Drawing.Point(278, 28);
            this.lblStatusFila.Name = "lblStatusFila";
            this.lblStatusFila.Size = new System.Drawing.Size(130, 19);
            this.lblStatusFila.TabIndex = 1;
            this.lblStatusFila.Text = "Carregando pedidos...";
            // 
            // btnAtualizar
            // 
            this.btnAtualizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAtualizar.BorderRadius = 10;
            this.btnAtualizar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.btnAtualizar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnAtualizar.ForeColor = System.Drawing.Color.White;
            this.btnAtualizar.Location = new System.Drawing.Point(860, 15);
            this.btnAtualizar.Name = "btnAtualizar";
            this.btnAtualizar.Size = new System.Drawing.Size(120, 40);
            this.btnAtualizar.TabIndex = 2;
            this.btnAtualizar.Text = "Atualizar";
            this.btnAtualizar.Click += new System.EventHandler(this.btnAtualizar_Click);
            // 
            // flowPedidos
            // 
            this.flowPedidos.AutoScroll = true;
            this.flowPedidos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.flowPedidos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPedidos.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowPedidos.Location = new System.Drawing.Point(0, 70);
            this.flowPedidos.Name = "flowPedidos";
            this.flowPedidos.Padding = new System.Windows.Forms.Padding(15);
            this.flowPedidos.Size = new System.Drawing.Size(1000, 530);
            this.flowPedidos.TabIndex = 1;
            this.flowPedidos.WrapContents = false;
            // 
            // tmrAtualizacao
            // 
            this.tmrAtualizacao.Interval = 5000;
            this.tmrAtualizacao.Tick += new System.EventHandler(this.tmrAtualizacao_Tick);
            // 
            // frmVendas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.flowPedidos);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmVendas";
            this.Text = "Produção de Vendas";
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}
