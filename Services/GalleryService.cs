using Microsoft.EntityFrameworkCore;
using SharedHosting.Data;
using SharedHosting.Models;

namespace SharedHosting.Services;

public class GalleryService : IGalleryService
{
    private readonly ApplicationDbContext _context;

    public GalleryService(ApplicationDbContext context)
    {
        _context = context;
    }

    private static readonly string[] ImageContentTypes = { "image/jpeg", "image/png", "image/gif", "image/bmp", "image/webp" };
    private static readonly string[] VideoContentTypes = { "video/mp4", "video/avi", "video/mov", "video/wmv" };
    private static readonly string[] GalleryContentTypes = ImageContentTypes.Concat(VideoContentTypes).ToArray();

    public async Task<IEnumerable<SharedFile>> GetGalleryFilesAsync(int workspaceId)
    {
        try
        {
            return await _context.Files
                .Include(f => f.Folder)
                .Where(f => f.Folder.WorkspaceId == workspaceId && GalleryContentTypes.Contains(f.ContentType))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving gallery files for workspace {workspaceId}: {ex.Message}");
        }
    }

    public async Task<IEnumerable<SharedFile>> GetGalleryFilesByFolderAsync(int folderId)
    {
        try
        {
            return await _context.Files
                .Where(f => f.FolderId == folderId && GalleryContentTypes.Contains(f.ContentType))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving gallery files for folder {folderId}: {ex.Message}");
        }
    }

    public async Task<IEnumerable<SharedFile>> GetAllGalleryFilesAsync(FolderType[] folderTypes)
    {
        try
        {
            return await _context.Files
                .Include(f => f.Folder)
                .Where(f => folderTypes.Contains(f.Folder.Type) && GalleryContentTypes.Contains(f.ContentType))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving gallery files for folder types {string.Join(", ", folderTypes)}: {ex.Message}");
        }
    }
}