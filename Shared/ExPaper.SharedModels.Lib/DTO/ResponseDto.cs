namespace ExPaper.SharedModels.Lib.DTO;

public record ResponseDto(
    object Result = null,
    string Message = null,
    bool IsSuccess = false)
{
    public ResponseDto WithResult(object result) => this with { Result = result };
    public ResponseDto WithMessage(string message) => this with { Message = message };
    public ResponseDto WithIsSuccess(bool isSuccess) => this with { IsSuccess = isSuccess };
}
