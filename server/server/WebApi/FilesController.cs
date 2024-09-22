using Microsoft.AspNetCore.Mvc;
using server.Application.Services;
using server.Domains.DTOs;

namespace server.WebApi;

[ApiController]
[Route("/api/files")]
public class FilesController : ControllerBase
{
    private readonly FilesService _service;
    public FilesController(FilesService service)
    {
        _service = service;
    }
    [HttpPost("create-personal-folder")]
    public OperationResult CreatePersonalFolder(string folderName)
    {
        return _service.CreatePersonalFolder(folderName);
    }
}