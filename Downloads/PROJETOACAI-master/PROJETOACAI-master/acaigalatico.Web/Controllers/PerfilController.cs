using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using acaigalatico.Web.ViewModels;
using acaigalatico.Infrastructure.Context;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace acaigalatico.Web.Controllers
{
    [Authorize]
    public class PerfilController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppDbContext _context;

        public PerfilController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var email = await _userManager.GetEmailAsync(user);
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

            var vm = new PerfilViewModel
            {
                NewUserName = user?.UserName ?? "",
                NewEmail = email ?? "",
                CurrentPhotoUrl = usuario?.FotoPerfil ?? ""
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(PerfilViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Index", "Home");

            var currentEmail = await _userManager.GetEmailAsync(user);

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(model.NewEmail) && model.NewEmail != currentEmail)
                    await _userManager.SetEmailAsync(user, model.NewEmail);

                if (!string.IsNullOrWhiteSpace(model.NewUserName) && model.NewUserName != user.UserName)
                    await _userManager.SetUserNameAsync(user, model.NewUserName);

                if (!string.IsNullOrWhiteSpace(model.NewPassword))
                {
                    if (!string.IsNullOrWhiteSpace(model.CurrentPassword))
                        await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                }

                var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == currentEmail);
                if (usuario != null)
                {
                    usuario.Nome = model.NewUserName;
                    usuario.Email = model.NewEmail;

                    if (model.Photo != null && model.Photo.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profiles");
                        if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                        var safeName = Path.GetFileName(model.Photo.FileName);
                        var unique = System.Guid.NewGuid().ToString() + "_" + safeName;
                        var filePath = Path.Combine(uploadsFolder, unique);
                        using (var fs = new FileStream(filePath, FileMode.Create))
                        {
                            await model.Photo.CopyToAsync(fs);
                        }
                        usuario.FotoPerfil = "/uploads/profiles/" + unique;
                    }

                    _context.Usuarios.Update(usuario);
                    await _context.SaveChangesAsync();
                }

                await _userManager.UpdateAsync(user);
                await _signInManager.RefreshSignInAsync(user);

                TempData["SuccessMessage"] = "Perfil atualizado com sucesso.";
                return RedirectToAction(nameof(Index));
            }

            return View("Index", model);
        }
    }
}
