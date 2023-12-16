using AutoMapper;
using ExPaper.Document.API.Models;
using ExPaper.Document.API.Services.IServices;
using ExPaper.SharedModels.Lib.DTO;
using ExPaper.SharedModels.Lib.Utilitys;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace ExPaper.Document.API.Controllers;


[Route("api/[controller]")]
[ApiController]
[Authorize]

[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public class DocumentController : ControllerBase
{
    private readonly IDocumentService _documentService;
    private readonly ILogger<DocumentController> _logger;
    private readonly IMapper _mapper;


    public DocumentController(
        IDocumentService documentService,
        ILogger<DocumentController> logger,
        IMapper mapper)
    {
        _documentService = documentService;
        _logger = logger;
        _mapper = mapper;
    }




    [HttpGet("GetAll")]
    [Authorize(Roles = nameof(SD.Role.ADMIN) + "," + nameof(SD.Role.MANAGER) + "," + nameof(SD.Role.SEPER_USER) + "," + nameof(SD.Role.USER))]
    public async Task<IActionResult> GetAll()
    {
        var responseDto = await _documentService.GetAsync();
        if (responseDto is null) return NotFound();
        if (responseDto.IsSuccess)
        {
            return Ok(responseDto);
        }
        return BadRequest(responseDto);
    }



    [HttpGet("GetById/{id}")]
    [Authorize(Roles = nameof(SD.Role.ADMIN) + "," + nameof(SD.Role.MANAGER) + "," + nameof(SD.Role.SEPER_USER) + "," + nameof(SD.Role.USER))]
    public async Task<IActionResult> GetById(Guid id)
    {
        Expression<Func<DocumentModel, bool>> expression = x => x.Id == id;
        var responseDto = await _documentService.GetAsync(expression);
        if (responseDto is null) return NotFound();
        if (responseDto.IsSuccess)
        {
            return Ok(responseDto);
        }
        return BadRequest(responseDto);
    }



    [HttpGet("GetByFolderTabId/{folderTabId}")]
    [Authorize(Roles = nameof(SD.Role.ADMIN) + "," + nameof(SD.Role.MANAGER) + "," + nameof(SD.Role.SEPER_USER) + "," + nameof(SD.Role.USER))]
    public async Task<IActionResult> GetByFolderTabId(Guid folderTabId)
    {
        Expression<Func<DocumentModel, bool>> expression = x => x.TabId == folderTabId;
        var responseDto = await _documentService.GetAsync(expression);
        if (responseDto is null) return NotFound();
        if (responseDto.IsSuccess)
        {
            return Ok(responseDto);
        }
        return BadRequest(responseDto);
    }




    [HttpPut("AddUpdate")]
    [Authorize(Roles = nameof(SD.Role.ADMIN) + "," + nameof(SD.Role.MANAGER) + "," + nameof(SD.Role.SEPER_USER) + "," + nameof(SD.Role.USER))]
    public async Task<IActionResult> AddUpdate([FromBody] DocumentDto documentDto)
    {
        var responseDto = await _documentService.AddUpdateAsync(_mapper.Map<DocumentModel>(documentDto));
        if (responseDto is null) return NotFound();
        if (responseDto.IsSuccess)
        {
            return Ok(responseDto);
        }
        return BadRequest(responseDto);
    }



    [HttpDelete("RemoveById/{id}")]
    [Authorize(Roles = nameof(SD.Role.MANAGER) + "," + nameof(SD.Role.SEPER_USER))]
    public async Task<IActionResult> RemoveById(Guid id)
    {
        Expression<Func<DocumentModel, bool>> expression = x => x.Id == id;
        var responseDto = await _documentService.RemoveAsync(expression);
        if (responseDto is null) return NotFound();
        if (responseDto.IsSuccess)
        {
            return Ok(responseDto);
        }
        return BadRequest(responseDto);
    }
}
