using ExPaper.SharedModels.Lib.DTO;
using ExPaper.SharedModels.Lib.Utilitys;
using ExPaper.Web.Services.IServices;

namespace ExPaper.Web.Services;

public class FolderTabService : IFolderTabService
{
    private readonly IBaseService _baseService;



    public FolderTabService(IBaseService baseService)
    {
        _baseService = baseService;
    }




    public async Task<ResponseDto> GetAsync()
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.GET,
            Url: $"{Urls.FolderTabApiBase}/{Urls.FolderTab_GetAll}",
            Data: null,
            AccessToken: null));

        return responseDto;
    }



    public async Task<ResponseDto> GetByIdAsync(Guid id)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.GET,
            Url: $"{Urls.FolderTabApiBase}/{Urls.FolderTab_GetById}/{id}",
            Data: null,
            AccessToken: null));

        return responseDto;
    }


    public async Task<ResponseDto> GetByFolderIdAsync(Guid folderId)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.GET,
            Url: $"{Urls.FolderTabApiBase}/{Urls.FolderTab_GetByFolderId}/{folderId}",
            Data: null,
            AccessToken: null));

        return responseDto;
    }


    public async Task<ResponseDto> AddUpdateAsync(FolderTabDto folderTabDto)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.PUT,
            Url: $"{Urls.FolderTabApiBase}/{Urls.FolderTab_AddUpdate}",
            Data: folderTabDto,
            AccessToken: null));

        return responseDto;
    }



    public async Task<ResponseDto> RemoveByIdAsync(Guid id)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.DELETE,
            Url: $"{Urls.FolderTabApiBase}/{Urls.FolderTab_RemoveById}/{id}",
            Data: null,
            AccessToken: null));

        return responseDto;
    }



    public async Task<ResponseDto> RemoveByFolderIdAsync(Guid folderId)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.DELETE,
            Url: $"{Urls.FolderTabApiBase}/{Urls.FolderTab_RemoveByFolderId}/{folderId}",
            Data: null,
            AccessToken: null));

        return responseDto;
    }
}
