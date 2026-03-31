using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using acaigalatico.Web.Models;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace acaigalatico.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly acaigalatico.Infrastructure.Context.AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, acaigalatico.Infrastructure.Context.AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        Dictionary<string, string> contents;
        try
        {
            contents = _context.SiteContents.AsNoTracking().ToDictionary(c => c.Key, c => c.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar conteúdo do site (Home).");
            contents = new Dictionary<string, string>();
        }
        return View(contents);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public async Task<IActionResult> Contact()
        {
            Dictionary<string, string> contents;
            try
            {
                contents = await _context.SiteContents
                    .AsNoTracking()
                    .Where(c => c.Page == "Contato")
                    .ToDictionaryAsync(c => c.Key, c => c.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar conteúdo do site (Contato).");
                contents = new Dictionary<string, string>();
            }

            return View(contents);
        }

    [HttpPost]
    public IActionResult SendMessage(string nome, string email, string telefone, string assunto, string mensagem)
    {
        try
        {
            // Validação de segurança dos limites
            if (string.IsNullOrEmpty(nome) || nome.Length > 70)
                throw new Exception("O nome deve ter entre 1 e 70 caracteres.");
            
            if (string.IsNullOrEmpty(email) || email.Length > 100)
                throw new Exception("O e-mail deve ter entre 1 e 100 caracteres.");

            if (!string.IsNullOrEmpty(telefone) && telefone.Length > 20)
                throw new Exception("O telefone deve ter no máximo 20 caracteres.");

            if (string.IsNullOrEmpty(mensagem) || mensagem.Length > 1000)
                throw new Exception("A mensagem deve ter entre 1 e 1000 caracteres.");

            // Salva a mensagem localmente para o administrador ver depois
            SaveMessageToFile(nome, email, telefone, assunto, mensagem);
            
            // Define a mensagem de sucesso solicitada pelo usuário
            TempData["SuccessMessage"] = "Mensagem recebida, obrigada pelo feedback!";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Erro ao processar mensagem: {ex.Message}";
        }

        return RedirectToAction("Contact");
    }

    private void SaveMessageToFile(string nome, string email, string telefone, string assunto, string mensagem)
    {
        try
        {
            // Salva na pasta do projeto para garantir permissão e fácil acesso
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Mensagens_Recebidas");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fileName = $"Msg_{DateTime.Now:yyyyMMdd_HHmmss}_{nome.Replace(" ", "_")}.txt";
            string filePath = Path.Combine(folderPath, fileName);

            string content = $"DATA: {DateTime.Now}\n" +
                             $"DE: {nome} ({email})\n" +
                             $"TELEFONE: {telefone}\n" +
                             $"ASSUNTO: {assunto}\n" +
                             $"--------------------------------------------------\n" +
                             $"{mensagem}\n" +
                             $"--------------------------------------------------\n";

            System.IO.File.WriteAllText(filePath, content);
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao salvar arquivo local: {ex.Message}");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
