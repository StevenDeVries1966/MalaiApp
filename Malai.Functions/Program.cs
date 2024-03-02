using DataLayer.Classes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
     //.ConfigureFunctionsWebApplication()
     .ConfigureServices(s =>
     {
         s.AddDbContext<MalaiContext>(o =>
            o.UseSqlServer(Environment.GetEnvironmentVariable("connectionstring")), ServiceLifetime.Transient);
         s.AddAutoMapper(typeof(Program).Assembly);
     })
    .Build();
host.Run();