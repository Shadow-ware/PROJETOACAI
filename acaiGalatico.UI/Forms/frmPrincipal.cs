using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace acaiGalatico.UI.Forms
{
    public partial class frmPrincipal : Form
    {
        private readonly string _nomeUsuario;
        private readonly Image? _fotoUsuario;
        private frmPedidos? _frmPedidos;
        private Guna2Button? _currentButton;

        public frmPrincipal(string nomeUsuario, Image? fotoUsuario)
        {
            _nomeUsuario = nomeUsuario;
            _fotoUsuario = fotoUsuario;
            InitializeComponent();
            lblUsuario.Text = _nomeUsuario;
            if (_fotoUsuario != null)
            {
                picUsuario.Image = _fotoUsuario;
            }
            CarregarRecursos();
        }

        private void CarregarRecursos()
        {
            try
            {
                string[] caminhosPossiveis = {
                    @"..\acaigalatico.Web\wwwroot\images\logo-sem-escrita.png",
                    @"..\..\..\..\acaigalatico.Web\wwwroot\images\logo-sem-escrita.png",
                    @"wwwroot\images\logo-sem-escrita.png"
                };

                foreach (var caminho in caminhosPossiveis)
                {
                    if (File.Exists(caminho))
                    {
                        picLogoPrincipal.Image = Image.FromFile(caminho);
                        break;
                    }
                }
            }
            catch { }
        }

       


        private void btnPedidos_Click(object sender, EventArgs e)
        {
            AbrirPedidos();
        }

     


        private void btnSair_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AbrirPedidos()
        {
            _frmPedidos ??= new frmPedidos();
            CarregarFormulario(_frmPedidos);
            lblPaginaTitulo.Text = "Pedidos";
            AtivarMenu(btnPedidos);
        }

        private void CarregarFormulario(Form form)
        {
            foreach (Control control in pnlConteudo.Controls)
            {
                control.Visible = false;
            }

            if (!pnlConteudo.Controls.Contains(form))
            {
                form.TopLevel = false;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Dock = DockStyle.Fill;
                pnlConteudo.Controls.Add(form);
                form.Show();
            }

            form.BringToFront();
            form.Visible = true;
        }


        private void AtivarMenu(Guna2Button button)
        {
            if (_currentButton != null)
            {
                _currentButton.Checked = false;
                _currentButton.ForeColor = Color.FromArgb(219, 228, 243);
                _currentButton.FillColor = Color.Transparent;
            }

            _currentButton = button;
            _currentButton.Checked = true;
            _currentButton.ForeColor = Color.White;
            _currentButton.FillColor = Color.FromArgb(155, 89, 182);
        }
    }
}
