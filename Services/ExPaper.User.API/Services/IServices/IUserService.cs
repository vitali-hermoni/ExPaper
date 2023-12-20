using ExPaper.SharedModels.Lib.DTO;
using System.Linq.Expressions;

namespace ExPaper.User.API.Services.IServices;

public interface IUserService
{
    Task<ResponseDto> GetByIdAsync(Guid id);
    Task<ResponseDto> GetByMailAsync(string mail);
    Task<ResponseDto> GetUsersByIdAsync(string userId);
	Task<ResponseDto> GetUsersByIdListAsync(string userIds);
    Task<ResponseDto> EditUserAsync(UserDto userDto);
    Task<ResponseDto> DeleteUserByIdAsync(int id);
}
