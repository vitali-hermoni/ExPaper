using AutoMapper;
using ExPaper.Document.API.Data;
using ExPaper.Document.API.Models;
using ExPaper.Document.API.Services.IServices;
using ExPaper.SharedModels.Lib.DTO;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Linq.Expressions;
using Grpc.AspNetCore;
using System.Reflection.Metadata;
using Grpc.Core;

namespace ExPaper.Document.API.Services;


public class DocumentService : IDocumentService
{
    private readonly AppDbContext _appDbContext;
    private readonly ILogger<DocumentService> _logger;
    private readonly IMapper _mapper;


    public DocumentService(
        AppDbContext appDbContext,
        ILogger<DocumentService> logger, 
        IMapper mapper)
    {
        _appDbContext = appDbContext;
        _logger = logger;
        _mapper = mapper;
    }





    public async Task<ResponseDto> GetAsync(Expression<Func<DocumentModel, bool>> expression = null)
    {
        try
        {
            IQueryable<DocumentModel> query = _appDbContext.Document;
            query = expression is not null ? query.Where(expression) : query;
            var documents = await query.AsNoTracking().ToListAsync();
            var result = JsonConvert.SerializeObject(_mapper.Map<List<DocumentDto>>(documents));
            return new ResponseDto(Result: result, IsSuccess: true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new ResponseDto(Message: ex.Message);
        }
    }



    public async Task<ResponseDto> AddUpdateAsync(DocumentModel documentModel)
    {
        try
        {
            var obj = _appDbContext.Document.Update(documentModel);
            await _appDbContext.SaveChangesAsync();
            return new ResponseDto(IsSuccess: true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new ResponseDto(Message: ex.Message);
        }
    }



    public async Task<ResponseDto> RemoveAsync(Expression<Func<DocumentModel, bool>> expression)
    {
        try
        {
            IQueryable<DocumentModel> query = _appDbContext.Document;
            query = expression is not null ? query.Where(expression) : query;
            var documents = await query.AsNoTracking().ToListAsync();
            _appDbContext.Remove(documents.First());
            await _appDbContext.SaveChangesAsync();
            return new ResponseDto(IsSuccess: true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new ResponseDto(Message: ex.Message);
        }
    }
}
