using ExPaper.SharedModels.Lib.DTO;
using ExPaper.Web.ViewModels;

namespace ExPaper.Web.Services.IServices;

public interface IFolderService
{
    Task<ResponseDto> GetAsync();
    Task<ResponseDto> GetByIdAsync(Guid id);
    Task<ResponseDto> GetByOrgIdAsync(Guid orgId);
    Task<ResponseDto> AddUpdateAsync(FolderOrgListViewModel folderOrgListViewModel);
    Task<ResponseDto> DeleteByIdAsync(Guid id);
    Task<ResponseDto> AddToOrganisationAsync(Guid folderId, Guid organisationId);
}
