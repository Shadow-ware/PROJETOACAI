using acaigalatico.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using acaigalatico.Domain.Interfaces;
using acaigalatico.Infrastructure.Repositories;
using acaigalatico.Application.Interfaces;
using acaigalatico.Application.Services;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using acaigalatico.Web.Services;

AppContext.SetSwitch("System.Net.Quic.Enabled", false);
AppContext.SetSwitch("System.Net.SocketsHttpHandler.Http3Support", false);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Força Kestrel a usar apenas HTTP/1.1 e HTTP/2 (desabilita HTTP/3/MsQuic)
builder.WebHost.ConfigureKestrel(options =>
{
    options.ConfigureEndpointDefaults(o =>
        o.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2);
});

// --- INICIO DA CONFIGURAÇÃO DO BANCO ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Server=(localdb)\\mssqllocaldb;Database=AcaiGalaticoDb_v2;Trusted_Connection=True;MultipleActiveResultSets=true;Connect Timeout=60;Max Pool Size=200";

builder.Services.AddDbContextPool<AppDbContext>(options =>
{
    if (connectionString.Contains("Data Source=", StringComparison.OrdinalIgnoreCase))
    {
        options.UseSqlite(connectionString);
    }
    else
    {
        options.UseSqlServer(connectionString, b =>
        {
            b.MigrationsAssembly("acaigalatico.Infrastructure");
            b.EnableRetryOnFailure();
        });
    }

    options.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
});

builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>();

// --- FIM DA CONFIGURAÇÃO DO BANCO ---

// Registrar o serviço de Seed
builder.Services.AddScoped<acaigalatico.Infrastructure.SeedingService>();
builder.Services.AddHostedService<StartupSeedHostedService>();

// Registrar Repositorios e Services
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IVendaRepository, VendaRepository>();
builder.Services.AddScoped<IVendaService, VendaService>();

var app = builder.Build();

// --- CONFIGURAÇÃO DE CULTURA (PT-BR) ---
var defaultCulture = new CultureInfo("pt-BR");
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(defaultCulture),
    SupportedCultures = new List<CultureInfo> { defaultCulture },
    SupportedUICultures = new List<CultureInfo> { defaultCulture }
};
app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// app.UseHttpsRedirection();

var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
provider.Mappings[".avif"] = "image/avif";
app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", context =>
{
    context.Response.Redirect("/Home/Index");
    return Task.CompletedTask;
});

app.MapGet("/Admin", context =>
{
    context.Response.Redirect("/Admin/Index");
    return Task.CompletedTask;
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

try 
{
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"[CRITICAL] Application crashed: {ex.Message}");
    Console.WriteLine(ex.StackTrace);
    throw;
}
