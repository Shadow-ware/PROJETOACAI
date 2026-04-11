using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using acaigalatico.Web.ViewModels;
using acaigalatico.Infrastructure.Context;
using acaigalatico.Domain.Entities;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acaigalatico.Web.Controllers
{
    [Authorize]
    [Route("[controller]")]
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
        [Route("")]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    Console.WriteLine("[DEBUG] Usuário não encontrado no Identity.");
                    return RedirectToPage("/Account/Login", new { area = "Identity" });
                }

                var email = await _userManager.GetEmailAsync(user);
                Console.WriteLine($"[DEBUG] Carregando perfil para o email: {email}");

                var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);
                if (usuario == null)
                {
                    Console.WriteLine("[DEBUG] Usuário não encontrado na tabela customizada 'Usuarios'.");
                    // Criar um objeto vm básico para não quebrar a página
                    return View(new PerfilViewModel { 
                        NewUserName = user.UserName ?? "Usuário", 
                        NewEmail = email ?? "" 
                    });
                }

                // Decompor endereço (formato simples: Rua, Numero, Bairro, Cidade, CEP)
                string rua = "", numero = "", bairro = "", cidade = "", cep = "";
                if (!string.IsNullOrEmpty(usuario.Endereco))
                {
                    var partes = usuario.Endereco.Split('|');
                    if (partes.Length >= 1) rua = partes[0];
                    if (partes.Length >= 2) numero = partes[1];
                    if (partes.Length >= 3) bairro = partes[2];
                    if (partes.Length >= 4) cidade = partes[3];
                    if (partes.Length >= 5) cep = partes[4];
                }

                var vm = new PerfilViewModel
                {
                    NewUserName = usuario.Nome ?? user.UserName ?? "",
                    NewEmail = email ?? "",
                    Telefone = usuario.Telefone ?? "",
                    Rua = rua,
                    Numero = numero,
                    Bairro = bairro,
                    Cidade = cidade,
                    Cep = cep,
                    CurrentPhotoUrl = usuario.FotoPerfil ?? ""
                };

                return View(vm);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Erro crítico no PerfilController Index: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return RedirectToAction("Index", "Home");
            }
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
                // 1. Atualizar Identity se necessário
                if (!string.IsNullOrWhiteSpace(model.NewEmail) && model.NewEmail != currentEmail)
                {
                    var setEmailResult = await _userManager.SetEmailAsync(user, model.NewEmail);
                    if (!setEmailResult.Succeeded)
                    {
                        foreach (var error in setEmailResult.Errors)
                            ModelState.AddModelError("", error.Description);
                        return View("Index", model);
                    }
                    await _userManager.SetUserNameAsync(user, model.NewEmail);
                }

                if (!string.IsNullOrWhiteSpace(model.NewPassword))
                {
                    if (string.IsNullOrWhiteSpace(model.CurrentPassword))
                    {
                        ModelState.AddModelError("CurrentPassword", "A senha atual é obrigatória para alterar a senha.");
                        return View("Index", model);
                    }

                    var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                    if (!changePasswordResult.Succeeded)
                    {
                        foreach (var error in changePasswordResult.Errors)
                            ModelState.AddModelError("CurrentPassword", error.Description);
                        return View("Index", model);
                    }
                }

                // 2. Atualizar Tabela Customizada (Usuario)
                var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == currentEmail);
                if (usuario != null)
                {
                    usuario.Nome = model.NewUserName;
                    usuario.Email = model.NewEmail;
                    usuario.Telefone = model.Telefone;
                    usuario.Endereco = $"{model.Rua}|{model.Numero}|{model.Bairro}|{model.Cidade}|{model.Cep}";

                    if (model.Photo != null && model.Photo.Length > 0)
                    {
                        // Validar extensão
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                        var extension = Path.GetExtension(model.Photo.FileName).ToLower();
                        if (!allowedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("Photo", "Apenas imagens JPG ou PNG são permitidas.");
                            return View("Index", model);
                        }

                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profiles");
                        if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.Photo.FileName);
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fs = new FileStream(filePath, FileMode.Create))
                        {
                            await model.Photo.CopyToAsync(fs);
                        }

                        // Remover foto antiga se existir
                        if (!string.IsNullOrEmpty(usuario.FotoPerfil) && !usuario.FotoPerfil.Contains("logo-sem-escrita.png"))
                        {
                            var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", usuario.FotoPerfil.TrimStart('/'));
                            if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                        }

                        usuario.FotoPerfil = "/uploads/profiles/" + uniqueFileName;
                    }

                    _context.Usuarios.Update(usuario);
                    await _context.SaveChangesAsync();
                }

                await _userManager.UpdateAsync(user);
                await _signInManager.RefreshSignInAsync(user);

                TempData["SuccessMessage"] = "Perfil atualizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            return View("Index", model);
        }
    }
}
