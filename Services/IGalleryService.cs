using SharedHosting.Models;

namespace SharedHosting.Services;

public interface IGalleryService
{
    Task<IEnumerable<SharedFile>> GetGalleryFilesAsync(int workspaceId);
    Task<IEnumerable<SharedFile>> GetGalleryFilesByFolderAsync(int folderId);
    Task<IEnumerable<SharedFile>> GetAllGalleryFilesAsync(FolderType[] folderTypes);
}