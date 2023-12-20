using ExPaper.Document.API.Models;
using ExPaper.SharedModels.Lib.DTO;
using System.Linq.Expressions;

namespace ExPaper.Document.API.Services.IServices;

public interface IDocumentService
{
    Task<ResponseDto> GetAsync(Expression<Func<DocumentModel, bool>> expression = null);
    Task<ResponseDto> AddUpdateAsync(DocumentModel documentModel);
    Task<ResponseDto> RemoveAsync(Expression<Func<DocumentModel, bool>> expression);
}
