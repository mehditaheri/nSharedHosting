using SharedHosting.Models;

namespace SharedHosting.Services;

public interface IFolderService
{
    Task<Folder?> GetFolderByIdAsync(int id);
    Task<IEnumerable<Folder>> GetFoldersByWorkspaceAsync(int workspaceId);
    Task<IEnumerable<Folder>> GetSubFoldersAsync(int parentFolderId);
    Task<Folder> CreateFolderAsync(Folder folder);
    Task<Folder> UpdateFolderAsync(Folder folder);
    Task DeleteFolderAsync(int id);
}