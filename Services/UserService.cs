using Microsoft.EntityFrameworkCore;
using SharedHosting.Data;
using SharedHosting.Models;

namespace SharedHosting.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApplicationUser?> GetUserByIdAsync(int id)
    {
        try
        {
            return await _context.Users.FindAsync(id);
        }
        catch (Exception ex)
        {
            // Log error
            throw new Exception($"Error retrieving user with ID {id}: {ex.Message}");
        }
    }

    public async Task<ApplicationUser?> GetUserByUsernameAsync(string username)
    {
        try
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving user with username {username}: {ex.Message}");
        }
    }

    public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
    {
        try
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving user with email {email}: {ex.Message}");
        }
    }

    public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
    {
        try
        {
            return await _context.Users.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving all users: {ex.Message}");
        }
    }

    public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user)
    {
        try
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error creating user: {ex.Message}");
        }
    }

    public async Task<ApplicationUser> UpdateUserAsync(ApplicationUser user)
    {
        try
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error updating user: {ex.Message}");
        }
    }

    public async Task DeleteUserAsync(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found");
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting user with ID {id}: {ex.Message}");
        }
    }
}