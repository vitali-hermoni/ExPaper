using AutoMapper;
using ExPaper.Global.API.Data;
using ExPaper.Global.API.Models;
using ExPaper.Global.API.Services.IServices;
using ExPaper.SharedModels.Lib.DTO;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ExPaper.Global.API.Services;

public class GlobalSettingsService : IGlobalSettingsService
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<GlobalSettingsService> _logger;

    public GlobalSettingsService(AppDbContext appDbContext, 
        IMapper mapper,
        ILogger<GlobalSettingsService> logger)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
        _logger = logger;
    }



    public async Task<ResponseDto> GetAsync(Expression<Func<GlobalSettingModel, bool>> expression = null)
    {
        try
        {
            IQueryable<GlobalSettingModel> query = _appDbContext.GlobalSetting;
            query = expression is not null ? query.Where(expression) : query;
            var globalSettings = await query.AsNoTracking().ToListAsync();
            var globalSettingsDto = _mapper.Map<List<GlobalSettingsDto>>(globalSettings);
            return new ResponseDto(Result: globalSettingsDto, IsSuccess: true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new ResponseDto(Message: ex.Message);
        }
    }
    

    
    public async Task<ResponseDto> AddUpdateAsync(GlobalSettingsDto globalSettingDto)
    {
        try
        {            
            var obj = _appDbContext.Update(_mapper.Map<GlobalSettingModel>(globalSettingDto));
            await _appDbContext.SaveChangesAsync();
            var result = _mapper.Map<GlobalSettingsDto>(obj.Entity);
            return new ResponseDto(Result: result, IsSuccess: true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new ResponseDto(Message: ex.Message);
        }
    }    



    public async Task<ResponseDto> RemoveAsync(Expression<Func<GlobalSettingModel, bool>> expression)
    {
        ResponseDto responseDto = new();
        try
        {
            var globalSetting = await _appDbContext.GlobalSetting.Where(expression).FirstAsync();
            if (globalSetting is not null)
            {
                var obj = _appDbContext.GlobalSetting.Remove(globalSetting);
                await _appDbContext.SaveChangesAsync();
                var result = _mapper.Map<GlobalSettingsDto>(obj.Entity);

                return responseDto
                    .WithResult(result)
                    .WithMessage("Objekt wurde erfolgreich gelöscht")
                    .WithIsSuccess(true);
            }
            else
            {
                return responseDto.WithMessage("globalSetting ist null");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return responseDto.WithMessage(ex.Message);
        }
    }
}
