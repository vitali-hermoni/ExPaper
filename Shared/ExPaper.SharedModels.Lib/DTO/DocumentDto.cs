using Microsoft.AspNetCore.Http;

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
    public DocumentDto WithName(string name) => this with { Name = name };
    public DocumentDto WithPath(string path) => this with { Path = path};
}
