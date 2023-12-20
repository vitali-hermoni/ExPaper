using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ExPaper.Document.API.Models;

#nullable disable
public class DocumentModel
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [StringLength(1000)]
    public string Description { get; set; }

    [StringLength(1000)]
    public string Tags { get; set; }

    [Required]
    [StringLength(1000)]
    public string Path { get; set; }

    [StringLength(1000)]
    public string Image { get; set; }

    [Required]
    public Guid TabId { get; set; }
}
