using ExPaper.SharedModels.Lib.DTO;
using ExPaper.SharedModels.Lib.Utilitys;
using ExPaper.Web.Services.IServices;
using ExPaper.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace ExPaper.Web.Controllers
{
    public class FolderTabController : Controller
    {
        private readonly ILogger<FolderTabController> _logger;
        private readonly IFolderService _folderService;
        private readonly IFolderTabService _folderTabService;
        private readonly IDocumentService _documentService;

        public FolderTabController(
            ILogger<FolderTabController> logger,
            IFolderService folderService,
            IFolderTabService folderTabService,
            IDocumentService documentService)
        {
            _logger = logger;
            _folderService = folderService;
            _folderTabService = folderTabService;
            _documentService = documentService;
        }



        public async Task<IActionResult> Index(Guid folderId, string folderName)
        {
            try
            {                
                var responseFolderTabDto = await _folderTabService.GetByFolderIdAsync(folderId);
                if (responseFolderTabDto.IsSuccess) 
                {
                    var folderTabs = JsonConvert.DeserializeObject<List<FolderTabDto>>(Convert.ToString(responseFolderTabDto.Result));
                    FolderTabsViewModel folderTabViewModel = new(FolderId: folderId, FolderName: folderName, FolderTabDtos: folderTabs);

                    return View(folderTabViewModel);
                }
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return RedirectToAction(nameof(Index), "Home");
            }
        }



        public async Task<IActionResult> AddUpdateView(Guid folderTabId, Guid folderId)
        {
            try
            {
                if (folderTabId == Guid.Empty)
                {
                    return View(nameof(AddUpdate), new FolderTabDto(Id: Guid.Empty, Name: "", Color: "", FolderId: folderTabId));
                }

                var responseDto = await _folderTabService.GetByIdAsync(folderTabId);
                var folderTabDto = JsonConvert.DeserializeObject<List<FolderTabDto>>(Convert.ToString(responseDto.Result)).First();

                return View(nameof(AddUpdate), folderTabDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View(nameof(AddUpdate));
            }
        }



        public async Task<IActionResult> AddUpdate(FolderTabDto folderTabDto, string selectedColor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(selectedColor))
                    {
                        selectedColor = "WHITE";
                    }

                    var responseDto = await _folderTabService.AddUpdateAsync(
                        new FolderTabDto(Id: folderTabDto.Id, Name: folderTabDto.Name, FolderId: folderTabDto.FolderId, Color: selectedColor));

                    if (responseDto is not null && responseDto.IsSuccess)
                    {
                        TempData[SD.TempDataOk] = ConstStrings.Erfolgreich;

                        var responseFolderTabDto = await _folderTabService.GetByFolderIdAsync(folderTabDto.FolderId);
                        if (responseFolderTabDto.IsSuccess)
                        {
                            var responseFolderDto = await _folderService.GetByIdAsync(folderTabDto.FolderId);
                            var folderDto = JsonConvert.DeserializeObject<List<FolderDto>>(Convert.ToString(responseFolderDto.Result)).First();

                            var folderTabs = JsonConvert.DeserializeObject<List<FolderTabDto>>(Convert.ToString(responseFolderTabDto.Result));
                            FolderTabsViewModel folderTabViewModel = new(
                                FolderId: folderTabDto.FolderId,
                                FolderName: folderDto.Name,
                                FolderTabDtos: folderTabs);

                            return View(nameof(Index), folderTabViewModel);
                        }
                    }
                    else
                    {
                        TempData[SD.TempDataError] = responseDto?.Message;
                    }
                }
                return View();
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
                if (ModelState.IsValid)
                {
                    var documentResponseDto = await _documentService.GetByFolderTabIdAsync(id);
                    if (documentResponseDto is not null && documentResponseDto.IsSuccess)
                    {
                        TempData[SD.TempDataError] = ConstStrings.DocumentEvalible;
                        return RedirectToAction(nameof(Index), "FolderTab");
                    }

                    var responseDto = await _folderTabService.RemoveByIdAsync(id);
                    if (responseDto.IsSuccess)
                    {
                        TempData[SD.TempDataOk] = ConstStrings.DeleteSuccessful;
                        return RedirectToAction(nameof(Index), "FolderTab");
                    }
                    else
                    {
                        TempData[SD.TempDataError] = ConstStrings.DeleteNotSuccessful;
                        return RedirectToAction(nameof(Index), "FolderTab");
                    }
                }
                TempData[SD.TempDataError] = ConstStrings.ModelStateNotValid;
                return RedirectToAction(nameof(Index), "FolderTab");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
