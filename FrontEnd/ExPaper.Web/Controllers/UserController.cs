using ExPaper.SharedModels.Lib.DTO;
using ExPaper.SharedModels.Lib.Utilitys;
using ExPaper.Web.Services;
using ExPaper.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ExPaper.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly ITokenProvider _tokenProvider;
        private readonly IUserService _userService;
		private readonly ILogger<UserController> _logger;



		public UserController(
            ITokenProvider tokenProvider,
            IUserService userService,
            ILogger<UserController> logger)
        {
            _tokenProvider = tokenProvider;
            _userService = userService;
			_logger = logger;
		}




        public async Task<IActionResult> Index()
        {
            try
            {
                var responseDto = await _userService.GetUsersAsync();
                if (responseDto is not null && responseDto.IsSuccess)
                {
                    TempData[SD.TempDataOk] = responseDto.Message;
                    var users = JsonConvert.DeserializeObject<List<UserDto>>(Convert.ToString(responseDto.Result));
                    return View(nameof(Index), users);
                }
                else
                {
                    TempData[SD.TempDataError] = responseDto?.Message;
                    return RedirectToAction(nameof(Index), "Home");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
				TempData[SD.TempDataError] = "Exception";
				return RedirectToAction(nameof(Index), "Home");
			}
        }
    }
}
