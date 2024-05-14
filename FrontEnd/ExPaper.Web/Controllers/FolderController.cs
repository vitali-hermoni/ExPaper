using ExPaper.SharedModels.Lib.DTO;
using ExPaper.SharedModels.Lib.Utilitys;
using ExPaper.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using ExPaper.Web.ViewModels;
using System.Security.Claims;

namespace ExPaper.Web.Controllers
{
    public class FolderController : Controller
    {
        private readonly IFolderService _folderService;
        private readonly IFolderTabService _folderTabService;
        private readonly IOrganisationService _organisationService;
        private readonly ILogger<FolderController> _logger;

        public FolderController(
            IFolderService folderService,
            IFolderTabService folderTabService,
            IOrganisationService organisationService,
            ILogger<FolderController> logger)
        {
            _folderService = folderService;
            _folderTabService = folderTabService;
            _organisationService = organisationService;
            _logger = logger;
        }



        public async Task<IActionResult> Index(string orgName)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    List<FolderViewModel> folderViewModels = new();
                    var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value;
                    var responseOrgsDto = await _organisationService.GetOrgsByUserId(Guid.Parse(userId));
                    if (responseOrgsDto is not null && responseOrgsDto.IsSuccess)
                    {
                        var allOrgs = JsonConvert.DeserializeObject<List<OrganisationDto>>(Convert.ToString(responseOrgsDto.Result));
                        var filterOrgs = allOrgs;
                        if (orgName is not null)
                        {
                            filterOrgs = allOrgs.Where(x => x.Name == orgName).ToList();
                        }

                        foreach (var org in filterOrgs)
                        {
                            var responseFolderDto = await _folderService.GetByOrgIdAsync(org.Id);
                            var folders = JsonConvert.DeserializeObject<List<FolderDto>>(Convert.ToString(responseFolderDto.Result));

                            folderViewModels.Add(new FolderViewModel(OrgId: org.Id, OrgName: org.Name, FolderDtos: folders));
                        }

                        TempData[SD.TempDataOk] = responseOrgsDto.Message;
                        ViewBag.Organisations = allOrgs;
                        return View(nameof(Index), folderViewModels);
                    }
                    else
                    {
                        TempData[SD.TempDataError] = responseOrgsDto?.Message;
                    }
                }
                else 
                {
                    TempData[SD.TempDataError] = "Sie sind nicht angemeldet!";
                }
                return RedirectToAction(nameof(Index), "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return RedirectToAction(nameof(Index), "Home");
            }
        }




        public async Task<IActionResult> AddUpdateView(Guid id)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.Equals("Sub", StringComparison.OrdinalIgnoreCase))?.Value;

                var responseOrgsDto = await _organisationService.GetOrgsByUserId(Guid.Parse(userId));
                var orgs = JsonConvert.DeserializeObject<List<OrganisationDto>>(Convert.ToString(responseOrgsDto.Result));

                if (id == Guid.Empty)
                {
                    var model = new FolderOrgListViewModel(new FolderDto(
                        Id: Guid.Empty,
                        Name: "",
                        Description: null,
                        Color: null,
                        Year: 0,
                        Month: null,
                        Quarter: 0,
                        Image: null,
                        OrganisationId: Guid.Empty), OrganisationDtos: orgs, ImageFile: null);

                    return View("AddUpdate", model);
                }
                else
                {
                    var responseDto = await _folderService.GetByIdAsync(id);
                    var folderDto = JsonConvert.DeserializeObject<List<FolderDto>>(Convert.ToString(responseDto.Result)).First();
                    var model = new FolderOrgListViewModel(folderDto, OrganisationDtos: orgs, ImageFile: null);

                    return View("AddUpdate", model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View("AddUpdate");
            }
        }




        public async Task<IActionResult> AddUpdate(FolderOrgListViewModel folderOrgListViewModel, string selectedColor, string selectedOrganizationId)
        {
            try
            {
                if (string.IsNullOrEmpty(selectedColor)) selectedColor = "WHITE";
                var folderDtoWithColor = folderOrgListViewModel.FolderDto.WithColor(selectedColor);
                var folderDtoWithOrg = folderDtoWithColor.WithOrganisation(Guid.Parse(selectedOrganizationId));

                var folderOrgListViewModelWith = folderOrgListViewModel.WithFolderDto(folderDtoWithOrg);
                var responseDto = await _folderService.AddUpdateAsync(folderOrgListViewModelWith);
                if (responseDto is not null && responseDto.IsSuccess)
                {
                    TempData[SD.TempDataOk] = responseDto.WithMessage(ConstStrings.Erfolgreich).Message;
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
                return RedirectToAction(nameof(Index));
            }
        }



        public async Task<IActionResult> DeleteById(Guid id)
        {
            try
            {
                var folderTabResponseDto = await _folderTabService.GetByFolderIdAsync(id);
                if (folderTabResponseDto.Result is not null && folderTabResponseDto.IsSuccess)
                {
                    TempData[SD.TempDataOk] = ConstStrings.TabsEvalible;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData[SD.TempDataError] = folderTabResponseDto?.Message;
                }
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }



        public async Task<IActionResult> AddToOrganisation(Guid folderId, Guid organisationId)
        {
            try
            {
                var responseDto = await _folderService.AddToOrganisationAsync(folderId, organisationId);
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
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
