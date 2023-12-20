using ExPaper.SharedModels.Lib.DTO;

namespace ExPaper.SharedModels.Lib.DTO;

public record LoginResponseDto(
    UserDto UserDto = null,
    string Token = null)
{

}
