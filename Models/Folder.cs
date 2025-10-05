using System.ComponentModel.DataAnnotations;

namespace SharedHosting.Models;

public enum FolderType
{
    Private,
    Shared,
    Public
}

public class Folder
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public FolderType Type { get; set; } = FolderType.Private;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Foreign keys
    public int WorkspaceId { get; set; }
    public int? ParentFolderId { get; set; }

    // Navigation properties
    public Workspace Workspace { get; set; } = null!;
    public Folder? ParentFolder { get; set; }
    public ICollection<Folder> SubFolders { get; set; } = new List<Folder>();
    public ICollection<SharedFile> Files { get; set; } = new List<SharedFile>();
}