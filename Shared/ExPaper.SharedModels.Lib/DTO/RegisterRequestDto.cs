using System.ComponentModel.DataAnnotations;

namespace ExPaper.SharedModels.Lib.DTO;

public record RegisterRequestDto
{
    [Required(ErrorMessage = "E-Mail ist erforderlich")]
    [EmailAddress]
    public string Email { get; init; }

    [Required(ErrorMessage = "Name ist erforderlich")]
    public string Name { get; init; }

    [Required(ErrorMessage = "Passwort ist erforderlich")]
    [StringLength(100, ErrorMessage = "Minimum {0} Zeichen.", MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string Password { get; init; }

    [Required(ErrorMessage = "Passwort ist erforderlich")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Die Passwörter stimmen nicht überein.")]
    public string ConfirmPassword { get; init; }

    public string? Role { get; init; }

    public RegisterRequestDto WithRole(string role) => this with { Role = role };
}
