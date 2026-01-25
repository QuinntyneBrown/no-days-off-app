using FluentAssertions;
using Xunit;

namespace NoDaysOff.IntegrationTests;

public class ApiGatewayTests
{
    [Fact(Skip = "Requires Aspire test host setup")]
    public async Task Gateway_HealthCheck_ReturnsHealthy()
    {
        // This test would use Aspire.Hosting.Testing to spin up the distributed application
        // For now, it serves as a placeholder for integration testing
        await Task.CompletedTask;
        true.Should().BeTrue();
    }

    [Fact(Skip = "Requires Aspire test host setup")]
    public async Task Gateway_UnauthorizedRequest_Returns401()
    {
        // Test that unauthenticated requests return 401
        await Task.CompletedTask;
        true.Should().BeTrue();
    }

    [Fact(Skip = "Requires Aspire test host setup")]
    public async Task Gateway_AuthenticatedRequest_RoutesToService()
    {
        // Test that authenticated requests are properly routed
        await Task.CompletedTask;
        true.Should().BeTrue();
    }
}
