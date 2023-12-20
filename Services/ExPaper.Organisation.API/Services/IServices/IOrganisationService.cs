using ExPaper.Organisation.API.Models;
using ExPaper.SharedModels.Lib.DTO;
using System.Linq.Expressions;

namespace ExPaper.Organisation.API.Services.IServices;

public interface IOrganisationService
{
    Task<ResponseDto> GetAsync(Expression<Func<OrganisationModel, bool>>? expression = null);
    Task<ResponseDto> AddUpdateAsync(OrganisationModel organisationModel);
    Task<ResponseDto> RemoveAsync(Expression<Func<OrganisationModel, bool>> expression);
    Task<ResponseDto> AddUserAsync(Guid userId, Guid orgId);
    Task<ResponseDto> DeleteUserAsync(Guid userId, Guid orgId);
}
