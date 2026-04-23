#nullable disable
using Guna.UI2.WinForms;

namespace acaiGalatico.UI.Forms
{
    partial class frmPrincipal
    {
        private System.ComponentModel.IContainer components = null;
        private Guna2BorderlessForm borderlessForm;
        private Guna2ShadowForm shadowForm;
        private Guna2GradientPanel pnlSidebar;
        private Guna2Panel pnlTopo;
        private Guna2Panel pnlConteudo;
        private Guna2HtmlLabel lblMarca;
        private Guna2HtmlLabel lblPainel;
        private Guna2PictureBox picLogoPrincipal;
        private Guna2HtmlLabel lblPaginaTitulo;
        private Guna2HtmlLabel lblUsuario;
        private Guna2Button btnPedidos;
        private Guna2Button btnVendas;
        private Guna2Button btnSair;
        private Guna2CirclePictureBox picUsuario;
        private Guna2ControlBox ctlClose;

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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            borderlessForm = new Guna2BorderlessForm(components);
            shadowForm = new Guna2ShadowForm(components);
            pnlSidebar = new Guna2GradientPanel();
            picLogoPrincipal = new Guna2PictureBox();
            btnSair = new Guna2Button();
            btnVendas = new Guna2Button();
            btnPedidos = new Guna2Button();
            lblPainel = new Guna2HtmlLabel();
            lblMarca = new Guna2HtmlLabel();
            pnlTopo = new Guna2Panel();
            ctlClose = new Guna2ControlBox();
            picUsuario = new Guna2CirclePictureBox();
            lblUsuario = new Guna2HtmlLabel();
            lblPaginaTitulo = new Guna2HtmlLabel();
            pnlConteudo = new Guna2Panel();
            pnlSidebar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogoPrincipal).BeginInit();
            pnlTopo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picUsuario).BeginInit();
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
            // pnlSidebar
            // 
            pnlSidebar.BackColor = Color.FromArgb(15, 12, 41);
            pnlSidebar.Controls.Add(picLogoPrincipal);
            pnlSidebar.Controls.Add(btnSair);
            pnlSidebar.Controls.Add(btnVendas);
            pnlSidebar.Controls.Add(btnPedidos);
            pnlSidebar.Controls.Add(lblPainel);
            pnlSidebar.Controls.Add(lblMarca);
            pnlSidebar.CustomizableEdges = customizableEdges14;
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.FillColor = Color.FromArgb(15, 12, 41);
            pnlSidebar.Location = new Point(0, 0);
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.ShadowDecoration.CustomizableEdges = customizableEdges15;
            pnlSidebar.Size = new Size(224, 768);
            pnlSidebar.TabIndex = 0;
            // 
            // picLogoPrincipal
            // 
            picLogoPrincipal.BackColor = Color.Transparent;
            picLogoPrincipal.CustomizableEdges = customizableEdges8;
            picLogoPrincipal.ImageRotate = 0F;
            picLogoPrincipal.Location = new Point(24, 20);
            picLogoPrincipal.Name = "picLogoPrincipal";
            picLogoPrincipal.ShadowDecoration.CustomizableEdges = customizableEdges9;
            picLogoPrincipal.Size = new Size(50, 50);
            picLogoPrincipal.SizeMode = PictureBoxSizeMode.Zoom;
            picLogoPrincipal.TabIndex = 9;
            picLogoPrincipal.TabStop = false;
            // 
            // btnSair
            // 
            btnSair.Animated = true;
            btnSair.BorderRadius = 10;
            btnSair.CustomizableEdges = customizableEdges10;
            btnSair.DisabledState.BorderColor = Color.DarkGray;
            btnSair.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSair.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSair.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSair.FillColor = Color.FromArgb(231, 76, 60);
            btnSair.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnSair.ForeColor = Color.White;
            btnSair.HoverState.FillColor = Color.FromArgb(192, 57, 43);
            btnSair.Location = new Point(24, 630);
            btnSair.Name = "btnSair";
            btnSair.ShadowDecoration.CustomizableEdges = customizableEdges11;
            btnSair.Size = new Size(176, 42);
            btnSair.TabIndex = 8;
            btnSair.Text = "Sair";
            btnSair.Click += btnSair_Click;
            // 
            // btnPedidos
            // 
            btnPedidos.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            btnPedidos.Checked = true;
            btnPedidos.CheckedState.FillColor = Color.FromArgb(155, 89, 182);
            btnPedidos.CheckedState.ForeColor = Color.White;
            btnPedidos.CustomizableEdges = customizableEdges12;
            btnPedidos.DisabledState.BorderColor = Color.DarkGray;
            btnPedidos.DisabledState.CustomBorderColor = Color.DarkGray;
            btnPedidos.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnPedidos.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnPedidos.FillColor = Color.Transparent;
            btnPedidos.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnPedidos.ForeColor = Color.FromArgb(219, 228, 243);
            btnPedidos.HoverState.FillColor = Color.FromArgb(60, 74, 109);
            btnPedidos.Location = new Point(12, 185);
            btnPedidos.Name = "btnPedidos";
            btnPedidos.ShadowDecoration.CustomizableEdges = customizableEdges13;
            btnPedidos.Size = new Size(200, 71);
            btnPedidos.TabIndex = 3;
            btnPedidos.Text = "Pedidos";
            btnPedidos.TextAlign = HorizontalAlignment.Left;
            btnPedidos.Click += btnPedidos_Click;
            // 
            // btnVendas
            // 
            btnVendas.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            btnVendas.CheckedState.FillColor = Color.FromArgb(155, 89, 182);
            btnVendas.CheckedState.ForeColor = Color.White;
            btnVendas.CustomizableEdges = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            btnVendas.DisabledState.BorderColor = Color.DarkGray;
            btnVendas.DisabledState.CustomBorderColor = Color.DarkGray;
            btnVendas.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnVendas.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnVendas.FillColor = Color.Transparent;
            btnVendas.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnVendas.ForeColor = Color.FromArgb(219, 228, 243);
            btnVendas.HoverState.FillColor = Color.FromArgb(60, 74, 109);
            btnVendas.Location = new Point(12, 262);
            btnVendas.Name = "btnVendas";
            btnVendas.ShadowDecoration.CustomizableEdges = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            btnVendas.Size = new Size(200, 71);
            btnVendas.TabIndex = 4;
            btnVendas.Text = "Produção (Vendas)";
            btnVendas.TextAlign = HorizontalAlignment.Left;
            btnVendas.Click += btnVendas_Click;
            // 
            // lblPainel
            // 
            lblPainel.BackColor = Color.Transparent;
            lblPainel.ForeColor = Color.FromArgb(170, 183, 205);
            lblPainel.Location = new Point(24, 76);
            lblPainel.Name = "lblPainel";
            lblPainel.Size = new Size(84, 17);
            lblPainel.TabIndex = 1;
            lblPainel.Text = "PAINEL ADMIN";
            // 
            // lblMarca
            // 
            lblMarca.BackColor = Color.Transparent;
            lblMarca.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            lblMarca.ForeColor = Color.White;
            lblMarca.Location = new Point(80, 28);
            lblMarca.Name = "lblMarca";
            lblMarca.Size = new Size(145, 34);
            lblMarca.TabIndex = 0;
            lblMarca.Text = "Açaí Galático";
            // 
            // pnlTopo
            // 
            pnlTopo.Controls.Add(ctlClose);
            pnlTopo.Controls.Add(picUsuario);
            pnlTopo.Controls.Add(lblUsuario);
            pnlTopo.Controls.Add(lblPaginaTitulo);
            pnlTopo.CustomizableEdges = customizableEdges6;
            pnlTopo.Dock = DockStyle.Top;
            pnlTopo.FillColor = Color.White;
            pnlTopo.Location = new Point(224, 0);
            pnlTopo.Name = "pnlTopo";
            pnlTopo.ShadowDecoration.CustomizableEdges = customizableEdges7;
            pnlTopo.ShadowDecoration.Depth = 6;
            pnlTopo.ShadowDecoration.Enabled = true;
            pnlTopo.Size = new Size(1056, 74);
            pnlTopo.TabIndex = 1;
            // 
            // ctlClose
            // 
            ctlClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ctlClose.CustomizableEdges = customizableEdges3;
            ctlClose.FillColor = Color.Transparent;
            ctlClose.HoverState.FillColor = Color.FromArgb(244, 67, 54);
            ctlClose.HoverState.IconColor = Color.White;
            ctlClose.IconColor = Color.FromArgb(89, 103, 132);
            ctlClose.Location = new Point(1003, 14);
            ctlClose.Name = "ctlClose";
            ctlClose.ShadowDecoration.CustomizableEdges = customizableEdges4;
            ctlClose.Size = new Size(37, 29);
            ctlClose.TabIndex = 3;
            // 
            // picUsuario
            // 
            picUsuario.FillColor = Color.FromArgb(232, 236, 244);
            picUsuario.ImageRotate = 0F;
            picUsuario.Location = new Point(728, 18);
            picUsuario.Name = "picUsuario";
            picUsuario.ShadowDecoration.CustomizableEdges = customizableEdges5;
            picUsuario.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            picUsuario.Size = new Size(36, 36);
            picUsuario.TabIndex = 2;
            picUsuario.TabStop = false;
            // 
            // lblUsuario
            // 
            lblUsuario.BackColor = Color.Transparent;
            lblUsuario.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lblUsuario.ForeColor = Color.FromArgb(43, 52, 69);
            lblUsuario.Location = new Point(630, 26);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(90, 19);
            lblUsuario.TabIndex = 1;
            lblUsuario.Text = "Administrador";
            lblUsuario.TextAlignment = ContentAlignment.TopRight;
            // 
            // lblPaginaTitulo
            // 
            lblPaginaTitulo.BackColor = Color.Transparent;
            lblPaginaTitulo.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblPaginaTitulo.ForeColor = Color.FromArgb(26, 36, 56);
            lblPaginaTitulo.Location = new Point(24, 22);
            lblPaginaTitulo.Name = "lblPaginaTitulo";
            lblPaginaTitulo.Size = new Size(70, 27);
            lblPaginaTitulo.TabIndex = 0;
            lblPaginaTitulo.Text = "Pedidos";
            // 
            // pnlConteudo
            // 
            pnlConteudo.CustomizableEdges = customizableEdges1;
            pnlConteudo.Dock = DockStyle.Fill;
            pnlConteudo.FillColor = Color.FromArgb(245, 243, 255);
            pnlConteudo.Location = new Point(224, 74);
            pnlConteudo.Name = "pnlConteudo";
            pnlConteudo.ShadowDecoration.CustomizableEdges = customizableEdges2;
            pnlConteudo.Size = new Size(1056, 694);
            pnlConteudo.TabIndex = 2;
            // 
            // frmPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(241, 245, 249);
            ClientSize = new Size(1280, 768);
            Controls.Add(pnlConteudo);
            Controls.Add(pnlTopo);
            Controls.Add(pnlSidebar);
            FormBorderStyle = FormBorderStyle.None;
            MaximumSize = new Size(1280, 850);
            MinimumSize = new Size(1280, 766);
            Name = "frmPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Painel Admin";
            WindowState = FormWindowState.Maximized;
            pnlSidebar.ResumeLayout(false);
            pnlSidebar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picLogoPrincipal).EndInit();
            pnlTopo.ResumeLayout(false);
            pnlTopo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picUsuario).EndInit();
            ResumeLayout(false);
        }
    }
}
