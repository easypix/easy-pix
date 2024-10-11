using Microsoft.AspNetCore.Mvc;
using server.Application.Services.Classes;
using server.Application.Services.Interfaces;
using server.Domains.DTOs;

namespace server.WebApi.Controllers;

[ApiController]
[Route("/api/files")]
public class FilesController : ControllerBase
{
    private readonly IFilesService _service;
    public FilesController(IFilesService service)
    {
        _service = service;
    }
    [HttpPost("create-personal-folder")]
    public OperationResult CreatePersonalFolder(string folderName)
    {
        return _service.CreatePersonalFolder(folderName);
    }
}