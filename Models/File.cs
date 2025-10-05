using System.ComponentModel.DataAnnotations;

namespace SharedHosting.Models;

public class SharedFile
{
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public long Size { get; set; } // in bytes

    [MaxLength(10)]
    public string? Extension { get; set; }

    [MaxLength(100)]
    public string? ContentType { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Foreign key
    public int FolderId { get; set; }

    // Navigation property
    public Folder Folder { get; set; } = null!;
}