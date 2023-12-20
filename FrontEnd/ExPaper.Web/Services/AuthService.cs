using ExPaper.SharedMethods.Lib.Converter;
using ExPaper.SharedModels.Lib.DTO;
using ExPaper.SharedModels.Lib.Utilitys;
using ExPaper.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExPaper.Web.Services;

public class AuthService : IAuthService
{
    private readonly IBaseService _baseService;

    public AuthService(IBaseService baseService)
    {
        _baseService = baseService;
    }




    public async Task<ResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.POST,
            Url: $"{Urls.AuthApiBase}/{Urls.Auth_Login}",
            Data: loginRequestDto,
            AccessToken: null));

        return responseDto;
    }



    public async Task<ResponseDto> RegisterAsync(RegisterRequestDto registerRequestDto)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.POST,
            Url: $"{Urls.AuthApiBase}/{Urls.Auth_Register}",
            Data: registerRequestDto,
            AccessToken: null));

        return responseDto;
    }



    public async Task<ResponseDto> EditUserAsync(UserDto userDto)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.POST,
            Url: $"{Urls.AuthApiBase}/{Urls.Auth_EditUser}",
            Data: userDto,
            AccessToken: null));

        return responseDto;
    }



    public Task<ResponseDto> DeleteUserByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }



    public async Task<ResponseDto> AssignRoleAsync(RegisterRequestDto registerRequestDto)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.POST,
            Url: $"{Urls.AuthApiBase}/{Urls.Auth_AssignRole}",
            Data: registerRequestDto,
            AccessToken: null));

        return responseDto;
    }




    public List<SelectListItem> GetRoles()
    {
        return new List<SelectListItem>()
            {
                new SelectListItem{Text="Admin", Value=nameof(SD.Role.ADMIN)},
                new SelectListItem{Text="Manager", Value=nameof(SD.Role.MANAGER)},
                new SelectListItem{Text="Super User", Value=nameof(SD.Role.SEPER_USER)},
                new SelectListItem{Text="User", Value=nameof(SD.Role.USER)}
            };
    }



    public async Task<ResponseDto> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.POST,
            Url: $"{Urls.AuthApiBase}/{Urls.Auth_ForgotPassword}",
            Data: forgotPasswordDto,
            AccessToken: null));

        return responseDto;
    }



    public async Task<ResponseDto> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordRequestDto)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.POST,
            Url: $"{Urls.AuthApiBase}/{Urls.Auth_ResetPassword}",
            Data: resetPasswordRequestDto,
            AccessToken: null));

        return responseDto;
    }
}
