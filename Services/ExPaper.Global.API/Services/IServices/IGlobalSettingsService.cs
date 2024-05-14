using ExPaper.Global.API.Models;
using ExPaper.SharedModels.Lib.DTO;
using System.Linq.Expressions;

namespace ExPaper.Global.API.Services.IServices;

public interface IGlobalSettingsService
{
    Task<ResponseDto> GetAsync(Expression<Func<GlobalSettingModel, bool>> expression = null);

    // TODO: GlobalSettingsDto to GlobalSettingsModel
    Task<ResponseDto> AddUpdateAsync(GlobalSettingsDto globalSettingModel);
    Task<ResponseDto> RemoveAsync(Expression<Func<GlobalSettingModel, bool>> expression);
}
