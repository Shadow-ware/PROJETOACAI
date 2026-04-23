using System.Windows.Forms;

namespace acaiGalatico.UI.Forms
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
            CarregarRecursos();
        }

        private void CarregarRecursos()
        {
            try
            {
                // Tenta localizar o logo em caminhos relativos comuns
                string[] caminhosPossiveis = {
                    @"..\acaigalatico.Web\wwwroot\images\logo-sem-escrita.png",
                    @"..\..\..\..\acaigalatico.Web\wwwroot\images\logo-sem-escrita.png",
                    @"wwwroot\images\logo-sem-escrita.png"
                };

                foreach (var caminho in caminhosPossiveis)
                {
                    if (File.Exists(caminho))
                    {
                        picLogo.Image = Image.FromFile(caminho);
                        break;
                    }
                }
            }
            catch { /* Ignora erro de imagem para não travar o login */ }
        }

        private void btnEntrar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                MessageBox.Show("Preencha login e senha.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnEntrar.Enabled = false;
            btnEntrar.Text = "Entrando...";

            try
            {
                if (txtEmail.Text.Trim() == "Adminacai" && txtSenha.Text == "NKL0029*")
                {
                    var principal = new frmPrincipal("Administrador", null);
                    principal.Show();
                    Hide();
                }
                else
                {
                    MessageBox.Show("Login ou senha inválidos.", "Erro de autenticação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                btnEntrar.Enabled = true;
                btnEntrar.Text = "Entrar";
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Application.Exit();
        }

        private void btnFechar_Click(object? sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtSenha_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnEntrar.PerformClick();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void pnlLeft_Paint(object? sender, PaintEventArgs e)
        {

        }

        private void txtEmail_TextChanged(object? sender, EventArgs e)
        {

        }
    }
}
