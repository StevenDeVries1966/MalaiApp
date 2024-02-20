using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using DataLayer.Classes;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Malai.Functions.Startup))]

namespace Malai.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var keyVaultUrl = new Uri(Environment.GetEnvironmentVariable("KeyVaultUrl")!);
            var secretClient = new SecretClient(keyVaultUrl, new DefaultAzureCredential());
            var cs = secretClient.GetSecret("testsecret1").Value.Value;
            builder.Services.AddDbContext<MalaiContext>(options => options.UseSqlServer(cs));
        }
    }
}
