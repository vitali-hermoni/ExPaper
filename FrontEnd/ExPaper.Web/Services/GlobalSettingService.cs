using ExPaper.SharedModels.Lib.Utilitys;
using ExPaper.SharedModels.Lib.DTO;
using ExPaper.Web.Services.IServices;
using System.Linq.Expressions;
using ExPaper.SharedMethods.Lib.Converter;

namespace ExPaper.Web.Services;

public class GlobalSettingService : IGlobalSettingService
{
    private readonly IBaseService _baseService;

    public GlobalSettingService(IBaseService baseService)
    {
        _baseService = baseService;
    }



    public async Task<ResponseDto> GetAsync()
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.GET,
            Url: $"{Urls.GlobalSettingsApiBase}/{Urls.GlobalSettings_Get}",
            Data: null,
            AccessToken: null));

        return responseDto;
    }




    public async Task<ResponseDto> GetByIdAsync(Guid id)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.GET,
            Url: $"{Urls.GlobalSettingsApiBase}/{Urls.GlobalSettings_GetById}/{id}",
            Data: null,
            AccessToken: null));

        return responseDto;
    }


    public async Task<ResponseDto> GetByNameAsync(string name)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.GET,
            Url: $"{Urls.GlobalSettingsApiBase}/{Urls.GlobalSettings_GetByName}/{name}",
            Data: null,
            AccessToken: null));

        return responseDto;
    }



    public async Task<ResponseDto> AddUpdateAsync(GlobalSettingsDto globalSettingsDto)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.PUT,
            Url: $"{Urls.GlobalSettingsApiBase}/{Urls.GlobalSettings_AddUpdate}",
            Data: globalSettingsDto,
            AccessToken: null));

        return responseDto;
    }



    public async Task<ResponseDto> DeleteByIdAsync(Guid id)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.DELETE,
            Url: $"{Urls.GlobalSettingsApiBase}/{Urls.GlobalSettings_Delete}/{id}",
            Data: null,
            AccessToken: null));

        return responseDto;
    }
}
