using SharedHosting.Models;

namespace SharedHosting.Services;

public interface ICurrentUserService
{
    Task<ApplicationUser?> GetCurrentUserAsync();
    ApplicationUser GetCurrentUser(); // Keep for backward compatibility, but deprecated
}