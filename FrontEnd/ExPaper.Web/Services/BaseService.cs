using ExPaper.SharedModels.Lib.DTO;
using ExPaper.Web.Services.IServices;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static ExPaper.SharedModels.Lib.Utilitys.SD;

namespace ExPaper.Web.Services;

public class BaseService : IBaseService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ITokenProvider _tokenProvider;
    private readonly ILogger<BaseService> _logger;

    public BaseService(
        IHttpClientFactory httpClientFactory, 
        ITokenProvider tokenProvider,
        ILogger<BaseService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _tokenProvider = tokenProvider;
        _logger = logger;
    }
     



    public async Task<ResponseDto> SendAsync(RequestDto requestDto, bool withBearer = true)
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient("ExPaper");
            HttpRequestMessage message = new();
            if (requestDto.ContentType == ContentType.MultipartFormData)
            {
                message.Headers.Add("Accept", "*/*");
            }
            else
            {
                message.Headers.Add("Accept", "application/json");
            }

            if (withBearer)
            {
                var token = _tokenProvider.GetToken();
                message.Headers.Add("Authorization", $"Bearer {token}");
            }

            message.RequestUri = new Uri(requestDto.Url);

            if (requestDto.ContentType == ContentType.MultipartFormData)
            {
                var content = new MultipartFormDataContent();

                foreach (var prop in requestDto.Data.GetType().GetProperties())
                {
                    var value = prop.GetValue(requestDto.Data);
                    if (value is FormFile)
                    {
                        var file = (FormFile)value;
                        if (file != null)
                        {
                            content.Add(new StreamContent(file.OpenReadStream()), prop.Name, file.FileName);
                        }
                    }
                    else
                    {
                        content.Add(new StringContent(value == null ? "" : value.ToString()), prop.Name);
                    }
                }
                message.Content = content;
            }
            else
            {
                if (requestDto.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                }
            }


            switch (requestDto.ApiType)
            {
                case ApiType.POST:
                    message.Method = HttpMethod.Post;
                    break;
                case ApiType.DELETE:
                    message.Method = HttpMethod.Delete;
                    break;
                case ApiType.PUT:
                    message.Method = HttpMethod.Put;
                    break;
                default:
                    message.Method = HttpMethod.Get;
                    break;
            }

            var apiResponse = await client.SendAsync(message);

            switch (apiResponse.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    return new ResponseDto(Message: "Not Found");
                case HttpStatusCode.Forbidden:
                    return new ResponseDto(Message: "Access Denied");
                case HttpStatusCode.Unauthorized:
                    return new ResponseDto(Message: "Unauthorized");
                case HttpStatusCode.InternalServerError:
                    return new ResponseDto(Message: "Internal Server Error");
                case HttpStatusCode.BadRequest:
                    var badRequestContent = await apiResponse.Content.ReadAsStringAsync();
                    try
                    {
                        var badResponseDto = JsonConvert.DeserializeObject<ResponseDto>(badRequestContent);
                        //var badResponseDto = new ResponseDto(Result: badRequestContent);
                        return badResponseDto;
                    }
                    catch(Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                        return new() { IsSuccess = false, Message = badRequestContent };
                    }                    

                default:
                    if (apiResponse.IsSuccessStatusCode)
                    {
                        if (apiResponse.Content.Headers.ContentLength != 0)
                        {
                            var apiContent = await apiResponse.Content.ReadAsStringAsync();
                            return JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                        }
                        return new ResponseDto(IsSuccess: true);
                    }                    
                    return new ResponseDto(Message: apiResponse.StatusCode.ToString());
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new ResponseDto(Message: ex.Message);           
        }
    }
}
