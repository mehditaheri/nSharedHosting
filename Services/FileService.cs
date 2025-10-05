using Microsoft.EntityFrameworkCore;
using SharedHosting.Data;
using SharedHosting.Models;

namespace SharedHosting.Services;

public class FileService : IFileService
{
    private readonly ApplicationDbContext _context;

    public FileService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SharedFile?> GetFileByIdAsync(int id)
    {
        try
        {
            return await _context.Files.Include(f => f.Folder).FirstOrDefaultAsync(f => f.Id == id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving file with ID {id}: {ex.Message}");
        }
    }

    public async Task<IEnumerable<SharedFile>> GetFilesByFolderAsync(int folderId)
    {
        try
        {
            return await _context.Files.Where(f => f.FolderId == folderId).ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving files for folder {folderId}: {ex.Message}");
        }
    }

    public async Task<IEnumerable<SharedFile>> GetFilesByFolderTypesAsync(FolderType[] folderTypes)
    {
        try
        {
            return await _context.Files
                .Include(f => f.Folder)
                .Where(f => folderTypes.Contains(f.Folder.Type))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving files for folder types {string.Join(", ", folderTypes)}: {ex.Message}");
        }
    }

    public async Task<SharedFile> CreateFileAsync(SharedFile file)
    {
        try
        {
            _context.Files.Add(file);
            await _context.SaveChangesAsync();
            return file;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error creating file: {ex.Message}");
        }
    }

    public async Task<SharedFile> UpdateFileAsync(SharedFile file)
    {
        try
        {
            _context.Files.Update(file);
            await _context.SaveChangesAsync();
            return file;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error updating file: {ex.Message}");
        }
    }

    public async Task DeleteFileAsync(int id)
    {
        try
        {
            var file = await _context.Files.FindAsync(id);
            if (file == null)
            {
                throw new KeyNotFoundException($"File with ID {id} not found");
            }
            _context.Files.Remove(file);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting file with ID {id}: {ex.Message}");
        }
    }
}