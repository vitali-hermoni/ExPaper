namespace ExPaper.SharedModels.Lib.DTO;

public record ForgotPasswordCallbackDto(string UserId, string Code, string UserMail)
{

}
