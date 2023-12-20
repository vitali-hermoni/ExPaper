using System.ComponentModel.DataAnnotations;

namespace ExPaper.Organisation.API.Models;

#nullable disable
public class OrganisationModel
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(250)]
    public string Name { get; set; }

    public string UserIds { get; set; }
}
