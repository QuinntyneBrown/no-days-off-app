using Shared.Authentication;

namespace NoDaysOff.Tests.Common.Mocks;

public class MockCurrentUserService : ICurrentUserService
{
    public int UserId { get; set; } = 1;
    public int TenantId { get; set; } = 1;
    public string? Email { get; set; } = "test@test.com";
    public string? FirstName { get; set; } = "Test";
    public string? LastName { get; set; } = "User";
    public bool IsAuthenticated { get; set; } = true;
    public IEnumerable<string> Roles { get; set; } = new[] { "User" };
}
