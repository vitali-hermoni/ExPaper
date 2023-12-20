using ExPaper.SharedModels.Lib.DTO;

namespace ExPaper.Web.Services.IServices;

public interface IUserService
{
    Task<ResponseDto> GetByIdAsync(Guid id);
    Task<ResponseDto> GetByMailAsync(string mail);
    Task<ResponseDto> GetUsersAsync();
}
