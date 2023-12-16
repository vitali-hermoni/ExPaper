using ExPaper.FolderTab.API.Models;
using ExPaper.SharedModels.Lib.DTO;
using System.Linq.Expressions;

namespace ExPaper.FolderTab.API.Services.IServices;

public interface IFolderTabService
{
    Task<ResponseDto> GetAsync(Expression<Func<FolderTabModel, bool>>? expression = null);
    Task<ResponseDto> AddUpdateAsync(FolderTabModel folderTabModel);
    Task<ResponseDto> RemoveAsync(Expression<Func<FolderTabModel, bool>> expression);

    Task<ResponseDto> CheckFolderHasTabsAsync(int folderId);
}
