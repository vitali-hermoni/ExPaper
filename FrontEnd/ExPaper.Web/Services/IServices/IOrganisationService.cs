using ExPaper.SharedModels.Lib.DTO;
using System.Linq.Expressions;

namespace ExPaper.Web.Services.IServices
{
    public interface IOrganisationService
    {
        Task<ResponseDto> GetAsync();
        Task<ResponseDto> GetOrgsByUserId(Guid userId);
        Task<ResponseDto> GetByIdAsync(Guid id);
        Task<ResponseDto> AddUpdateAsync(OrganisationDto organisationDto);
        Task<ResponseDto> DeleteByIdAsync(Guid id);
        Task<ResponseDto> GetUsersAsync();
        Task<ResponseDto> AddUserAsync(Guid userId, Guid organisationId);
        Task<ResponseDto> DeleteUserAsync(Guid userId, Guid organisationId);
        Task<ResponseDto> GetUserListForOrganisationAsync(string userIds);
    }
}
