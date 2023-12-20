using ExPaper.SharedMethods.Lib.Converter;
using ExPaper.SharedModels.Lib.DTO;
using ExPaper.SharedModels.Lib.Utilitys;
using ExPaper.Web.Services.IServices;
using ExPaper.Web.ViewModels;

namespace ExPaper.Web.Services;

public class FolderService : IFolderService
{
    private readonly IBaseService _baseService;
    private readonly ILogger<FolderService> logger;

    public FolderService(IBaseService baseService, ILogger<FolderService> logger)
    {
        _baseService = baseService;
        this.logger = logger;
    }



    public async Task<ResponseDto> GetAsync()
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.GET,
            Url: $"{Urls.FolderApiBase}/{Urls.Folder_GetAll}",
            Data: null,
            AccessToken: null));

        return responseDto;
    }



    public async Task<ResponseDto> GetByIdAsync(Guid id)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.GET,
            Url: $"{Urls.FolderApiBase}/{Urls.Folder_GetById}/{id}",
            Data: null,
            AccessToken: null));

        return responseDto;
    }


    public async Task<ResponseDto> GetByOrgIdAsync(Guid orgId)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.GET,
            Url: $"{Urls.FolderApiBase}/{Urls.Folder_GetByOrgId}/{orgId}",
            Data: null,
            AccessToken: null));

        return responseDto;
    }


    public async Task<ResponseDto> AddUpdateAsync(FolderOrgListViewModel folderOrgListViewModel)
    {
        try
        {
            string filePath;
            if (folderOrgListViewModel.ImageFile != null && folderOrgListViewModel.ImageFile.Length > 0)
            {
                // Pfad für das Hochladen definieren
                var uploadsFolder = @"/home/expaper/Dokumente";

                var fileName = Guid.NewGuid().ToString() + "_" + folderOrgListViewModel.ImageFile.FileName;
                filePath = uploadsFolder + "/" + folderOrgListViewModel.ImageFile.FileName;

                // Bild auf den Serverpfad hochladen
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await folderOrgListViewModel.ImageFile.CopyToAsync(stream);
                }

                var folderDto = folderOrgListViewModel.FolderDto.WithImage(filePath);
                folderOrgListViewModel = folderOrgListViewModel.WithFolderDto(folderOrgListViewModel.FolderDto.WithImage(filePath));
            }
            else
            {
                filePath = null;
            }

            var responseDto = await _baseService.SendAsync(new RequestDto(
                ApiType: SD.ApiType.PUT,
                Url: $"{Urls.FolderApiBase}/{Urls.Folder_AddUpdate}",
                Data: folderOrgListViewModel.FolderDto.WithImage(filePath),
                AccessToken: null));

            return responseDto;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new ResponseDto(Result: null, Message: ex.Message);
        }
    }



    public async Task<ResponseDto> DeleteByIdAsync(Guid id)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.DELETE,
            Url: $"{Urls.FolderApiBase}/{Urls.Folder_RemoveById}/{id}",
            Data: null,
            AccessToken: null));

        return responseDto;
    }



    public async Task<ResponseDto> AddToOrganisationAsync(Guid folderId, Guid organisationId)
    {
        var responseDto = await _baseService.SendAsync(new RequestDto(
            ApiType: SD.ApiType.PUT,
            Url: $"{Urls.FolderApiBase}/{Urls.Folder_AddToOrganisation}/{folderId}/{organisationId}",
            Data: null,
            AccessToken: null));

        return responseDto;
    }
}
