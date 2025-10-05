using Microsoft.EntityFrameworkCore;
using SharedHosting.Data;
using SharedHosting.Models;

namespace SharedHosting.Services;

public class WorkspaceService : IWorkspaceService
{
    private readonly ApplicationDbContext _context;

    public WorkspaceService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Workspace?> GetWorkspaceByIdAsync(int id)
    {
        try
        {
            return await _context.Workspaces.Include(w => w.Owner).FirstOrDefaultAsync(w => w.Id == id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving workspace with ID {id}: {ex.Message}");
        }
    }

    public async Task<IEnumerable<Workspace>> GetWorkspacesByOwnerAsync(int ownerId)
    {
        try
        {
            return await _context.Workspaces.Where(w => w.OwnerId == ownerId).ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving workspaces for owner {ownerId}: {ex.Message}");
        }
    }

    public async Task<IEnumerable<Workspace>> GetAllWorkspacesAsync()
    {
        try
        {
            return await _context.Workspaces.Include(w => w.Owner).ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving all workspaces: {ex.Message}");
        }
    }

    public async Task<Workspace> CreateWorkspaceAsync(Workspace workspace)
    {
        try
        {
            _context.Workspaces.Add(workspace);
            await _context.SaveChangesAsync();
            return workspace;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error creating workspace: {ex.Message}");
        }
    }

    public async Task<Workspace> UpdateWorkspaceAsync(Workspace workspace)
    {
        try
        {
            _context.Workspaces.Update(workspace);
            await _context.SaveChangesAsync();
            return workspace;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error updating workspace: {ex.Message}");
        }
    }

    public async Task DeleteWorkspaceAsync(int id)
    {
        try
        {
            var workspace = await _context.Workspaces.FindAsync(id);
            if (workspace == null)
            {
                throw new KeyNotFoundException($"Workspace with ID {id} not found");
            }
            _context.Workspaces.Remove(workspace);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting workspace with ID {id}: {ex.Message}");
        }
    }
}