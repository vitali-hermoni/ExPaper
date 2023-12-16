using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ExPaper.Auth.API.Models;

public class AppUser : IdentityUser
{
    [Required]
    public string Name { get; set; }

    [Required]
    public Guid IdenKey { get; set; }
}
