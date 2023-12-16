using ExPaper.SharedModels.Lib.DTO;
using System.Linq.Expressions;

namespace ExPaper.Web.Services.IServices;

public interface IGlobalSettingService
{
    Task<ResponseDto> GetAsync();
    Task<ResponseDto> GetByIdAsync(Guid id);
    Task<ResponseDto> GetByNameAsync(string name);
    Task<ResponseDto> AddUpdateAsync(GlobalSettingsDto globalSettingsDto);
    Task<ResponseDto> DeleteByIdAsync(Guid id);
}
