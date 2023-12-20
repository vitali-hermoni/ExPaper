using System.ComponentModel.DataAnnotations;

namespace ExPaper.SharedModels.Lib.DTO;

#nullable disable
public record DocumentDto(
    Guid Id, 
    string Name, 
    DateTime Date, 
    string Description, 
    string Tags, 
    string Path, 
    string Image, 
    Guid TabId)
{
    public DocumentDto WithPath(string path) => this with { Path = path};
}
