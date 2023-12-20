using ExPaper.SharedMethods.Lib.Converter;
using ExPaper.SharedModels.Lib.DTO;
using ExPaper.SharedModels.Lib.Utilitys;
using ExPaper.Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ExPaper.Web.Controllers
{
    public class GlobalSettingsController : Controller
    {
        private readonly IGlobalSettingService _globalSettingService;
        private readonly ILogger<GlobalSettingsController> _logger;

        public GlobalSettingsController(
            IGlobalSettingService globalSettingService,
            ILogger<GlobalSettingsController> logger)
        {
            _globalSettingService = globalSettingService;
            _logger = logger;
        }


		public async Task<IActionResult> Index()
        {
            try
            {
				List<GlobalSettingsDto> globalSettingsDtos = new();

				var responseDto = await _globalSettingService.GetAsync();
				if (responseDto is not null && responseDto.IsSuccess)
				{
					TempData[SD.TempDataOk] = responseDto.Message;
					globalSettingsDtos = JsonConvert.DeserializeObject<List<GlobalSettingsDto>>(Convert.ToString(responseDto.Result));
					return View(nameof(Index), globalSettingsDtos);
				}
				TempData[SD.TempDataError] = responseDto?.Message;
				return RedirectToAction(nameof(Index), "Home");
			}
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
				TempData[SD.TempDataError] = "Exception";
				return RedirectToAction(nameof(Index), "Home");
			}
        }



		[HttpGet]
		public async Task<IActionResult> AddUpdateView(Guid id)
		{
            try
            {
				if (id == Guid.Empty)
				{
					return View("AddUpdate", new GlobalSettingsDto(Id: Guid.Empty, Name: "", Value: ""));
				}
				else
				{
					var responseDto = await _globalSettingService.GetByIdAsync(id);
					var globalSettingsDto = JsonConvert.DeserializeObject<List<GlobalSettingsDto>>(Convert.ToString(responseDto.Result)).First();

					return View("AddUpdate", globalSettingsDto);
				}
			}
            catch (Exception ex)
            {
				_logger.LogError(ex, ex.Message);
				TempData[SD.TempDataError] = "Exception";
				return RedirectToAction(nameof(Index), "Home");
			}
		}


		[HttpPost]
		public async Task<IActionResult> AddUpdate(GlobalSettingsDto globalSettingsDto)
        {
            try
            {
				if (ModelState.IsValid)
				{
					var responseDto = await _globalSettingService.AddUpdateAsync(globalSettingsDto);
					if (responseDto is not null && responseDto.IsSuccess)
					{
						TempData[SD.TempDataOk] = responseDto.WithMessage(ConstStrings.Erfolgreich).Message;
					}
					else
					{
						TempData[SD.TempDataError] = responseDto?.Message;
					}
				}
				return RedirectToAction(nameof(Index));
			}
            catch (Exception ex)
            {
				_logger.LogError(ex, ex.Message);
				TempData[SD.TempDataError] = "Exception";
				return RedirectToAction(nameof(Index), "Home");
			}
        }



		public async Task<IActionResult> DeleteById(Guid id)
        {
			try
			{
				var responseDto = await _globalSettingService.DeleteByIdAsync(id);
				if (responseDto is not null && responseDto.IsSuccess)
				{
					TempData[SD.TempDataOk] = responseDto.Message;
				}
				else
				{
					TempData[SD.TempDataError] = responseDto?.Message;
				}
				return RedirectToAction(nameof(Index));
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
