using SharedHosting.Models;

namespace SharedHosting.Services;

public interface IWorkspaceService
{
    Task<Workspace?> GetWorkspaceByIdAsync(int id);
    Task<IEnumerable<Workspace>> GetWorkspacesByOwnerAsync(int ownerId);
    Task<IEnumerable<Workspace>> GetAllWorkspacesAsync();
    Task<Workspace> CreateWorkspaceAsync(Workspace workspace);
    Task<Workspace> UpdateWorkspaceAsync(Workspace workspace);
    Task DeleteWorkspaceAsync(int id);
}