using System.Net.Mime;
using static ExPaper.SharedModels.Lib.Utilitys.SD;
using ContentType = ExPaper.SharedModels.Lib.Utilitys.SD.ContentType;

namespace ExPaper.SharedModels.Lib.DTO;

public record RequestDto(
    ApiType ApiType,
    string Url,
    object? Data,
    string? AccessToken,
    ContentType? ContentType = ContentType.Json)
{

}
