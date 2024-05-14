using ExPaper.SharedModels.Lib.DTO;
using ExPaper.SharedModels.Lib.Utilitys;
using ExPaper.Web.Services.IServices;
using ExPaper.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ExPaper.Web.Controllers
{
    public class DocumentController : Controller
    {
        private readonly ILogger<DocumentController> _logger;
        private readonly IDocumentService _documentService;
        private readonly IFolderTabService _folderTabService;
        private readonly IFolderService _folderService;
        private readonly IOrganisationService _organisationService;
        private readonly IGlobalSettingService _globalSettingService;
        private readonly IFileService _fileService;


        public DocumentController(
            ILogger<DocumentController> logger,
            IDocumentService documentService,
            IFolderTabService folderTabService,
            IFolderService folderService,
            IOrganisationService organisationService,
            IGlobalSettingService globalSettingService,
            IFileService fileService)
        {
            _logger = logger;
            _documentService = documentService;
            _folderTabService = folderTabService;
            _folderService = folderService;
            _organisationService = organisationService;
            _globalSettingService = globalSettingService;
            _fileService = fileService;
        }




        public async Task<IActionResult> Index(Guid folderTabId)
        {
            try
            {
                var responseDocumentDto = await _documentService.GetByFolderTabIdAsync(folderTabId);
                if (responseDocumentDto.IsSuccess)
                {
                    var documentDtos = JsonConvert.DeserializeObject<List<DocumentDto>>(Convert.ToString(responseDocumentDto.Result));
                    var documentViewModel = new DocumentsViewModel(FolderTabId: folderTabId, DocumentDtos: documentDtos);

                    return View(documentViewModel);
                }

                var documentViewModelNoDocuments = new DocumentsViewModel(FolderTabId: folderTabId, DocumentDtos: null);
                return View(documentViewModelNoDocuments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return RedirectToAction(nameof(Index), "Home");
            }
        }



        //public async Task<IActionResult> WordEditor()
        //{
        //    return View();
        //}



        public async Task<IActionResult> DocumentView(Guid documentId)
        {
            try
            {
                if (documentId == Guid.Empty)
                {
                    return View(nameof(AddUpdate), new DocumentDto(
                        Id: Guid.Empty,
                        Name: "",
                        Date: DateTime.Now,
                        Description: "",
                        Tags: "",
                        Path: "",
                        Image: "",
                        TabId: Guid.Empty));
                }

                var responseDto = await _documentService.GetByIdAsync(documentId);
                var documentDto = JsonConvert.DeserializeObject<List<DocumentDto>>(Convert.ToString(responseDto.Result)).First();

                var serverAddressResponseDto = await _globalSettingService.GetByNameAsync(SD.DocumentViewerAddress);
                if (serverAddressResponseDto.IsSuccess)
                {
                    var globalSettingDto = JsonConvert.DeserializeObject<List<GlobalSettingsDto>>(Convert.ToString(serverAddressResponseDto.Result)).First();
                    var docPath = $"{globalSettingDto.Value}{documentDto.Path}";
                    
                    documentDto = documentDto.WithPath(docPath);
                    return Redirect(documentDto.Path);

                    //return View(nameof(DocumentView), documentDto);
                }

                TempData[SD.TempDataError] = serverAddressResponseDto.Message;
                return View(nameof(DocumentView), documentDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View(nameof(AddUpdate));
            }
        }



        public async Task<IActionResult> AddUpdateView(Guid folderTabId, Guid documentId)
        {
            try
            {
                if (documentId == Guid.Empty)
                {
                    return View(nameof(AddUpdate), new DocumentDto(
                        Id: Guid.Empty,
                        Name: "", 
                        Date: DateTime.Now, 
                        Description: "", 
                        Tags: "", 
                        Path: "", 
                        Image: "", 
                        TabId: folderTabId));
                }

                var responseDto = await _documentService.GetByIdAsync(documentId);
                var documentDto = JsonConvert.DeserializeObject<List<DocumentDto>>(Convert.ToString(responseDto.Result)).First();

                return View(nameof(AddUpdate), documentDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View(nameof(AddUpdate));
            }
        }



        public async Task<IActionResult> AddUpdate(DocumentDto documentDto, IFormFile file = null)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    if (documentDto.Name is null)
                    {
                        documentDto = documentDto.WithName(file.FileName);
                    }

                    var folderTabResponseDto = await _folderTabService.GetByIdAsync(documentDto.TabId);
                    var folderTabDtos = JsonConvert.DeserializeObject<List<FolderTabDto>>(Convert.ToString(folderTabResponseDto.Result));

                    var folderResponseDto = await _folderService.GetByIdAsync(folderTabDtos.First().FolderId);
                    var folderDtos = JsonConvert.DeserializeObject<List<FolderDto>>(Convert.ToString(folderResponseDto.Result));

                    var organisationResponseDto = await _organisationService.GetByIdAsync(folderDtos.First().OrganisationId);
                    var organisationDtos = JsonConvert.DeserializeObject<List<OrganisationDto>>(Convert.ToString(organisationResponseDto.Result));

                    var sharePathResponseDto = await _globalSettingService.GetByNameAsync(SD.WindowsSharePath);
                    var shareUserResponseDto = await _globalSettingService.GetByNameAsync(SD.WindowsShareUser);
                    var sharePassResponseDto = await _globalSettingService.GetByNameAsync(SD.WindowsSharePass);

                    var sharePathDto = JsonConvert.DeserializeObject<List<GlobalSettingsDto>>(Convert.ToString(sharePathResponseDto.Result));
                    var userNameDto = JsonConvert.DeserializeObject<List<GlobalSettingsDto>>(Convert.ToString(shareUserResponseDto.Result));
                    var passwordDto = JsonConvert.DeserializeObject<List<GlobalSettingsDto>>(Convert.ToString(sharePassResponseDto.Result));

                    string fileName = $"{Guid.NewGuid()}_{documentDto.Name}.pdf";
                    string filePath = Path.Combine(
                        organisationDtos.First().Name,
                        folderDtos.First().Name,
                        folderTabDtos.First().Name,
                        folderDtos.First().Year.ToString());


                    //var result = await _fileService.CopyFileToNetworkShare(
                    //    file,
                    //    fileName,
                    //    filePath, 
                    //    sharePathDto.First().Value, 
                    //    userNameDto.First().Value, 
                    //    passwordDto.First().Value);

                    documentDto = documentDto.WithPath(Path.Combine(filePath, fileName));
                    var responseDto = new ResponseDto();

                    if (file is not null)
                    {
                        responseDto = await _documentService.AddUpdateAsync(documentDto, file);
                    }
                    else
                    {
                        responseDto = await _documentService.AddUpdateAsync(documentDto);
                    }


                    if (responseDto.IsSuccess)
                    {
                        TempData[SD.TempDataOk] = ConstStrings.AddUpdateSuccessful;
                    }
                    else
                    {
                        TempData[SD.TempDataError] = ConstStrings.AddUpdateNotSuccessful;
                    }

                    var documentsResponseDto = await _documentService.GetByFolderTabIdAsync(documentDto.TabId);

                    if (documentsResponseDto.IsSuccess)
                    {
                        var documentDtos = JsonConvert.DeserializeObject<List<DocumentDto>>(Convert.ToString(documentsResponseDto.Result));
                        DocumentsViewModel documentsViewModel = new(FolderTabId: documentDto.TabId, DocumentDtos: documentDtos);
                        return View(nameof(Index), documentsViewModel);
                    }
                }

                return View();
            }
            catch (UnauthorizedAccessException uex)
            {
                TempData[SD.TempDataError] = ConstStrings.Unauthorized;
                _logger.LogError(uex, uex.Message);
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
                var sharePathResponseDto = await _globalSettingService.GetByNameAsync(SD.WindowsSharePath);
                var shareUserResponseDto = await _globalSettingService.GetByNameAsync(SD.WindowsShareUser);
                var sharePassResponseDto = await _globalSettingService.GetByNameAsync(SD.WindowsSharePass);

                var sharePathDto = JsonConvert.DeserializeObject<List<GlobalSettingsDto>>(Convert.ToString(sharePathResponseDto.Result));
                var userNameDto = JsonConvert.DeserializeObject<List<GlobalSettingsDto>>(Convert.ToString(shareUserResponseDto.Result));
                var passwordDto = JsonConvert.DeserializeObject<List<GlobalSettingsDto>>(Convert.ToString(sharePassResponseDto.Result));

                var documentResponseDto = await  _documentService.GetByIdAsync(id);
                var documentDto = JsonConvert.DeserializeObject<List<DocumentDto>>(Convert.ToString(documentResponseDto.Result));
                var folderTabId = documentDto.First().TabId;


                var result = _fileService.DeleteFileFromNetworkShare(
                    documentDto.First().Path,
                    sharePathDto.First().Value,
                    userNameDto.First().Value,
                    passwordDto.First().Value);

                var deleteResponseDto = await _documentService.RemoveByIdAsync(id);
                if (deleteResponseDto.IsSuccess)
                {
                    var documentsResponseDto = await _documentService.GetByFolderTabIdAsync(folderTabId);
                    var documentDtos = JsonConvert.DeserializeObject<List<DocumentDto>>(Convert.ToString(documentsResponseDto.Result));
                    DocumentsViewModel documentsViewModel = new(FolderTabId: folderTabId, DocumentDtos: documentDtos);
                    return View(nameof(Index), documentsViewModel);
                }

                if (result)
                {
                    
                }

                return View(nameof(Index), new DocumentsViewModel(FolderTabId: folderTabId, DocumentDtos: null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
