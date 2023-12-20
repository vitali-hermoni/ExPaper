using ExPaper.SharedModels.Lib.DTO;
using ExPaper.SharedModels.Lib.Utilitys;
using ExPaper.Web.Services.IServices;
using ExPaper.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ExPaper.Web.Controllers
{
	public class OrganisationController : Controller
	{
		private readonly IOrganisationService _organisationService;
		private readonly ILogger<OrganisationController> _logger;

		public OrganisationController(
			IOrganisationService organisationService,
			ILogger<OrganisationController> logger)
		{
			_organisationService = organisationService;
			_logger = logger;
		}


		public async Task<IActionResult> Index()
		{
			try
			{
				var responseDto = await _organisationService.GetAsync();
				if (responseDto is not null && responseDto.IsSuccess)
				{
					TempData[SD.TempDataOk] = responseDto.Message;
					var organisations = JsonConvert.DeserializeObject<List<OrganisationDto>>(Convert.ToString(responseDto.Result));
					return View(nameof(Index), organisations);
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




		public async Task<IActionResult> AddUpdateView(Guid id)
		{
			try
			{
				if (id == Guid.Empty)
				{
					return View(nameof(AddUpdate), new OrganisationDto(Id: Guid.Empty, Name: "", UserIds: ""));
				}

				var responseDto = await _organisationService.GetByIdAsync(id);
				var organisationDto = JsonConvert.DeserializeObject<List<OrganisationDto>>(Convert.ToString(responseDto.Result)).First();

				return View(nameof(AddUpdate), organisationDto);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				TempData[SD.TempDataError] = "Exception";
				return RedirectToAction(nameof(Index), "Home");
			}
		}



		public async Task<IActionResult> AddUpdate(OrganisationDto organisationDto)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var responseDto = await _organisationService.AddUpdateAsync(organisationDto);
					if (responseDto is not null && responseDto.IsSuccess)
					{
						TempData[SD.TempDataOk] = responseDto.WithMessage(ConstStrings.Erfolgreich).Message;
						return RedirectToAction(nameof(Index));
					}
					else
					{
						TempData[SD.TempDataError] = responseDto?.Message;
						return RedirectToAction(nameof(Index), "Home");
					}
				}
				return RedirectToAction(nameof(Index), "Home");
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
                var responseDto = await _organisationService.DeleteByIdAsync(id);
                if (responseDto is not null && responseDto.IsSuccess)
                {
                    TempData[SD.TempDataOk] = responseDto.Message;
					return RedirectToAction(nameof(Index), "Home");
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




		public async Task<IActionResult> AddUser(Guid userId, Guid orgId, string orgName, string userIds)
        {
			try
			{
				var responseDto = await _organisationService.AddUserAsync(userId, orgId);
				if (responseDto is not null && responseDto.IsSuccess)
				{
					TempData[SD.TempDataOk] = responseDto.Message;
					return RedirectToAction(nameof(Index));
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




		public async Task<IActionResult> DeleteUser(Guid userId, Guid orgId, string orgName, string userIds)
		{
			try
			{
				var responseDto = await _organisationService.DeleteUserAsync(userId, orgId);
				if (responseDto is not null && responseDto.IsSuccess)
				{
					TempData[SD.TempDataOk] = responseDto.Message;
					return RedirectToAction(nameof(Index));
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




		public async Task<IActionResult> GetUserListForOrganisation(string userIds, Guid id, string orgName)
		{
			try
			{
				if (string.IsNullOrEmpty(userIds)) 
				{
					userIds = "[]";
				}

				var responseDtoUserInOrg = await _organisationService.GetUserListForOrganisationAsync(userIds);
                var responseDtoUserInDb = await _organisationService.GetUsersAsync();
                if (responseDtoUserInOrg is not null && responseDtoUserInOrg.IsSuccess && responseDtoUserInDb is not null && responseDtoUserInDb.IsSuccess)
				{
                    OrgUserListViewModel orgUserListViewModel = new();
					TempData[SD.TempDataOk] = responseDtoUserInOrg.Message;
					orgUserListViewModel.UserInOrg = JsonConvert.DeserializeObject<List<UserDto>>(Convert.ToString(responseDtoUserInOrg.Result));
                    orgUserListViewModel.UserInDb = JsonConvert.DeserializeObject<List<UserDto>>(Convert.ToString(responseDtoUserInDb.Result));
                    orgUserListViewModel.OrgId = id;
                    orgUserListViewModel.OrgName = orgName;
                    orgUserListViewModel.UserIds = userIds;
                    return View("UserList", orgUserListViewModel);
				}
				else
				{
					TempData[SD.TempDataError] = responseDtoUserInOrg?.Message;
				}
				TempData[SD.TempDataError] = responseDtoUserInOrg?.Message;
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
