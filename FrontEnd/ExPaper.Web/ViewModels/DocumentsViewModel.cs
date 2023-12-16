using ExPaper.SharedModels.Lib.DTO;

namespace ExPaper.Web.ViewModels;

public record DocumentsViewModel(Guid FolderTabId, List<DocumentDto> DocumentDtos)
{
    
}
