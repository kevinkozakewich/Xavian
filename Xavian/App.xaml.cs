using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Windows;
using Xavian.DataContext;

namespace Xavian
{
    public partial class App : Application
    {
        public IConfiguration Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            ConfigureApp();

            var mainWindow = new MainWindow(); // Assuming MainWindow does not require DI
            mainWindow.Show();
            base.OnStartup(e);
        }

        private void ConfigureApp()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            // Setup configuration
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .Build();

            var connectionString = GetConnectionString();
            var optionsBuilder = new DbContextOptionsBuilder<XavianDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            using (var context = new XavianDbContext(optionsBuilder.Options))
            {
                context.Database.Migrate();
            }
        }

        private string GetConnectionString()
        {
            var variable = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            bool isDevelopment = variable == null || variable == "Development";
            if (!isDevelopment)
            {
                var keyVaultEndpoint = new Uri(Configuration["AzureKeyVault"] ??
                    throw new InvalidOperationException("Key Vault endpoint not configured."));
                var secretClient = new SecretClient(keyVaultEndpoint, new DefaultAzureCredential());
                KeyVaultSecret dbSecret = secretClient.GetSecret("XavianConnectionString");
                return dbSecret.Value ?? throw new InvalidOperationException("Azure database missing in secrets store.");
            }
            else
            {
                return Configuration.GetConnectionString("DefaultConnection") ??
                    throw new InvalidOperationException("Local database missing in appsettings.json.");
            }
        }
    }
}
