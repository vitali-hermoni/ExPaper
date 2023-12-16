using AutoMapper;
using ExPaper.Folder.API.Data;
using ExPaper.Folder.API.Models;
using ExPaper.Folder.API.Services.IServices;
using ExPaper.SharedModels.Lib.DTO;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Linq.Expressions;

namespace ExPaper.Folder.API.Services;

public class FolderService : IFolderService
{
    private readonly AppDbContext _appDbContext;
    private readonly ILogger<FolderService> _logger;
    private readonly IMapper _mapper;



    public FolderService(AppDbContext appDbContext,
        ILogger<FolderService> logger,
        IMapper mapper)
    {
        _appDbContext = appDbContext;
        _logger = logger;
        _mapper = mapper;
    }




    public async Task<ResponseDto> GetAsync(Expression<Func<FolderModel, bool>> expression = null)
    {
        try
        {
            IQueryable<FolderModel> query = _appDbContext.Folder;
            query = expression is not null ? query.Where(expression) : query;
            var folders = await query.AsNoTracking().ToListAsync();
            var result = JsonConvert.SerializeObject(_mapper.Map<List<FolderDto>>(folders));
            return new ResponseDto(Result: result, IsSuccess: true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new ResponseDto(Message: ex.Message);
        }
    }



    public async Task<ResponseDto> AddUpdateAsync(FolderModel folderModel)
    {
        try
        {
            var obj = _appDbContext.Folder.Update(folderModel);
            await _appDbContext.SaveChangesAsync();
            return new ResponseDto(IsSuccess: true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new ResponseDto(Message: ex.Message);
        }
    }



    public async Task<ResponseDto> RemoveAsync(Expression<Func<FolderModel, bool>> expression)
    {
        try
        {
            IQueryable<FolderModel> query = _appDbContext.Folder;
            query = expression is not null ? query.Where(expression) : query;
            var folder = await query.AsNoTracking().ToListAsync();
            _appDbContext.Remove(folder.First());
            await _appDbContext.SaveChangesAsync();
            return new ResponseDto(IsSuccess: true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new ResponseDto(Message: ex.Message);
        }
    }



    public async Task<ResponseDto> AddToOrganisationAsync(Guid folderId, Guid organisationId)
    {
        try
        {
            Expression<Func<FolderModel, bool>> expression = x => x.Id == folderId;
            var responseDto = await GetAsync(expression);
           
            if (responseDto.IsSuccess) 
            {
                var folder = JsonConvert.DeserializeObject<List<FolderModel>>(Convert.ToString(responseDto.Result)).First();
                folder.OrganisationId = organisationId;
                var response = await AddUpdateAsync(folder);
                if (response.IsSuccess)
                {
                    return new ResponseDto(IsSuccess: true);
                }
                return new ResponseDto(Result: nameof(StatusCodes.Status404NotFound), Message: nameof(StatusCodes.Status404NotFound));
            }
            return new ResponseDto(Message: nameof(StatusCodes.Status404NotFound));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.Message);
            return new ResponseDto(Message: ex.Message);
        }
    }
}
