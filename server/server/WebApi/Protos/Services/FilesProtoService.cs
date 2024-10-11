using FileService;
using Grpc.Core;
using server.Application.Services.Interfaces;

namespace server.WebApi.Protos;

public class FilesProtoService : Files.FilesBase
{
    private readonly IFilesService _filesService;
    public FilesProtoService(IFilesService filesService)
    {
        _filesService = filesService;
    }

    public override Task<CreateFolderResponse> CreateFolder(CreateFolderRequest request, ServerCallContext context)
    {
        var result = _filesService.CreatePersonalFolder(request.FolderName);
        return Task.FromResult(new CreateFolderResponse()
            {
                IsSuccess = result.IsSuccess,
                ErrorCode = result.ErrorCode
            }
        );
    }

    public override Task<IsFolderExistResponse> IsFolderExist(IsFolderExistRequest request, ServerCallContext context)
    {
        var result = _filesService.IsFolderExist(request.FolderName);
        return Task.FromResult(new IsFolderExistResponse()
        {
            ErrorCode = result.ErrorCode,
            IsSuccess = result.IsSuccess,
            IsExist = result.Data
        });
    }
}