using ExPaper.SharedModels.Lib.DTO;
using ExPaper.SharedModels.Lib.Utilitys;
using ExPaper.Web.Services.IServices;

namespace ExPaper.Web.Services;

public class UserService : IUserService
{
	private readonly IBaseService _baseService;

	public UserService(IBaseService baseService)
    {
		_baseService = baseService;
	}
		

	

	public async Task<ResponseDto> GetUsersAsync()
	{
		var responseDto = await _baseService.SendAsync(new RequestDto(
			ApiType: SD.ApiType.GET,
			Url: $"{Urls.UserApiBase}/{Urls.User_GetUserList}",
			Data: null,
			AccessToken: null));

		return responseDto;
	}



    public async Task<ResponseDto> GetByIdAsync(Guid id)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.GET,
            Url: $"{Urls.UserApiBase}/{Urls.User_GetById}/{id}",
            Data: null,
            AccessToken: null));

        return responseDto;
    }



    public async Task<ResponseDto> GetByMailAsync(string mail)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.GET,
            Url: $"{Urls.UserApiBase}/{Urls.User_GetByMail}/{mail}",
            Data: null,
            AccessToken: null));

        return responseDto;
    }
}
