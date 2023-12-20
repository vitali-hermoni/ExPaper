using AutoMapper;
using ExPaper.FolderTab.API.Data;
using ExPaper.FolderTab.API.Models;
using ExPaper.FolderTab.API.Services.IServices;
using ExPaper.SharedModels.Lib.DTO;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace ExPaper.FolderTab.API.Services;

public class FolderTabService : IFolderTabService
{
    private readonly ILogger<FolderTabService> _logger;
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public FolderTabService(
        ILogger<FolderTabService> logger,
        AppDbContext appDbContext,
        IMapper mapper)
    {
        _logger = logger;
        _appDbContext = appDbContext;
        _mapper = mapper;
    }




    public async Task<ResponseDto> GetAsync(Expression<Func<FolderTabModel, bool>>? expression = null)
    {
        try
        {
            IQueryable<FolderTabModel> query = _appDbContext.FolderTab;
            query = expression is not null ? query.Where(expression) : query;
            var folderTabs = await query.AsNoTracking().ToListAsync();
            var result = JsonConvert.SerializeObject(_mapper.Map<List<FolderTabDto>>(folderTabs));
            return new ResponseDto(Result: result, IsSuccess: true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new ResponseDto(Message: ex.Message);
        }
    }



    public async Task<ResponseDto> AddUpdateAsync(FolderTabModel folderTabModel)
    {
        try
        {
            var obj = _appDbContext.FolderTab.Update(folderTabModel);
            await _appDbContext.SaveChangesAsync();
            return new ResponseDto(IsSuccess: true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new ResponseDto(Message: ex.Message);
        }
    }



    public async Task<ResponseDto> RemoveAsync(Expression<Func<FolderTabModel, bool>> expression)
    {
        try
        {
            IQueryable<FolderTabModel> query = _appDbContext.FolderTab;
            query = expression is not null ? query.Where(expression) : query;
            var folder = await query.AsNoTracking().ToListAsync();
            _appDbContext.RemoveRange(folder);
            await _appDbContext.SaveChangesAsync();
            return new ResponseDto(IsSuccess: true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new ResponseDto(Message: ex.Message);
        }
    }



    public Task<ResponseDto> CheckFolderHasTabsAsync(int folderId)
    {
        throw new NotImplementedException();
    }
}
