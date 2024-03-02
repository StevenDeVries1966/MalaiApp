//using FnSysInfo.Functions.Model;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;

//var host = new HostBuilder()
//    .ConfigureFunctionsWorkerDefaults()
//     .ConfigureServices(s =>
//     {
//         s.AddDbContext<SysinfoContext>(o =>
//            o.UseSqlServer(Environment.GetEnvironmentVariable("SysInfoConnectionString")), ServiceLifetime.Transient);
//         s.AddAutoMapper(typeof(Program).Assembly);
//     })
//    .Build();
//host.Run();

[STAThread]
static void Main()
{
}
