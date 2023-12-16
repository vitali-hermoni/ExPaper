using ExPaper.SharedModels.Lib.DTO;
using ExPaper.SharedModels.Lib.Utilitys;
using ExPaper.Web.Services.IServices;

namespace ExPaper.Web.Services;

public class DocumentService : IDocumentService
{
    private readonly IBaseService _baseService;

    public DocumentService(IBaseService baseService)
    {
        _baseService = baseService;
    }



    public async Task<ResponseDto> GetAsync()
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.GET,
            Url: $"{Urls.FolderTabApiBase}/{Urls.Document_GetAll}",
            Data: null,
            AccessToken: null));

        return responseDto;
    }



    public async Task<ResponseDto> GetByIdAsync(Guid id)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.GET,
            Url: $"{Urls.DocumentApiBase}/{Urls.Document_GetById}/{id}",
            Data: null,
            AccessToken: null));

        return responseDto;
    }


    public async Task<ResponseDto> GetByFolderTabIdAsync(Guid folderTabId)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.GET,
            Url: $"{Urls.DocumentApiBase}/{Urls.Document_GetByFolderTabId}/{folderTabId}",
            Data: null,
            AccessToken: null));

        return responseDto;
    }


    public async Task<ResponseDto> AddUpdateAsync(DocumentDto documentDto)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.PUT,
            Url: $"{Urls.DocumentApiBase}/{Urls.Document_AddUpdate}",
            Data: documentDto,
            AccessToken: null));

        return responseDto;
    }



    public async Task<ResponseDto> RemoveByIdAsync(Guid id)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.DELETE,
            Url: $"{Urls.DocumentApiBase}/{Urls.Document_RemoveById}/{id}",
            Data: null,
            AccessToken: null));

        return responseDto;
    }



    public async Task<ResponseDto> RemoveByFolderTabIdAsync(Guid folderTabId)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.DELETE,
            Url: $"{Urls.DocumentApiBase}/{Urls.Document_RemoveByFolderTabId}/{folderTabId}",
            Data: null,
            AccessToken: null));

        return responseDto;
    }
}
