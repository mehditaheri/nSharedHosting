using Microsoft.EntityFrameworkCore;
using SharedHosting.Data;
using SharedHosting.Models;

namespace SharedHosting.Services;

public class FolderService : IFolderService
{
    private readonly ApplicationDbContext _context;

    public FolderService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Folder?> GetFolderByIdAsync(int id)
    {
        try
        {
            return await _context.Folders.Include(f => f.Workspace).Include(f => f.ParentFolder).FirstOrDefaultAsync(f => f.Id == id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving folder with ID {id}: {ex.Message}");
        }
    }

    public async Task<IEnumerable<Folder>> GetFoldersByWorkspaceAsync(int workspaceId)
    {
        try
        {
            return await _context.Folders.Where(f => f.WorkspaceId == workspaceId && f.ParentFolderId == null).ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving folders for workspace {workspaceId}: {ex.Message}");
        }
    }

    public async Task<IEnumerable<Folder>> GetSubFoldersAsync(int parentFolderId)
    {
        try
        {
            return await _context.Folders.Where(f => f.ParentFolderId == parentFolderId).ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving subfolders for parent {parentFolderId}: {ex.Message}");
        }
    }

    public async Task<Folder> CreateFolderAsync(Folder folder)
    {
        try
        {
            _context.Folders.Add(folder);
            await _context.SaveChangesAsync();
            return folder;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error creating folder: {ex.Message}");
        }
    }

    public async Task<Folder> UpdateFolderAsync(Folder folder)
    {
        try
        {
            _context.Folders.Update(folder);
            await _context.SaveChangesAsync();
            return folder;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error updating folder: {ex.Message}");
        }
    }

    public async Task DeleteFolderAsync(int id)
    {
        try
        {
            var folder = await _context.Folders.FindAsync(id);
            if (folder == null)
            {
                throw new KeyNotFoundException($"Folder with ID {id} not found");
            }
            _context.Folders.Remove(folder);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting folder with ID {id}: {ex.Message}");
        }
    }
}