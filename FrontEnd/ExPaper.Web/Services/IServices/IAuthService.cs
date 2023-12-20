using ExPaper.SharedModels.Lib.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExPaper.Web.Services.IServices;

public interface IAuthService
{
    Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
    Task<ResponseDto?> RegisterAsync(RegisterRequestDto registerRequestDto);
    Task<ResponseDto?> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
    Task<ResponseDto?> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordRequestDto);
    Task<ResponseDto?> AssignRoleAsync(RegisterRequestDto registerRequestDto);
    List<SelectListItem> GetRoles();
    Task<ResponseDto> EditUserAsync(UserDto userDto);
    Task<ResponseDto> DeleteUserByIdAsync(Guid id);
}
