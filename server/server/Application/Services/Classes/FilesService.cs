using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using server.Application.Services.Interfaces;
using server.Domains.DTOs;
using server.Domains.Enums;

namespace server.Application.Services.Classes;

public class FilesService : IFilesService
{
    private readonly ILogger<FilesService> _logger;
    
    private readonly string RootFolder;
    public FilesService(IConfiguration configuration, ILogger<FilesService> logger)
    {
        RootFolder = configuration["StorageSettings:RootFolder"];
        
        // Проверка на пустое значение RootFolder
        if (string.IsNullOrWhiteSpace(RootFolder))
        {
            _logger.LogError("RootFolder is not configured in appsettings.json");
            throw new ArgumentNullException(nameof(RootFolder), "RootFolder is not configured in appsettings.json");
        }

        // Убедитесь, что путь оканчивается слэшем
        if (!RootFolder.EndsWith(Path.DirectorySeparatorChar))
        {
            RootFolder += Path.DirectorySeparatorChar;
        }
        
        _logger = logger;
    }
    public OperationResult CreatePersonalFolder(string folderName)
    {
        DirectoryInfo directory;
        try
        {
            directory = Directory.CreateDirectory(RootFolder + folderName);
        }
        catch (UnauthorizedAccessException e)
        {
            _logger.LogError(e.Message);
            return OperationResult.Failure(ErrorCode.UnauthorizedAccessException);
        }
        catch (ArgumentException e)
        {
            _logger.LogError(e.Message);
            return OperationResult.Failure(ErrorCode.PathIsWrong);
        }
        catch (DirectoryNotFoundException e)
        {
            _logger.LogError(e.Message);
            return OperationResult.Failure(ErrorCode.DirectoryNotFoundException);
        }
        catch (NotSupportedException e)
        {
            _logger.LogError(e.Message);
            return OperationResult.Failure(ErrorCode.PathIsWrong);
        }
        catch (IOException e)
        {
            _logger.LogError(e.Message);
            return OperationResult.Failure(ErrorCode.ErrorWhileCreatingDirectory);
        }
        
        if (directory.Exists)
        {
            return OperationResult.Success();
        }
        return OperationResult.Failure(ErrorCode.ErrorWhileCreatingDirectory);
    }
}