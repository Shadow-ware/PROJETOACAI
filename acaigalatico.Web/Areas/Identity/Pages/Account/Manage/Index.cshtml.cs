using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using acaigalatico.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace acaigalatico.Web.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppDbContext _context;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public string Username { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public int OrdersCount { get; private set; }
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public bool EmailVerified { get; private set; }
        public string MaskedEmail { get; private set; } = string.Empty;
        public string MaskedPhone { get; private set; } = string.Empty;
        public string ProfilePictureUrl { get; private set; } = string.Empty;
        public List<OrderSummary> Orders { get; private set; } = new();

        public class OrderSummary
        {
            public int Id { get; set; }
            public DateTime Data { get; set; }
            public decimal ValorTotal { get; set; }
            public string Status { get; set; } = string.Empty;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Nome de usuário")]
            public string NewUserName { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string NewEmail { get; set; } = string.Empty;

            [Phone]
            [Display(Name = "Telefone")]
            public string? PhoneNumber { get; set; }

            [Display(Name = "Sobrenome")]
            public string? LastName { get; set; }

            [Display(Name = "Foto de perfil")]
            public Microsoft.AspNetCore.Http.IFormFile? ProfilePhoto { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToPage("/Account/Login");

            Username = user.UserName;
            Email = await _userManager.GetEmailAsync(user);
            var phone = await _userManager.GetPhoneNumberAsync(user);
            EmailVerified = user.EmailConfirmed;

            var claims = await _userManager.GetClaimsAsync(user);
            LastName = claims.FirstOrDefault(c => c.Type == "family_name")?.Value ?? "";

            var parts = (Username ?? "").Split(' ');
            FirstName = parts.Length > 0 ? parts[0] : (Username ?? "Usuário");

            MaskedEmail = string.IsNullOrEmpty(Email) ? "" : MaskEmail(Email);
            MaskedPhone = string.IsNullOrEmpty(phone) ? "" : MaskPhone(phone);

            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == Email);
            ProfilePictureUrl = usuario?.FotoPerfil ?? "";

            var cliente = _context.Clientes.FirstOrDefault(c => c.Nome == Username);
            if (cliente != null)
            {
                OrdersCount = _context.Vendas.Count(v => v.ClienteId == cliente.Id);
                Orders = _context.Vendas
                    .Where(v => v.ClienteId == cliente.Id)
                    .OrderByDescending(v => v.DataVenda)
                    .Select(v => new OrderSummary
                    {
                        Id = v.Id,
                        Data = v.DataVenda,
                        ValorTotal = v.ValorTotal,
                        Status = v.Status.ToString()
                    }).Take(10).ToList();
            }
            else
                OrdersCount = 0;

            Input = new InputModel
            {
                NewUserName = Username,
                NewEmail = Email,
                PhoneNumber = phone,
                LastName = LastName
            };

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateProfileAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToPage("/Account/Login");

            var currentEmail = await _userManager.GetEmailAsync(user);
            var currentUserName = user.UserName;
            var currentPhone = await _userManager.GetPhoneNumberAsync(user);

            if (Input.NewEmail != currentEmail)
                await _userManager.SetEmailAsync(user, Input.NewEmail);

            if (Input.NewUserName != currentUserName)
                await _userManager.SetUserNameAsync(user, Input.NewUserName);

            if (Input.PhoneNumber != currentPhone)
                await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);

            // Atualiza na tabela Usuario personalizada
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == currentEmail);
            if (usuario != null)
            {
                usuario.Nome = Input.NewUserName;
                usuario.Email = Input.NewEmail;
                usuario.Telefone = Input.PhoneNumber;

                // Processar upload de foto se houver
                if (Input.ProfilePhoto != null)
                {
                    var uploadsFolder = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profiles");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Input.ProfilePhoto.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Input.ProfilePhoto.CopyToAsync(fileStream);
                    }

                    usuario.FotoPerfil = "/uploads/profiles/" + uniqueFileName;
                }

                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();
            }

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);

            TempData["ProfileSaved"] = "Perfil atualizado com sucesso.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAccountAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToPage("/Account/Login");

            // Impede a exclusão do usuário Adminacai
            if (user.UserName.Equals("Adminacai", StringComparison.OrdinalIgnoreCase))
            {
                TempData["ErrorMessage"] = "A conta do administrador principal não pode ser excluída.";
                return RedirectToPage();
            }

            await _userManager.DeleteAsync(user);
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Index");
        }

        private static string MaskEmail(string email)
        {
            var at = email.IndexOf('@');
            if (at <= 1) return email;
            var name = email.Substring(0, at);
            var domain = email.Substring(at);
            var maskedName = name[0] + new string('*', name.Length - 2) + name[^1];
            return maskedName + domain;
        }

        private static string MaskPhone(string phone)
        {
            var digits = new string(phone.Where(char.IsDigit).ToArray());
            if (digits.Length <= 4) return phone;
            var last4 = digits[^4..];
            return new string('*', digits.Length - 4) + last4;
        }
    }
}
