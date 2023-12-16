namespace ExPaper.SharedModels.Lib.DTO;

public record UserDto(
    Guid? Id, 
    string Email, 
    string Name, 
    string PhoneNumber,
    Guid? IdentKey = null)
{

}
