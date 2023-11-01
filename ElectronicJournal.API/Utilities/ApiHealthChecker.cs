using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ElectronicJournal.API.Utilities
{
    public class ApiHealthChecker : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
            => Task.FromResult(result: HealthCheckResult.Healthy());
    }
}
