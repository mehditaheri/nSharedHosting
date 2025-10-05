using System.ComponentModel.DataAnnotations;

namespace SharedHosting.Models;

public class Workspace
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Foreign key
    public int OwnerId { get; set; }

    // Navigation properties
    public ApplicationUser Owner { get; set; } = null!;
    public ICollection<Folder> Folders { get; set; } = new List<Folder>();
}