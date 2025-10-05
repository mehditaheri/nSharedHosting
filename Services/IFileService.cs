using SharedHosting.Models;

namespace SharedHosting.Services;

public interface IFileService
{
    Task<SharedFile?> GetFileByIdAsync(int id);
    Task<IEnumerable<SharedFile>> GetFilesByFolderAsync(int folderId);
    Task<IEnumerable<SharedFile>> GetFilesByFolderTypesAsync(FolderType[] folderTypes);
    Task<SharedFile> CreateFileAsync(SharedFile file);
    Task<SharedFile> UpdateFileAsync(SharedFile file);
    Task DeleteFileAsync(int id);
}