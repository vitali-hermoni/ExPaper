using ExPaper.SharedModels.Lib.DTO;

namespace ExPaper.Web.Services.IServices;

public interface IFolderTabService
{
    Task<ResponseDto> GetAsync();
    Task<ResponseDto> GetByIdAsync(Guid id);
    Task<ResponseDto> GetByFolderIdAsync(Guid folderId);
    Task<ResponseDto> AddUpdateAsync(FolderTabDto folderTabDto);
    Task<ResponseDto> RemoveByIdAsync(Guid id);
    Task<ResponseDto> RemoveByFolderIdAsync(Guid folderId);
}
