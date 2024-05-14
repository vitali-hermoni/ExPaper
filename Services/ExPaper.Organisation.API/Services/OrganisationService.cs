using AutoMapper;
using ExPaper.Organisation.API.Data;
using ExPaper.Organisation.API.Models;
using ExPaper.Organisation.API.Services.IServices;
using ExPaper.SharedModels.Lib.DTO;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace ExPaper.Organisation.API.Services;

public class OrganisationService : IOrganisationService
{
    private readonly AppDbContext _appDbContext;
    private readonly ILogger<OrganisationService> _logger;
    private readonly IMapper _mapper;



    public OrganisationService(
        AppDbContext appDbContext,
        ILogger<OrganisationService> logger,
        IMapper mapper)
    {
        _appDbContext = appDbContext;
        _logger = logger;
        _mapper = mapper;
    }




    public async Task<ResponseDto> GetAsync(Expression<Func<OrganisationModel, bool>> expression = null)
    {
        try
        {
            IQueryable<OrganisationModel> query = _appDbContext.Organisation;
            query = expression is not null ? query.Where(expression) : query;
            var organisationModels = await query.AsNoTracking().ToListAsync();
            var organisationDtos = JsonConvert.SerializeObject(_mapper.Map<List<OrganisationDto>>(organisationModels));
            return new ResponseDto(Result: organisationDtos, IsSuccess: true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new ResponseDto(Message: ex.Message);
        }
    }



    public async Task<ResponseDto> AddUpdateAsync(OrganisationModel organisationModel)
    {
        try
        {
            var obj = _appDbContext.Organisation.Update(organisationModel);
            await _appDbContext.SaveChangesAsync();
            return new ResponseDto(IsSuccess: true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new ResponseDto(Message: ex.Message);
        }
    }




    public async Task<ResponseDto> RemoveAsync(Expression<Func<OrganisationModel, bool>> expression)
    {
        try
        {
            IQueryable<OrganisationModel> query = _appDbContext.Organisation;
            query = expression is not null ? query.Where(expression) : query;
            var organisation = await query.AsNoTracking().ToListAsync();
            _appDbContext.Remove(organisation.First());
            await _appDbContext.SaveChangesAsync();
            return new ResponseDto(IsSuccess: true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new ResponseDto(Message: ex.Message);
        }
    }




    public async Task<ResponseDto> AddUserAsync(Guid userId, Guid orgId)
    {
        try
        {
            Expression<Func<OrganisationModel, bool>> expression = x => x.Id == orgId;
            var responseDto = await GetAsync(expression);

            if (responseDto is not null && responseDto.IsSuccess)
            {
                var organisationDto = JsonConvert.DeserializeObject<List<OrganisationDto>>(Convert.ToString(responseDto.Result)).First();
                if (!string.IsNullOrEmpty(organisationDto.UserIds))
                {
                    var userIds = JsonConvert.DeserializeObject<List<string>>(organisationDto.UserIds);
                    userIds.Add(userId.ToString());
                    organisationDto = organisationDto.WithUserIds(JsonConvert.SerializeObject(userIds));
                }
                else
                {
                    organisationDto = organisationDto.WithUserIds(JsonConvert.SerializeObject(new List<string> { userId.ToString() }));
                }

                await AddUpdateAsync(_mapper.Map<OrganisationModel>(organisationDto));
                return new ResponseDto(IsSuccess: true);
            }
            return new ResponseDto(Message: nameof(StatusCodes.Status400BadRequest));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new ResponseDto(Message: ex.Message);
        }
    }




    public async Task<ResponseDto> DeleteUserAsync(Guid userId, Guid orgId)
    {
        try
        {
            Expression<Func<OrganisationModel, bool>> expression = x => x.Id == orgId;
            var responseDto = await GetAsync(expression);

            if (responseDto is not null && responseDto.IsSuccess)
            {
                var organisationDto = JsonConvert.DeserializeObject<List<OrganisationDto>>(Convert.ToString(responseDto.Result)).First();
                if (!string.IsNullOrEmpty(organisationDto.UserIds))
                {
                    var userIds = JsonConvert.DeserializeObject<List<string>>(organisationDto.UserIds);
                    if (!userIds.Contains(userId.ToString())) 
                    {
                        return new ResponseDto(Message: nameof(StatusCodes.Status404NotFound));
                    }
                    userIds.Remove(userId.ToString());
                    organisationDto = organisationDto.WithUserIds(JsonConvert.SerializeObject(userIds));
                }
                else
                {
                    return new ResponseDto(Result: nameof(StatusCodes.Status404NotFound), Message: nameof(StatusCodes.Status404NotFound));
                }

                await AddUpdateAsync(_mapper.Map<OrganisationModel>(organisationDto));
                return new ResponseDto(Result: nameof(StatusCodes.Status200OK), IsSuccess: true);
            }
            return new ResponseDto(Message: nameof(StatusCodes.Status400BadRequest));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new ResponseDto(Message: ex.Message);
        }
    }
}
