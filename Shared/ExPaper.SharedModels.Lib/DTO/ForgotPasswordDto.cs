using System.ComponentModel.DataAnnotations;

namespace ExPaper.SharedModels.Lib.DTO;

public record ForgotPasswordDto()
{
    [Required(ErrorMessage = "E-Mail ist erforderlich")]
    [EmailAddress]
    public string Email { get; init; }
}
