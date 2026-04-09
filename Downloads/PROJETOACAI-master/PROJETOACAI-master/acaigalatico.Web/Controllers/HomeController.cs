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

using acaigalatico.Web.ViewModels;
using acaigalatico.Web.Services;

namespace acaigalatico.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly acaigalatico.Infrastructure.Context.AppDbContext _context;
    private readonly EmailService _emailService;

    public HomeController(ILogger<HomeController> logger, 
                          acaigalatico.Infrastructure.Context.AppDbContext context,
                          EmailService emailService)
    {
        _logger = logger;
        _context = context;
        _emailService = emailService;
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
    public async Task<IActionResult> Contato(string nome, string email, string telefone, string assunto, string mensagem)
    {
        if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(mensagem))
        {
            TempData["ErrorMessage"] = "Preencha todos os campos!";
            return RedirectToAction("Contact");
        }

        try
        {
            var enviado = await _emailService.EnviarEmail(nome, email, telefone, assunto, mensagem);

            if (enviado)
            {
                // Salva localmente também como backup
                SaveMessageToFile(nome, email, telefone, assunto, mensagem);
                TempData["SuccessMessage"] = "Mensagem enviada com sucesso!";
            }
            else
            {
                TempData["ErrorMessage"] = "Erro ao enviar mensagem.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro no envio de e-mail.");
            TempData["ErrorMessage"] = $"Erro ao enviar mensagem: {ex.Message}";
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
