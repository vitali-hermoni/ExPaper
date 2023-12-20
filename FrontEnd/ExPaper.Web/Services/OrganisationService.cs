using ExPaper.SharedMethods.Lib.Converter;
using ExPaper.SharedModels.Lib.DTO;
using ExPaper.SharedModels.Lib.Utilitys;
using ExPaper.Web.Services.IServices;

namespace ExPaper.Web.Services;

public class OrganisationService : IOrganisationService
{
    private readonly IBaseService _baseService;

    public OrganisationService(IBaseService baseService)
    {
        _baseService = baseService;
    }


    public async Task<ResponseDto> GetAsync()
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.GET,
            Url: $"{Urls.OrganisationApiBase}/{Urls.Organisation_GetAll}",
            Data: null,
            AccessToken: null));

        return responseDto;
    }



    public async Task<ResponseDto> GetOrgsByUserId(Guid userId)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.GET,
            Url: $"{Urls.OrganisationApiBase}/{Urls.Organisation_GetOrgsByUserId}/{userId}",
            Data: null,
            AccessToken: null));

        return responseDto;
    }



    public async Task<ResponseDto> GetByIdAsync(Guid id)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.GET,
            Url: $"{Urls.OrganisationApiBase}/{Urls.Organisation_GetById}/{id}",
            Data: null,
            AccessToken: null));

        return responseDto;
    }


    public async Task<ResponseDto> AddUpdateAsync(OrganisationDto organisationDto)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.PUT,
            Url: $"{Urls.OrganisationApiBase}/{Urls.Organisation_AddUpdate}",
            Data: organisationDto,
            AccessToken: null));

        return responseDto;
    }
    
    
    
    public async Task<ResponseDto> DeleteByIdAsync(Guid id)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.DELETE,
            Url: $"{Urls.OrganisationApiBase}/{Urls.Organisation_RemoveById}/{id}",
            Data: null,
            AccessToken: null));

        return responseDto;
    }




    public async Task<ResponseDto> AddUserAsync(Guid userId, Guid orgId)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.PUT,
            Url: $"{Urls.OrganisationApiBase}/{Urls.Organisation_AddUser}/{userId}/{orgId}",
            Data: null,
            AccessToken: null));

        return responseDto;
    }



    public async Task<ResponseDto> DeleteUserAsync(Guid userId, Guid orgId)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.DELETE,
            Url: $"{Urls.OrganisationApiBase}/{Urls.Organisation_DeleteUser}/{userId}/{orgId}",
            Data: null,
            AccessToken: null));

        return responseDto;
    }



	public async Task<ResponseDto> GetUserListForOrganisationAsync(string userIds)
	{
		var responseDto = await _baseService.SendAsync(new RequestDto(
			ApiType: SD.ApiType.GET,
			Url: $"{Urls.UserApiBase}/{Urls.User_GetUserListForOrganisateonAsync}",
			Data: userIds,
			AccessToken: null));

		return responseDto;
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
}
