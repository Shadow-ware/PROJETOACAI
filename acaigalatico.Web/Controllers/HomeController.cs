using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using acaigalatico.Domain.Entities;
using acaigalatico.Web.Models;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using acaigalatico.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace acaigalatico.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly acaigalatico.Infrastructure.Context.AppDbContext _context;
    private readonly IEmailService _emailService;
    private readonly IConteudoSiteService _conteudoService;

    public HomeController(ILogger<HomeController> logger, acaigalatico.Infrastructure.Context.AppDbContext context, IEmailService emailService, IConteudoSiteService conteudoService)
    {
        _logger = logger;
        _context = context;
        _emailService = emailService;
        _conteudoService = conteudoService;
    }

    public IActionResult Index()
    {
        var model = _conteudoService.GetInicio();
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage(string nome, string email, string telefone, string assunto, string mensagem)
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
            
            // Envia o e-mail real via SendGrid
            string subject = $"Novo Contato: {assunto} - {nome}";
            string emailBody = $@"
                <h3>Nova mensagem de contato do site Açaí Galáctico</h3>
                <p><strong>Nome:</strong> {nome}</p>
                <p><strong>Email:</strong> {email}</p>
                <p><strong>Telefone:</strong> {telefone}</p>
                <p><strong>Assunto:</strong> {assunto}</p>
                <p><strong>Mensagem:</strong></p>
                <p>{mensagem}</p>
            ";

            await _emailService.SendEmailAsync("liuliuvks@gmail.com", subject, emailBody);

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
