using server.Domains.DTOs;

namespace server.Application.Services.Interfaces;

public interface IFilesService
{
    public OperationResult CreatePersonalFolder(string folderName);
}