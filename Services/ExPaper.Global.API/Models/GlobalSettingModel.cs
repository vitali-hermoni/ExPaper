using System.ComponentModel.DataAnnotations;

namespace ExPaper.Global.API.Models;

#nullable disable
public class GlobalSettingModel
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [MaxLength(500)]
    public string Value { get; set; }
}
