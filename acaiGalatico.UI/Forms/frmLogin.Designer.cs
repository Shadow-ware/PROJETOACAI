#nullable enable
using Guna.UI2.WinForms;

namespace acaiGalatico.UI.Forms
{
    partial class frmLogin
    {
        private System.ComponentModel.IContainer? components = null;
        private Guna2BorderlessForm borderlessForm;
        private Guna2ShadowForm shadowForm;
        private Guna2Panel pnlLeft;
        private Guna2Panel pnlCard;
        private Guna2HtmlLabel lblBrand;
        private Guna2HtmlLabel lblBrandSubTitle;
        private Guna2HtmlLabel lblTitle;
        private Guna2HtmlLabel lblSubTitle;
        private Guna2HtmlLabel lblEmail;
        private Guna2HtmlLabel lblSenha;
        private Guna2TextBox txtEmail;
        private Guna2TextBox txtSenha;
        private Guna2Button btnEntrar;
        private Guna2CircleButton btnFechar;

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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            borderlessForm = new Guna2BorderlessForm(components);
            shadowForm = new Guna2ShadowForm(components);
            pnlLeft = new Guna2Panel();
            lblBrandSubTitle = new Guna2HtmlLabel();
            lblBrand = new Guna2HtmlLabel();
            pnlCard = new Guna2Panel();
            btnFechar = new Guna2CircleButton();
            btnEntrar = new Guna2Button();
            txtSenha = new Guna2TextBox();
            txtEmail = new Guna2TextBox();
            lblSenha = new Guna2HtmlLabel();
            lblEmail = new Guna2HtmlLabel();
            lblSubTitle = new Guna2HtmlLabel();
            lblTitle = new Guna2HtmlLabel();
            pnlLeft.SuspendLayout();
            pnlCard.SuspendLayout();
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
            // pnlLeft
            // 
            pnlLeft.Controls.Add(lblBrandSubTitle);
            pnlLeft.Controls.Add(lblBrand);
            pnlLeft.CustomizableEdges = customizableEdges10;
            pnlLeft.Dock = DockStyle.Left;
            pnlLeft.FillColor = Color.FromArgb(26, 36, 56);
            pnlLeft.Location = new Point(0, 0);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.ShadowDecoration.CustomizableEdges = customizableEdges11;
            pnlLeft.Size = new Size(300, 560);
            pnlLeft.TabIndex = 0;
            pnlLeft.Paint += pnlLeft_Paint;
            // 
            // lblBrandSubTitle
            // 
            lblBrandSubTitle.BackColor = Color.Transparent;
            lblBrandSubTitle.ForeColor = Color.FromArgb(198, 205, 223);
            lblBrandSubTitle.Location = new Point(40, 234);
            lblBrandSubTitle.Name = "lblBrandSubTitle";
            lblBrandSubTitle.Size = new Size(151, 17);
            lblBrandSubTitle.TabIndex = 1;
            lblBrandSubTitle.Text = "Painel desktop para pedidos";
            // 
            // lblBrand
            // 
            lblBrand.BackColor = Color.Transparent;
            lblBrand.Font = new Font("Segoe UI Semibold", 26F, FontStyle.Bold);
            lblBrand.ForeColor = Color.White;
            lblBrand.Location = new Point(40, 184);
            lblBrand.Name = "lblBrand";
            lblBrand.Size = new Size(208, 49);
            lblBrand.TabIndex = 0;
            lblBrand.Text = "Açaí Galático";
            // 
            // pnlCard
            // 
            pnlCard.BackColor = Color.Transparent;
            pnlCard.BorderRadius = 24;
            pnlCard.Controls.Add(btnFechar);
            pnlCard.Controls.Add(btnEntrar);
            pnlCard.Controls.Add(txtSenha);
            pnlCard.Controls.Add(txtEmail);
            pnlCard.Controls.Add(lblSenha);
            pnlCard.Controls.Add(lblEmail);
            pnlCard.Controls.Add(lblSubTitle);
            pnlCard.Controls.Add(lblTitle);
            pnlCard.CustomizableEdges = customizableEdges8;
            pnlCard.FillColor = Color.White;
            pnlCard.Location = new Point(358, 73);
            pnlCard.Name = "pnlCard";
            pnlCard.ShadowDecoration.BorderRadius = 24;
            pnlCard.ShadowDecoration.CustomizableEdges = customizableEdges9;
            pnlCard.ShadowDecoration.Depth = 12;
            pnlCard.ShadowDecoration.Enabled = true;
            pnlCard.Size = new Size(430, 414);
            pnlCard.TabIndex = 1;
            // 
            // btnFechar
            // 
            btnFechar.DisabledState.BorderColor = Color.DarkGray;
            btnFechar.DisabledState.CustomBorderColor = Color.DarkGray;
            btnFechar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnFechar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnFechar.FillColor = Color.FromArgb(243, 77, 77);
            btnFechar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnFechar.ForeColor = Color.White;
            btnFechar.Location = new Point(372, 18);
            btnFechar.Name = "btnFechar";
            btnFechar.ShadowDecoration.CustomizableEdges = customizableEdges1;
            btnFechar.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            btnFechar.Size = new Size(36, 36);
            btnFechar.TabIndex = 7;
            btnFechar.Text = "X";
            btnFechar.Click += btnFechar_Click;
            // 
            // btnEntrar
            // 
            btnEntrar.BorderRadius = 12;
            btnEntrar.CustomizableEdges = customizableEdges2;
            btnEntrar.DisabledState.BorderColor = Color.DarkGray;
            btnEntrar.DisabledState.CustomBorderColor = Color.DarkGray;
            btnEntrar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnEntrar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnEntrar.FillColor = Color.FromArgb(84, 61, 255);
            btnEntrar.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnEntrar.ForeColor = Color.White;
            btnEntrar.Location = new Point(36, 322);
            btnEntrar.Name = "btnEntrar";
            btnEntrar.ShadowDecoration.CustomizableEdges = customizableEdges3;
            btnEntrar.Size = new Size(358, 48);
            btnEntrar.TabIndex = 6;
            btnEntrar.Text = "Entrar";
            btnEntrar.Click += btnEntrar_Click;
            // 
            // txtSenha
            // 
            txtSenha.BorderRadius = 10;
            txtSenha.Cursor = Cursors.IBeam;
            txtSenha.CustomizableEdges = customizableEdges4;
            txtSenha.DefaultText = "";
            txtSenha.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtSenha.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtSenha.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtSenha.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtSenha.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSenha.Font = new Font("Segoe UI", 10F);
            txtSenha.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSenha.Location = new Point(36, 245);
            txtSenha.Margin = new Padding(3, 4, 3, 4);
            txtSenha.Name = "txtSenha";
            txtSenha.PasswordChar = '●';
            txtSenha.PlaceholderText = "Digite sua senha";
            txtSenha.SelectedText = "";
            txtSenha.ShadowDecoration.CustomizableEdges = customizableEdges5;
            txtSenha.Size = new Size(358, 48);
            txtSenha.TabIndex = 5;
            txtSenha.KeyDown += txtSenha_KeyDown;
            // 
            // txtEmail
            // 
            txtEmail.BorderRadius = 10;
            txtEmail.Cursor = Cursors.IBeam;
            txtEmail.CustomizableEdges = customizableEdges6;
            txtEmail.DefaultText = "";
            txtEmail.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtEmail.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtEmail.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtEmail.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtEmail.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtEmail.Font = new Font("Segoe UI", 10F);
            txtEmail.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtEmail.Location = new Point(36, 159);
            txtEmail.Margin = new Padding(3, 4, 3, 4);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "Digite seu login";
            txtEmail.SelectedText = "";
            txtEmail.ShadowDecoration.CustomizableEdges = customizableEdges7;
            txtEmail.Size = new Size(358, 48);
            txtEmail.TabIndex = 4;
        
            // 
            // lblSenha
            // 
            lblSenha.BackColor = Color.Transparent;
            lblSenha.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblSenha.ForeColor = Color.FromArgb(59, 70, 97);
            lblSenha.Location = new Point(36, 221);
            lblSenha.Name = "lblSenha";
            lblSenha.Size = new Size(40, 19);
            lblSenha.TabIndex = 3;
            lblSenha.Text = "Senha";
            // 
            // lblEmail
            // 
            lblEmail.BackColor = Color.Transparent;
            lblEmail.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblEmail.ForeColor = Color.FromArgb(59, 70, 97);
            lblEmail.Location = new Point(36, 135);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(36, 19);
            lblEmail.TabIndex = 2;
            lblEmail.Text = "Login";
            // 
            // lblSubTitle
            // 
            lblSubTitle.BackColor = Color.Transparent;
            lblSubTitle.ForeColor = Color.FromArgb(107, 114, 128);
            lblSubTitle.Location = new Point(36, 82);
            lblSubTitle.Name = "lblSubTitle";
            lblSubTitle.Size = new Size(203, 17);
            lblSubTitle.TabIndex = 1;
            lblSubTitle.Text = "Entre para acessar o painel de pedidos";
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(28, 36, 52);
            lblTitle.Location = new Point(36, 32);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(169, 39);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Bem-vindo(a)";
            // 
            // frmLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(241, 245, 249);
            ClientSize = new Size(860, 560);
            Controls.Add(pnlCard);
            Controls.Add(pnlLeft);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
            pnlLeft.ResumeLayout(false);
            pnlLeft.PerformLayout();
            pnlCard.ResumeLayout(false);
            pnlCard.PerformLayout();
            ResumeLayout(false);
        }
    }
}
