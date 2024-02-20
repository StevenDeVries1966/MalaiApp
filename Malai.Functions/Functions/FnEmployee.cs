using DataLayer.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Malai.AzureApi.Functions
{
    public class FnEmployee
    {
        private readonly ILogger<FnEmployee> _logger;
        private readonly MalaiContext _ctx;

        public FnEmployee(ILogger<FnEmployee> logger, MalaiContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        [FunctionName("FnEmployee")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "employees/{empcode}")] HttpRequest req, string empcode)
        {
            HttpResponseData responseOut = null;
            if (req.Method == HttpMethods.Get)
            {
                if (string.IsNullOrEmpty(empcode))
                {
                    var employees = _ctx.lstClients;
                    return new CreatedResult("/emp", employees); ;
                }
                else
                {
                    var employee = _ctx.lstClients.FirstOrDefault(o => o.clt_code.ToLower() == empcode.ToLower());
                    return new OkObjectResult(employee);
                }
            }
            return new BadRequestResult();
        }
    }
}
