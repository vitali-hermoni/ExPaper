using ExPaper.SharedModels.Lib.DTO;

namespace ExPaper.Web.Services.IServices;

public interface IDocumentService
{
    Task<ResponseDto> GetAsync();
    Task<ResponseDto> GetByIdAsync(Guid id);
    Task<ResponseDto> GetByFolderTabIdAsync(Guid folderTabId);
    Task<ResponseDto> AddUpdateAsync(DocumentDto documentDto, IFormFile formFile = null);
    Task<ResponseDto> RemoveByIdAsync(Guid id);
    Task<ResponseDto> RemoveByFolderTabIdAsync(Guid folderTabId);
}
