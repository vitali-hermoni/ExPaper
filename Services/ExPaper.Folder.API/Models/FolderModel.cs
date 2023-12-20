using System.ComponentModel.DataAnnotations;

namespace ExPaper.Folder.API.Models;

#nullable disable
public class FolderModel
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [StringLength(1000)]
    public string Description { get; set; }

    [StringLength(10)]
    public string Color { get; set; } = "#272b30";

    [Required]
    public int Year { get; set; }

    public string Month { get; set; }

    public int Quarter { get; set; }

    public string Image { get; set; }

    [Required]
    public Guid OrganisationId { get; set; }
}
