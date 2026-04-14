using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using acaigalatico.Infrastructure.Context;
using acaigalatico.Application.Interfaces;
using acaigalatico.Application.Services;
using acaigalatico.Web.Services;
using acaigalatico.Infrastructure.Repositories;
using acaigalatico.Domain.Interfaces;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// --- CONFIGURAÇÃO DO BANCO ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? 
    "Data Source=AcaiGalatico.db";

// Log para debug do caminho do banco no Web
Console.WriteLine($"[WEB-DB-DEBUG] String de conexão: {connectionString}");
var dbPath = connectionString.Split('=')[1];
Console.WriteLine($"[WEB-DB-DEBUG] Caminho absoluto do DB: {Path.GetFullPath(dbPath)}");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// --- IDENTITY ---
builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>();

// --- DEPENDENCY INJECTION ---
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IVendaRepository, VendaRepository>();
builder.Services.AddScoped<IVendaService, VendaService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<IPagamentoService, PagamentoService>();
builder.Services.AddSingleton<IConteudoSiteService, ConteudoSiteService>();
builder.Services.AddScoped<acaigalatico.Infrastructure.SeedingService>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// --- HTTP CLIENT ---
builder.Services.AddHttpClient();

// --- SWAGGER ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// --- SEED AUTOMÁTICO ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated();
        var seeding = services.GetRequiredService<acaigalatico.Infrastructure.SeedingService>();
        seeding.SeedAsync().GetAwaiter().GetResult();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERROR] Falha no Seed: {ex.Message}");
    }
}

// --- CULTURA ---
var defaultCulture = new CultureInfo("pt-BR");
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(defaultCulture),
    SupportedCultures = new List<CultureInfo> { defaultCulture },
    SupportedUICultures = new List<CultureInfo> { defaultCulture }
};
app.UseRequestLocalization(localizationOptions);

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();

// --- SWAGGER PIPELINE ---
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Açaí Galáctico API v1");
});

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
