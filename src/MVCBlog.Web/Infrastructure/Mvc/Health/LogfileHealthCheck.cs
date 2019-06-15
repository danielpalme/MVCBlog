using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MVCBlog.Web.Infrastructure.Mvc.Health
{
    public class LogfileHealthCheck : IHealthCheck
    {
        private readonly IHostingEnvironment environment;

        public LogfileHealthCheck(IHostingEnvironment environment)
        {
            this.environment = environment;
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            string logsDirectory = Path.Combine(this.environment.ContentRootPath, "Logs");

            foreach (var file in Directory.EnumerateFiles(logsDirectory, "*.txt"))
            {
                return Task.FromResult(
                    HealthCheckResult.Healthy("Log files do exist."));
            }

            return Task.FromResult(
                HealthCheckResult.Degraded("No log files found. Missing write permission?"));
        }
    }
}