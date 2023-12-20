using ExPaper.SharedModels.Lib.DTO;

namespace ExPaper.Web.ViewModels;

#nullable disable
public record FolderTabsViewModel(Guid FolderId, string? FolderName, List<FolderTabDto>? FolderTabDtos)
{
    public FolderTabsViewModel WithFolderTabDtos(List<FolderTabDto> folderTabDtos) => this with { FolderTabDtos = folderTabDtos };
}
