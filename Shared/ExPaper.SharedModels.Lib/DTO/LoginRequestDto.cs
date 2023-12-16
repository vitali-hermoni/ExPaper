using System.ComponentModel.DataAnnotations;

namespace ExPaper.SharedModels.Lib.DTO;

public record LoginRequestDto
{
    [Required(ErrorMessage = "Benutzername ist erforderlich")]
    public string? UserName { get; init; }


    [Required(ErrorMessage = "Passwort ist erforderlich")]
    [DataType(DataType.Password)]
    public string Password { get; init; }


    public bool RememberMe { get; init; }
}
