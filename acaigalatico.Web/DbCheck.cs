using System;
using System.Linq;
using acaigalatico.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace acaigalatico.Web
{
    public class DbCheck
    {
        public static void Check(IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                try
                {
                    var count = context.SiteContents.Count();
                    Console.WriteLine($"[DB-CHECK] SiteContents count: {count}");
                    var entries = context.SiteContents.Where(c => c.Key.StartsWith("Admin_")).ToList();
                    foreach (var entry in entries)
                    {
                        Console.WriteLine($"[DB-CHECK] Key: {entry.Key}, Value: {entry.Value}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[DB-CHECK] Error: {ex.Message}");
                }
            }
        }
    }
}
