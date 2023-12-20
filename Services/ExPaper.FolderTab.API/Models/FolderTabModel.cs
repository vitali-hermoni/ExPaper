using System.ComponentModel.DataAnnotations;

namespace ExPaper.FolderTab.API.Models;

#nullable disable
public class FolderTabModel
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }


    [StringLength(10)]
    public string Color { get; set; }

    [Required]
    public Guid FolderId { get; set; }
}
