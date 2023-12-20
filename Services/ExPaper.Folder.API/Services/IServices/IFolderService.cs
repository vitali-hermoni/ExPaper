using ExPaper.Folder.API.Models;
using ExPaper.SharedModels.Lib.DTO;
using System.Linq.Expressions;

namespace ExPaper.Folder.API.Services.IServices;

public interface IFolderService
{
    Task<ResponseDto> GetAsync(Expression<Func<FolderModel, bool>> expression = null);
    Task<ResponseDto> AddUpdateAsync(FolderModel folderModel);
    Task<ResponseDto> RemoveAsync(Expression<Func<FolderModel, bool>> expression);
    Task<ResponseDto> AddToOrganisationAsync(Guid folderId, Guid organisationId);
}
