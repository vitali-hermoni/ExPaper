using System.ComponentModel.DataAnnotations;

namespace ExPaper.SharedModels.Lib.DTO;

#nullable disable
public record FolderTabDto(
    Guid Id, 
    string Name,
    Guid FolderId,
    string? Color = null)
{
    public FolderTabDto WithColor(string color) => this with { Color = color };
}
