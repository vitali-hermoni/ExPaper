using ExPaper.SharedModels.Lib.DTO;

namespace ExPaper.Web.Services.IServices;

public interface IBaseService
{
    Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true);
}
