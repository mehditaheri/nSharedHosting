using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SharedHosting.Models;

namespace SharedHosting.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public async Task<ApplicationUser?> GetCurrentUserAsync()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null || !user.Identity.IsAuthenticated)
        {
            return null;
        }

        return await _userManager.GetUserAsync(user);
    }

    // For backward compatibility, but async
    public ApplicationUser GetCurrentUser()
    {
        // This is synchronous, but we need async. For now, return null or throw.
        // Ideally, update all callers to use async.
        throw new InvalidOperationException("Use GetCurrentUserAsync instead.");
    }
}