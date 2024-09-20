using Microsoft.AspNetCore.Mvc;
using server.Domains.DTOs;

namespace server.WebApi;

[ApiController]
[Route("/api/files")]
public class FilesController : ControllerBase
{
    [HttpPost("create-personal-folder")]
    public OperationResult CreatePersonalFolder(string folderName)
    {
        return OperationResult.Success();
    }
}